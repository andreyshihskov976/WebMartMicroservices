using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.DTOs.Order;
using WebMart.Extensions.Enums;
using WebMart.Extensions.Pages;
using WebMart.Extensions.SyncDataServices;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace Namespace
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IHttpDataService _httpDataService;

        public OrdersController(IOrderRepo repository, IMapper mapper,
        IMessageBusClient messageBusClient, IHttpDataService httpDataService)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _httpDataService = httpDataService;
        }

        [Authorize("admins_only")]
        [HttpGet("[action]", Name = "GetOrders")]
        public ActionResult<ICollection<OrderReadDto>> GetOrders([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting all Orders...");

            var orders = _repository.GetAllOrders();

            var ordersDtos = PagedList<OrderReadDto>.ToPagedList(
                _mapper.Map<ICollection<OrderReadDto>>(orders),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new
            {
                ordersDtos.TotalCount,
                ordersDtos.PageSize,
                ordersDtos.CurrentPage,
                ordersDtos.TotalPages,
                ordersDtos.HasNext,
                ordersDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(ordersDtos);
        }

        [Authorize("users_allowed")]
        [HttpGet("[action]", Name = "GetOrderById")]
        public ActionResult<OrderReadDto> GetOrderById(Guid id)
        {
            Console.WriteLine($"--> Gettng Order with id: {id}...");

            var order = _repository.GetOrderById(id);

            if (order != null)
            {
                return Ok(_mapper.Map<OrderReadDto>(order));
            }

            return NotFound();
        }

        [Authorize("users_allowed")]
        [HttpGet("[action]", Name = "GetOrdersByCustomerId")]
        public ActionResult<ICollection<OrderReadDto>> GetOrdersByCustomerId([FromQuery] PageParams parameters)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine($"--> Gettng Basket with id of customer: {customerId}...");

            var orders = _repository.GetOrdersByCustomerId(customerId);

            var ordersDtos = PagedList<OrderReadDto>.ToPagedList(
                _mapper.Map<ICollection<OrderReadDto>>(orders),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new
            {
                ordersDtos.TotalCount,
                ordersDtos.PageSize,
                ordersDtos.CurrentPage,
                ordersDtos.TotalPages,
                ordersDtos.HasNext,
                ordersDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(ordersDtos);
        }

        [Authorize("users_allowed")]
        [HttpPost("[action]", Name = "CreateOrder")]
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderCreateDto orderCreateDto)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine($"--> Creating Order for Basket with id: {orderCreateDto.BasketId}...");

            if (!_repository.BasketExists(orderCreateDto.BasketId))
            {
                var basketPublishedDto = await GetBasketFromCatalogServiceAsync(orderCreateDto.BasketId);
                if (basketPublishedDto != null)
                {
                    var basket = _mapper.Map<Basket>(basketPublishedDto);
                    _repository.CreateMissingBasket(basket);
                    if (basket.CustomerId != customerId)
                    {
                        return Forbid();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            var order = _mapper.Map<Order>(orderCreateDto);

            order.OrderDate = DateTime.UtcNow;

            _repository.CreateOrder(order);
            _repository.SaveChanges();

            SendAsyncMessage(order, EventType.OrderAdded);

            var orderReadDto = _mapper.Map<OrderReadDto>(order);

            return CreatedAtRoute(
                nameof(GetOrderById),
                new { Id = orderReadDto.Id },
                orderReadDto
            );
        }

        [Authorize("users_allowed")]
        [HttpDelete("[action]", Name = "DeleteOrder")]
        public ActionResult DeleteOrder([FromQuery] Guid id)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine("--> Deleting Order...");

            var order = _repository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Basket.CustomerId != customerId)
            {
                return Forbid();
            }

            if (order.Status >= OrderStatus.AwaitingShipment)
            {
                return Conflict();
            }

            _repository.DeleteOrder(order);
            _repository.SaveChanges();

            SendAsyncMessage(order, EventType.OrderDeleted);

            var basketReadDto = _mapper.Map<OrderReadDto>(order);

            return NoContent();
        }

        [Authorize("admins_only")]
        [HttpPut("[action]", Name = "UpdateOrder")]
        public ActionResult UpdateOrder(Guid id, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            var order = _repository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map(orderUpdateDto, order);
            _repository.UpdateOrder(order);
            _repository.SaveChanges();

            return NoContent();
        }

        private void SendAsyncMessage(Order orderReadDto, EventType eventType)
        {
            try
            {
                var orderPublishedDto = _mapper.Map<OrderPublishedDto>(orderReadDto);
                orderPublishedDto.Event = eventType;
                _messageBusClient.Publish(orderPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }
        }

        private async Task<BasketPublishedDto> GetBasketFromCatalogServiceAsync(Guid basketId)
        {
            try
            {
                Console.WriteLine("--> Sending request to the CatalogService");
                var response = await _httpDataService.SendGetRequest
                (
                    $"https://localhost:6274/api/Baskets/GetPublishedBasketById?id={basketId}"
                );
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BasketPublishedDto>(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            return null;
        }
    }
}
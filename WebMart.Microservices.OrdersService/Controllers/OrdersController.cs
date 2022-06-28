using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.Extensions.AsyncDataServices;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.DTOs.Order;
using WebMart.Microservices.Extensions.Enums;
using WebMart.Microservices.Extensions.SyncDataServices;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("[action]", Name = "GetAllOrders")]
        public ActionResult<IEnumerable<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto>> GetAllOrders()
        {
            Console.WriteLine("--> Getting all Orders...");

            var orders = _repository.GetAllOrders();

            return base.Ok(_mapper.Map<IEnumerable<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto>>(orders));
        }

        [HttpGet("[action]", Name = "GetAllOrdersDetailed")]
        public ActionResult<IEnumerable<OrderDetailedReadDto>> GetAllOrdersDetailed()
        {
            Console.WriteLine("--> Getting all Orders...");

            var orders = _repository.GetAllOrdersDetailed();

            return Ok(_mapper.Map<IEnumerable<OrderDetailedReadDto>>(orders));
        }

        [HttpGet("[action]", Name = "GetOrderById")]
        public ActionResult<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto> GetOrderById(Guid id)
        {
            Console.WriteLine($"--> Gettng Order with id: {id}...");

            var order = _repository.GetOrderById(id);

            if (order != null)
            {
                return base.Ok(_mapper.Map<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto>(order));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetOrderByIdDetailed")]
        public ActionResult<OrderDetailedReadDto> GetOrderByIdDetailed(Guid id)
        {
            Console.WriteLine($"--> Gettng Order with id: {id}...");

            var basket = _repository.GetOrderByIdDetailed(id);

            if (basket != null)
            {
                return Ok(_mapper.Map<OrderDetailedReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetOrderByCustomerId")]
        public ActionResult<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto> GetOrderByCustomerId([FromQuery] int customerId)
        {
            Console.WriteLine($"--> Gettng Order with id of customer: {customerId}...");

            var basket = _repository.GetOrderByCustomerId(customerId);

            if (basket != null)
            {
                return base.Ok(_mapper.Map<WebMart.Microservices.Extensions.DTOs.Order.OrderReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetOrderByCustomerIdDetailed")]
        public ActionResult<OrderDetailedReadDto> GetOrderByCustomerIdDetailed([FromQuery] int customerId)
        {
            Console.WriteLine($"--> Gettng Basket with id of customer: {customerId}...");

            var basket = _repository.GetOrderByCustomerIdDetailed(customerId);

            if (basket != null)
            {
                return Ok(_mapper.Map<OrderDetailedReadDto>(basket));
            }

            return NotFound();
        }

        [HttpPost("[action]", Name = "CreateOrder")]
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderCreateDto orderCreateDto)
        {
            Console.WriteLine($"--> Creating Order for Basket with id: {orderCreateDto.BasketId}...");

            var basket = _repository.GetBasketById(orderCreateDto.BasketId);

            if (basket == null)
            {
                var basketPublishedDto = await GetBasketFromCatalogServiceAsync(orderCreateDto.BasketId);
                if (basketPublishedDto != null)
                {
                    basket = _mapper.Map<Basket>(basketPublishedDto);
                    _repository.CreateMissingBasket(basket);
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

            var orderReadDto = _mapper.Map<OrderReadDto>(order);

            SendAsyncMessage(orderReadDto, EventType.OrderAdded);

            orderReadDto.BasketId = basket.Id;

            return CreatedAtRoute(
                nameof(GetOrderById),
                new { Id = orderReadDto.Id },
                orderReadDto
            );
        }

        [HttpDelete("[action]", Name = "DeleteOrder")]
        public ActionResult DeleteOrder([FromQuery] Guid id)
        {
            Console.WriteLine("--> Deleting Basket...");

            var order = _repository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            _repository.DeleteOrder(order);
            _repository.SaveChanges();

            var basketReadDto = _mapper.Map<OrderReadDto>(order);

            SendAsyncMessage(basketReadDto, EventType.OrderDeleted);

            return NoContent();
        }

        private void SendAsyncMessage(OrderReadDto orderReadDto, EventType eventType)
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
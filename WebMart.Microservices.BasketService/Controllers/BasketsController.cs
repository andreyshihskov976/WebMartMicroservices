using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Extensions.SyncDataServices;
using WebMart.Extensions.DTOs.Product;
using WebMart.Extensions.Enums;
using Newtonsoft.Json;
using WebMart.Extensions.Pages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IdentityModel;

namespace WebMart.Microservices.BasketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IHttpDataService _httpDataService;

        public BasketsController(IBasketRepo repository, IMapper mapper,
        IMessageBusClient messageBusClient, IHttpDataService httpDataService)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _httpDataService = httpDataService;
        }

        [Authorize("admins_only")]
        [HttpGet("[action]", Name = "GetBaskets")]
        public ActionResult<ICollection<BasketReadDto>> GetBaskets([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting all Baskets...");

            var baskets = _repository.GetAllBaskets();

            var basketsDtos = PagedList<BasketReadDto>.ToPagedList(
                _mapper.Map<ICollection<BasketReadDto>>(baskets),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new
            {
                basketsDtos.TotalCount,
                basketsDtos.PageSize,
                basketsDtos.CurrentPage,
                basketsDtos.TotalPages,
                basketsDtos.HasNext,
                basketsDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(basketsDtos);
        }

        [Authorize("users_allowed")]
        [HttpGet("[action]", Name = "GetBasketById")]
        public ActionResult<BasketReadDto> GetBasketById(Guid id)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine($"--> Gettng Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if(!basket.CustomerId.Equals(customerId) && User.FindFirst(JwtClaimTypes.Scope)?.Value != "admin_permissions")
            {
                return Forbid();
            }

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketReadDto>(basket));
            }

            return NotFound();
        }

        [Authorize("m2m.communication")]
        [HttpGet("[action]", Name = "GetPublishedBasketById")]
        public ActionResult<BasketPublishedDto> GetPublishedBasketById(Guid id)
        {
            Console.WriteLine($"--> Gettng published Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketPublishedDto>(basket));
            }

            return NotFound();
        }

        [Authorize("users_allowed")]
        [HttpGet("[action]", Name = "GetBasketsByCustomerId")]
        public ActionResult<ICollection<BasketReadDto>> GetBasketsByCustomerId([FromQuery] PageParams parameters)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine($"--> Getting all Baskets for Customer with id: {customerId}...");

            var baskets = _repository.GetBasketsByCustomerId(customerId);

            var basketsDtos = PagedList<BasketReadDto>.ToPagedList(
                _mapper.Map<ICollection<BasketReadDto>>(baskets),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new
            {
                basketsDtos.TotalCount,
                basketsDtos.PageSize,
                basketsDtos.CurrentPage,
                basketsDtos.TotalPages,
                basketsDtos.HasNext,
                basketsDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(basketsDtos);
        }

        [Authorize("users_allowed")]
        [HttpPost("[action]", Name = "CreateBasket")]
        public async Task<ActionResult> CreateBasketAsync([FromQuery] Guid productId)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (!_repository.ProductExists(productId))
            {
                var productPublishedDto = await GetProductFromCatalogServiceAsync(productId);
                if (productPublishedDto != null)
                {
                    var product = _mapper.Map<Product>(productPublishedDto);
                    _repository.CreateMissingProduct(product);
                }
                else
                {
                    return NotFound();
                }
            }

            var basketCreateDto = new BasketCreateDto { CustomerId = customerId, ProductId = productId };

            Console.WriteLine($"--> Adding Product with id: {basketCreateDto.ProductId} in Basket for Customer with id: {basketCreateDto.CustomerId}...");

            var basket = _mapper.Map<Basket>(basketCreateDto);
            _repository.CreateBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketAdded);

            var basketReadDto = _mapper.Map<BasketReadDto>(basket);

            return CreatedAtRoute(
                nameof(GetBasketById),
                new { Id = basketReadDto.Id },
                basketReadDto
            );
        }

        [Authorize("users_allowed")]
        [HttpDelete("[action]", Name = "DeleteBasket")]
        public ActionResult DeleteBasket([FromQuery] Guid id)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine("--> Deleting Basket...");

            var basket = _repository.GetBasketById(id);

            if(!basket.CustomerId.Equals(customerId))
            {
                return Forbid();
            }

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.IsOrdered)
            {
                return Conflict();
            }

            _repository.DeleteBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketDeleted);

            return NoContent();
        }

        [Authorize("users_allowed")]
        [HttpPut("[action]", Name = "UpdateBasket")]
        public ActionResult UpdateBasket([FromQuery] Guid id, [FromBody] BasketUpdateDto basketUpdateDto)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Console.WriteLine("--> Deleting Basket...");

            var basket = _repository.GetBasketById(id);

            if(!basket.CustomerId.Equals(customerId))
            {
                return Forbid();
            }

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.IsOrdered)
            {
                return Conflict();
            }

            _mapper.Map(basketUpdateDto, basket);
            _repository.UpdateBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketUpdated);

            return NoContent();
        }

        private void SendAsyncMessage(Basket basket, EventType eventType)
        {
            try
            {
                var basketPublishedDto = _mapper.Map<BasketPublishedDto>(basket);
                basketPublishedDto.Event = eventType;
                _messageBusClient.Publish(basketPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }
        }

        private async Task<ProductPublishedDto> GetProductFromCatalogServiceAsync(Guid productId)
        {
            try
            {
                Console.WriteLine("--> Sending request to the CatalogService");
                var response = await _httpDataService.SendGetRequest
                (
                    $"https://localhost:5129/api/Products/GetPublishedProductById?id={productId}"
                );
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ProductPublishedDto>(result);
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
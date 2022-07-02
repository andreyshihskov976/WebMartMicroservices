using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.AsyncDataServices;
using WebMart.Microservices.Extensions.SyncDataServices;
using WebMart.Microservices.Extensions.DTOs.Product;
using WebMart.Microservices.Extensions.Enums;
using Newtonsoft.Json;
using WebMart.Microservices.Extensions.Pages;

namespace WebMart.Microservices.BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("[action]", Name = "GetAllBaskets")]
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

        [HttpGet("[action]", Name = "GetBasketById")]
        public ActionResult<BasketReadDto> GetBasketById(Guid id)
        {
            Console.WriteLine($"--> Gettng Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]")]
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

        [HttpGet("[action]", Name = "GetBasketByCustomerId")]
        public ActionResult<BasketReadDto> GetOpenBasketByCustomerId([FromQuery] int customerId)
        {
            Console.WriteLine($"--> Gettng Basket with id of customer: {customerId}...");

            var basket = _repository.GetOpenBasketByCustomerId(customerId);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetProductsInBasket")]
        public ActionResult<ICollection<ProductReadDto>> GetProductsInBasket([FromQuery] Guid basketId, [FromQuery] PageParams parameters)
        {
            Console.WriteLine($"--> Getting Product from basket with id: {basketId}");

            var products = _repository.GetProductsInBasket(basketId);

            var productsDtos = PagedList<ProductReadDto>.ToPagedList(
                _mapper.Map<ICollection<ProductReadDto>>(products),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new
            {
                productsDtos.TotalCount,
                productsDtos.PageSize,
                productsDtos.CurrentPage,
                productsDtos.TotalPages,
                productsDtos.HasNext,
                productsDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(productsDtos);
        }

        [HttpPost("[action]", Name = "CreateBasket")]
        public ActionResult CreateBasket([FromBody] BasketCreateDto basketCreateDto)
        {
            Console.WriteLine($"--> Creating Basket for Customer with id: {basketCreateDto.CustomerId}...");

            if (_repository.OpenBasketForCustomerExists(basketCreateDto.CustomerId))
            {
                return RedirectToAction(
                    nameof(GetOpenBasketByCustomerId),
                    new { customerId = basketCreateDto.CustomerId }
                );
            }

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

        [HttpDelete("[action]", Name = "DeleteBasket")]
        public ActionResult DeleteBasket([FromQuery] Guid id)
        {
            Console.WriteLine("--> Deleting Basket...");

            var basket = _repository.GetBasketById(id);

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.IsClosed)
            {
                return BadRequest();
            }

            _repository.DeleteBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketDeleted);

            return NoContent();
        }

        [HttpPost("[action]", Name = "AddProductToBasket")]
        public async Task<ActionResult> AddProductToBasketAsync([FromQuery] Guid basketId, [FromQuery] Guid productId)
        {
            Console.WriteLine($"--> Adding Product with id: {basketId} in Basket with id: {productId}");

            var basket = _repository.GetBasketById(basketId);

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.IsClosed)
            {
                return BadRequest();
            }

            var product = _repository.GetProductById(productId);

            if (product == null)
            {
                //Add missing Product to local Db
                var productPublishedDto = await GetProductFromCatalogServiceAsync(productId);
                if (productPublishedDto != null)
                {
                    product = _mapper.Map<Product>(productPublishedDto);
                    _repository.CreateMissingProduct(product);
                }
                else
                {
                    return NotFound();
                }
            }

            if (basket.Products == null)
            {
                basket.Products = new List<Product>();
            }

            basket.Products.Add(product);

            _repository.UpdateBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketDeleted);

            return NoContent();
        }

        [HttpDelete("[action]", Name = "RemoveProductFromBasket")]
        public ActionResult RemoveProductFromBasket([FromQuery] Guid basketId, [FromQuery] Guid productId)
        {
            Console.WriteLine($"--> Removing Product with id: {basketId} in Basket with id: {productId}");

            var basket = _repository.GetBasketById(basketId);

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.IsClosed)
            {
                return BadRequest();
            }

            var product = _repository.GetProductById(productId);

            basket.Products.Remove(product);

            _repository.UpdateBasket(basket);
            _repository.SaveChanges();

            SendAsyncMessage(basket, EventType.BasketDeleted);

            return NoContent();
        }


        private void SendAsyncMessage(Basket basketReadDto, EventType eventType)
        {
            try
            {
                var basketPublishedDto = _mapper.Map<BasketPublishedDto>(basketReadDto);
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
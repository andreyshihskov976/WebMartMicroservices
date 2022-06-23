using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.EventProcessing;
using WebMart.Microservices.Extensions.AsyncDataServices;

namespace WebMart.Microservices.BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public BasketsController(IBasketRepo repository, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("[action]", Name = "GetAllBaskets")]
        public ActionResult<IEnumerable<BasketReadDto>> GetAllBaskets()
        {
            Console.WriteLine("--> Getting all Baskets...");

            var baskets = _repository.GetAllBaskets();

            return Ok(_mapper.Map<IEnumerable<BasketReadDto>>(baskets));
        }

        [HttpGet("[action]", Name = "GetAllBasketsDetailed")]
        public ActionResult<IEnumerable<BasketDetailedReadDto>> GetAllBasketsDetailed()
        {
            Console.WriteLine("--> Getting all Baskets...");

            var baskets = _repository.GetAllBasketsDetailed();

            return Ok(_mapper.Map<IEnumerable<BasketDetailedReadDto>>(baskets));
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

        [HttpGet("[action]", Name = "GetBasketByIdDetailed")]
        public ActionResult<BasketDetailedReadDto> GetBasketByIdDetailed(Guid id)
        {
            Console.WriteLine($"--> Gettng Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketDetailedReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetBasketByCustomerId")]
        public ActionResult<BasketReadDto> GetBasketByCustomerId([FromQuery] int customerId, [FromQuery] bool isOrdered)
        {
            Console.WriteLine($"--> Gettng Basket with id of customer: {customerId}...");

            var basket = _repository.GetBasketByCustomerId(customerId, isOrdered);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketReadDto>(basket));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetBasketByCustomerIdDetailed")]
        public ActionResult<BasketDetailedReadDto> GetBasketByCustomerIdDetailed([FromQuery] int customerId, [FromQuery] bool isOrdered)
        {
            Console.WriteLine($"--> Gettng Basket with id of customer: {customerId}...");

            var basket = _repository.GetBasketByCustomerIdDetailed(customerId, isOrdered);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketDetailedReadDto>(basket));
            }

            return NotFound();
        }

        [HttpPost("[action]", Name = "CreateBasket")]
        public ActionResult CreateBasket([FromBody] BasketCreateDto basketCreateDto)
        {
            Console.WriteLine($"--> Creating Basket for Customer with id: {basketCreateDto.CustomerId}...");

            var basket = _repository.GetBasketByCustomerId(basketCreateDto.CustomerId, false);

            if (basket != null)
            {
                return Ok(_mapper.Map<BasketReadDto>(basket));
            }

            basket = _mapper.Map<Basket>(basketCreateDto);
            _repository.CreateBasket(basket);
            _repository.SaveChanges();

            var basketReadDto = _mapper.Map<BasketReadDto>(basket);

            SendAsyncMessage(basketReadDto, EventType.BasketAdded);

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

            _repository.DeleteBasket(basket);
            _repository.SaveChanges();

            var basketReadDto = _mapper.Map<BasketReadDto>(basket);

            SendAsyncMessage(basketReadDto, EventType.BasketDeleted);

            return NoContent();
        }

        [HttpPost("[action]", Name = "AddProductToBasket")]
        public ActionResult AddProductToBasket([FromQuery] Guid basketId, [FromQuery] Guid productId)
        {
            Console.WriteLine($"--> Adding Product with id: {basketId} in Basket with id: {productId}");

            var basket = _repository.GetBasketById(basketId);

            if(basket == null)
            {
                return NotFound();
            }

            var product = _repository.GetProductByExternalId(productId);

            if(product == null)
            {
                //Get product from CatalogService
                return NotFound();
            }

            basket.Products.Add(product);

            _repository.UpdateBasket(basket);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("[action]", Name = "RemoveProductFromBasket")]
        public ActionResult RemoveProductFromBasket([FromQuery] Guid basketId, [FromQuery] Guid productId)
        {
            Console.WriteLine($"--> Removing Product with id: {basketId} in Basket with id: {productId}");

            var basket = _repository.GetBasketById(basketId);

            if(basket == null)
            {
                return NotFound();
            }

            var product = _repository.GetProductByExternalId(productId);

            if(product == null)
            {
                //Get product from CatalogService
                return NotFound();
            }

            basket.Products.Remove(product);

            _repository.UpdateBasket(basket);
            _repository.SaveChanges();

            return NoContent();
        }


        private void SendAsyncMessage(BasketReadDto basketReadDto, EventType eventType)
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
    }
}
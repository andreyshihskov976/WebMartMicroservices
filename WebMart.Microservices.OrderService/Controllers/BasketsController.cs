using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.DTOs.Order;
using WebMart.Extensions.Pages;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace Namespace
{
    [ApiController]
    [Authorize("admins_only")]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepo _repository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("[action]", Name = "GetAllProducts")]
        public ActionResult<ICollection<OrderBasketReadDto>> GetBaskets([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting all Baskets...");

            var baskets = _repository.GetAllBaskets();

            var basketsDtos = PagedList<OrderBasketReadDto>.ToPagedList(
                _mapper.Map<ICollection<OrderBasketReadDto>>(baskets),
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

        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<BasketReadDto> GetBasketById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if(basket != null)
            {
                return Ok(_mapper.Map<OrderBasketReadDto>(basket));
            }

            return NotFound();
        }
    }
}
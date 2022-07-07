using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.Pages;
using WebMart.Microservices.OrdersService.Models;
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

        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<Basket> GetBasketById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Basket with id: {id}...");

            var basket = _repository.GetBasketById(id);

            if(basket != null)
            {
                Ok(basket);
            }

            return NotFound();
        }
    }
}
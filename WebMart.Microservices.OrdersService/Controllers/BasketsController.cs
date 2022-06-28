using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<IEnumerable<Basket>> GetAllBaskets()
        {
            Console.WriteLine("--> Getting all Baskets...");

            var baskets = _repository.GetAllBaskets();

            return Ok(baskets);
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
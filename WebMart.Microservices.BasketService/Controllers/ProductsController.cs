using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;

namespace WebMart.Microservices.BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("[action]", Name = "GetAllProducts")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            Console.WriteLine("--> Getting all Products...");

            var products = _repository.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<Product> GetProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with id: {id}...");

            var product = _repository.GetProductById(id);

            if(product != null)
            {
                Ok(product);
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetProductByExternalId")]
        public ActionResult<Product> GetProductByExternalId([FromQuery] Guid externalId)
        {
            Console.WriteLine($"--> Getting Product with external id: {externalId}...");

            var product = _repository.GetProductById(externalId);

            if(product != null)
            {
                Ok(product);
            }

            return NotFound();
        }
    }
}
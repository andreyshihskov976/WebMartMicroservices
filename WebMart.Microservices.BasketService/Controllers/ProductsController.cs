using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Extensions.DTOs.Product;
using WebMart.Extensions.Pages;
using Microsoft.AspNetCore.Authorization;

namespace WebMart.Microservices.BasketService.Controllers
{
    [ApiController]
    [Authorize("admins_only")]
    [Route("api/[controller]")]
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
        public ActionResult<ICollection<ProductReadDto>> GetProducts([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting all Products...");

            var products = _repository.GetAllProducts();

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

        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<ProductReadDto> GetProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with id: {id}...");

            var product = _repository.GetProductById(id);

            if(product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound();
        }
    }
}
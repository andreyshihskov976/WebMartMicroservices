using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;
using CatalogService.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
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

        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            Console.WriteLine("--> Getting Products...");

            var products = _repository.GetAllProducts();

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ProductReadDto> GetProductById(Guid id)
        {
            Console.WriteLine($"--> Getting Product by Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound();
        }

        [HttpGet("{categoryId}", Name = "GetProductsByCategoryId")]
        public ActionResult<ProductReadDto> GetProductsByCategoryId(Guid categoryId)
        {
            Console.WriteLine($"--> Getting Product by Category with Id: {categoryId}...");

            var products = _repository.GetProductsByCategoryId(categoryId);
            if(products != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(products));
            }

            return NotFound();
        }


        [HttpPost]
        public ActionResult CreateProduct(ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            var product = _mapper.Map<Product>(productCreateDto);
            _repository.CreateProduct(product);
            _repository.SaveChanges();

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            return CreatedAtRoute(
                nameof(GetProductById),
                new { Id = productReadDto.Id },
                productReadDto
                );
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public ActionResult DeleteProduct(Guid id)
        {
            Console.WriteLine($"--> Deleting Product with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.DeleteProduct(product);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public ActionResult UpdateProduct(Guid id, ProductCreateDto productCreateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(productCreateDto, product);
            _repository.UpdateProduct(product);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}

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
            Console.WriteLine($"--> Getting Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound();
        }

        [HttpGet("InCategory/{categoryId}", Name = "GetProductsByCategoryId")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsByCategoryId(Guid categoryId)
        {
            Console.WriteLine($"--> Getting Products by Category with Id: {categoryId}...");

            var products = _repository.GetProductsByCategoryId(categoryId);
            
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("InSubCategory/{subCategoryId}", Name = "GetProductsBySubCategoryId")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsBySubCategoryId(Guid subCategoryId)
        {
            Console.WriteLine($"--> Getting Products by SubCategory with Id: {subCategoryId}...");

            var products = _repository.GetProductsByCategoryId(subCategoryId);

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("Detailed/{id}", Name = "GetDetailedProductById")]
        public ActionResult<Product> GetDetailedProductById(Guid id)
        {
            Console.WriteLine($" Getting detailed Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if(product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }


        [HttpPost("{subCategoryId}", Name = "CreateProduct")]
        public ActionResult CreateProduct(Guid subCategoryId, ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            if(!_repository.IsSubCategoryExists(subCategoryId))
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productCreateDto);
            _repository.CreateProduct(subCategoryId, product);
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

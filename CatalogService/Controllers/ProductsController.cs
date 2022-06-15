using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;
using CatalogService.Pages;
using CatalogService.Pages.Models;
using CatalogService.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts([FromQuery] ProductParameters parameters)
        {
            Console.WriteLine("--> Getting SubСategories by pages...");

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

        [HttpGet("InCategory/{categoryId}/", Name = "GetProductsByCategoryId")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsByCategoryId(Guid categoryId, [FromQuery] ProductParameters parameters)
        {
            Console.WriteLine($"--> Getting Products by Category with Id: {categoryId} by pages...");

            if(_repository.IsSubCategoryExists(categoryId))
            {
                return NotFound();
            }

            var products = _repository.GetProductsByCategoryId(categoryId);

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

        [HttpGet("InSubCategory/{subCategoryId}/", Name = "GetProductsBySubCategoryId")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsBySubCategoryId(Guid subCategoryId, [FromQuery] ProductParameters parameters)
        {
            Console.WriteLine($"--> Getting Products by SubCategory with Id: {subCategoryId} by pages...");

            if(!_repository.IsSubCategoryExists(subCategoryId))
            {
                return NotFound();
            }

            var products = _repository.GetProductsByCategoryId(subCategoryId);

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

        [HttpGet("Detailed/{id}", Name = "GetDetailedProductById")]
        public ActionResult<Product> GetDetailedProductById(Guid id)
        {
            Console.WriteLine($" Getting detailed Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }


        [HttpPost("{subCategoryId}", Name = "CreateProduct")]
        public ActionResult CreateProduct(Guid subCategoryId, ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            if (!_repository.IsSubCategoryExists(subCategoryId))
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

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.Extensions.AsyncDataServices;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;
using WebMart.Microservices.Extensions.DTOs.Product;
using WebMart.Microservices.Extensions.Pages;
using WebMart.Microservices.Extensions.Enums;

namespace WebMart.Microservices.CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public ProductsController(IProductRepo repository, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("[action]", Name = "GetProducts")]
        public ActionResult<ICollection<ProductReadDto>> GetProducts([FromQuery] PageParams parameters)
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

        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<ProductReadDto> GetProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetDetailedProductById")]
        public ActionResult<ProductDetailedReadDto> GetDetailedProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with Id: {id}...");

            var product = _repository.GetProductByIdDetailed(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductDetailedReadDto>(product));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetPublishedProductById")]
        public ActionResult<ProductPublishedDto> GetPublishedProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting published Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductPublishedDto>(product));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetProductsInCategory")]
        public ActionResult<ICollection<ProductReadDto>> GetProductsInCategory(
            [FromQuery] Guid categoryId, [FromQuery] PageParams parameters)
        {
            Console.WriteLine($"--> Getting Products by Category with Id: {categoryId} by pages...");

            if (_repository.IsCategoryExists(categoryId))
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

        [HttpGet("[action]", Name = "GetProductsInSubCategory")]
        public ActionResult<ICollection<ProductReadDto>> GetProductsInSubCategory(
            [FromQuery] Guid subCategoryId, [FromQuery] PageParams parameters)
        {
            Console.WriteLine($"--> Getting Products by SubCategory with Id: {subCategoryId} by pages...");

            if (!_repository.IsSubCategoryExists(subCategoryId))
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

        [HttpPost("[action]", Name = "CreateProductInSubCategory")]
        public ActionResult CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            if (!_repository.IsSubCategoryExists(productCreateDto.SubCategoryId))
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productCreateDto);
            _repository.CreateProduct(product);
            _repository.SaveChanges();

            SendAsyncMessage(product, EventType.ProductAdded);

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            return CreatedAtRoute(
                nameof(GetProductById),
                new { Id = productReadDto.Id },
                productReadDto
            );
        }

        [HttpDelete("[action]", Name = "DeleteProduct")]
        public ActionResult DeleteProduct([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Deleting Product with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.DeleteProduct(product);
            _repository.SaveChanges();

            SendAsyncMessage(product, EventType.ProductDeleted);

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            return NoContent();
        }

        [HttpPut("[action]", Name = "UpdateProduct")]
        public ActionResult UpdateProduct([FromQuery] Guid id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(productUpdateDto, product);
            _repository.UpdateProduct(product);
            _repository.SaveChanges();

            SendAsyncMessage(product, EventType.ProductModified);

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            return NoContent();
        }

        private void SendAsyncMessage(Product product, EventType eventType)
        {
            try
            {
                var productPublishedDto = _mapper.Map<ProductPublishedDto>(product);
                productPublishedDto.Event = eventType;
                _messageBusClient.Publish(productPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }
        }
    }
}
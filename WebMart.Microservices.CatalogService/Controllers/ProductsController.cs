using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;
using WebMart.Extensions.DTOs.Product;
using WebMart.Extensions.Pages;
using WebMart.Extensions.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebMart.Microservices.CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetProductById")]
        public ActionResult<ProductReadDto> GetProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with Id: {id}...");

            var product = _repository.GetProductById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound("This product does not exist.");
        }

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetDetailedProductById")]
        public ActionResult<ProductDetailedReadDto> GetDetailedProductById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Product with Id: {id}...");

            var product = _repository.GetProductByIdDetailed(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductDetailedReadDto>(product));
            }

            return NotFound("This product does not exist.");
        }

        [Authorize("m2m.communication")]
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

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetProductsByCategoryId")]
        public ActionResult<ICollection<ProductReadDto>> GetProductsByCategoryId(
            [FromQuery] Guid categoryId, [FromQuery] PageParams parameters)
        {
            Console.WriteLine($"--> Getting Products by Category with Id: {categoryId} by pages...");

            if (!_repository.IsCategoryExists(categoryId))
            {
                return NotFound("This category does not exist.");
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

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetProductsInSubCategory")]
        public ActionResult<ICollection<ProductReadDto>> GetProductsInSubCategory(
            [FromQuery] Guid subCategoryId, [FromQuery] PageParams parameters)
        {
            Console.WriteLine($"--> Getting Products by SubCategory with Id: {subCategoryId} by pages...");

            if (!_repository.IsSubCategoryExists(subCategoryId))
            {
                return NotFound("This subcategory does not exist.");
            }

            var products = _repository.GetProductsBySubCategoryId(subCategoryId);

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

        [Authorize("admins_only")]
        [HttpPost("[action]", Name = "CreateProduct")]
        public ActionResult CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            if (!_repository.IsSubCategoryExists(productCreateDto.SubCategoryId))
            {
                return NotFound("This subcategory does not exist.");
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

        [Authorize("admins_only")]
        [HttpDelete("[action]", Name = "DeleteProduct")]
        public ActionResult DeleteProduct([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Deleting Product with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound("This product does not exist.");
            }

            _repository.DeleteProduct(product);
            _repository.SaveChanges();

            SendAsyncMessage(product, EventType.ProductDeleted);

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            return NoContent();
        }

        [Authorize("admins_only")]
        [HttpPut("[action]", Name = "UpdateProduct")]
        public ActionResult UpdateProduct([FromQuery] Guid id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var product = _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound("This product does not exist.");
            }

            _mapper.Map(productUpdateDto, product);
            _repository.UpdateProduct(product);
            _repository.SaveChanges();

            SendAsyncMessage(product, EventType.ProductUpdated);

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
using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;
using CatalogService.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductsController(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            Console.WriteLine("--> Getting Products...");

            var products = _repository.GetAllEntities();

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<CategoryReadDto> GetProductById(Guid id)
        {
            Console.WriteLine($"--> Getting Product by Id: {id}...");

            var product = _repository.GetEntityById(id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductCreateDto productCreateDto)
        {
            Console.WriteLine("--> Creating Product...");

            var product = _mapper.Map<Product>(productCreateDto);
            _repository.CreateEntity(product);
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

            var product = _repository.GetEntityById(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.DeleteEntity(product);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public ActionResult UpdateProduct(Guid id, ProductCreateDto productCreateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var product = _repository.GetEntityById(id);

            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(productCreateDto, product);
            _repository.UpdateEntity(product);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}

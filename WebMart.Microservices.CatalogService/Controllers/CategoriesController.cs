using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;
using WebMart.Microservices.Extensions.DTOs.Category;
using WebMart.Microservices.Extensions.Pages;

namespace WebMart.Microservices.CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("[action]", Name = "GetTestString")]
        public ActionResult<string> GetTestString()
        {
            return Ok("--> Message from Catalog API");
        }

        [HttpGet("[action]", Name = "GetCategoriesByPages")]
        public ActionResult<ICollection<CategoryReadDto>> GetCategories([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting Сategories by pages...");

            var categories = _repository.GetAllCategories();

            var categoriesDtos = PagedList<CategoryReadDto>.ToPagedList(
                _mapper.Map<ICollection<CategoryReadDto>>(categories),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new{
                categoriesDtos.TotalCount,
                categoriesDtos.PageSize,
                categoriesDtos.CurrentPage,
                categoriesDtos.TotalPages,
                categoriesDtos.HasNext,
                categoriesDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

	        return Ok(categoriesDtos);
        }

        [HttpGet("[action]", Name = "GetCategoryById")]
        public ActionResult<CategoryReadDto> GetCategoryById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Category by Id: {id}...");

            var category = _repository.GetCategoryById(id);
            if (category != null)
            {
                return Ok(_mapper.Map<CategoryReadDto>(category));
            }

            return NotFound();
        }

        [HttpPost("[action]", Name = "CreateCategory")]
        public ActionResult CreateCategory([FromBody] CategoryCreateUpdateDto categoryCreateDto)
        {
            Console.WriteLine("--> Creating Category...");

            var category = _mapper.Map<Category>(categoryCreateDto);
            _repository.CreateCategory(category);
            _repository.SaveChanges();

            var categoryReadDto = _mapper.Map<CategoryReadDto>(category);

            return CreatedAtRoute(
                nameof(GetCategoryById),
                new { Id = categoryReadDto.Id },
                categoryReadDto
                );
        }

        [HttpDelete("[action]", Name = "DeleteCategory")]
        public ActionResult DeleteCategory([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Deleting Category with Id: {id}");

            var category = _repository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            _repository.DeleteCategory(category);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("[action]", Name = "UpdateCategory")]
        public ActionResult UpdateCategory([FromQuery] Guid id, [FromBody] CategoryCreateUpdateDto categoryCreateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var category = _repository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(categoryCreateDto,category);
            _repository.UpdateCategory(category);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
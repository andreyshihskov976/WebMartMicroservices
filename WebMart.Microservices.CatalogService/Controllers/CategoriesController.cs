using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;
using WebMart.Extensions.DTOs.Category;
using WebMart.Extensions.Pages;
using Microsoft.AspNetCore.Authorization;

namespace WebMart.Microservices.CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize("admins_only")]
        [HttpGet("[action]", Name = "TestAuth")]
        public ActionResult<string> TestAuth()
        {
            return Ok("String for Admins only");
        }

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetCategories")]
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

        [AllowAnonymous]
        [HttpGet("[action]", Name = "GetCategoryById")]
        public ActionResult<CategoryReadDto> GetCategoryById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting Category by Id: {id}...");

            var category = _repository.GetCategoryById(id);
            if (category != null)
            {
                return Ok(_mapper.Map<CategoryReadDto>(category));
            }

            return NotFound("This category does not exist.");
        }

        [Authorize("admins_only")]
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

        [Authorize("admins_only")]
        [HttpDelete("[action]", Name = "DeleteCategory")]
        public ActionResult DeleteCategory([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Deleting Category with Id: {id}");

            var category = _repository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("This category does not exist.");
            }

            _repository.DeleteCategory(category);
            _repository.SaveChanges();

            return NoContent();
        }

        [Authorize("admins_only")]
        [HttpPut("[action]", Name = "UpdateCategory")]
        public ActionResult UpdateCategory([FromQuery] Guid id, [FromBody] CategoryCreateUpdateDto categoryCreateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var category = _repository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("This category does not exist.");
            }

            _mapper.Map(categoryCreateDto,category);
            _repository.UpdateCategory(category);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
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
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoriesController(IRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryReadDto>> GetAllCategories()
        {
            Console.WriteLine("--> Getting Categories...");

            var categories = _repository.GetAllEntities();

            return Ok(_mapper.Map<IEnumerable<CategoryReadDto>>(categories));
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public ActionResult<CategoryReadDto> GetCategoryById(Guid id)
        {
            Console.WriteLine($"--> Getting Category by Id: {id}...");

            var category = _repository.GetEntityById(id);
            if (category != null)
            {
                return Ok(_mapper.Map<CategoryReadDto>(category));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            Console.WriteLine("--> Creating Category...");

            var category = _mapper.Map<Category>(categoryCreateDto);
            _repository.CreateEntity(category);
            _repository.SaveChanges();

            var categoryReadDto = _mapper.Map<CategoryReadDto>(category);

            return CreatedAtRoute(
                nameof(GetCategoryById),
                new { Id = categoryReadDto.Id },
                categoryReadDto
                );
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        public ActionResult DeleteCategory(Guid id)
        {
            Console.WriteLine($"--> Deleting Category with Id: {id}");

            var category = _repository.GetEntityById(id);

            if (category == null)
            {
                return NotFound();
            }

            _repository.DeleteEntity(category);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        public ActionResult UpdateCategory(Guid id, CategoryCreateDto categoryCreateDto)
        {
            Console.WriteLine($"--> Updating Category with Id: {id}");

            var category = _repository.GetEntityById(id);

            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(categoryCreateDto,category);
            _repository.UpdateEntity(category);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}

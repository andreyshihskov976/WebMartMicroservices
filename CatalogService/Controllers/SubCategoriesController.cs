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
    public class SubCategoriesController : ControllerBase
    {
        private readonly IRepository<SubCategory> _repository;
        private readonly IMapper _mapper;

        public SubCategoriesController(IRepository<SubCategory> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SubCategoryReadDto>> GetAllSubCategories()
        {
            Console.WriteLine("--> Getting SubCategories...");

            var subCategories = _repository.GetAllEntities();

            return Ok(_mapper.Map<IEnumerable<SubCategoryReadDto>>(subCategories));
        }

        [HttpGet("{id}", Name = "GetSubCategoryById")]
        public ActionResult<SubCategoryReadDto> GetSubCategoryById(Guid id)
        {
            Console.WriteLine($"--> Getting SubCategory by Id: {id}...");

            var subCategory = _repository.GetEntityById(id);
            if (subCategory != null)
            {
                return Ok(_mapper.Map<SubCategoryReadDto>(subCategory));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateSubCategory(SubCategoryCreateDto subCategoryCreateDto)
        {
            Console.WriteLine("--> Creating SubCategory...");

            var subCategory = _mapper.Map<SubCategory>(subCategoryCreateDto);
            _repository.CreateEntity(subCategory);
            _repository.SaveChanges();

            var subCategoryReadDto = _mapper.Map<SubCategoryReadDto>(subCategory);

            return CreatedAtRoute(
                nameof(GetSubCategoryById),
                new { Id = subCategoryReadDto.Id },
                subCategoryReadDto
                );
        }

        [HttpDelete("{id}", Name = "DeleteSubCategory")]
        public ActionResult DeleteSubCategory(Guid id)
        {
            Console.WriteLine($"--> Deleting SubCategory with Id: {id}");

            var subCategory = _repository.GetEntityById(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _repository.DeleteEntity(subCategory);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateSubCategory")]
        public ActionResult UpdateSubCategory(Guid id, CategoryCreateDto subCategoryCreateDto)
        {
            Console.WriteLine($"--> Updating SubCategory with Id: {id}");

            var subCategory = _repository.GetEntityById(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _mapper.Map(subCategoryCreateDto, subCategory);
            _repository.UpdateEntity(subCategory);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}

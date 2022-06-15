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
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryRepo _repository;
        private readonly IMapper _mapper;

        public SubCategoriesController(ISubCategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SubCategoryReadDto>> GetSubCategories([FromQuery] SubCategoryParameters parameters)
        {
            Console.WriteLine("--> Getting SubСategories by pages...");

            var subCategories = _repository.GetAllSubCategories();

            var subCategoriesDtos = PagedList<SubCategoryReadDto>.ToPagedList(
                _mapper.Map<ICollection<SubCategoryReadDto>>(subCategories),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new{
                subCategoriesDtos.TotalCount,
                subCategoriesDtos.PageSize,
                subCategoriesDtos.CurrentPage,
                subCategoriesDtos.TotalPages,
                subCategoriesDtos.HasNext,
                subCategoriesDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

	        return Ok(subCategoriesDtos);
        }

        [HttpGet("{id}", Name = "GetSubCategoryById")]
        public ActionResult<SubCategoryReadDto> GetSubCategoryById(Guid id)
        {
            Console.WriteLine($"--> Getting SubCategory by Id: {id}...");

            var subCategory = _repository.GetSubCategoryById(id);
            if (subCategory != null)
            {
                return Ok(_mapper.Map<SubCategoryReadDto>(subCategory));
            }

            return NotFound();
        }

        [HttpGet("InCategory/{categoryId}/", Name = "GetSubCategoryByCategoryId")]
        public ActionResult<IEnumerable<SubCategoryReadDto>> GetSubCategoriesByCategoryId(Guid categoryId, [FromQuery] SubCategoryParameters parameters)
        {
            Console.WriteLine($"--> Getting SubCategories by Category with Id: {categoryId} by pages...");

            if(!_repository.IsCategoryExists(categoryId))
            {
                return NotFound();
            }

            var subCategories = _repository.GetSubCategoriesByCategoryId(categoryId);

            var subCategoriesDtos = PagedList<SubCategoryReadDto>.ToPagedList(
                _mapper.Map<ICollection<SubCategoryReadDto>>(subCategories),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new{
                subCategoriesDtos.TotalCount,
                subCategoriesDtos.PageSize,
                subCategoriesDtos.CurrentPage,
                subCategoriesDtos.TotalPages,
                subCategoriesDtos.HasNext,
                subCategoriesDtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(subCategoriesDtos);
        }

        [HttpPost("{categoryId}", Name = "CreateSubCategory")]
        public ActionResult CreateSubCategory(Guid categoryId, SubCategoryCreateDto subCategoryCreateDto)
        {
            Console.WriteLine($"--> Creating SubCategory for Category with Id: {categoryId}...");

            if (!_repository.IsCategoryExists(categoryId))
            {
                return NotFound();
            }

            var subCategory = _mapper.Map<SubCategory>(subCategoryCreateDto);
            _repository.CreateSubCategory(categoryId, subCategory);
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

            var subCategory = _repository.GetSubCategoryById(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _repository.DeleteSubCategory(subCategory);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateSubCategory")]
        public ActionResult UpdateSubCategory(Guid id, CategoryCreateDto subCategoryCreateDto)
        {
            Console.WriteLine($"--> Updating SubCategory with Id: {id}");

            var subCategory = _repository.GetSubCategoryById(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _mapper.Map(subCategoryCreateDto, subCategory);
            _repository.UpdateSubCategory(subCategory);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}

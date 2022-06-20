using AutoMapper;
using CatalogService.DTOs.Category;
using CatalogService.DTOs.SubCategory;
using CatalogService.Models;
using CatalogService.Pages;
using CatalogService.Repos.Interfaces;
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

        [HttpGet("[action]", Name = "GetSubCategoriesByPages")]
        public ActionResult<ICollection<SubCategoryReadDto>> GetSubCategoriesByPages([FromQuery] PageParams parameters)
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

        [HttpGet("[action]", Name = "GetSubCategoryById")]
        public ActionResult<SubCategoryReadDto> GetSubCategoryById([FromQuery] Guid id)
        {
            Console.WriteLine($"--> Getting SubCategory by Id: {id}...");

            var subCategory = _repository.GetSubCategoryById(id);
            if (subCategory != null)
            {
                return Ok(_mapper.Map<SubCategoryReadDto>(subCategory));
            }

            return NotFound();
        }

        [HttpGet("[action]", Name = "GetSubCategoryInCategory")]
        public ActionResult<ICollection<SubCategoryReadDto>> GetSubCategoriesInCategory(
            [FromQuery] Guid categoryId, [FromQuery] PageParams parameters)
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

        [HttpPost("[action]", Name = "CreateSubCategoryInCategory")]
        public ActionResult CreateSubCategoryInCategory(
            [FromQuery] Guid categoryId, [FromBody] SubCategoryCreateDto subCategoryCreateDto)
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

        [HttpDelete("[action]", Name = "DeleteSubCategory")]
        public ActionResult DeleteSubCategory([FromQuery] Guid id)
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

        [HttpPut("[action]", Name = "UpdateSubCategory")]
        public ActionResult UpdateSubCategory([FromQuery] Guid id, [FromBody] SubCategoryCreateDto subCategoryCreateDto)
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
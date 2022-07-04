using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;
using WebMart.Extensions.DTOs.SubCategory;
using WebMart.Extensions.Pages;

namespace WebMart.Microservices.CatalogService.Controllers
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
        public ActionResult<ICollection<SubCategoryReadDto>> GetSubCategories([FromQuery] PageParams parameters)
        {
            Console.WriteLine("--> Getting SubСategories by pages...");

            var subCategories = _repository.GetAllSubCategories();

            var subCategoriesDtosPaged = PagedList<SubCategoryReadDto>.ToPagedList(
                _mapper.Map<ICollection<SubCategoryReadDto>>(subCategories),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new{
                subCategoriesDtosPaged.TotalCount,
                subCategoriesDtosPaged.PageSize,
                subCategoriesDtosPaged.CurrentPage,
                subCategoriesDtosPaged.TotalPages,
                subCategoriesDtosPaged.HasNext,
                subCategoriesDtosPaged.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

	        return Ok(subCategoriesDtosPaged);
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

            var subCategoriesDtosPaged = PagedList<SubCategoryReadDto>.ToPagedList(
                _mapper.Map<ICollection<SubCategoryReadDto>>(subCategories),
                parameters.PageNumber,
                parameters.PageSize
            );

            var meta = new{
                subCategoriesDtosPaged.TotalCount,
                subCategoriesDtosPaged.PageSize,
                subCategoriesDtosPaged.CurrentPage,
                subCategoriesDtosPaged.TotalPages,
                subCategoriesDtosPaged.HasNext,
                subCategoriesDtosPaged.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(subCategoriesDtosPaged);
        }

        [HttpPost("[action]", Name = "CreateSubCategoryInCategory")]
        public ActionResult CreateSubCategory([FromBody] SubCategoryCreateDto subCategoryCreateDto)
        {
            Console.WriteLine($"--> Creating SubCategory...");

            if (!_repository.IsCategoryExists(subCategoryCreateDto.CategoryId))
            {
                return NotFound();
            }

            var subCategory = _mapper.Map<SubCategory>(subCategoryCreateDto);
            _repository.CreateSubCategory(subCategory);
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
        public ActionResult UpdateSubCategory([FromQuery] Guid id, [FromBody] SubCategoryUpdateDto subCategoryUpdateDto)
        {
            Console.WriteLine($"--> Updating SubCategory with Id: {id}");

            var subCategory = _repository.GetSubCategoryById(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _mapper.Map(subCategoryUpdateDto, subCategory);
            _repository.UpdateSubCategory(subCategory);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
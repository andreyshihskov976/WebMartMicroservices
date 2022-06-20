using AutoMapper;
using CatalogService.DTOs.Category;
using CatalogService.DTOs.Product;
using CatalogService.DTOs.SubCategory;
using CatalogService.Models;

namespace CatalogService.MapperProfiles
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            //Source -> Target
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();

            CreateMap<SubCategory, SubCategoryReadDto>();
            CreateMap<SubCategoryCreateDto, SubCategory>();

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
        }
    }
}
using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;

namespace CatalogService.Profiles
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

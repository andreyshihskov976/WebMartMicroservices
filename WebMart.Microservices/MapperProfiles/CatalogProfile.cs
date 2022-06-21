using AutoMapper;
using WebMart.Microservices.DTOs.Category;
using WebMart.Microservices.DTOs.Product;
using WebMart.Microservices.DTOs.SubCategory;
using WebMart.Microservices.Models;

namespace WebMart.Microservices.MapperProfiles
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
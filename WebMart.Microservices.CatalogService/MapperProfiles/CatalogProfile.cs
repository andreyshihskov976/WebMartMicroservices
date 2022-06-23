using AutoMapper;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.Extensions.DTOs.Category;
using WebMart.Microservices.Extensions.DTOs.Product;
using WebMart.Microservices.Extensions.DTOs.SubCategory;

namespace WebMart.Microservices.CatalogService.MapperProfiles
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            //Source -> Target
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryDetailedReadDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<SubCategory, SubCategoryReadDto>();
            CreateMap<SubCategoryCreateDto, SubCategory>();
            CreateMap<SubCategory, SubCategoryDetailedReadDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductReadDto, ProductPublishedDto>();
            CreateMap<Product, ProductDetailedReadDto>()
                .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => src.SubCategory));
        }
    }
}
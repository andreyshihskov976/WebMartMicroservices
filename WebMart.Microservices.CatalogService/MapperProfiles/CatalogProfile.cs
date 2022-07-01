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

            CreateMap<SubCategory, SubCategoryReadDto>();
            CreateMap<SubCategoryCreateDto, SubCategory>();

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductDetailedReadDto>()
                .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => src.SubCategory))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.SubCategory.Category));

            CreateMap<Product, ProductPublishedDto>();
        }
    }
}
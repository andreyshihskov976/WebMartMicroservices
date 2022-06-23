using AutoMapper;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.DTOs.Product;

namespace WebMart.Microservices.BasketService.MapperProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            //Source -> Target
            CreateMap<Basket, BasketReadDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
            CreateMap<BasketCreateDto, Basket>();
            CreateMap<BasketReadDto, BasketPublishedDto>();
            CreateMap<Basket, BasketDetailedReadDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, BasketProductReadDto>();

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
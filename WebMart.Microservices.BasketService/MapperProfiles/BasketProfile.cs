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
            CreateMap<Basket, BasketReadDto>();
            CreateMap<BasketCreateDto, Basket>();
            CreateMap<BasketReadDto, BasketPublishedDto>();
            CreateMap<Basket, BasketPublishedDto>();
            CreateMap<Basket, BasketDetailedReadDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

            CreateMap<Product, BasketProductReadDto>();

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>();
        }
    }
}
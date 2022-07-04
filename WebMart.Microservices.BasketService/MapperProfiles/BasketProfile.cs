using AutoMapper;
using WebMart.Microservices.BasketService.Models;
using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.DTOs.Product;

namespace WebMart.Microservices.BasketService.MapperProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            //Source -> Target
            CreateMap<Basket, BasketReadDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Products.Sum(p => p.Price)));
            CreateMap<BasketCreateDto, Basket>();
            CreateMap<Basket, BasketPublishedDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Products.Sum(p => p.Price)));

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>();
        }
    }
}
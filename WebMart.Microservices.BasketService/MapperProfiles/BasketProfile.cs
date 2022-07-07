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
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Product.Price * src.Count));
            CreateMap<BasketCreateDto, Basket>();
            CreateMap<BasketUpdateDto, Basket>();

            CreateMap<Basket, BasketPublishedDto>()
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Product.Price * src.Count));

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>();
        }
    }
}
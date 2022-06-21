using AutoMapper;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.DTOs.Product;
using WebMart.Microservices.Extensions.DTOs.TakenProduct;

namespace WebMart.Microservices.BasketService.MapperProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            //Source -> Target
            CreateMap<Basket, BasketReadDto>();
            CreateMap<BasketCreateDto, Basket>();

            CreateMap<TakenProduct, TakenProductReadDto>();
            CreateMap<TakenProductCreateDto, TakenProduct>();

            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
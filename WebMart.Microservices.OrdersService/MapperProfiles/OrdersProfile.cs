using AutoMapper;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.Extensions.DTOs.Order;
using WebMart.Microservices.Extensions.DTOs.Basket;

namespace WebMart.Microservices.BasketService.MapperProfiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            //Source -> Target
            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.Basket, opt => opt.MapFrom(src => src.Basket));
            CreateMap<OrderCreateDto, Order>();
            CreateMap<Order, OrderPublishedDto>();

            CreateMap<Basket, BasketReadDto>();
            CreateMap<BasketPublishedDto, Basket>();
        }
    }
}
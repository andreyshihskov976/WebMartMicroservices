using AutoMapper;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Extensions.DTOs.Order;
using WebMart.Extensions.DTOs.Basket;

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
            CreateMap<OrderUpdateDto, Order>();
            CreateMap<Order, OrderPublishedDto>();

            CreateMap<Basket, OrderBasketReadDto>();
            CreateMap<BasketPublishedDto, Basket>();
        }
    }
}
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
            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderReadDto, OrderPublishedDto>();
            CreateMap<Order, OrderDetailedReadDto>();

            CreateMap<Basket, BasketReadDto>();
            CreateMap<BasketPublishedDto, Basket>();
        }
    }
}
using AutoMapper;
using System.Text.Json;
using WebMart.Microservices.Extensions.DTOs.Events;
using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.Enums;
using WebMart.Microservices.OrdersService.Repos.Interfaces;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.Extensions.EventProcessing;

namespace WebMart.Microservices.OrdersService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.BasketAdded:
                    AddBasket(message);
                    break;
                case EventType.BasketDeleted:
                    DeleteBasket(message);
                    break;
                    case EventType.BasketUpdated:
                    UpdateBasket(message);
                    break;
                default:
                    Console.WriteLine($"There is no option for {eventType} event");
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            Console.WriteLine($"--> {eventType.Event} event was detected");

            return eventType.Event;
        }

        private void AddBasket(string basketPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBasketRepo>();

                var basketPublishedDto = JsonSerializer.Deserialize<BasketPublishedDto>(basketPublishedMessage);

                try
                {
                    var basket = _mapper.Map<Basket>(basketPublishedDto);
                    if (!repo.BasketExists(basket.Id))
                    {
                        repo.CreateBasket(basket);
                        repo.SaveChanges();
                        Console.WriteLine("--> Basket added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Basket already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Basket to DB: {ex.Message}");
                }
            }
        }

        private void DeleteBasket(string basketPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBasketRepo>();

                var basketPublishedDto = JsonSerializer.Deserialize<BasketPublishedDto>(basketPublishedMessage);

                try
                {
                    var basket = _mapper.Map<Basket>(basketPublishedDto);
                    if (repo.BasketExists(basket.Id))
                    {
                        repo.DeleteBasket(basket);
                        repo.SaveChanges();
                        Console.WriteLine("--> Basket deleted!");
                    }
                    else
                    {
                        Console.WriteLine("--> Basket is already not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete Basket from DB: {ex.Message}");
                }
            }
        }

        private void UpdateBasket(string basketPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBasketRepo>();

                var basketPublishedDto = JsonSerializer.Deserialize<BasketPublishedDto>(basketPublishedMessage);

                try
                {
                    var basket = _mapper.Map<Basket>(basketPublishedDto);
                    if (repo.BasketExists(basket.Id))
                    {
                        repo.UpdateBasket(basket);
                        repo.SaveChanges();
                        Console.WriteLine("--> Basket updated!");
                    }
                    else
                    {
                        Console.WriteLine("--> Basket is not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update Basket to DB: {ex.Message}");
                }
            }
        }
    }
}

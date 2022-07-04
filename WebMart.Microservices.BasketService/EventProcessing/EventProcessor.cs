using AutoMapper;
using System.Text.Json;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Extensions.DTOs.Events;
using WebMart.Extensions.DTOs.Product;
using WebMart.Extensions.DTOs.Order;
using WebMart.Extensions.EventProcessing;
using WebMart.Extensions.Enums;

namespace WebMart.Microservices.BasketService.EventProcessing
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
                case EventType.ProductAdded:
                    AddProduct(message);
                    break;
                case EventType.ProductUpdated:
                    UpdateProduct(message);
                    break;
                case EventType.ProductDeleted:
                    DeleteProduct(message);
                    break;
                case EventType.OrderAdded:
                    UpdateBasket(message, isOrdered: true);
                    break;
                case EventType.OrderDeleted:
                    UpdateBasket(message, isOrdered: false);
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

        private void AddProduct(string productPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var productPublishedDto = JsonSerializer.Deserialize<ProductPublishedDto>(productPublishedMessage);

                try
                {
                    var product = _mapper.Map<Product>(productPublishedDto);
                    if (!repo.ProductExists(product.Id))
                    {
                        repo.CreateProduct(product);
                        repo.SaveChanges();
                        Console.WriteLine("--> Product added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Product already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }

        private void DeleteProduct(string productPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var productPublishedDto = JsonSerializer.Deserialize<ProductPublishedDto>(productPublishedMessage);

                try
                {
                    var product = repo.GetProductById(productPublishedDto.Id);
                    if (product != null)
                    {
                        repo.DeleteProduct(product);
                        repo.SaveChanges();
                        Console.WriteLine("--> Product deleted!");
                    }
                    else
                    {
                        Console.WriteLine("--> Product already not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete Product from DB: {ex.Message}");
                }
            }
        }

        private void UpdateProduct(string productPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var productPublishedDto = JsonSerializer.Deserialize<ProductPublishedDto>(productPublishedMessage);

                try
                {
                    var productInRepo = repo.GetProductById(productPublishedDto.Id);
                    if (productInRepo != null)
                    {
                        _mapper.Map(productPublishedDto, productInRepo);
                        repo.UpdateProduct(productInRepo);
                        repo.SaveChanges();
                        Console.WriteLine("--> Product updated!");
                    }
                    else
                    {
                        Console.WriteLine("--> Product does not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update Product in DB: {ex.Message}");
                }
            }
        }

        private void UpdateBasket(string orderPublishedMessage, bool isOrdered)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBasketRepo>();

                var orderPublishedDto = JsonSerializer.Deserialize<OrderPublishedDto>(orderPublishedMessage);

                try
                {
                    var basketInRepo = repo.GetBasketById(orderPublishedDto.BasketId);
                    if (basketInRepo != null)
                    {
                        basketInRepo.IsClosed = isOrdered;
                        repo.UpdateBasket(basketInRepo);
                        repo.SaveChanges();
                        Console.WriteLine("--> Basket updated!");
                    }
                    else
                    {
                        Console.WriteLine("--> Basket does not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update Basket in DB: {ex.Message}");
                }
            }
        }
    }
}

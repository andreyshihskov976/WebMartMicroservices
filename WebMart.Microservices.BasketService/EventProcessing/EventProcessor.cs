using AutoMapper;
using System.Text.Json;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Microservices.Extensions.DTOs.Events;
using WebMart.Microservices.Extensions.DTOs.Product;
using WebMart.Microservices.Extensions.EventProcessing;

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
                case EventType.ProductModified:
                    UpdateProduct(message);
                    break;
                case EventType.ProductDeleted:
                    DeleteProduct(message);
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
                    if (!repo.ExternalProductExists(product.ExternalId))
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
                    var product = repo.GetProductByExternalId(productPublishedDto.Id);
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
                    var productInRepo = repo.GetProductByExternalId(productPublishedDto.Id);
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

    }
}

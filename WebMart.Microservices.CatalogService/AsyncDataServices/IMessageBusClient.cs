using WebMart.Microservices.Extensions.DTOs.Product;

namespace WebMart.Microservices.CatalogService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewProduct(ProductPublishedDto productPublishedDto);

    }
}

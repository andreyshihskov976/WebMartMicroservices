using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface ITakenProductRepo
    {
        bool SaveChanges();
        ICollection<TakenProduct> GetAllTakenProducts();
        ICollection<TakenProduct> GetAllTakenProductsByBasketId(Guid basketId);
        TakenProduct GetTakenProductById(Guid takenProductId);
        void CreateTakenProduct(TakenProduct takenProduct);
        void DeleteTakenProduct(TakenProduct takenProduct);
        bool TakenProductExists(Guid takenProductId);
    }
}
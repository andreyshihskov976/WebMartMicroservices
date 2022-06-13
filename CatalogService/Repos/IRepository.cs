namespace CatalogService.Repos
{
    public interface IRepository<T>
    {
        bool SaveChanges();
        IEnumerable<T> GetAllEntities();
        T GetEntityById(Guid entityId);
        void CreateEntity(T entity);
        void DeleteEntity(T entity);
        void UpdateEntity(T entity);
    }
}

using InventoryApp.Domain;

namespace InventoryApp.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> SearchByNameAsync(string name);
        Task<bool> UpdateStockAsync(int productId, int newStock);
    }
}

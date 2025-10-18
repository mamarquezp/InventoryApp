using InventoryApp.Domain;

namespace InventoryApp.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByNitAsync(string nit);
    }
}

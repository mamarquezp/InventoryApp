using InventoryApp.Domain;

namespace InventoryApp.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByNitAsync(string nit);
        Task<Client?> GetByEmailAsync(string correo);
        Task<List<Client>> SearchByNameAsync(string nombre);
    }
}

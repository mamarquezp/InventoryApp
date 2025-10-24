using InventoryApp.Domain;

namespace InventoryApp.Repositories
{
    public interface ISaleRepository
    {
        Task<int> InsertSaleAsync(MySql.Data.MySqlClient.MySqlConnection con,
                                  MySql.Data.MySqlClient.MySqlTransaction tx,
                                  Sale sale);
        Task InsertSaleDetailAsync(MySql.Data.MySqlClient.MySqlConnection con,
                                   MySql.Data.MySqlClient.MySqlTransaction tx,
                                   SaleDetail detail);
        Task<List<SaleMasterView>> GetSalesMasterAsync(DateTime? from, DateTime? to, int? clientId);
        Task<List<SaleDetailView>> GetSaleDetailsAsync(int saleId);
    }
}

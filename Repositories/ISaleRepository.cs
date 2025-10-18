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
    }
}

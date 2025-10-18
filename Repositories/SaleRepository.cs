using InventoryApp.Domain;
using MySql.Data.MySqlClient;

namespace InventoryApp.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        public async Task<int> InsertSaleAsync(MySqlConnection con, MySqlTransaction tx, Sale sale)
        {
            using var cmd = new MySqlCommand(
                "INSERT INTO venta (cliente_id, fecha, total) VALUES (@c, @f, @t); SELECT LAST_INSERT_ID();",
                con, tx);
            cmd.Parameters.AddWithValue("@c", sale.ClienteId);
            cmd.Parameters.AddWithValue("@f", sale.Fecha);
            cmd.Parameters.AddWithValue("@t", sale.Total);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task InsertSaleDetailAsync(MySqlConnection con, MySqlTransaction tx, SaleDetail d)
        {
            using var cmd = new MySqlCommand(
                @"INSERT INTO detalle_venta (venta_id, producto_id, cantidad, precio_unit, subtotal)
              VALUES (@v, @p, @c, @pu, @s)", con, tx);
            cmd.Parameters.AddWithValue("@v", d.VentaId);
            cmd.Parameters.AddWithValue("@p", d.ProductoId);
            cmd.Parameters.AddWithValue("@c", d.Cantidad);
            cmd.Parameters.AddWithValue("@pu", d.PrecioUnit);
            cmd.Parameters.AddWithValue("@s", d.Subtotal);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

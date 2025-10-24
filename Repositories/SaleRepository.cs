using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

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

        public async Task<List<SaleMasterView>> GetSalesMasterAsync(DateTime? from, DateTime? to, int? clientId)
        {
            var list = new List<SaleMasterView>();
            using var con = DbConnectionFactory.Instance.CreateOpen();

            var sql = new StringBuilder(
                @"SELECT v.id, v.fecha, c.nombre AS cliente_nombre, v.total 
                  FROM venta v
                  JOIN cliente c ON v.cliente_id = c.id
                  WHERE 1=1 ");

            if (from.HasValue)
                sql.Append(" AND v.fecha >= @from ");
            if (to.HasValue)
                sql.Append(" AND v.fecha <= @to ");
            if (clientId.HasValue)
                sql.Append(" AND v.cliente_id = @cid ");

            sql.Append(" ORDER BY v.fecha DESC ");

            using var cmd = new MySqlCommand(sql.ToString(), con);

            if (from.HasValue)
                cmd.Parameters.AddWithValue("@from", from.Value.Date);
            if (to.HasValue)
                cmd.Parameters.AddWithValue("@to", to.Value.Date.AddDays(1).AddTicks(-1)); // Hasta el final del día
            if (clientId.HasValue)
                cmd.Parameters.AddWithValue("@cid", clientId.Value);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new SaleMasterView
                {
                    VentaId = rd.GetInt32("id"),
                    Fecha = rd.GetDateTime("fecha"),
                    Cliente = rd.GetString("cliente_nombre"),
                    Total = rd.GetDecimal("total")
                });
            }
            return list;
        }

        public async Task<List<SaleDetailView>> GetSaleDetailsAsync(int saleId)
        {
            var list = new List<SaleDetailView>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            const string sql =
                @"SELECT d.producto_id, p.nombre AS producto_nombre, d.cantidad, d.precio_unit, d.subtotal
                  FROM detalle_venta d
                  JOIN producto p ON d.producto_id = p.id
                  WHERE d.venta_id = @vid";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@vid", saleId);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new SaleDetailView
                {
                    ProductoId = rd.GetInt32("producto_id"),
                    Producto = rd.GetString("producto_nombre"),
                    Cantidad = rd.GetInt32("cantidad"),
                    PrecioUnit = rd.GetDecimal("precio_unit"),
                    Subtotal = rd.GetDecimal("subtotal")
                });
            }
            return list;
        }
    }
}

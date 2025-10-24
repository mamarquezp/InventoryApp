using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace InventoryApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetAllAsync()
        {
            var list = new List<Product>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, precio, stock FROM producto ORDER BY id", con);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Product
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Precio = rd.GetDecimal("precio"),
                    Stock = rd.GetInt32("stock")
                });
            }
            return list;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, precio, stock FROM producto WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Product
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Precio = rd.GetDecimal("precio"),
                    Stock = rd.GetInt32("stock")
                };
            }
            return null;
        }


        public async Task<int> InsertAsync(Product p)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "INSERT INTO producto (nombre, precio, stock) VALUES (@n, @p, @s); SELECT LAST_INSERT_ID();", con);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@p", p.Precio);
            cmd.Parameters.AddWithValue("@s", p.Stock);
            object? id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task<bool> UpdateAsync(Product p)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "UPDATE producto SET nombre=@n, precio=@p, stock=@s WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@p", p.Precio);
            cmd.Parameters.AddWithValue("@s", p.Stock);
            cmd.Parameters.AddWithValue("@id", p.Id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("DELETE FROM producto WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            var list = new List<Product>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "SELECT id, nombre, precio, stock FROM producto WHERE nombre LIKE @q ORDER BY nombre", con);
            cmd.Parameters.AddWithValue("@q", $"%{name}%");
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Product
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Precio = rd.GetDecimal("precio"),
                    Stock = rd.GetInt32("stock")
                });
            }
            return list;
        }

        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("UPDATE producto SET stock=@s WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@s", newStock);
            cmd.Parameters.AddWithValue("@id", productId);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
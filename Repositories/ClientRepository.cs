using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace InventoryApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public async Task<List<Client>> GetAllAsync()
        {
            var list = new List<Client>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, nit FROM cliente ORDER BY nombre", con);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit")
                });
            }
            return list;
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, nit FROM cliente WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
                return new Client { Id = rd.GetInt32("id"), Nombre = rd.GetString("nombre"), Nit = rd.GetString("nit") };
            return null;
        }

        public async Task<int> InsertAsync(Client c)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "INSERT INTO cliente (nombre, nit) VALUES (@n, @nit); SELECT LAST_INSERT_ID();", con);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@nit", c.Nit);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<bool> UpdateAsync(Client c)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("UPDATE cliente SET nombre=@n, nit=@nit WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@nit", c.Nit);
            cmd.Parameters.AddWithValue("@id", c.Id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("DELETE FROM cliente WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Client?> GetByNitAsync(string nit)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, nit FROM cliente WHERE nit=@nit", con);
            cmd.Parameters.AddWithValue("@nit", nit);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
                return new Client { Id = rd.GetInt32("id"), Nombre = rd.GetString("nombre"), Nit = rd.GetString("nit") };
            return null;
        }
    }
}

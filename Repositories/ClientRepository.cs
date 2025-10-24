using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;

namespace InventoryApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        // Expresión regular para validar formato de correo electrónico
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public async Task<List<Client>> GetAllAsync()
        {
            var list = new List<Client>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "SELECT id, nombre, nit, correo, telefono, direccion FROM cliente ORDER BY nombre", con);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit"),
                    Correo = rd.IsDBNull(rd.GetOrdinal("correo")) ? "" : rd.GetString("correo"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion")
                });
            }
            return list;
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "SELECT id, nombre, nit, correo, telefono, direccion FROM cliente WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit"),
                    Correo = rd.IsDBNull(rd.GetOrdinal("correo")) ? "" : rd.GetString("correo"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion")
                };
            }
            return null;
        }

        public async Task<int> InsertAsync(Client c)
        {
            // Validaciones
            ValidateClient(c);

            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                @"INSERT INTO cliente (nombre, nit, correo, telefono, direccion) 
                  VALUES (@n, @nit, @correo, @tel, @dir); 
                  SELECT LAST_INSERT_ID();", con);

            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@nit", c.Nit);
            cmd.Parameters.AddWithValue("@correo", c.Correo);
            cmd.Parameters.AddWithValue("@tel", c.Telefono);
            cmd.Parameters.AddWithValue("@dir", c.Direccion);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<bool> UpdateAsync(Client c)
        {
            // Validaciones
            ValidateClient(c);

            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                @"UPDATE cliente 
                  SET nombre=@n, nit=@nit, correo=@correo, telefono=@tel, direccion=@dir 
                  WHERE id=@id", con);

            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@nit", c.Nit);
            cmd.Parameters.AddWithValue("@correo", c.Correo);
            cmd.Parameters.AddWithValue("@tel", c.Telefono);
            cmd.Parameters.AddWithValue("@dir", c.Direccion);
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
            using var cmd = new MySqlCommand(
                "SELECT id, nombre, nit, correo, telefono, direccion FROM cliente WHERE nit=@nit", con);
            cmd.Parameters.AddWithValue("@nit", nit);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit"),
                    Correo = rd.IsDBNull(rd.GetOrdinal("correo")) ? "" : rd.GetString("correo"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion")
                };
            }
            return null;
        }

        public async Task<Client?> GetByEmailAsync(string correo)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "SELECT id, nombre, nit, correo, telefono, direccion FROM cliente WHERE correo=@correo", con);
            cmd.Parameters.AddWithValue("@correo", correo);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit"),
                    Correo = rd.IsDBNull(rd.GetOrdinal("correo")) ? "" : rd.GetString("correo"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion")
                };
            }
            return null;
        }

        public async Task<List<Client>> SearchByNameAsync(string nombre)
        {
            var list = new List<Client>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                @"SELECT id, nombre, nit, correo, telefono, direccion 
                  FROM cliente 
                  WHERE nombre LIKE @q 
                  ORDER BY nombre", con);
            cmd.Parameters.AddWithValue("@q", $"%{nombre}%");
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Nit = rd.GetString("nit"),
                    Correo = rd.IsDBNull(rd.GetOrdinal("correo")) ? "" : rd.GetString("correo"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion")
                });
            }
            return list;
        }

        /// <summary>
        /// Valida los datos del cliente antes de insertar o actualizar
        /// </summary>
        private void ValidateClient(Client c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(c.Correo))
                throw new ArgumentException("El correo electrónico es obligatorio.");

            if (!EmailRegex.IsMatch(c.Correo))
                throw new ArgumentException("El formato del correo electrónico no es válido.");
        }
    }
}

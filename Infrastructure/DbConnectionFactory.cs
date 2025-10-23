
using MySql.Data.MySqlClient;

namespace InventoryApp.Infrastructure
{
    public sealed class DbConnectionFactory
    {
        private static readonly Lazy<DbConnectionFactory> _instance =
            new(() => new DbConnectionFactory());

        public static DbConnectionFactory Instance => _instance.Value;

        // Ajusta tu cadena. Ideal: léela de app.config/user-secrets.
        private readonly string _connString =
            "Server=localhost;Port=3306;Database=inventario_db;Uid=root;Pwd=mar41662137;SslMode=none;";

        private DbConnectionFactory() { }

        /// Crea una conexión nueva y abierta (usa connection pooling de MySQL).
        public MySqlConnection CreateOpen()
        {
            var conn = new MySqlConnection(_connString);
            conn.Open();
            return conn;
        }
    }
}

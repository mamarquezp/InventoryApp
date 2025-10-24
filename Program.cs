using InventoryApp.Repositories;
using InventoryApp.Services;
using InventoryApp.WinForms;

namespace InventoryApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();

            // 🔹 Instanciar repositorios
            var productRepo = new ProductRepository();
            var clientRepo = new ClientRepository();
            var saleRepo = new SaleRepository();

            // 🔹 Crear el servicio con los repositorios
            var salesService = new SalesService(productRepo, saleRepo);

            // 🔹 Ejecutar el formulario principal pasando las dependencias
            Application.Run(new MainForm(productRepo, clientRepo, salesService, saleRepo));
        }
    }
}
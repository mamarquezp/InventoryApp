using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using InventoryApp.Repositories;
using MySql.Data.MySqlClient;

namespace InventoryApp.Services
{
    public class SalesService
    {
        private readonly IProductRepository _productRepo;
        private readonly ISaleRepository _saleRepo;

        public SalesService(IProductRepository productRepo, ISaleRepository saleRepo)
        {
            _productRepo = productRepo;
            _saleRepo = saleRepo;
        }

        public async Task<int> CreateSaleAsync(int clienteId, IEnumerable<SaleItem> items)
        {
            // Validaciones de negocio
            if (!items.Any()) throw new InvalidOperationException("La venta no tiene ítems.");

            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var tx = await con.BeginTransactionAsync();

            try
            {
                // Verificar stock y calcular total
                decimal total = 0m;
                foreach (var it in items)
                {
                    var p = await _productRepo.GetByIdAsync(it.ProductoId)
                             ?? throw new InvalidOperationException($"Producto {it.ProductoId} no existe.");
                    if (it.Cantidad <= 0) throw new InvalidOperationException("Cantidad inválida.");
                    if (p.Stock < it.Cantidad)
                        throw new InvalidOperationException($"Stock insuficiente para {p.Nombre}. Disponible: {p.Stock}");

                    total += it.Cantidad * p.Precio;
                }

                // Insertar cabecera
                var sale = new Sale { ClienteId = clienteId, Fecha = DateTime.Now, Total = total };
                int saleId = await _saleRepo.InsertSaleAsync(con, (MySqlTransaction)tx, sale);

                // Insertar detalles + actualizar stock
                foreach (var it in items)
                {
                    var detail = new SaleDetail
                    {
                        VentaId = saleId,
                        ProductoId = it.ProductoId,
                        Cantidad = it.Cantidad,
                        PrecioUnit = it.PrecioUnit == 0 ? it.Subtotal / Math.Max(1, it.Cantidad) : it.PrecioUnit
                    };
                    await _saleRepo.InsertSaleDetailAsync(con, (MySqlTransaction)tx, detail);

                    // Actualiza stock (seguro dentro de la misma transacción)
                    using var cmd = new MySqlCommand(
                        "UPDATE producto SET stock = stock - @q WHERE id=@id AND stock >= @q", con, (MySqlTransaction)tx);
                    cmd.Parameters.AddWithValue("@q", it.Cantidad);
                    cmd.Parameters.AddWithValue("@id", it.ProductoId);
                    int rows = await cmd.ExecuteNonQueryAsync();
                    if (rows == 0)
                        throw new InvalidOperationException("No se pudo actualizar stock (condición de carrera).");
                }

                await tx.CommitAsync();
                return saleId;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
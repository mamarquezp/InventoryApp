namespace InventoryApp.Domain
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnit { get; set; }
        public decimal Subtotal => PrecioUnit * Cantidad;
    }
}

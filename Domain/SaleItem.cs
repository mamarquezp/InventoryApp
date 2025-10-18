namespace InventoryApp.Domain
{
    public class SaleItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal PrecioUnit { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnit * Cantidad;
    }
}

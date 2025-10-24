using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Domain
{
    public class SaleMasterView // DTO para el grid Maestro de Ventas
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = "";
        public decimal Total { get; set; }
    }
}

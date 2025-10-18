using InventoryApp.Domain;
using InventoryApp.Repositories;
using InventoryApp.Services;
using System.Data;

namespace InventoryApp.WinForms
{
    public partial class NewSaleInlineForm : Form
    {
        private readonly IProductRepository _productRepo;
        private readonly IClientRepository _clientRepo;
        private readonly SalesService _salesService;

        private DataTable _cart = new();
        private List<Product> _products = new();

        public NewSaleInlineForm(IProductRepository productRepo, IClientRepository clientRepo, SalesService salesService)
        {
            InitializeComponent();
            _productRepo = productRepo;
            _clientRepo = clientRepo;
            _salesService = salesService;
        }

        private async void NewSaleInlineForm_Load(object sender, EventArgs e)
        {
            // ---------- Clientes ----------
            var clients = await _clientRepo.GetAllAsync();
            cmbCliente.DataSource = clients;
            cmbCliente.DisplayMember = "Nombre";
            cmbCliente.ValueMember = "Id";

            // ---------- Productos ----------
            _products = await _productRepo.GetAllAsync();

            // ---------- DataTable Carrito ----------
            _cart = new DataTable("cart");
            _cart.Columns.Add("ProductoId", typeof(int));
            _cart.Columns.Add("Nombre", typeof(string));
            _cart.Columns.Add("PrecioUnit", typeof(decimal));
            _cart.Columns.Add("Cantidad", typeof(int));
            _cart.Columns.Add("Subtotal", typeof(decimal));

            // ---------- Grid ----------
            gridCart.AutoGenerateColumns = false;
            gridCart.AllowUserToAddRows = true;
            gridCart.AllowUserToDeleteRows = true;
            gridCart.DataSource = _cart;

            // Columna Combo de productos
            var colProd = new DataGridViewComboBoxColumn
            {
                Name = "colProducto",
                HeaderText = "Producto",
                DataSource = _products,
                DisplayMember = "Nombre",
                ValueMember = "Id",
                DataPropertyName = "ProductoId",
                ValueType = typeof(int),
                Width = 230,
                DisplayStyleForCurrentCellOnly = true
            };
            gridCart.Columns.Add(colProd);

            // Nombre
            gridCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                ReadOnly = true,
                Width = 240
            });

            // Precio
            gridCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrecio",
                HeaderText = "Precio",
                DataPropertyName = "PrecioUnit",
                DefaultCellStyle = { Format = "N2" },
                Width = 100
            });

            // Cantidad
            gridCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCant",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 90
            });

            // Subtotal
            gridCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSub",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                ReadOnly = true,
                DefaultCellStyle = { Format = "N2" },
                Width = 110
            });

            // --- Eventos ---
            gridCart.CurrentCellDirtyStateChanged += (s, ev) =>
            {
                if (gridCart.IsCurrentCellDirty)
                    gridCart.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            gridCart.CellValueChanged += gridCart_CellValueChanged;
            gridCart.CellEndEdit += (s, ev2) => UpdateTotal();

            _cart.RowChanged += (s, ev2) => UpdateTotal();
            _cart.RowDeleted += (s, ev2) => UpdateTotal();

            UpdateTotal();
        }

        private void gridCart_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = gridCart.Columns[e.ColumnIndex];
            if (col == null) return;

            var rowView = gridCart.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null) return;

            var row = rowView.Row;

            // Cuando se selecciona un producto
            if (col.Name == "colProducto" && row["ProductoId"] != DBNull.Value)
            {
                int prodId = Convert.ToInt32(row["ProductoId"]);
                var prod = _products.FirstOrDefault(p => p.Id == prodId);
                if (prod == null) return;

                row["Nombre"] = prod.Nombre;
                row["PrecioUnit"] = prod.Precio;
                if (row["Cantidad"] == DBNull.Value || Convert.ToInt32(row["Cantidad"]) <= 0)
                    row["Cantidad"] = 1;
            }

            // Recalcular subtotal (por producto o cantidad o precio)
            if (col.Name is "colCant" or "colPrecio" or "colProducto")
            {
                if (row["Cantidad"] == DBNull.Value) row["Cantidad"] = 0;
                if (row["PrecioUnit"] == DBNull.Value) row["PrecioUnit"] = 0m;

                decimal precio = Convert.ToDecimal(row["PrecioUnit"]);
                int cantidad = Convert.ToInt32(row["Cantidad"]);
                row["Subtotal"] = precio * cantidad;
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0m;
            foreach (DataRow r in _cart.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                if (r["Subtotal"] == DBNull.Value) continue;
                total += Convert.ToDecimal(r["Subtotal"]);
            }
            lblTotal.Text = "Total: Q " + total.ToString("N2");
        }

        private async void btnConfirmarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCliente.SelectedValue is not int clienteId)
                {
                    MessageBox.Show("Seleccione un cliente."); return;
                }
                if (_cart.Rows.Count == 0)
                {
                    MessageBox.Show("Agregue al menos un producto."); return;
                }

                var items = new List<SaleItem>();
                foreach (DataRow r in _cart.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    items.Add(new SaleItem
                    {
                        ProductoId = Convert.ToInt32(r["ProductoId"]),
                        Nombre = r["Nombre"].ToString() ?? "",
                        PrecioUnit = Convert.ToDecimal(r["PrecioUnit"]),
                        Cantidad = Convert.ToInt32(r["Cantidad"])
                    });
                }

                int saleId = await _salesService.CreateSaleAsync(clienteId, items);
                MessageBox.Show($"Venta registrada correctamente (ID={saleId})", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _cart.Clear();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la venta:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

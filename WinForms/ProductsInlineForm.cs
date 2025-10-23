using InventoryApp.Domain;
using InventoryApp.Repositories;
using System.Windows.Forms;

namespace InventoryApp.WinForms
{
    public partial class ProductsInlineForm : Form
    {
        private readonly IProductRepository _productRepo;

        public ProductsInlineForm(IProductRepository productRepo)
        {
            InitializeComponent();
            _productRepo = productRepo;
        }

        private async void ProductsInlineForm_Load(object sender, EventArgs e)
        {
            await LoadProductsAsync();
            SetupDataGridView();
            AgregarBotones(); // Agregar botones Editar y Eliminar
            AplicarEstilosVisuales();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                dgvProducts.Rows.Clear();
                var products = await _productRepo.GetAllAsync();
                foreach (var product in products)
                {
                    int rowIndex = dgvProducts.Rows.Add(
                        product.Id,
                        product.Nombre,
                        product.Precio,
                        product.Stock
                    );
                    dgvProducts.Rows[rowIndex].Tag = product;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dgvProducts.Columns["Id"].ReadOnly = true;
            dgvProducts.Columns["Precio"].DefaultCellStyle.Format = "N2";
            dgvProducts.CellEndEdit += DgvProducts_CellEndEdit;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.UserDeletingRow += DgvProducts_UserDeletingRow;
            dgvProducts.CellValidating += DgvProducts_CellValidating;
        }

        // Botones dentro del DataGridView
        private void AgregarBotones()
        {
            if (dgvProducts.Columns["Editar"] == null)
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.HeaderText = "Editar";
                editButton.Text = "✏️ Editar";
                editButton.Name = "Editar";
                editButton.UseColumnTextForButtonValue = true;
                editButton.Width = 90;
                dgvProducts.Columns.Add(editButton);
            }

            if (dgvProducts.Columns["Eliminar"] == null)
            {
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.HeaderText = "Eliminar";
                deleteButton.Text = "🗑️ Eliminar";
                deleteButton.Name = "Eliminar";
                deleteButton.UseColumnTextForButtonValue = true;
                deleteButton.Width = 100;
                dgvProducts.Columns.Add(deleteButton);
            }
        }

        private async void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvProducts.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                dgvProducts.BeginEdit(true);
                MessageBox.Show("Puedes editar los valores directamente en la fila seleccionada.", "Editar producto",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (colName == "Eliminar")
            {
                var row = dgvProducts.Rows[e.RowIndex];
                if (row.Cells["Id"].Value == null) return;

                int id = Convert.ToInt32(row.Cells["Id"].Value);
                var result = MessageBox.Show($"¿Eliminar el producto ID {id}?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = await _productRepo.DeleteAsync(id);
                    if (success)
                    {
                        dgvProducts.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Producto eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el producto.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void DgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var row = dgvProducts.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                var id = Convert.ToInt32(row.Cells["Id"].Value ?? 0);
                var nombre = row.Cells["Nombre"].Value?.ToString() ?? "";
                var precioStr = row.Cells["Precio"].Value?.ToString();
                var stockStr = row.Cells["Stock"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(precioStr) ||
                    string.IsNullOrWhiteSpace(stockStr))
                {
                    return;
                }

                if (!decimal.TryParse(precioStr, out decimal precioVal) || precioVal < 0)
                {
                    MessageBox.Show("Precio inválido (debe ser un número >= 0).", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(stockStr, out int stockVal) || stockVal < 0)
                {
                    MessageBox.Show("Stock inválido (debe ser un número >= 0).", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (id == 0)
                {
                    var newProduct = new Product { Nombre = nombre, Precio = precioVal, Stock = stockVal };
                    int newId = await _productRepo.InsertAsync(newProduct);
                    row.Cells["Id"].Value = newId;
                    row.Tag = newProduct;
                    MessageBox.Show($"Producto creado exitosamente. ID: {newId}", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var product = new Product { Id = id, Nombre = nombre, Precio = precioVal, Stock = stockVal };
                    bool success = await _productRepo.UpdateAsync(product);
                    if (!success)
                    {
                        MessageBox.Show("Error al actualizar el producto.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    row.Tag = product;
                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvProducts_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row?.Cells["Id"].Value == null) return;
                int id = Convert.ToInt32(e.Row.Cells["Id"].Value);

                var result = MessageBox.Show($"¿Eliminar el producto ID {id}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = await _productRepo.DeleteAsync(id);
                    if (!success)
                    {
                        MessageBox.Show("No se pudo eliminar el producto.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvProducts.Columns[e.ColumnIndex].Name;
            string value = e.FormattedValue?.ToString() ?? "";

            if (columnName == "Nombre" && string.IsNullOrWhiteSpace(value))
            {
                e.Cancel = true;
                dgvProducts.Rows[e.RowIndex].ErrorText = "Nombre obligatorio";
            }
            else if ((columnName == "Precio" || columnName == "Stock") &&
                     (!decimal.TryParse(value, out decimal num) || num < 0))
            {
                e.Cancel = true;
                dgvProducts.Rows[e.RowIndex].ErrorText = "Debe ser >= 0";
            }
            else
            {
                dgvProducts.Rows[e.RowIndex].ErrorText = "";
            }
        }

        // Apartado de colores y eso
        private void AplicarEstilosVisuales()
        {
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Text = "Gestión de Productos";
            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(52, 73, 94);
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;

            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.GridColor = Color.FromArgb(220, 220, 220);
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvProducts.EnableHeadersVisualStyles = false;

            dgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvProducts.DefaultCellStyle.BackColor = Color.White;
            dgvProducts.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }
    }
}

using InventoryApp.Domain;
using InventoryApp.Repositories;
using System.Data;

namespace InventoryApp.WinForms
{
    public partial class ProductsInlineForm : Form
    {
        private readonly IProductRepository _productRepo;

        private DataTable _table = new();
        private readonly BindingSource _bs = new();
        private bool _persisting = false;

        public ProductsInlineForm(IProductRepository productRepo)
        {
            InitializeComponent();
            _productRepo = productRepo;
        }

        private async void ProductsInlineForm_Load(object sender, EventArgs e)
        {
            await LoadTableAsync();
            SetupGrid();
            SetupContextMenu();

            // Validaciones y errores
            dataGridProducts.CellValidating += dataGridProducts_CellValidating;
            dataGridProducts.DataError += (s, ev) => { ev.ThrowException = false; };

            // Persistencia inmediata por celda (más estable que RowValidated)
            dataGridProducts.CellValidated += dataGridProducts_CellValidated;



            // DELETE con tecla Supr
            dataGridProducts.UserDeletingRow += dataGridProducts_UserDeletingRow;
        }

        // ================================
        // Carga de datos
        // ================================
        private async System.Threading.Tasks.Task LoadTableAsync()
        {
            _table = BuildSchema();

            var productos = await _productRepo.GetAllAsync();
            foreach (var p in productos)
            {
                var r = _table.NewRow();
                r["id"] = p.Id;
                r["nombre"] = p.Nombre;
                r["precio"] = p.Precio;
                r["stock"] = p.Stock;
                _table.Rows.Add(r);
            }

            _table.AcceptChanges();
            _bs.DataSource = _table;
            dataGridProducts.DataSource = _bs;
        }

        private static DataTable BuildSchema()
        {
            var t = new DataTable("producto");

            var cId = t.Columns.Add("id", typeof(int));
            cId.AllowDBNull = true;   // filas nuevas sin id
            cId.Unique = false;       // no PK aquí (evita problemas con nulos)

            t.Columns.Add("nombre", typeof(string));
            t.Columns.Add("precio", typeof(decimal));
            t.Columns.Add("stock", typeof(int));

            return t;
        }

        // ================================
        // Configuración visual
        // ================================
        private void SetupGrid()
        {
            dataGridProducts.AutoGenerateColumns = true;
            dataGridProducts.AllowUserToAddRows = true;
            dataGridProducts.AllowUserToDeleteRows = true;
            dataGridProducts.MultiSelect = false;
            dataGridProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridProducts.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridProducts.Columns["id"] is DataGridViewColumn idCol)
            {
                idCol.HeaderText = "ID";
                idCol.ReadOnly = true;  // lo pone la BD
                idCol.Width = 70;
            }
            if (dataGridProducts.Columns["nombre"] is DataGridViewColumn nomCol)
            {
                nomCol.HeaderText = "Nombre";
                nomCol.ReadOnly = false;
            }
            if (dataGridProducts.Columns["precio"] is DataGridViewColumn preCol)
            {
                preCol.HeaderText = "Precio";
                preCol.DefaultCellStyle.Format = "N2";
                preCol.ReadOnly = false;
            }
            if (dataGridProducts.Columns["stock"] is DataGridViewColumn stkCol)
            {
                stkCol.HeaderText = "Stock";
                stkCol.ReadOnly = false;
            }
        }

        // ================================
        // Menú contextual (Eliminar)
        // ================================
        private void SetupContextMenu()
        {
            var ctx = new ContextMenuStrip();
            var miEliminar = new ToolStripMenuItem("Eliminar");

            miEliminar.Click += async (s, ev) =>
            {
                if (dataGridProducts.CurrentRow?.DataBoundItem is not DataRowView drv) return;
                await DeleteRowAsync(drv, confirm: true);
            };

            ctx.Items.Add(miEliminar);
            dataGridProducts.ContextMenuStrip = ctx;
        }

        // ================================
        // Validaciones
        // ================================
        private void dataGridProducts_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dataGridProducts.Columns[e.ColumnIndex].Name;
            var value = e.FormattedValue?.ToString() ?? "";

            if (colName == "nombre")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    e.Cancel = true;
                    dataGridProducts.Rows[e.RowIndex].ErrorText = "El nombre es requerido.";
                }
                else dataGridProducts.Rows[e.RowIndex].ErrorText = string.Empty;
            }
            else if (colName == "precio")
            {
                if (!decimal.TryParse(value, out var d) || d < 0)
                {
                    e.Cancel = true;
                    dataGridProducts.Rows[e.RowIndex].ErrorText = "Precio inválido (>= 0).";
                }
                else dataGridProducts.Rows[e.RowIndex].ErrorText = string.Empty;
            }
            else if (colName == "stock")
            {
                if (!int.TryParse(value, out var q) || q < 0)
                {
                    e.Cancel = true;
                    dataGridProducts.Rows[e.RowIndex].ErrorText = "Stock inválido (>= 0).";
                }
                else dataGridProducts.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        // ================================
        // Persistencia inmediata por celda
        // ================================
        private async void dataGridProducts_CellValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (_persisting) return;
            if (e.RowIndex < 0 || e.RowIndex >= dataGridProducts.Rows.Count) return;

            var gridRow = dataGridProducts.Rows[e.RowIndex];
            if (gridRow.IsNewRow) return;

            // Asegura que lo editado pasó al DataTable
            dataGridProducts.EndEdit();
            _bs.EndEdit();

            if (gridRow.DataBoundItem is not DataRowView drv) return;
            var row = drv.Row;

            // Si la fila está "vacía", no persistimos
            if (IsNullOrEmpty(row, "nombre") &&
                IsNullOrZero(row, "precio") &&
                IsNullOrZero(row, "stock"))
                return;

            try
            {
                _persisting = true;

                // INSERT (id nulo o 0 y fila Added)
                if ((row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                    && IsValidRow(row))
                {
                    var p = new Product
                    {
                        Nombre = row["nombre"]?.ToString() ?? "",
                        Precio = ToDecimal(row["precio"]),
                        Stock = ToInt(row["stock"])
                    };

                    int newId = await _productRepo.InsertAsync(p);

                    row["id"] = newId;
                    row.AcceptChanges(); // sincroniza estados
                    return;
                }

                // UPDATE (id válido y fila Modified)
                if (row.RowState == DataRowState.Modified && IsValidRow(row))
                {
                    int id = ToInt(row["id"]);
                    if (id > 0)
                    {
                        var p = new Product
                        {
                            Id = id,
                            Nombre = row["nombre"]?.ToString() ?? "",
                            Precio = ToDecimal(row["precio"]),
                            Stock = ToInt(row["stock"])
                        };

                        var ok = await _productRepo.UpdateAsync(p);
                        if (ok) row.AcceptChanges();
                        else row.RowError = "No se pudo actualizar en BD.";
                    }
                }
            }
            catch (Exception ex)
            {
                row.RowError = ex.Message;
                MessageBox.Show("Error al persistir: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _persisting = false;
            }
        }

        // ================================
        // DELETE (tecla Supr)
        // ================================
        private async void dataGridProducts_UserDeletingRow(object? sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row?.DataBoundItem is not DataRowView drv) return;

            // Confirmación
            var resp = MessageBox.Show("¿Eliminar este producto?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) { e.Cancel = true; return; }

            // Ejecuta el borrado real (maneja Added/Existente adentro)
            var ok = await DeleteRowAsync(drv, confirm: false);
            if (!ok) e.Cancel = true;
        }

        // Elimina fila (desde menú o tecla) con protecciones
        private async System.Threading.Tasks.Task<bool> DeleteRowAsync(DataRowView drv, bool confirm)
        {
            if (_persisting) return false;

            var row = drv.Row;

            try
            {
                _persisting = true;

                // Si nunca se insertó en BD
                if (row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                {
                    row.Delete(); // solo quita del DataTable
                    return true;
                }

                int id = ToInt(row["id"]);
                if (id <= 0) return false;

                if (confirm)
                {
                    var okConf = MessageBox.Show($"¿Eliminar el producto #{id}?", "Confirmar",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if (!okConf) return false;
                }

                var ok = await _productRepo.DeleteAsync(id);
                if (ok)
                {
                    row.Delete(); // elimina del DataTable
                    return true;
                }

                MessageBox.Show("No se pudo eliminar en BD (¿referenciado en ventas?).",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                _persisting = false;
            }
        }

        // ================================
        // Helpers
        // ================================
        private static bool IsValidRow(DataRow r)
            => !IsNullOrEmpty(r, "nombre") && TryGetDecimal(r, "precio", out _) && TryGetInt(r, "stock", out _);

        private static bool IsNullOrEmpty(DataRow r, string col)
            => !r.Table.Columns.Contains(col) || r[col] == DBNull.Value || string.IsNullOrWhiteSpace(r[col]?.ToString());

        private static bool IsNullOrZero(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return true;
            var s = r[col].ToString();
            if (decimal.TryParse(s, out var d)) return d == 0m;
            if (int.TryParse(s, out var i)) return i == 0;
            return true;
        }

        private static bool TryGetDecimal(DataRow r, string col, out decimal value)
        {
            value = 0m;
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return false;
            return decimal.TryParse(r[col].ToString(), out value);
        }

        private static bool TryGetInt(DataRow r, string col, out int value)
        {
            value = 0;
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return false;
            return int.TryParse(r[col].ToString(), out value);
        }

        private static int ToInt(object? o)
            => o == null || o == DBNull.Value ? 0 : int.TryParse(o.ToString(), out var i) ? i : 0;

        private static decimal ToDecimal(object? o)
            => o == null || o == DBNull.Value ? 0m : decimal.TryParse(o.ToString(), out var d) ? d : 0m;

    }

}

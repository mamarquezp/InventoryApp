using InventoryApp.Domain;
using InventoryApp.Repositories;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InventoryApp.WinForms
{
    public partial class ClientsInlineForm : Form
    {
        private readonly IClientRepository _clientRepo;
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ClientsInlineForm(IClientRepository clientRepo)
        {
            InitializeComponent();
            _clientRepo = clientRepo;
        }

        private async void ClientsInlineForm_Load(object sender, EventArgs e)
        {
            await LoadClientsAsync();
            SetupDataGridView();
            AgregarBotones(); // Agregar botones Editar y Eliminar
            AplicarEstilosVisuales();
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                dgvClients.Rows.Clear();
                var clients = await _clientRepo.GetAllAsync();
                foreach (var client in clients)
                {
                    int rowIndex = dgvClients.Rows.Add(
                        client.Id,
                        client.Nombre,
                        client.Nit,
                        client.Correo,
                        client.Telefono,
                        client.Direccion
                    );
                    dgvClients.Rows[rowIndex].Tag = client;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dgvClients.Columns["Id"].ReadOnly = true;
            dgvClients.CellEndEdit += DgvClients_CellEndEdit;
            dgvClients.CellClick += DgvClients_CellClick;
            dgvClients.UserDeletingRow += DgvClients_UserDeletingRow;
            dgvClients.CellValidating += DgvClients_CellValidating;
        }

        // Botones dentro del DataGridView
        private void AgregarBotones()
        {
            if (dgvClients.Columns["Editar"] == null)
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.HeaderText = "Editar";
                editButton.Text = "✏️ Editar";
                editButton.Name = "Editar";
                editButton.UseColumnTextForButtonValue = true;
                editButton.Width = 90;
                dgvClients.Columns.Add(editButton);
            }

            if (dgvClients.Columns["Eliminar"] == null)
            {
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.HeaderText = "Eliminar";
                deleteButton.Text = "🗑️ Eliminar";
                deleteButton.Name = "Eliminar";
                deleteButton.UseColumnTextForButtonValue = true;
                deleteButton.Width = 100;
                dgvClients.Columns.Add(deleteButton);
            }
        }

        private async void DgvClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvClients.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                dgvClients.BeginEdit(true);
                MessageBox.Show("Puedes editar los valores directamente en la fila seleccionada.", "Editar cliente",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (colName == "Eliminar")
            {
                var row = dgvClients.Rows[e.RowIndex];
                if (row.Cells["Id"].Value == null) return;

                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string nombre = row.Cells["Nombre"].Value?.ToString() ?? "";

                var result = MessageBox.Show($"¿Eliminar el cliente '{nombre}' (ID {id})?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bool success = await _clientRepo.DeleteAsync(id);
                        if (success)
                        {
                            dgvClients.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Cliente eliminado exitosamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar el cliente.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException myEx) 
                    {
                        if (myEx.Number == 1451) // Error de Foreign Key
                        {
                            MessageBox.Show($"No se puede eliminar al cliente '{nombre}'.\nEl cliente ya tiene ventas registradas.", "Acción Denegada",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show($"Error de base de datos: {myEx.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void DgvClients_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvClients.Rows[e.RowIndex];

            if (row.IsNewRow) return;

            try
            {
                // Limpiar el error de la fila (de CellValidating)
                row.ErrorText = "";

                // Leemos todos los valores de la fila
                var id = Convert.ToInt32(row.Cells["Id"].Value ?? 0);
                var nombre = row.Cells["Nombre"].Value?.ToString() ?? "";
                var nit = row.Cells["Nit"].Value?.ToString() ?? "";
                var correo = row.Cells["Correo"].Value?.ToString() ?? "";
                var telefono = row.Cells["Telefono"].Value?.ToString() ?? "";
                var direccion = row.Cells["Direccion"].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(nit) ||
                    string.IsNullOrWhiteSpace(correo))
                {
                    return; 
                }

                if (!EmailRegex.IsMatch(correo))
                {
                    MessageBox.Show("El formato del correo electrónico no es válido.", "Error de Formato",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dgvClients.CurrentCell = row.Cells["Correo"];
                    dgvClients.BeginEdit(true);
                    return;
                }
                if (id == 0)
                {
                    var newClient = new Client
                    {
                        Nombre = nombre,
                        Correo = correo,
                        Telefono = telefono,
                        Direccion = direccion,
                        Nit = nit
                    };

                    int newId = await _clientRepo.InsertAsync(newClient);

                    MessageBox.Show($"Cliente creado exitosamente. ID: {newId}", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadClientsAsync(); // Recargamos el grid
                }
                else
                {
                    var client = new Client
                    {
                        Id = id,
                        Nombre = nombre,
                        Correo = correo,
                        Telefono = telefono,
                        Direccion = direccion,
                        Nit = nit
                    };

                    bool success = await _clientRepo.UpdateAsync(client);
                    if (!success)
                    {
                        MessageBox.Show("Error al actualizar el cliente.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    row.Tag = client;
                    MessageBox.Show("Cliente actualizado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException argEx) // Captura validaciones del Repositorio
            {
                MessageBox.Show(argEx.Message, "Error de validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string nitVal = row.Cells["Nit"].Value?.ToString() ?? "vacío";
                string correoVal = row.Cells["Correo"].Value?.ToString() ?? "vacío";

                if (ex.Message.Contains("Duplicate entry") && ex.Message.Contains("uk_cliente_nit"))
                {
                    MessageBox.Show($"Error al guardar: Ya existe un cliente con el NIT '{nitVal}'.", "NIT Duplicado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Message.Contains("Duplicate entry")) // Asumiendo que correo también sea unique
                {
                    MessageBox.Show($"Error al guardar: Ya existe un cliente con el correo '{correoVal}'.", "Correo Duplicado",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void DgvClients_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row?.Cells["Id"].Value == null) return;
                int id = Convert.ToInt32(e.Row.Cells["Id"].Value);
                string nombre = e.Row.Cells["Nombre"].Value?.ToString() ?? "";

                var result = MessageBox.Show($"¿Eliminar el cliente '{nombre}' (ID {id})?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = await _clientRepo.DeleteAsync(id);
                    if (!success)
                    {
                        MessageBox.Show("No se pudo eliminar el cliente.", "Error",
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
                e.Cancel = true;
            }
        }

        private void DgvClients_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvClients.Columns[e.ColumnIndex].Name;
            string value = e.FormattedValue?.ToString() ?? "";

            dgvClients.Rows[e.RowIndex].ErrorText = ""; // Limpiar error previo

            if (columnName == "Correo")
            {
                if (!string.IsNullOrWhiteSpace(value) && !EmailRegex.IsMatch(value))
                {
                    e.Cancel = true;
                    dgvClients.Rows[e.RowIndex].ErrorText = "Formato de correo inválido";
                }
            }
        }

        // Apartado de colores y estilos visuales
        private void AplicarEstilosVisuales()
        {
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Text = "Gestión de Clientes";
            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(52, 73, 94);
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;

            dgvClients.BorderStyle = BorderStyle.None;
            dgvClients.BackgroundColor = Color.White;
            dgvClients.GridColor = Color.FromArgb(220, 220, 220);
            dgvClients.RowHeadersVisible = false;
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.MultiSelect = false;

            dgvClients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvClients.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvClients.EnableHeadersVisualStyles = false;

            dgvClients.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvClients.DefaultCellStyle.BackColor = Color.White;
            dgvClients.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgvClients.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvClients.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvClients.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvClients.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }
    }
}


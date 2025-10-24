using InventoryApp.Domain;
using InventoryApp.Repositories;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryApp.WinForms
{
    public partial class ViewSalesForm : Form
    {
            private readonly ISaleRepository _saleRepo;
            private readonly IClientRepository _clientRepo;
            private List<Client> _clients = new();


            public ViewSalesForm(ISaleRepository saleRepo, IClientRepository clientRepo)
            {
                InitializeComponent();
                _saleRepo = saleRepo;
                _clientRepo = clientRepo;
            }

            private async void ViewSalesForm_Load(object sender, EventArgs e)
            {
                // Configurar DataGridViews (visualización)
                SetupGrids();

                // Cargar filtros
                await LoadClientFilterAsync();

                // Carga inicial
                await LoadSalesMasterAsync();
            }

            private void SetupGrids()
            {
                // Grid Maestro
                dgvSalesMaster.AutoGenerateColumns = false;
                dgvSalesMaster.ReadOnly = true;
                dgvSalesMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvSalesMaster.MultiSelect = false;
                dgvSalesMaster.AllowUserToAddRows = false;

                dgvSalesMaster.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Venta ID", DataPropertyName = "VentaId", Width = 80 });
                dgvSalesMaster.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 150, DefaultCellStyle = { Format = "g" } });
                dgvSalesMaster.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cliente", DataPropertyName = "Cliente", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dgvSalesMaster.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = "Total", Width = 100, DefaultCellStyle = { Format = "N2" } });

                // Grid Detalle
                dgvSalesDetail.AutoGenerateColumns = false;
                dgvSalesDetail.ReadOnly = true;
                dgvSalesDetail.AllowUserToAddRows = false;

                dgvSalesDetail.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Producto", DataPropertyName = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dgvSalesDetail.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cantidad", DataPropertyName = "Cantidad", Width = 80 });
                dgvSalesDetail.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Precio Unit.", DataPropertyName = "PrecioUnit", Width = 100, DefaultCellStyle = { Format = "N2" } });
                dgvSalesDetail.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Subtotal", DataPropertyName = "Subtotal", Width = 100, DefaultCellStyle = { Format = "N2" } });

                // Evento de selección
                dgvSalesMaster.SelectionChanged += DgvSalesMaster_SelectionChanged;
            }

            private async Task LoadClientFilterAsync()
            {
                _clients = await _clientRepo.GetAllAsync();

                // Añadir opción Todos
                var clientsWithAll = new List<Client> { new Client { Id = 0, Nombre = "[Todos los Clientes]" } };
                clientsWithAll.AddRange(_clients);

                cmbClients.DataSource = clientsWithAll;
                cmbClients.DisplayMember = "Nombre";
                cmbClients.ValueMember = "Id";
            }

            private async Task LoadSalesMasterAsync()
            {
                try
                {
                    // Limpiar detalle
                    dgvSalesDetail.DataSource = null;

                    // filtros
                    DateTime? from = chkFilterDate.Checked ? dtpFrom.Value : null;
                    DateTime? to = chkFilterDate.Checked ? dtpTo.Value : null;
                    int? clientId = (cmbClients.SelectedValue is int id && id > 0) ? id : null;

                    var sales = await _saleRepo.GetSalesMasterAsync(from, to, clientId);
                    dgvSalesMaster.DataSource = sales;

                    if (!sales.Any())
                    {
                        lblStatus.Text = "No se encontraron ventas con esos filtros.";
                    }
                    else
                    {
                        lblStatus.Text = $"Mostrando {sales.Count} ventas.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private async void DgvSalesMaster_SelectionChanged(object? sender, EventArgs e)
            {
                if (dgvSalesMaster.CurrentRow?.DataBoundItem is SaleMasterView selectedSale)
                {
                    try
                    {
                        var details = await _saleRepo.GetSaleDetailsAsync(selectedSale.VentaId);
                        dgvSalesDetail.DataSource = details;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    dgvSalesDetail.DataSource = null;
                }
            }

            private async void btnFilter_Click(object sender, EventArgs e)
            {
                await LoadSalesMasterAsync();
            }

            private void chkFilterDate_CheckedChanged(object sender, EventArgs e)
            {
                dtpFrom.Enabled = chkFilterDate.Checked;
                dtpTo.Enabled = chkFilterDate.Checked;
            }
        
    }
}

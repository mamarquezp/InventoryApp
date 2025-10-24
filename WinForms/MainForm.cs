
using InventoryApp.Repositories;
using InventoryApp.Services;

namespace InventoryApp.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IProductRepository _productRepo;
        private readonly IClientRepository _clientRepo;
        private readonly SalesService _salesService;
        private readonly ISaleRepository _saleRepo;

        public MainForm(IProductRepository productRepo, IClientRepository clientRepo, SalesService salesService, ISaleRepository saleRepo)
        {
            InitializeComponent();
            _productRepo = productRepo;
            _clientRepo = clientRepo;
            _salesService = salesService;
            _saleRepo = saleRepo;
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new ProductsInlineForm(_productRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new NewSaleInlineForm(_productRepo, _clientRepo, _salesService);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new ClientsInlineForm(_clientRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnVerVentas_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                // Usamos los repositorios que ya tiene el MainForm
                using var f = new ViewSalesForm(_saleRepo, _clientRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }
    }
}
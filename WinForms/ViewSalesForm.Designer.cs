namespace InventoryApp.WinForms
{
    partial class ViewSalesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelFilters = new Panel();
            btnFilter = new Button();
            cmbClients = new ComboBox();
            dtpTo = new DateTimePicker();
            lblTo = new Label();
            dtpFrom = new DateTimePicker();
            chkFilterDate = new CheckBox();
            lblStatus = new Label();
            splitContainer = new SplitContainer();
            dgvSalesMaster = new DataGridView();
            dgvSalesDetail = new DataGridView();
            panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSalesMaster).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesDetail).BeginInit();
            SuspendLayout();
            // 
            // panelFilters
            // 
            panelFilters.BackColor = Color.WhiteSmoke;
            panelFilters.Controls.Add(btnFilter);
            panelFilters.Controls.Add(cmbClients);
            panelFilters.Controls.Add(dtpTo);
            panelFilters.Controls.Add(lblTo);
            panelFilters.Controls.Add(dtpFrom);
            panelFilters.Controls.Add(chkFilterDate);
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Location = new Point(0, 0);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(1138, 55);
            panelFilters.TabIndex = 0;
            // 
            // btnFilter
            // 
            btnFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilter.Location = new Point(800, 15);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(94, 25);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filtrar";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // cmbClients
            // 
            cmbClients.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClients.FormattingEnabled = true;
            cmbClients.Location = new Point(510, 16);
            cmbClients.Name = "cmbClients";
            cmbClients.Size = new Size(270, 23);
            cmbClients.TabIndex = 4;
            // 
            // dtpTo
            // 
            dtpTo.Enabled = false;
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(340, 16);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(106, 23);
            dtpTo.TabIndex = 3;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(316, 20);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(15, 15);
            lblTo.TabIndex = 2;
            lblTo.Text = "a";
            // 
            // dtpFrom
            // 
            dtpFrom.Enabled = false;
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(200, 16);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(106, 23);
            dtpFrom.TabIndex = 1;
            // 
            // chkFilterDate
            // 
            chkFilterDate.AutoSize = true;
            chkFilterDate.Location = new Point(20, 18);
            chkFilterDate.Name = "chkFilterDate";
            chkFilterDate.Size = new Size(160, 19);
            chkFilterDate.TabIndex = 0;
            chkFilterDate.Text = "Filtrar por Rango de Fecha:";
            chkFilterDate.UseVisualStyleBackColor = true;
            chkFilterDate.CheckedChanged += chkFilterDate_CheckedChanged;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 574);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(5, 0, 0, 0);
            lblStatus.Size = new Size(1138, 25);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Listo.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 55);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(dgvSalesMaster);
            splitContainer.Panel1.Padding = new Padding(5);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(dgvSalesDetail);
            splitContainer.Panel2.Padding = new Padding(5);
            splitContainer.Size = new Size(1138, 519);
            splitContainer.SplitterDistance = 280;
            splitContainer.TabIndex = 2;
            // 
            // dgvSalesMaster
            // 
            dgvSalesMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSalesMaster.Dock = DockStyle.Fill;
            dgvSalesMaster.Location = new Point(5, 5);
            dgvSalesMaster.Name = "dgvSalesMaster";
            dgvSalesMaster.Size = new Size(1128, 270);
            dgvSalesMaster.TabIndex = 0;
            // 
            // dgvSalesDetail
            // 
            dgvSalesDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSalesDetail.Dock = DockStyle.Fill;
            dgvSalesDetail.Location = new Point(5, 5);
            dgvSalesDetail.Name = "dgvSalesDetail";
            dgvSalesDetail.Size = new Size(1128, 225);
            dgvSalesDetail.TabIndex = 0;
            // 
            // ViewSalesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1138, 599);
            Controls.Add(splitContainer);
            Controls.Add(lblStatus);
            Controls.Add(panelFilters);
            Name = "ViewSalesForm";
            Text = "Visualizador de Ventas";
            Load += ViewSalesForm_Load;
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSalesMaster).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesDetail).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelFilters;
        private Button btnFilter;
        private ComboBox cmbClients;
        private DateTimePicker dtpTo;
        private Label lblTo;
        private DateTimePicker dtpFrom;
        private CheckBox chkFilterDate;
        private Label lblStatus;
        private SplitContainer splitContainer;
        private DataGridView dgvSalesMaster;
        private DataGridView dgvSalesDetail;
    }
}
namespace InventoryApp.WinForms
{
    partial class NewSaleInlineForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            gridCart = new DataGridView();
            lblTotal = new Label();
            btnConfirmarVenta = new Button();
            cmbCliente = new ComboBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCart).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(cmbCliente);
            panel1.Controls.Add(btnConfirmarVenta);
            panel1.Controls.Add(lblTotal);
            panel1.Controls.Add(gridCart);
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1126, 562);
            panel1.TabIndex = 0;
            // 
            // gridCart
            // 
            gridCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridCart.Location = new Point(11, 245);
            gridCart.Name = "gridCart";
            gridCart.Size = new Size(1107, 308);
            gridCart.TabIndex = 0;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotal.Location = new Point(11, 194);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(146, 32);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "Total: Q 0.00";
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.Location = new Point(1029, 203);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(75, 23);
            btnConfirmarVenta.TabIndex = 2;
            btnConfirmarVenta.Text = "Confirmar";
            btnConfirmarVenta.UseVisualStyleBackColor = true;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
            // 
            // cmbCliente
            // 
            cmbCliente.FormattingEnabled = true;
            cmbCliente.Location = new Point(11, 109);
            cmbCliente.Name = "cmbCliente";
            cmbCliente.Size = new Size(228, 23);
            cmbCliente.TabIndex = 3;
            // 
            // NewSaleInlineForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1131, 566);
            Controls.Add(panel1);
            Name = "NewSaleInlineForm";
            Text = "NewSaleInlineForm";
            Load += NewSaleInlineForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridCart).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox cmbCliente;
        private Button btnConfirmarVenta;
        private Label lblTotal;
        private DataGridView gridCart;
    }
}
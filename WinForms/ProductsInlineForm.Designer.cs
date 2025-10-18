namespace InventoryApp.WinForms
{
    partial class ProductsInlineForm
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
            label_productos = new Label();
            dataGridProducts = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridProducts).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label_productos);
            panel1.Controls.Add(dataGridProducts);
            panel1.Location = new Point(3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1136, 596);
            panel1.TabIndex = 0;
            // 
            // label_productos
            // 
            label_productos.AutoSize = true;
            label_productos.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_productos.Location = new Point(22, 25);
            label_productos.Name = "label_productos";
            label_productos.Size = new Size(189, 50);
            label_productos.TabIndex = 4;
            label_productos.Text = "Productos";
            // 
            // dataGridProducts
            // 
            dataGridProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridProducts.Location = new Point(0, 188);
            dataGridProducts.Name = "dataGridProducts";
            dataGridProducts.Size = new Size(1133, 396);
            dataGridProducts.TabIndex = 0;
            // 
            // ProductsInlineForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1140, 598);
            Controls.Add(panel1);
            Name = "ProductsInlineForm";
            Text = "ProductsInlineForm";
            Load += ProductsInlineForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridProducts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridProducts;
        private Label label_productos;
    }
}
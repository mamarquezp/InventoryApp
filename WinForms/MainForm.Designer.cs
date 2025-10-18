namespace InventoryApp.WinForms
{
    partial class MainForm
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
            btnVentas = new Button();
            btnProductos = new Button();
            btnClientes = new Button();
            btnVerVentas = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnVerVentas);
            panel1.Controls.Add(btnClientes);
            panel1.Controls.Add(btnVentas);
            panel1.Controls.Add(btnProductos);
            panel1.Location = new Point(2, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1064, 558);
            panel1.TabIndex = 0;
            // 
            // btnVentas
            // 
            btnVentas.Location = new Point(727, 130);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(294, 97);
            btnVentas.TabIndex = 1;
            btnVentas.Text = "REGISTRAR VENTAS";
            btnVentas.UseVisualStyleBackColor = true;
            btnVentas.Click += btnVentas_Click;
            // 
            // btnProductos
            // 
            btnProductos.Location = new Point(69, 130);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(294, 97);
            btnProductos.TabIndex = 0;
            btnProductos.Text = "PRODUCTOS";
            btnProductos.UseVisualStyleBackColor = true;
            btnProductos.Click += btnProductos_Click;
            // 
            // btnClientes
            // 
            btnClientes.Location = new Point(397, 130);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(294, 97);
            btnClientes.TabIndex = 2;
            btnClientes.Text = "CLIENTES";
            btnClientes.UseVisualStyleBackColor = true;
            // 
            // btnVerVentas
            // 
            btnVerVentas.Location = new Point(397, 281);
            btnVerVentas.Name = "btnVerVentas";
            btnVerVentas.Size = new Size(294, 97);
            btnVerVentas.TabIndex = 3;
            btnVerVentas.Text = "VISUALIZAR VENTAS";
            btnVerVentas.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 562);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "MainForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnVentas;
        private Button btnProductos;
        private Button btnVerVentas;
        private Button btnClientes;
    }
}
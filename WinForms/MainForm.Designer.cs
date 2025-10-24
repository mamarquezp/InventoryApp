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
            btnVerVentas = new Button();
            btnClientes = new Button();
            btnVentas = new Button();
            btnProductos = new Button();
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
            panel1.Location = new Point(3, 5);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1520, 930);
            panel1.TabIndex = 0;
            // 
            // btnVerVentas
            // 
            btnVerVentas.Location = new Point(567, 468);
            btnVerVentas.Margin = new Padding(4, 5, 4, 5);
            btnVerVentas.Name = "btnVerVentas";
            btnVerVentas.Size = new Size(420, 162);
            btnVerVentas.TabIndex = 3;
            btnVerVentas.Text = "VISUALIZAR VENTAS";
            btnVerVentas.UseVisualStyleBackColor = true;
            btnVerVentas.Click += btnVerVentas_Click;
            // 
            // btnClientes
            // 
            btnClientes.Location = new Point(567, 217);
            btnClientes.Margin = new Padding(4, 5, 4, 5);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(420, 162);
            btnClientes.TabIndex = 2;
            btnClientes.Text = "CLIENTES";
            btnClientes.UseVisualStyleBackColor = true;
            // 
            // btnVentas
            // 
            btnVentas.Location = new Point(1039, 217);
            btnVentas.Margin = new Padding(4, 5, 4, 5);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(420, 162);
            btnVentas.TabIndex = 1;
            btnVentas.Text = "REGISTRAR VENTAS";
            btnVentas.UseVisualStyleBackColor = true;
            btnVentas.Click += btnVentas_Click;
            // 
            // btnProductos
            // 
            btnProductos.Location = new Point(99, 217);
            btnProductos.Margin = new Padding(4, 5, 4, 5);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(420, 162);
            btnProductos.TabIndex = 0;
            btnProductos.Text = "PRODUCTOS";
            btnProductos.UseVisualStyleBackColor = true;
            btnProductos.Click += btnProductos_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1520, 937);
            Controls.Add(panel1);
            Margin = new Padding(4, 5, 4, 5);
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
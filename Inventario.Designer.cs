using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Inventario
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.Text = "Inventario";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(980, 620);
            this.MinimizeBox = true;
            this.MaximizeBox = true;

            // Grid
            dataGridView1 = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 520,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true, // edición solo mediante botones
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular), // 🔸 Texto más grande
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    BackColor = Color.FromArgb(5, 80, 45),
                    ForeColor = Color.White
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            };

            // Botones
            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 70, // 🔸 Un poco más alto para que se vean mejor
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(5, 80, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold) // 🔸 Aumentado
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += buttonCerrar_Click;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold) // 🔸 Aumentado
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += btnEliminar_Click;

            btnModificar = new Button
            {
                Text = "Modificar",
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(17, 105, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold) // 🔸 Aumentado
            };
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.Click += btnModificar_Click;

            btnAgregar = new Button
            {
                Text = "Agregar",
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(17, 105, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold) // 🔸 Aumentado
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += btnAgregar_Click;

            panelBotones.Controls.AddRange(new Control[] { btnCerrar, btnEliminar, btnModificar, btnAgregar });

            Controls.Add(panelBotones);
            Controls.Add(dataGridView1);
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public class Home_JefeDeArea : Form
    {
        public Home_JefeDeArea()
        {
            Text = "Inicio – Jefe de Área";
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            // Panel principal
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };
            Controls.Add(mainPanel);

            // Título principal
            var lblTitulo = new Label
            {
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 80, 45),
                Text = "Panel de Inicio – JEFE DE ÁREA"
            };
            mainPanel.Controls.Add(lblTitulo);

            // Descripción del panel
            var lblDescripcion = new Label
            {
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 30, 30),
                Text = "Acceso a funciones operativas, consultas, ventas y alta de entornos formativos."
            };
            mainPanel.Controls.Add(lblDescripcion);

            // Contenedor de botones
            var botones = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            mainPanel.Controls.Add(botones);

            // Botones principales
            botones.Controls.Add(CrearBoton("Consulta de Actividad", () => AbrirFormulario(new Consulta_de_Actividad())));
            botones.Controls.Add(CrearBoton("Consulta de Ventas", () => AbrirFormulario(new Consulta_de_Ventas())));
            botones.Controls.Add(CrearBoton("Registro de Ventas", () => AbrirFormulario(new Registro_de_una_Venta())));
            botones.Controls.Add(CrearBoton("Inventario", () => AbrirFormulario(new Inventario())));
            botones.Controls.Add(CrearBoton("Usuarios (ABM)", () => AbrirFormulario(new Consulta_de_Usuarios__Modificacion__Baja())));

            // NUEVA OPCIÓN — Alta de Entornos Formativos
            botones.Controls.Add(CrearBoton("Alta de Entornos Formativos", () => AbrirFormulario(new Alta_de_Entornos_Formativos())));

            // Botón salir
            botones.Controls.Add(CrearBoton("Cerrar Sesión", () =>
            {
                if (MessageBox.Show("¿Cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Close();
            }));
        }

        // Helper para crear botones uniformes
        private Button CrearBoton(string texto, Action accion)
        {
            var btn = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(17, 105, 59),
                FlatStyle = FlatStyle.Flat,
                Width = 280,
                Height = 60,
                Margin = new Padding(15),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => accion();
            return btn;
        }

        // Helper para abrir formularios hijos
        private void AbrirFormulario(Form frm)
        {
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }
    }
}

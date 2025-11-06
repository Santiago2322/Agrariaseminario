using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Pantalla_Principal : Form
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        internal Label lblTitulo;
        private Panel panelMenu;
        internal Panel panelContenido;
        internal Panel panelStage;

        private Button btnHome;
        private Button btnRegVenta;
        private Button btnConsultaAct;
        private Button btnConsultaUsuarios;
        private Button btnConsultaVentas;
        private Button btnEntornos;
        private Button btnInventario;
        private Button btnAltaUsuarios;
        private Button btnAltaEntornos;
        private Button btnCerrarSesion;
        // 🔸 Registro de actividad deshabilitado por diseño
        private Button btnRegActividad;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Header
            this.panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.FromArgb(5, 80, 45),
                Padding = new Padding(16, 8, 16, 8)
            };
            this.lblTitulo = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Text = "Proyecto Agraria"
            };
            this.panelHeader.Controls.Add(this.lblTitulo);

            // Sidebar
            this.panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(17, 105, 59),
                Padding = new Padding(8)
            };

            // Contenido
            this.panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(16)
            };

            // Stage (contenedor interno)
            this.panelStage = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.panelContenido.Controls.Add(this.panelStage);

            // ===== Botones =====
            this.btnHome = CrearBoton("Inicio", new EventHandler(this.btnHome_Click));
            this.btnRegVenta = CrearBoton("Registro de Ventas", new EventHandler(this.btnRegVenta_Click));
            this.btnConsultaAct = CrearBoton("Consulta de Actividad", new EventHandler(this.btnConsultaAct_Click));
            this.btnConsultaUsuarios = CrearBoton("Usuarios (ABM)", new EventHandler(this.btnConsultaUsuarios_Click));
            this.btnConsultaVentas = CrearBoton("Consulta de Ventas", new EventHandler(this.btnConsultaVentas_Click));
            this.btnEntornos = CrearBoton("Entornos Formativos", new EventHandler(this.btnEntornos_Click));
            this.btnInventario = CrearBoton("Inventario", new EventHandler(this.btnInventario_Click));
            this.btnAltaUsuarios = CrearBoton("Alta de Usuarios", new EventHandler(this.btnAltaUsuarios_Click));
            this.btnAltaEntornos = CrearBoton("Alta de Entornos", new EventHandler(this.btnAltaEntornos_Click));
            this.btnCerrarSesion = CrearBoton("Cerrar Sesión", new EventHandler(this.btnCerrarSesion_Click));

            // Registro de Actividad (creado pero oculto)
            this.btnRegActividad = CrearBoton("Registro de Actividad", null);
            this.btnRegActividad.Visible = false;
            this.btnRegActividad.Enabled = false;

            // Lista final de botones (sin huecos)
            Button[] botones = {
                this.btnHome,
                this.btnRegVenta,
                this.btnConsultaAct,
                this.btnConsultaUsuarios,
                this.btnConsultaVentas,
                this.btnEntornos,
                this.btnInventario,
                this.btnAltaUsuarios,
                this.btnAltaEntornos,
                this.btnCerrarSesion
            };

            int top = 8;
            foreach (var b in botones)
            {
                b.Left = 8;
                b.Top = top;
                b.Width = this.panelMenu.Width - 16;
                b.Height = 44;
                this.panelMenu.Controls.Add(b);
                top += 50;
            }

            // Form principal
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "Pantalla Principal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1100, 700);
            this.BackColor = Color.White;

            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelHeader);
        }

        private Button CrearBoton(string texto, EventHandler onClick)
        {
            var b = new Button
            {
                Text = texto,
                BackColor = Color.FromArgb(17, 105, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            if (onClick != null) b.Click += onClick;
            return b;
        }
    }
}

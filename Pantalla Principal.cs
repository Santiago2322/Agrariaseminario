using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Pantalla_Principal : Form
    {
        private string UsuarioRol { get; set; }

        // Estado menú
        private bool menuVisible = true;

        // Botón del menú y encabezado
        private Button btnToggleMenu;
        private Panel panelHeader;

        // Panel de menú detectado (si lo tuvieras con otro nombre)
        private Control panelMenuDetectado;

        // Ancho fijo del menú lateral según tu Designer
        private const int AnchoMenu = 233;

        public Pantalla_Principal(string rol)
        {
            InitializeComponent();
            UsuarioRol = string.IsNullOrWhiteSpace(rol) ? "Invitado" : rol;

            Load += Pantalla_Principal_Load;
            Resize += Pantalla_Principal_Resize;

            // 1) Encabezado fijo sobre el área de trabajo (no tapa formularios)
            panelHeader = new Panel
            {
                Name = "panelHeader",
                Height = 36,
                BackColor = SystemColors.ControlLight,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(panelHeader);
            panelHeader.BringToFront();

            // 2) Botón "☰ Menú" dentro del encabezado (no sobre formularios)
            btnToggleMenu = new Button
            {
                Text = "☰ Menú",
                AutoSize = true,
                FlatStyle = FlatStyle.Standard,
                Location = new Point(8, 6)
            };
            btnToggleMenu.Click += (s, e) => ToggleMenu();
            panelHeader.Controls.Add(btnToggleMenu);

            // 3) Detectar panel de menú por nombre común (opcional)
            panelMenuDetectado = Controls
                .Cast<Control>()
                .FirstOrDefault(c =>
                    c is Panel &&
                    (string.Equals(c.Name, "panelMenu", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(c.Name, "panelLateral", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(c.Name, "panelOpciones", StringComparison.OrdinalIgnoreCase)));

            // Acomodar encabezado y workspace inicial
            AlinearEncabezadoYWorkspace();

            // Asegurar que los botones “Cerrar sesión” y “Salir” siempre queden visibles
            TraerBotonesDeSalidaAlFrente();
        }

        public Pantalla_Principal() : this("Invitado") { }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {
            AplicarRestriccionesPorRol();
            AlinearEncabezadoYWorkspace();
        }

        private void AplicarRestriccionesPorRol()
        {
            string rolActual = UsuarioRol;
            DeshabilitarTodo();

            switch (rolActual)
            {
                case "Administrador":
                    HabilitarTodo();
                    break;

                case "Jefe de Área":
                    label9.Enabled = true; // Alta Entorno
                    label10.Enabled = true; // Consulta/Modif/Eliminar Entorno
                    label11.Enabled = true; // Registro Actividad
                    label12.Enabled = true; // Consulta Actividad
                    label15.Enabled = true; // Inventario
                    label16.Enabled = true; // Consulta Ventas
                    break;

                case "Docente":
                    label11.Enabled = true; // Registro Actividad
                    label12.Enabled = true; // Consulta Actividad
                    label15.Enabled = true; // Inventario
                    break;

                case "Invitado":
                    label12.Enabled = true; // Solo consulta de actividad
                    break;

                default:
                    MessageBox.Show($"Rol '{rolActual}' no reconocido. Acceso limitado.",
                        "Permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            if (Salir != null) Salir.Enabled = true;
            if (btnCerrarSesion != null) btnCerrarSesion.Enabled = true;
        }

        private void HabilitarTodo()
        {
            label2.Enabled = true;
            label3.Enabled = true;
            label9.Enabled = true;
            label10.Enabled = true;
            label11.Enabled = true;
            label12.Enabled = true;
            label15.Enabled = true;
            label16.Enabled = true;
            label17.Enabled = true;
        }

        private void DeshabilitarTodo()
        {
            label2.Enabled = false;
            label3.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            label12.Enabled = false;
            label15.Enabled = false;
            label16.Enabled = false;
            label17.Enabled = false;
        }

        // --------------------- Navegación embebida ---------------------
        private void AbrirFormularioEnPanel<MiForm>() where MiForm : Form, new()
        {
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);

            var f = new MiForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            panelContenedor.Controls.Add(f);
            panelContenedor.Tag = f;
            f.Show();

            // Oculto menú (modo foco). El encabezado queda, así el botón no tapa nada.
            OcultarMenu();
        }

        // --------------------- Mostrar / Ocultar menú ---------------------
        private void ToggleMenu()
        {
            if (menuVisible) OcultarMenu();
            else MostrarMenu();
        }

        private void OcultarMenu()
        {
            menuVisible = false;
            btnToggleMenu.Text = "☰ Menú";

            if (panelMenuDetectado != null)
                panelMenuDetectado.Visible = false;

            SetVisibilidadLabelsMenu(false);

            // Workspace ocupa todo el ancho
            panelContenedor.Dock = DockStyle.None;
            panelContenedor.Left = 0;
            AlinearEncabezadoYWorkspace();

            TraerBotonesDeSalidaAlFrente();
        }

        private void MostrarMenu()
        {
            menuVisible = true;
            btnToggleMenu.Text = "← Volver";

            if (panelMenuDetectado != null)
                panelMenuDetectado.Visible = true;

            SetVisibilidadLabelsMenu(true);

            // Workspace corre a la derecha del menú
            panelContenedor.Dock = DockStyle.None;
            panelContenedor.Left = AnchoMenu + 3;
            AlinearEncabezadoYWorkspace();

            TraerBotonesDeSalidaAlFrente();
        }

        private void SetVisibilidadLabelsMenu(bool visible)
        {
            var labelsMenu = new[]
            {
                label2, label3, label9, label10, label11, label12, label15, label16, label17
            };
            foreach (var lbl in labelsMenu)
                if (lbl != null) lbl.Visible = visible;
        }

        // --------------------- Encabezado + Workspace ---------------------
        private void AlinearEncabezadoYWorkspace()
        {
            // Encabezado alineado con el borde izquierdo del área de trabajo
            panelHeader.Left = panelContenedor.Left;
            panelHeader.Width = ClientSize.Width - panelHeader.Left - 10;
            panelHeader.Top = 0;

            // Área de trabajo debajo del encabezado y hasta abajo
            panelContenedor.Top = panelHeader.Bottom;
            panelContenedor.Height = ClientSize.Height - panelContenedor.Top - 10;

            // Asegurar z-order
            panelHeader.BringToFront();
            btnToggleMenu.BringToFront();
        }

        private void Pantalla_Principal_Resize(object sender, EventArgs e)
        {
            // Mantener layout consistente al redimensionar
            AlinearEncabezadoYWorkspace();
            TraerBotonesDeSalidaAlFrente();
        }

        private void TraerBotonesDeSalidaAlFrente()
        {
            if (btnCerrarSesion != null) btnCerrarSesion.BringToFront();
            if (Salir != null) Salir.BringToFront();
            panelHeader?.BringToFront();
            btnToggleMenu?.BringToFront();
        }

        // --------------------- Clicks del menú ---------------------
        private void label2_Click(object sender, EventArgs e) { if (!label2.Enabled) return; AbrirFormularioEnPanel<Alta_de_usuarios>(); }
        private void label3_Click(object sender, EventArgs e) { if (!label3.Enabled) return; AbrirFormularioEnPanel<Consulta_de_Usuarios__Modificacion__Baja>(); }
        private void label9_Click(object sender, EventArgs e) { if (!label9.Enabled) return; AbrirFormularioEnPanel<Alta_de_Entornos_Formativos>(); }
        private void label10_Click(object sender, EventArgs e) { if (!label10.Enabled) return; AbrirFormularioEnPanel<Consulta_de_Entornos_Formativos__Modificar_Eliminar>(); }
        private void label11_Click(object sender, EventArgs e) { if (!label11.Enabled) return; AbrirFormularioEnPanel<Registro_de_Actividad>(); }
        private void label12_Click(object sender, EventArgs e) { if (!label12.Enabled) return; AbrirFormularioEnPanel<Consulta_de_Actividad>(); }
        private void label17_Click(object sender, EventArgs e) { if (!label17.Enabled) return; AbrirFormularioEnPanel<Registro_de_una_Venta>(); }
        private void label16_Click(object sender, EventArgs e) { if (!label16.Enabled) return; AbrirFormularioEnPanel<Consulta_de_Ventas>(); }
        private void label15_Click(object sender, EventArgs e) { if (!label15.Enabled) return; AbrirFormularioEnPanel<Inventario>(); }

        // --------------------- Salir / Cerrar sesión ---------------------
        private void Salir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas salir de la aplicación?",
                    "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Hide();
            try
            {
                var login = new Form1(); // tu formulario de login
                login.ShowDialog();
            }
            finally
            {
                Close();
            }
        }

        // Stubs que ya tenías
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox6_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { AbrirFormularioEnPanel<Registro_de_Actividad>(); }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox7_Click(object sender, EventArgs e) { }
        private void pictureBox8_Click(object sender, EventArgs e) { }
        private void pictureBox9_Click(object sender, EventArgs e) { }
        private void pictureBox10_Click(object sender, EventArgs e) { }
        private void pictureBox11_Click(object sender, EventArgs e) { }
        private void pictureBox12_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
    }
}

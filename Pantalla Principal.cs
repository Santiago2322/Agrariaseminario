using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Pantalla_Principal : Form
    {
        // ===== Feature flags =====
        private const bool FEATURE_REGISTRO_ACTIVIDAD = false; // ← OFF: oculta el botón y no lo cablea

        // ===== Estado / impresión =====
        private Form activeChild;
        private ToolStrip tool;
        private ToolStripButton btnPrint, btnCloseTab;
        private PrintDocument printDoc;
        private Bitmap printBmp;

        // Rol actual (actualizable tras login/cierre de sesión)
        private string rolUsuario;

        public Pantalla_Principal(string rol = "usuario")
        {
            InitializeComponent();

            rolUsuario = NormalizarRol(rol);

            SetupToolbar();
            AplicarPermisosPorRol();

            // Al entrar, mostramos el "home" (banner vacío)
            try { btnHome.Click += btnHome_Click; btnHome.PerformClick(); } catch { }

            KeyPreview = true;
            KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.P) btnPrint?.PerformClick();
                if (e.KeyCode == Keys.Escape) btnCloseTab?.PerformClick();
            };
        }

        private static string NormalizarRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return "usuario";
            var r = rol.Trim().ToLowerInvariant();
            if (r == "admin" || r == "administrador") return "admin";
            if (r == "jefe de area" || r == "jefe de área") return "jefe de area";
            if (r == "docente" || r == "profesor") return "docente";
            if (r == "invitado" || r == "guest") return "invitado";
            return "usuario";
        }

        private void SetPermiso(Button b, bool permitido, string tooltipSiNo)
        {
            if (b == null) return;
            b.Enabled = permitido;
            b.Tag = permitido ? "allow" : "deny";
            if (!permitido && !string.IsNullOrEmpty(tooltipSiNo))
                new ToolTip().SetToolTip(b, tooltipSiNo);
        }

        private bool Denegado(Button b)
        {
            if (b == null) return true;
            if (b.Tag is string tag && tag == "allow") return false;
            MessageBox.Show("No tenés permiso para usar esta opción.", "Acceso restringido",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        private void AplicarPermisosPorRol()
        {
            try { lblTitulo.Text = $"Proyecto Agraria — {rolUsuario.ToUpper()}"; } catch { }

            // por defecto (home/cerrar siempre on)
            SetPermiso(btnHome, true, "");
            SetPermiso(btnCerrarSesion, true, "");

            // Otras opciones
            if (FEATURE_REGISTRO_ACTIVIDAD && btnRegActividad != null)
                SetPermiso(btnRegActividad, false, "Docente o superior.");

            SetPermiso(btnRegVenta, false, "Jefe de área o admin.");
            SetPermiso(btnConsultaAct, false, "Sin permisos.");
            SetPermiso(btnConsultaUsuarios, false, "Solo administrador.");
            SetPermiso(btnConsultaVentas, false, "Sin permisos.");
            SetPermiso(btnEntornos, false, "Docente o superior.");
            SetPermiso(btnInventario, false, "Docente o superior.");
            SetPermiso(btnAltaUsuarios, false, "Solo administrador.");
            SetPermiso(btnAltaEntornos, false, "Solo administrador.");

            var r = rolUsuario;

            if (r == "admin")
            {
                if (FEATURE_REGISTRO_ACTIVIDAD && btnRegActividad != null) SetPermiso(btnRegActividad, true, "");
                SetPermiso(btnRegVenta, true, "");
                SetPermiso(btnConsultaAct, true, "");
                SetPermiso(btnConsultaUsuarios, true, "");
                SetPermiso(btnConsultaVentas, true, "");
                SetPermiso(btnEntornos, true, "");
                SetPermiso(btnInventario, true, "");
                SetPermiso(btnAltaUsuarios, true, "");
                SetPermiso(btnAltaEntornos, true, "");
            }
            else if (r == "jefe de area")
            {
                if (FEATURE_REGISTRO_ACTIVIDAD && btnRegActividad != null) SetPermiso(btnRegActividad, true, "");
                SetPermiso(btnRegVenta, true, "");
                SetPermiso(btnConsultaAct, true, "");
                SetPermiso(btnConsultaVentas, true, "");
                SetPermiso(btnEntornos, true, "");
                SetPermiso(btnInventario, true, "");
            }
            else if (r == "docente")
            {
                if (FEATURE_REGISTRO_ACTIVIDAD && btnRegActividad != null) SetPermiso(btnRegActividad, true, "");
                SetPermiso(btnConsultaAct, true, "");
                SetPermiso(btnEntornos, true, "");
                SetPermiso(btnInventario, true, "");
            }
            else // invitado/usuario
            {
                SetPermiso(btnConsultaAct, true, "");
            }
        }

        // ===== Toolbar / impresión =====
        private void SetupToolbar()
        {
            tool = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            btnPrint = new ToolStripButton("Imprimir");
            btnCloseTab = new ToolStripButton("Cerrar pestaña");

            btnPrint.Click += (s, e) => PrintActiveChild();
            btnCloseTab.Click += (s, e) => CloseActiveChild();

            tool.Items.Add(btnPrint);
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(btnCloseTab);
            Controls.Add(tool);

            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void PrintActiveChild()
        {
            if (activeChild == null)
            {
                MessageBox.Show("No hay contenido para imprimir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Captura rápida vía DrawToBitmap (diseño original)
            printBmp = new Bitmap(activeChild.ClientSize.Width, activeChild.ClientSize.Height);
            activeChild.DrawToBitmap(printBmp, new Rectangle(Point.Empty, activeChild.ClientSize));

            using (var dlg = new PrintDialog())
            {
                dlg.Document = printDoc;
                if (dlg.ShowDialog() == DialogResult.OK)
                    printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (printBmp == null) { e.HasMorePages = false; return; }

            var area = e.MarginBounds;
            float scale = Math.Min((float)area.Width / printBmp.Width, (float)area.Height / printBmp.Height);
            int w = (int)(printBmp.Width * scale);
            int h = (int)(printBmp.Height * scale);
            e.Graphics.DrawImage(printBmp, area.X, area.Y, w, h);
            e.HasMorePages = false;
        }

        // ===== Navegación / carga de child =====
        private void CloseActiveChild()
        {
            if (activeChild != null)
            {
                try { activeChild.Close(); } catch { }
                activeChild = null;
            }

            panelContenido.Controls.Clear();
            var banner = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Text = "Seleccione una opción del menú",
                ForeColor = Color.FromArgb(5, 80, 45)
            };
            panelContenido.Controls.Add(banner);
        }

        private void LoadChild(Form child)
        {
            if (activeChild != null)
            {
                try { activeChild.Close(); } catch { }
                activeChild = null;
            }

            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            panelContenido.SuspendLayout();
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(child);
            panelContenido.ResumeLayout();

            child.Show();
            activeChild = child;
        }

        private void MarkActive(Button btn)
        {
            foreach (Control c in panelMenu.Controls)
                if (c is Button b)
                {
                    b.BackColor = Color.FromArgb(17, 105, 59);
                    b.ForeColor = Color.White;
                }

            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(5, 80, 45);
                btn.ForeColor = Color.White;
            }
        }

        // ===== Handlers =====
        private void btnHome_Click(object s, EventArgs e) { MarkActive(s as Button); CloseActiveChild(); }

        // Aunque la feature esté apagada, dejamos el handler por si se activa en el futuro
        private void btnRegActividad_Click(object s, EventArgs e)
        {
            if (!FEATURE_REGISTRO_ACTIVIDAD) return;
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Registro_de_Actividad());
        }

        private void btnRegVenta_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Registro_de_una_Venta());
        }

        private void btnConsultaAct_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Consulta_de_Actividad());
        }

        private void btnConsultaUsuarios_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Consulta_de_Usuarios__Modificacion__Baja());
        }

        private void btnConsultaVentas_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Consulta_de_Ventas());
        }

        private void btnEntornos_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Consulta_de_Entornos_Formativos__Modificar_Eliminar());
        }

        private void btnInventario_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Inventario());
        }

        private void btnAltaUsuarios_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Alta_de_usuarios());
        }

        private void btnAltaEntornos_Click(object s, EventArgs e)
        {
            var b = s as Button; if (Denegado(b)) return;
            MarkActive(b); LoadChild(new Alta_de_Entornos_Formativos());
        }

        private void btnCerrarSesion_Click(object s, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea cerrar sesión?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Hide();
            using (var login = new Form1())
            {
                var result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        var prop = login.GetType().GetProperty("RolSeleccionado");
                        var valor = prop != null ? prop.GetValue(login) as string : null;
                        rolUsuario = NormalizarRol(string.IsNullOrWhiteSpace(valor) ? "usuario" : valor);
                    }
                    catch { rolUsuario = "usuario"; }

                    Show();
                    AplicarPermisosPorRol();
                    MarkActive(null);
                    CloseActiveChild();
                    btnHome.PerformClick();
                }
                else
                {
                    Close();
                }
            }
        }
    }
}


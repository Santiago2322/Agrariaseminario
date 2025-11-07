using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Form1 : Form
    {
        private readonly ErrorProvider ep = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private const string CONN =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public string RolSeleccionado { get; private set; } = "invitado";
        public bool AuthSucceeded { get; private set; } = false;

        private bool _wired;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            WireEventsOnce();
        }

        private void WireEventsOnce()
        {
            if (_wired) return; _wired = true;
            btnContinuar.Click -= btnContinuar_Click;
            lnkOlvide.LinkClicked -= lnkOlvide_LinkClicked;
            txtUsuario.TextChanged -= txtUsuario_TextChanged;
            txtClave.TextChanged -= txtClave_TextChanged;

            btnContinuar.Click += btnContinuar_Click;
            lnkOlvide.LinkClicked += lnkOlvide_LinkClicked;
            txtUsuario.TextChanged += txtUsuario_TextChanged;
            txtClave.TextChanged += txtClave_TextChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ep.SetError(txtUsuario, "");
            ep.SetError(txtClave, "");
            try { txtClave.UseSystemPasswordChar = true; } catch { }
            txtUsuario.Focus();
        }

        private void txtUsuario_TextChanged(object s, EventArgs e) =>
            ep.SetError(txtUsuario, string.IsNullOrWhiteSpace(txtUsuario.Text) ? "Usuario requerido" : "");

        private void txtClave_TextChanged(object s, EventArgs e) =>
            ep.SetError(txtClave, string.IsNullOrWhiteSpace(txtClave.Text) ? "Contraseña requerida" : "");

        private void lnkOlvide_LinkClicked(object s, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new FormOlvide_mi_Contraseña())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            string usuario = (txtUsuario.Text ?? "").Trim();
            string contrasenia = (txtClave.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasenia))
            {
                MessageBox.Show("Completá usuario y contraseña.", "Faltan datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rol = AutenticarYTraerRol(usuario, contrasenia);
            if (rol == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos, o cuenta inactiva.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtClave.SelectAll();
                txtClave.Focus();
                return;
            }

            RolSeleccionado = NormalizarRol(rol);
            AuthSucceeded = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string AutenticarYTraerRol(string usuario, string contrasenia)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "EXEC dbo.sp_Usuarios_Login @UsuarioLogin, @Contrasenia";
                cmd.Parameters.AddWithValue("@UsuarioLogin", usuario);
                cmd.Parameters.AddWithValue("@Contrasenia", contrasenia);

                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;

                    var estado = rd["Estado"] as string;
                    if (!string.IsNullOrWhiteSpace(estado))
                    {
                        var e = estado.Trim().ToLowerInvariant();
                        if (!(e == "activo" || e == "habilitado" || e == "1")) return null;
                    }
                    return rd["Rol"] as string ?? "invitado";
                }
            }
        }

        private static string NormalizarRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return "invitado";
            var r = rol.Trim().ToLowerInvariant();

            if (r.Contains("admin")) return "admin";
            if (r.Contains("jefe")) return "jefe de area";
            if (r.Contains("docente") || r.Contains("profesor")) return "docente";

            return "invitado";
        }
    }
}

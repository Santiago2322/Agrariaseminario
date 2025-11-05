using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Form1 : Form
    {
        private readonly ErrorProvider ep = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        // 🔗 Conexión a tu base de datos AGRARIA
        private const string CONN =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public string RolSeleccionado { get; private set; } = "usuario";   // Valor por defecto

        public Form1()
        {
            InitializeComponent();

            // Eventos manuales por si el diseñador no los guardó
            this.Load += Form1_Load;
            this.btnContinuar.Click += btnContinuar_Click;
            this.lnkOlvide.LinkClicked += lnkOlvide_LinkClicked;
            this.txtUsuario.TextChanged += txtUsuario_TextChanged;
            this.txtClave.TextChanged += txtClave_TextChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ep.SetError(txtUsuario, "");
            ep.SetError(txtClave, "");

            try { txtClave.UseSystemPasswordChar = true; } catch { }
            txtUsuario.Focus();
        }

        // ==================================================
        // VALIDACIONES DE CAMPOS
        // ==================================================
        private void txtUsuario_TextChanged(object sender, EventArgs e) =>
            ep.SetError(txtUsuario, string.IsNullOrWhiteSpace(txtUsuario.Text) ? "Usuario requerido" : "");

        private void txtClave_TextChanged(object sender, EventArgs e) =>
            ep.SetError(txtClave, string.IsNullOrWhiteSpace(txtClave.Text) ? "Contraseña requerida" : "");

        // ==================================================
        // LINK: OLVIDÉ MI CONTRASEÑA
        // ==================================================
        private void lnkOlvide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new FormOlvide_mi_Contraseña())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }

        // ==================================================
        // BOTÓN: CONTINUAR / LOGIN
        // ==================================================
        private void btnContinuar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasenia = txtClave.Text;

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

            MessageBox.Show($"Inicio de sesión correcto.\nRol detectado: {RolSeleccionado}",
                "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ==================================================
        // MÉTODO DE LOGIN (usa SP con usuario + contraseña)
        // ==================================================
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
                    if (!rd.Read())
                        return null; // No existe o credenciales incorrectas

                    var estado = rd["Estado"] as string;
                    if (!string.IsNullOrWhiteSpace(estado))
                    {
                        var e = estado.Trim().ToLower();
                        if (!(e == "activo" || e == "habilitado" || e == "1"))
                            return null; // Existe pero está inactivo
                    }

                    return rd["Rol"] as string ?? "usuario";
                }
            }
        }

        // ==================================================
        // NORMALIZADOR DE ROLES
        // ==================================================
        private static string NormalizarRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol))
                return "usuario";

            // Limpia y convierte a minúsculas
            var r = rol.Trim().ToLowerInvariant();

            // Detecta equivalencias
            if (r.Contains("admin") || r.Contains("jefe"))
                return "admin";

            if (r.Contains("docente") || r.Contains("profesor"))
                return "docente";

            return "usuario";
        }
    }
}

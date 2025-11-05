using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_usuarios : Form
    {
        // Conexión directa a la base "Agraria" en localhost\SQLEXPRESS
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        // Importante: NO se permite crear Admin desde este form
        private static readonly string[] ROLES_PERMITIDOS = { "Jefe de area", "Docente", "Invitado" };

        public Alta_de_usuarios()
        {
            InitializeComponent();

            // Handlers principales
            Load += Alta_de_usuarios_Load;
            buttonGuardar.Click += buttonGuardar_Click;
            buttonCancelar.Click += (s, e) => Close();

            // Validaciones de entrada
            textBoxDNI.KeyPress += soloNumeros_KeyPress;
            textBoxTelefono.KeyPress += tel_KeyPress;

            // Mitiga warning UIA (COM) en combos
            PrepararComboSeguro(comboBoxRol);
            PrepararComboSeguro(comboBoxEstado);
            PrepararComboSeguro(comboBoxArea);
            PrepararComboSeguro(comboBoxPreguntaSeg);
        }

        private void Alta_de_usuarios_Load(object sender, EventArgs e)
        {
            // Rol
            comboBoxRol.Items.Clear();
            comboBoxRol.Items.AddRange(ROLES_PERMITIDOS);
            if (comboBoxRol.Items.Count > 0) comboBoxRol.SelectedIndex = 0;

            // Estado
            comboBoxEstado.Items.Clear();
            comboBoxEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });
            comboBoxEstado.SelectedIndex = 0;

            // Área
            comboBoxArea.Items.Clear();
            comboBoxArea.Items.AddRange(new object[] { "Administración", "Animal", "Vegetal", "Vivero", "Huerta" });

            // Pregunta de seguridad
            if (comboBoxPreguntaSeg.Items.Count == 0)
            {
                comboBoxPreguntaSeg.Items.AddRange(new object[] {
                    "¿Nombre de tu primera mascota?",
                    "¿Ciudad donde naciste?",
                    "¿Comida favorita?"
                });
            }
        }

        // --- Guardar ---
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text) ||
                string.IsNullOrWhiteSpace(textBoxApellido.Text) ||
                string.IsNullOrWhiteSpace(textBoxUsuario.Text) ||
                comboBoxRol.SelectedItem == null)
            {
                MessageBox.Show("Complete Nombre, Apellido, Usuario y Rol.", "Campos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rolElegido = (comboBoxRol.SelectedItem?.ToString() ?? "").Trim();

            // Bloqueo explícito de Admin
            var textoRol = rolElegido.ToLowerInvariant();
            if (textoRol == "admin" || textoRol == "administrador")
            {
                MessageBox.Show("No está permitido crear usuarios con rol Administrador.", "Rol restringido",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ROLES_PERMITIDOS.Contains(rolElegido))
            {
                MessageBox.Show("Rol no permitido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // UsuarioLogin único
            var userLogin = textBoxUsuario.Text.Trim();
            if (ExisteUsuario(userLogin))
            {
                MessageBox.Show("Ya existe un usuario con ese login.", "Duplicado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insert (sin contraseña en el esquema actual)
            const string SQL = @"
INSERT INTO dbo.Usuarios
 (Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin,
  Direccion, Localidad, Provincia, Observaciones,
  Rol, Estado, Area, PreguntaSeguridad, RespuestaSeguridad)
VALUES
 (@Nombre, @Apellido, @DNI, @Email, @Telefono, @UsuarioLogin,
  @Direccion, @Localidad, @Provincia, @Observaciones,
  @Rol, @Estado, @Area, @PreguntaSeguridad, @RespuestaSeguridad);";

            try
            {
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = new SqlCommand(SQL, cn))
                {
                    object NV(string s) => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();

                    cmd.Parameters.AddWithValue("@Nombre", textBoxNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Apellido", textBoxApellido.Text.Trim());
                    cmd.Parameters.AddWithValue("@DNI", NV(textBoxDNI.Text));
                    cmd.Parameters.AddWithValue("@Email", NV(textBoxEmail.Text));
                    cmd.Parameters.AddWithValue("@Telefono", NV(textBoxTelefono.Text));
                    cmd.Parameters.AddWithValue("@UsuarioLogin", userLogin);
                    cmd.Parameters.AddWithValue("@Direccion", NV(textBoxDireccion.Text));
                    cmd.Parameters.AddWithValue("@Localidad", NV(textBoxLocalidad.Text));
                    cmd.Parameters.AddWithValue("@Provincia", NV(textBoxProvincia.Text));
                    cmd.Parameters.AddWithValue("@Observaciones", NV(textBoxObservaciones.Text));
                    cmd.Parameters.AddWithValue("@Rol", rolElegido);
                    cmd.Parameters.AddWithValue("@Estado", comboBoxEstado.Text?.Trim() ?? "Activo");
                    cmd.Parameters.AddWithValue("@Area", comboBoxArea.Text?.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PreguntaSeguridad", NV(comboBoxPreguntaSeg.Text));
                    cmd.Parameters.AddWithValue("@RespuestaSeguridad", NV(textBoxRespuestaSeg.Text));

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario registrado correctamente.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Utilidades ---
        private bool ExisteUsuario(string usuarioLogin)
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = new SqlCommand("SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin=@u", cn))
            {
                cmd.Parameters.AddWithValue("@u", usuarioLogin);
                cn.Open();
                var o = cmd.ExecuteScalar();
                return o != null && o != DBNull.Value;
            }
        }

        private static void PrepararComboSeguro(ComboBox cbo)
        {
            if (cbo == null) return;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo.AutoCompleteMode = AutoCompleteMode.None;
            cbo.AutoCompleteSource = AutoCompleteSource.None;
            try { cbo.AccessibleRole = AccessibleRole.ComboBox; } catch { }
        }

        private void soloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void tel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !"-()+ ".Contains(e.KeyChar))
                e.Handled = true;
        }
    }
}

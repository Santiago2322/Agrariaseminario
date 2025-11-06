using System;
using System.ComponentModel;           // ErrorProvider
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;  // Regex para email/usuario
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_usuarios : Form
    {
        // 🔗 Conexión
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        // Roles permitidos
        private static readonly string[] ROLES_PERMITIDOS = { "Jefe de area", "Docente", "Invitado" };

        // Límites máximos
        private const int MAX_NOMBRE = 50;
        private const int MAX_APELLIDO = 50;
        private const int MAX_DNI = 12;
        private const int MAX_EMAIL = 100;
        private const int MAX_TELEFONO = 20;
        private const int MAX_USUARIO = 30;
        private const int MAX_DIRECCION = 120;
        private const int MAX_LOCALIDAD = 80;
        private const int MAX_PROVINCIA = 80;
        private const int MAX_OBSERVACIONES = 300;
        private const int MAX_RESP_SEG = 120;

        // Límites mínimos (si el campo fue cargado)
        private const int MIN_DNI = 7;
        private const int MIN_TELEFONO = 6;
        private const int MIN_USUARIO = 4;

        // 🔔 ErrorProvider
        private readonly ErrorProvider ep = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        // Regex precompilados (C# 7.3 OK)
        private static readonly Regex RX_EMAIL =
            new Regex(@"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);

        private static readonly Regex RX_USUARIO =
            new Regex(@"^[A-Za-z0-9._\-]+$", RegexOptions.Compiled);

        public Alta_de_usuarios()
        {
            InitializeComponent();

            // Handlers principales
            Load += Alta_de_usuarios_Load;
            buttonGuardar.Click += buttonGuardar_Click;
            buttonCancelar.Click += (s, e) => Close();

            // Validaciones de entrada por tecla
            textBoxDNI.KeyPress += soloNumeros_KeyPress;
            textBoxTelefono.KeyPress += tel_KeyPress;
            textBoxNombre.KeyPress += soloLetras_KeyPress;
            textBoxApellido.KeyPress += soloLetras_KeyPress;

            // Sanitizadores (capturan PEGADO / arrastre / escritura)
            // — DNI y Teléfono: solo dígitos
            AttachSanitizer(textBoxDNI, SanitizeDigits);
            AttachSanitizer(textBoxTelefono, SanitizeDigits);
            // — Nombre y Apellido: solo letras/espacios/acentos
            AttachSanitizer(textBoxNombre, SanitizeLettersSpaces);
            AttachSanitizer(textBoxApellido, SanitizeLettersSpaces);
            // — Usuario: solo A–Z a–z 0–9 . _ -
            AttachSanitizer(textBoxUsuario, SanitizeUsuario);

            // Validación reactiva
            textBoxNombre.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxApellido.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxDNI.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxEmail.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxTelefono.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxUsuario.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxDireccion.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxLocalidad.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxProvincia.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxObservaciones.TextChanged += (s, e) => ValidarFormulario(false);
            textBoxRespuestaSeg.TextChanged += (s, e) => ValidarFormulario(false);

            comboBoxRol.SelectedIndexChanged += (s, e) => ValidarFormulario(false);
            comboBoxEstado.SelectedIndexChanged += (s, e) => ValidarFormulario(false);
            comboBoxArea.SelectedIndexChanged += (s, e) => ValidarFormulario(false);
            comboBoxPreguntaSeg.SelectedIndexChanged += (s, e) => ValidarFormulario(false);

            // Mitiga warning UIA (COM) en combos
            PrepararComboSeguro(comboBoxRol);
            PrepararComboSeguro(comboBoxEstado);
            PrepararComboSeguro(comboBoxArea);
            PrepararComboSeguro(comboBoxPreguntaSeg);
        }

        private void Alta_de_usuarios_Load(object sender, EventArgs e)
        {
            // MaxLength
            textBoxNombre.MaxLength = MAX_NOMBRE;
            textBoxApellido.MaxLength = MAX_APELLIDO;
            textBoxDNI.MaxLength = MAX_DNI;
            textBoxEmail.MaxLength = MAX_EMAIL;
            textBoxTelefono.MaxLength = MAX_TELEFONO;
            textBoxUsuario.MaxLength = MAX_USUARIO;
            textBoxDireccion.MaxLength = MAX_DIRECCION;
            textBoxLocalidad.MaxLength = MAX_LOCALIDAD;
            textBoxProvincia.MaxLength = MAX_PROVINCIA;
            textBoxObservaciones.MaxLength = MAX_OBSERVACIONES;
            textBoxRespuestaSeg.MaxLength = MAX_RESP_SEG;

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

            // Validación inicial
            ValidarFormulario(false);
        }

        // ===== Guardar =====
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario(true)) return;

            var rolElegido = (comboBoxRol.SelectedItem == null ? "" : comboBoxRol.SelectedItem.ToString()).Trim();

            // UsuarioLogin único
            var userLogin = textBoxUsuario.Text.Trim();
            if (ExisteUsuario(userLogin))
            {
                MessageBox.Show("Ya existe un usuario con ese login.", "Duplicado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ep.SetError(textBoxUsuario, "Ya existe un usuario con ese login.");
                textBoxUsuario.Focus();
                return;
            }

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
                    Func<string, object> NV = s => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();

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

                    var estado = (comboBoxEstado.Text == null ? "" : comboBoxEstado.Text.Trim());
                    if (string.IsNullOrEmpty(estado)) estado = "Activo";
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    cmd.Parameters.AddWithValue("@Area", NV(comboBoxArea.Text));
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

        // ======= VALIDACIÓN CENTRAL con ErrorProvider =======
        private bool ValidarFormulario(bool mostrarMensajes)
        {
            // Limpio errores
            ep.SetError(textBoxNombre, "");
            ep.SetError(textBoxApellido, "");
            ep.SetError(textBoxUsuario, "");
            ep.SetError(comboBoxRol, "");
            ep.SetError(textBoxDNI, "");
            ep.SetError(textBoxTelefono, "");
            ep.SetError(textBoxEmail, "");
            ep.SetError(textBoxDireccion, "");
            ep.SetError(textBoxLocalidad, "");
            ep.SetError(textBoxProvincia, "");
            ep.SetError(textBoxObservaciones, "");
            ep.SetError(comboBoxEstado, "");
            ep.SetError(comboBoxArea, "");
            ep.SetError(comboBoxPreguntaSeg, "");
            ep.SetError(textBoxRespuestaSeg, "");

            Control primerError = null;
            Action<Control, string> Err = (ctrl, msg) =>
            {
                ep.SetError(ctrl, msg);
                if (primerError == null) primerError = ctrl;
            };

            // Requeridos mínimos
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
                Err(textBoxNombre, "Requerido.");
            if (string.IsNullOrWhiteSpace(textBoxApellido.Text))
                Err(textBoxApellido, "Requerido.");
            if (string.IsNullOrWhiteSpace(textBoxUsuario.Text))
                Err(textBoxUsuario, "Requerido.");
            if (comboBoxRol.SelectedItem == null)
                Err(comboBoxRol, "Seleccione un rol.");

            // Formatos de nombre/apellido
            if (!string.IsNullOrWhiteSpace(textBoxNombre.Text) && !EsSoloLetrasEspacios(textBoxNombre.Text))
                Err(textBoxNombre, "Solo letras y espacios.");

            if (!string.IsNullOrWhiteSpace(textBoxApellido.Text) && !EsSoloLetrasEspacios(textBoxApellido.Text))
                Err(textBoxApellido, "Solo letras y espacios.");

            // DNI y Teléfono (si fueron cargados)
            var dniTrim = (textBoxDNI.Text ?? "").Trim();
            if (dniTrim.Length > 0)
            {
                if (!EsSoloDigitos(dniTrim)) Err(textBoxDNI, "Solo números.");
                else
                {
                    if (dniTrim.Length < MIN_DNI) Err(textBoxDNI, "Demasiado corto.");
                    if (dniTrim.Length > MAX_DNI) Err(textBoxDNI, "Se supera el máximo.");
                }
            }

            var telTrim = (textBoxTelefono.Text ?? "").Trim();
            if (telTrim.Length > 0)
            {
                if (!EsSoloDigitos(telTrim)) Err(textBoxTelefono, "Solo números.");
                else
                {
                    if (telTrim.Length < MIN_TELEFONO) Err(textBoxTelefono, "Demasiado corto.");
                    if (telTrim.Length > MAX_TELEFONO) Err(textBoxTelefono, "Se supera el máximo.");
                }
            }

            // Email (si fue cargado)
            var emailTrim = (textBoxEmail.Text ?? "").Trim();
            if (emailTrim.Length > 0)
            {
                if (emailTrim.Length > MAX_EMAIL) Err(textBoxEmail, "Se supera el máximo.");
                else if (!EsEmailValido(emailTrim)) Err(textBoxEmail, "Formato inválido.");
                else if (!DominioEmailPlausible(emailTrim)) Err(textBoxEmail, "Dominio inválido.");
            }

            // UsuarioLogin: formato y longitudes
            var usuarioTrim = (textBoxUsuario.Text ?? "").Trim();
            if (usuarioTrim.Length > 0)
            {
                if (usuarioTrim.Length < MIN_USUARIO) Err(textBoxUsuario, "Mínimo 4 caracteres.");
                if (usuarioTrim.Length > MAX_USUARIO) Err(textBoxUsuario, "Se supera el máximo.");
                if (!RX_USUARIO.IsMatch(usuarioTrim)) Err(textBoxUsuario, "Solo letras, números y . _ -");
                if (usuarioTrim.Contains(" ")) Err(textBoxUsuario, "No se permiten espacios.");
            }

            // Longitudes de otros (suaves)
            if ((textBoxDireccion.Text ?? "").Length > MAX_DIRECCION)
                Err(textBoxDireccion, "Se supera el máximo.");
            if ((textBoxLocalidad.Text ?? "").Length > MAX_LOCALIDAD)
                Err(textBoxLocalidad, "Se supera el máximo.");
            if ((textBoxProvincia.Text ?? "").Length > MAX_PROVINCIA)
                Err(textBoxProvincia, "Se supera el máximo.");
            if ((textBoxObservaciones.Text ?? "").Length > MAX_OBSERVACIONES)
                Err(textBoxObservaciones, "Se supera el máximo.");
            if ((textBoxRespuestaSeg.Text ?? "").Length > MAX_RESP_SEG)
                Err(textBoxRespuestaSeg, "Se supera el máximo.");

            // Rol permitido y bloqueo admin
            var rolElegido = (comboBoxRol.SelectedItem == null ? "" : comboBoxRol.SelectedItem.ToString()).Trim();
            var rolLower = rolElegido.ToLowerInvariant();
            if (rolLower == "admin" || rolLower == "administrador")
                Err(comboBoxRol, "No se permite Administrador.");
            else if (rolElegido.Length > 0 && !ROLES_PERMITIDOS.Contains(rolElegido))
                Err(comboBoxRol, "Rol no permitido.");

            // Resultado
            bool ok = (primerError == null);
            if (!ok && mostrarMensajes)
            {
                MessageBox.Show("Revisá los campos marcados.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try { primerError.Focus(); } catch { }
            }
            return ok;
        }

        // ====== Sanitizadores (para Pegar/Escribir) ======
        private void AttachSanitizer(TextBox txt, Func<string, string> sanitizer)
        {
            // Importante: no crear bucles — solo reasignar si cambia.
            txt.TextChanged += (s, e) =>
            {
                var tb = (TextBox)s;
                var before = tb.Text;
                var caret = tb.SelectionStart;
                var cleaned = sanitizer(before);

                if (cleaned != before)
                {
                    tb.Text = cleaned;
                    // Reposicionar caret lo mejor posible
                    tb.SelectionStart = Math.Min(caret, cleaned.Length);
                }
            };

            // Aceptamos pegar desde menú o Ctrl+V (TextChanged lo corrige).
            // Evitamos arrastrar texto (por las dudas):
            try { txt.AllowDrop = false; } catch { }
        }

        // Deja solo dígitos
        private string SanitizeDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var arr = new char[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
                if (char.IsDigit(s[i])) arr[j++] = s[i];
            return new string(arr, 0, j);
        }

        // Deja letras, espacios y acentos típicos
        private string SanitizeLettersSpaces(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var arr = new char[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (char.IsLetter(c) || c == ' ' || EsLetraAcentuada(c))
                    arr[j++] = c;
            }
            return new string(arr, 0, j);
        }

        // Deja A–Z a–z 0–9 . _ -
        private string SanitizeUsuario(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var arr = new char[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if ((c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') ||
                    c == '.' || c == '_' || c == '-')
                    arr[j++] = c;
            }
            return new string(arr, 0, j);
        }

        // ===== Utilidades de datos =====
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

        // ===== KeyPress =====
        private void soloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !"-()+ ".Contains(e.KeyChar))
                e.Handled = true;
        }

        private void soloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (char.IsLetter(e.KeyChar) || e.KeyChar == ' ' || EsLetraAcentuada(e.KeyChar))
                return;
            e.Handled = true;
        }

        // ===== Helpers de validación =====
        private static bool EsSoloDigitos(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;
            for (int i = 0; i < s.Length; i++)
                if (!char.IsDigit(s[i])) return false;
            return true;
        }

        private static bool EsSoloLetrasEspacios(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (!(char.IsLetter(c) || c == ' ' || EsLetraAcentuada(c)))
                    return false;
            }
            return true;
        }

        private static bool EsLetraAcentuada(char c)
        {
            switch (c)
            {
                case 'á':
                case 'é':
                case 'í':
                case 'ó':
                case 'ú':
                case 'Á':
                case 'É':
                case 'Í':
                case 'Ó':
                case 'Ú':
                case 'ñ':
                case 'Ñ':
                    return true;
                default: return false;
            }
        }

        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            if (email.Length > MAX_EMAIL) return false;
            if (!RX_EMAIL.IsMatch(email)) return false;

            int at = email.IndexOf('@');
            if (at <= 0 || at >= email.Length - 1) return false;
            string dominio = email.Substring(at + 1);
            if (dominio.StartsWith("-") || dominio.EndsWith("-")) return false;
            if (dominio.Contains("..")) return false;

            return true;
        }

        private static bool DominioEmailPlausible(string email)
        {
            int at = email.IndexOf('@');
            if (at < 0) return false;
            string dominio = email.Substring(at + 1);
            int p = dominio.IndexOf('.');
            return (p > 0 && p < dominio.Length - 1);
        }
    }
}

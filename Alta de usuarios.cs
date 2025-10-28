using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient; // NECESARIO
using System.Data;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_usuarios : Form
    {
        // 1. Cadena de conexión integrada (misma que en Form1.cs)
        private const string CADENA_CONEXION =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        public Alta_de_usuarios()
        {
            InitializeComponent();

            this.Load += Alta_de_usuarios_Load;
        }

        private void Alta_de_usuarios_Load(object sender, EventArgs e)
        {
            // Poblar combos (si ya los llenás en otro lado, dejá esto comentado)
            if (comboBoxRol.Items.Count == 0)
                comboBoxRol.Items.AddRange(new object[] { "Administrador", "Docente", "Alumno", "Invitado" });

            if (comboBoxEstado.Items.Count == 0)
            {
                comboBoxEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });
                comboBoxEstado.SelectedIndex = 0;
            }

            if (comboBoxArea.Items.Count == 0)
                comboBoxArea.Items.AddRange(new object[] { "Administración", "Animal", "Vegetal", "Vivero", "Huerta" });

            // Aplico el layout con variables (asegura que se vean bien dentro del panel contenedor)
            AplicarLayout();
        }

        /// <summary>
        /// Método auxiliar para posicionar los controles, ignorado por el diseñador.
        /// </summary>
        private void AplicarLayout()
        {
            // Columna izquierda
            int x1 = 28, x2 = 180, y = 60, sepY = 34, wBox = 260, h = 24;

            labelNombre.Location = new Point(x1, y);
            textBoxNombre.Location = new Point(x2, y - 3);
            textBoxNombre.Size = new Size(wBox, h);

            y += sepY;
            labelApellido.Location = new Point(x1, y);
            textBoxApellido.Location = new Point(x2, y - 3);
            textBoxApellido.Size = new Size(wBox, h);

            y += sepY;
            labelDNI.Location = new Point(x1, y);
            textBoxDNI.Location = new Point(x2, y - 3);
            textBoxDNI.Size = new Size(wBox, h);

            y += sepY;
            labelEmail.Location = new Point(x1, y);
            textBoxEmail.Location = new Point(x2, y - 3);
            textBoxEmail.Size = new Size(wBox, h);

            y += sepY;
            labelTelefono.Location = new Point(x1, y);
            textBoxTelefono.Location = new Point(x2, y - 3);
            textBoxTelefono.Size = new Size(wBox, h);

            y += sepY;
            labelUsuario.Location = new Point(x1, y);
            textBoxUsuario.Location = new Point(x2, y - 3);
            textBoxUsuario.Size = new Size(wBox, h);

            y += sepY;
            labelContrasenia.Location = new Point(x1, y);
            textBoxContrasenia.Location = new Point(x2, y - 3);
            textBoxContrasenia.Size = new Size(wBox, h);
            textBoxContrasenia.UseSystemPasswordChar = true;

            y += sepY;
            labelDireccion.Location = new Point(x1, y);
            textBoxDireccion.Location = new Point(x2, y - 3);
            textBoxDireccion.Size = new Size(wBox, h);

            y += sepY;
            labelLocalidad.Location = new Point(x1, y);
            textBoxLocalidad.Location = new Point(x2, y - 3);
            textBoxLocalidad.Size = new Size(wBox, h);

            y += sepY;
            labelProvincia.Location = new Point(x1, y);
            textBoxProvincia.Location = new Point(x2, y - 3);
            textBoxProvincia.Size = new Size(wBox, h);

            y += sepY;
            labelObservaciones.Location = new Point(x1, y);
            textBoxObservaciones.Location = new Point(x2, y - 3);
            textBoxObservaciones.Size = new Size(wBox, 60);

            // Columna derecha (combos)
            int x3 = 480;
            int y2 = 60;
            int wBox2 = wBox - 60;

            labelRol.Location = new Point(x3, y2);
            comboBoxRol.Location = new Point(x3 + 140, y2 - 3);
            comboBoxRol.Size = new Size(wBox2, h);

            y2 += sepY;
            labelEstado.Location = new Point(x3, y2);
            comboBoxEstado.Location = new Point(x3 + 140, y2 - 3);
            comboBoxEstado.Size = new Size(wBox2, h);

            y2 += sepY;
            labelArea.Location = new Point(x3, y2);
            comboBoxArea.Location = new Point(x3 + 140, y2 - 3);
            comboBoxArea.Size = new Size(wBox2, h);

            // Botones
            buttonGuardar.Location = new Point(480, 460);
            buttonGuardar.Size = new Size(110, 32);

            buttonCancelar.Location = new Point(600, 460);
            buttonCancelar.Size = new Size(110, 32);
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            // 1. Validaciones RÁPIDAS
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text) || string.IsNullOrWhiteSpace(textBoxDNI.Text) ||
                string.IsNullOrWhiteSpace(textBoxUsuario.Text) || string.IsNullOrWhiteSpace(textBoxContrasenia.Text) ||
                string.IsNullOrWhiteSpace(comboBoxRol.Text) || string.IsNullOrWhiteSpace(comboBoxEstado.Text))
            {
                MessageBox.Show("Complete los campos obligatorios (Nombre, DNI, Usuario, Contraseña, Rol y Estado).",
                                "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lógica SQL directa en el formulario (usando la cadena local)
            string sqlInsert = @"
                INSERT INTO Usuarios 
                (Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin, Contrasenia, Direccion, Localidad, 
                 Provincia, Observaciones, Rol, Estado, Area)
                VALUES 
                (@Nombre, @Apellido, @DNI, @Email, @Telefono, @UsuarioLogin, @Contrasenia, @Direccion, @Localidad, 
                 @Provincia, @Observaciones, @Rol, @Estado, @Area)";

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = new SqlCommand(sqlInsert, cn))
            {
                // Asignación de parámetros
                cmd.Parameters.AddWithValue("@Nombre", textBoxNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Apellido", textBoxApellido.Text.Trim());
                cmd.Parameters.AddWithValue("@DNI", textBoxDNI.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", (object)textBoxEmail.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefono", (object)textBoxTelefono.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UsuarioLogin", textBoxUsuario.Text.Trim());
                cmd.Parameters.AddWithValue("@Contrasenia", textBoxContrasenia.Text);
                cmd.Parameters.AddWithValue("@Direccion", (object)textBoxDireccion.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Localidad", (object)textBoxLocalidad.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Provincia", (object)textBoxProvincia.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Observaciones", (object)textBoxObservaciones.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rol", comboBoxRol.Text);
                cmd.Parameters.AddWithValue("@Estado", comboBoxEstado.Text);
                cmd.Parameters.AddWithValue("@Area", (object)comboBoxArea.Text ?? DBNull.Value);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Usuario registrado con éxito.", "Confirmación",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cierra el formulario para volver al menú principal (el panel contenedor)
                    this.Close();
                }
                catch (SqlException ex)
                {
                    // Manejo de errores comunes de unicidad (DNI o UsuarioLogin duplicados)
                    string mensajeError = ex.Number == 2627 ? "Error: El DNI o el Nombre de Usuario ya existe. Por favor, verifique." : "Error de DB: " + ex.Message;
                    MessageBox.Show(mensajeError, "Error al registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error inesperado:\n" + ex.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            // Cierra el formulario para volver al menú principal (el panel contenedor)
            this.Close();
        }

        // Validación para DNI (solo números)
        private void textBoxDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Validación para Teléfono (números y caracteres comunes de teléfono)
        private void textBoxTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                !"-()+ ".Contains(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
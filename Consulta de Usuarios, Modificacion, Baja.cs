using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing; // Necesario para Point

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Usuarios__Modificacion__Baja : Form
    {
        // La cadena de conexión integrada
        private const string CADENA_CONEXION =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        // Cadenas necesarias para la configuración inicial de la DB
        private const string CADENA_MASTER =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";

        public Consulta_de_Usuarios__Modificacion__Baja()
        {
            InitializeComponent();
            this.Load += Frm_Load;

            // Eventos de IU (se asume que estos controles existen en tu diseñador)
            // Se asume: txtBuscar, txtId, txtNombre, txtApellido, txtDni, txtEmail, txtTelefono, 
            // txtUsuario, txtDireccion, txtLocalidad, txtProvincia, txtObservaciones, cboRol, cboEstado, cboArea.
            dataGridViewUsuarios.SelectionChanged += dataGridViewUsuarios_SelectionChanged;
            button1.Click += button1_Buscar_Click;      // Buscar
            button2.Click += button2_Eliminar_Click;    // Eliminar
            button3.Click += button3_GuardarCambios_Click;  // Guardar
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            try
            {
                // Aseguramos que la DB y la tabla existan ANTES de intentar consultarla
                // Esta es la lógica que debe ejecutar al inicio de la aplicación
                EnsureDatabaseSetup();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar la base de datos para la consulta.\n" + ex.Message,
                    "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Si la DB falla, no cargamos datos
                return;
            }

            CargarUsuarios();

            // Configuración visual del DataGridView
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.ReadOnly = true;
        }

        // ====== CONFIGURACIÓN INICIAL DE DB (Mantenida para simplificación) ======

        // NOTA: Para un desarrollo rápido, se asume que la configuración de LocalDB
        // y la creación de la tabla se mantiene aquí como lo tenías.

        private void EnsureDatabaseSetup()
        {
            // 1. Crear la DB si no existe (usando CADENA_MASTER)
            using (var cn = new SqlConnection(CADENA_MASTER))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "IF DB_ID(N'agraria_basedatos') IS NULL CREATE DATABASE agraria_basedatos;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            // 2. Crear la tabla Usuarios si no existe (usando CADENA_CONEXION)
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(80) NOT NULL,
        Apellido NVARCHAR(80) NOT NULL,
        DNI VARCHAR(12) NOT NULL UNIQUE,
        Email NVARCHAR(120) NOT NULL,
        Telefono NVARCHAR(40) NULL,
        UsuarioLogin NVARCHAR(60) NOT NULL UNIQUE,
        Contrasenia NVARCHAR(200) NOT NULL,
        Direccion NVARCHAR(150) NULL,
        Localidad NVARCHAR(80) NULL,
        Provincia NVARCHAR(80) NULL,
        Observaciones NVARCHAR(300) NULL,
        Rol NVARCHAR(40) NULL,
        Estado NVARCHAR(20) NULL,
        Area NVARCHAR(60) NULL,
        FechaAlta DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaAlta DEFAULT SYSUTCDATETIME()
    );
END";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ====== CONSULTAR & BUSCAR (READ) ======

        private void CargarUsuarios(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter())
            {
                string sql = @"
SELECT IdUsuario, Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin,
        Direccion, Localidad, Provincia, Observaciones, Rol, Estado, Area
FROM    Usuarios";

                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += @" WHERE (Nombre LIKE @f OR Apellido LIKE @f OR DNI LIKE @f OR Email LIKE @f OR UsuarioLogin LIKE @f)";

                da.SelectCommand = new SqlCommand(sql, cn);
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro + "%");

                var dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    dataGridViewUsuarios.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error de DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Ocultar la columna Id
                if (dataGridViewUsuarios.Columns["IdUsuario"] != null)
                    dataGridViewUsuarios.Columns["IdUsuario"].Visible = false;
            }
        }

        private void button1_Buscar_Click(object sender, EventArgs e) =>
            CargarUsuarios(txtBuscar.Text.Trim());

        // ====== SELECCIÓN DE FILA ======

        private void dataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            // Esta lógica precarga los datos del DataGridView a los TextBoxes y ComboBoxes
            if (dataGridViewUsuarios.CurrentRow == null) return;
            var r = dataGridViewUsuarios.CurrentRow;

            // Se asume que estos controles existen en el diseñador
            txtId.Text = Convert.ToString(r.Cells["IdUsuario"]?.Value ?? "");
            txtNombre.Text = Convert.ToString(r.Cells["Nombre"]?.Value ?? "");
            txtApellido.Text = Convert.ToString(r.Cells["Apellido"]?.Value ?? "");
            txtDni.Text = Convert.ToString(r.Cells["DNI"]?.Value ?? "");
            txtEmail.Text = Convert.ToString(r.Cells["Email"]?.Value ?? "");
            txtTelefono.Text = Convert.ToString(r.Cells["Telefono"]?.Value ?? "");
            txtUsuario.Text = Convert.ToString(r.Cells["UsuarioLogin"]?.Value ?? "");
            txtDireccion.Text = Convert.ToString(r.Cells["Direccion"]?.Value ?? "");
            txtLocalidad.Text = Convert.ToString(r.Cells["Localidad"]?.Value ?? "");
            txtProvincia.Text = Convert.ToString(r.Cells["Provincia"]?.Value ?? "");
            txtObservaciones.Text = Convert.ToString(r.Cells["Observaciones"]?.Value ?? "");
            cboRol.Text = Convert.ToString(r.Cells["Rol"]?.Value ?? "");
            cboEstado.Text = Convert.ToString(r.Cells["Estado"]?.Value ?? "");
            cboArea.Text = Convert.ToString(r.Cells["Area"]?.Value ?? "");
        }

        // ====== ELIMINAR (DELETE) ======

        private void button2_Eliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out int idUsuario))
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Seguro que deseas eliminar este usuario?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Usuarios WHERE IdUsuario = @Id";
                cmd.Parameters.AddWithValue("@Id", idUsuario);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario eliminado correctamente.", "Confirmación",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios(txtBuscar.Text.Trim()); // Recargar la lista
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo eliminar.\n" + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====== GUARDAR CAMBIOS (UPDATE) ======

        private void button3_GuardarCambios_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out int idUsuario))
            {
                MessageBox.Show("Seleccioná un usuario y asegúrate que el ID sea válido.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación rápida de campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDni.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(cboRol.Text))
            {
                MessageBox.Show("Los campos Nombre, DNI, Usuario y Rol son obligatorios.", "Error de Validación",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE  Usuarios
   SET  Nombre=@Nombre, Apellido=@Apellido, DNI=@DNI, Email=@Email, Telefono=@Telefono,
        UsuarioLogin=@UsuarioLogin, Direccion=@Direccion, Localidad=@Localidad, Provincia=@Provincia,
        Observaciones=@Observaciones, Rol=@Rol, Estado=@Estado, Area=@Area
 WHERE  IdUsuario=@Id";

                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
                cmd.Parameters.AddWithValue("@DNI", txtDni.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Telefono", (object)txtTelefono.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UsuarioLogin", txtUsuario.Text.Trim());
                // NOTA: Se omite la contraseña en el UPDATE para simplicidad, no se modifica aquí.
                cmd.Parameters.AddWithValue("@Direccion", (object)txtDireccion.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Localidad", (object)txtLocalidad.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Provincia", (object)txtProvincia.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Observaciones", (object)txtObservaciones.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rol", cboRol.Text);
                cmd.Parameters.AddWithValue("@Estado", cboEstado.Text);
                cmd.Parameters.AddWithValue("@Area", cboArea.Text);
                cmd.Parameters.AddWithValue("@Id", idUsuario);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cambios guardados correctamente.",
                        "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios(txtBuscar.Text.Trim()); // Recargar la lista
                }
                catch (SqlException ex)
                {
                    // Manejo de error de duplicidad (DNI/UsuarioLogin)
                    string mensajeError = ex.Number == 2627 ? "Error: El DNI o el Nombre de Usuario ya existe y no se puede duplicar." : ex.Message;
                    MessageBox.Show("No se pudieron aplicar los cambios.\n" + mensajeError,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtId.Clear(); txtNombre.Clear(); txtApellido.Clear(); txtDni.Clear();
            txtEmail.Clear(); txtTelefono.Clear(); txtUsuario.Clear(); txtDireccion.Clear();
            txtLocalidad.Clear(); txtProvincia.Clear(); txtObservaciones.Clear();
            // Intentar deseleccionar combos
            if (cboRol.Items.Count > 0) cboRol.SelectedIndex = -1;
            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = -1;
            if (cboArea.Items.Count > 0) cboArea.SelectedIndex = -1;
        }

        // Aquí se deberían mantener los métodos EnsureLocalDbUp si los tenías.
        // Se omitió la lógica de LocalDB aquí para mantener el foco en el CRUD,
        // asumiendo que EnsureDatabaseSetup ya es suficiente.
    }
}
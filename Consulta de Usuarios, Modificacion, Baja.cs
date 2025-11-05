using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Usuarios__Modificacion__Baja : Form
    {
        // 🔗 Conexión directa a la base "Agraria"
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        private static readonly string[] ROLES_PERMITIDOS = { "Jefe de area", "Docente", "Invitado" };

        public Consulta_de_Usuarios__Modificacion__Baja()
        {
            InitializeComponent();

            this.Load += Frm_Load;

            // 🔘 Botones (ajustá nombres si difieren en el diseñador)
            button1.Click += button1_Buscar_Click;   // Buscar
            button2.Click += button2_Eliminar_Click; // Eliminar
            button3.Click += button3_Guardar_Click;  // Guardar (insert/update)

            // 📋 DataGridView
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.CellClick += DataGridViewUsuarios_CellClick;

            this.AutoScroll = true;
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            // Combo de roles
            cboRol.Items.Clear();
            cboRol.Items.AddRange(ROLES_PERMITIDOS);
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;

            // Estado y Área
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArea.DropDownStyle = ComboBoxStyle.DropDownList;

            // Pregunta de seguridad
            if (cboPreguntaSeg.Items.Count == 0)
            {
                cboPreguntaSeg.Items.AddRange(new object[]
                {
                    "¿Nombre de tu primera mascota?",
                    "¿Ciudad donde naciste?",
                    "¿Comida favorita?"
                });
            }
            cboPreguntaSeg.DropDownStyle = ComboBoxStyle.DropDownList;

            CargarUsuarios();
        }

        // ==== CARGA Y FILTRO ====
        private void CargarUsuarios(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(GetSqlListado(filtro), cn))
            {
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.AddWithValue("@f", $"%{filtro.Trim()}%");

                var dt = new DataTable();
                da.Fill(dt);
                dataGridViewUsuarios.DataSource = dt;
            }
        }

        private string GetSqlListado(string filtro)
        {
            string baseSql = @"
                SELECT 
                    IdUsuario,
                    Nombre,
                    Apellido,
                    DNI,
                    Email,
                    Telefono,
                    UsuarioLogin,
                    Direccion,
                    Localidad,
                    Provincia,
                    Observaciones,
                    Rol,
                    Estado,
                    Area,
                    PreguntaSeguridad,
                    RespuestaSeguridad
                FROM dbo.Usuarios ";

            if (string.IsNullOrWhiteSpace(filtro))
                return baseSql + "ORDER BY Apellido, Nombre;";

            return baseSql + @"
                WHERE (Nombre LIKE @f OR Apellido LIKE @f OR UsuarioLogin LIKE @f OR DNI LIKE @f)
                ORDER BY Apellido, Nombre;";
        }

        private void button1_Buscar_Click(object sender, EventArgs e)
        {
            CargarUsuarios(txtBuscar.Text.Trim());
        }

        // ==== SELECCIÓN DE GRILLA ====
        private void DataGridViewUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridViewUsuarios.Rows[e.RowIndex];
            if (row?.DataBoundItem == null) return;

            txtId.Text = row.Cells["IdUsuario"]?.Value?.ToString();
            txtNombre.Text = row.Cells["Nombre"]?.Value?.ToString();
            txtApellido.Text = row.Cells["Apellido"]?.Value?.ToString();
            txtDni.Text = row.Cells["DNI"]?.Value?.ToString();
            txtEmail.Text = row.Cells["Email"]?.Value?.ToString();
            txtTelefono.Text = row.Cells["Telefono"]?.Value?.ToString();
            txtUsuario.Text = row.Cells["UsuarioLogin"]?.Value?.ToString();
            txtDireccion.Text = row.Cells["Direccion"]?.Value?.ToString();
            txtLocalidad.Text = row.Cells["Localidad"]?.Value?.ToString();
            txtProvincia.Text = row.Cells["Provincia"]?.Value?.ToString();
            txtObservaciones.Text = row.Cells["Observaciones"]?.Value?.ToString();

            SetComboSafe(cboRol, row.Cells["Rol"]?.Value?.ToString());
            SetComboSafe(cboEstado, row.Cells["Estado"]?.Value?.ToString());
            SetComboSafe(cboArea, row.Cells["Area"]?.Value?.ToString());
            SetComboSafe(cboPreguntaSeg, row.Cells["PreguntaSeguridad"]?.Value?.ToString());

            txtRespuestaSeg.Text = row.Cells["RespuestaSeguridad"]?.Value?.ToString();
        }

        private void SetComboSafe(ComboBox combo, string value)
        {
            if (combo == null) return;
            if (string.IsNullOrWhiteSpace(value)) { combo.SelectedIndex = -1; return; }

            int idx = combo.Items.IndexOf(value);
            if (idx >= 0) combo.SelectedIndex = idx;
            else
            {
                combo.Items.Add(value);
                combo.SelectedIndex = combo.Items.Count - 1;
            }
        }

        // ==== GUARDAR / MODIFICAR ====
        private void button3_Guardar_Click(object sender, EventArgs e)
        {
            var rol = cboRol.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(rol) && !ROLES_PERMITIDOS.Contains(rol))
            {
                MessageBox.Show("Rol no permitido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtId.Text))
                InsertarUsuario();
            else
                ActualizarUsuario();
        }

        private void InsertarUsuario()
        {
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
                    CargarParametrosUsuario(cmd);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Usuario creado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el usuario:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarUsuario()
        {
            if (!int.TryParse(txtId.Text, out var id))
            {
                MessageBox.Show("Id inválido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string SQL = @"
                UPDATE dbo.Usuarios SET
                    Nombre=@Nombre,
                    Apellido=@Apellido,
                    DNI=@DNI,
                    Email=@Email,
                    Telefono=@Telefono,
                    UsuarioLogin=@UsuarioLogin,
                    Direccion=@Direccion,
                    Localidad=@Localidad,
                    Provincia=@Provincia,
                    Observaciones=@Observaciones,
                    Rol=@Rol,
                    Estado=@Estado,
                    Area=@Area,
                    PreguntaSeguridad=@PreguntaSeguridad,
                    RespuestaSeguridad=@RespuestaSeguridad
                WHERE IdUsuario=@Id;";

            try
            {
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = new SqlCommand(SQL, cn))
                {
                    CargarParametrosUsuario(cmd);
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cn.Open();

                    int n = cmd.ExecuteNonQuery();
                    if (n == 0)
                    {
                        MessageBox.Show("No se encontró el usuario.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                MessageBox.Show("✅ Cambios guardados correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios(txtBuscar.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarParametrosUsuario(SqlCommand cmd)
        {
            object V(string s) => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();

            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = V(txtNombre.Text);
            cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = V(txtApellido.Text);
            cmd.Parameters.Add("@DNI", SqlDbType.NVarChar, 20).Value = V(txtDni.Text);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = V(txtEmail.Text);
            cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 50).Value = V(txtTelefono.Text);
            cmd.Parameters.Add("@UsuarioLogin", SqlDbType.NVarChar, 100).Value = V(txtUsuario.Text);
            cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = V(txtDireccion.Text);
            cmd.Parameters.Add("@Localidad", SqlDbType.NVarChar, 100).Value = V(txtLocalidad.Text);
            cmd.Parameters.Add("@Provincia", SqlDbType.NVarChar, 100).Value = V(txtProvincia.Text);
            cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar, -1).Value = V(txtObservaciones.Text);
            cmd.Parameters.Add("@Rol", SqlDbType.NVarChar, 50).Value = V(cboRol.Text);
            cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 50).Value = V(cboEstado.Text);
            cmd.Parameters.Add("@Area", SqlDbType.NVarChar, 100).Value = V(cboArea.Text);
            cmd.Parameters.Add("@PreguntaSeguridad", SqlDbType.NVarChar, 200).Value = V(cboPreguntaSeg.Text);
            cmd.Parameters.Add("@RespuestaSeguridad", SqlDbType.NVarChar, 200).Value = V(txtRespuestaSeg.Text);
        }

        // ==== ELIMINAR ====
        private void button2_Eliminar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out var id))
            {
                MessageBox.Show("Seleccioná un usuario válido.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Eliminar este usuario?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = new SqlCommand("DELETE FROM dbo.Usuarios WHERE IdUsuario=@Id;", cn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("🗑️ Usuario eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios(txtBuscar.Text.Trim());
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==== LIMPIAR ====
        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtDni.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtUsuario.Clear();
            txtDireccion.Clear();
            txtLocalidad.Clear();
            txtProvincia.Clear();
            txtObservaciones.Clear();

            cboRol.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            cboArea.SelectedIndex = -1;
            cboPreguntaSeg.SelectedIndex = -1;
            txtRespuestaSeg.Clear();
        }
    }
}

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
            @"Data Source=DESKTOP-92OCSA4;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        private static readonly string[] ROLES_PERMITIDOS = { "Jefe de area", "Docente", "Invitado" };

        public Consulta_de_Usuarios__Modificacion__Baja()
        {
            InitializeComponent();

            // Eventos
            this.Load += Frm_Load;
            button1.Click += button1_Buscar_Click;   // Buscar
            button2.Click += button2_Eliminar_Click; // Eliminar
            button3.Click += button3_Guardar_Click;  // Guardar cambios

            dataGridViewUsuarios.SelectionChanged += DataGridViewUsuarios_SelectionChanged;

            this.AutoScroll = true;
        }

        // ===== LOAD =====
        private void Frm_Load(object sender, EventArgs e)
        {
            // Combos base (si tu diseñador no los precarga)
            if (cboRol.Items.Count == 0)
                cboRol.Items.AddRange(ROLES_PERMITIDOS);

            if (cboEstado.Items.Count == 0)
                cboEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });

            if (cboArea.Items.Count == 0)
                cboArea.Items.AddRange(new object[] { "Administración", "Animal", "Vegetal", "Vivero", "Huerta" });

            if (cboPreguntaSeg.Items.Count == 0)
            {
                cboPreguntaSeg.Items.AddRange(new object[]
                {
                    "¿Nombre de tu primera mascota?",
                    "¿Ciudad donde naciste?",
                    "¿Comida favorita?"
                });
            }

            // Grid
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Arranco bloqueado
            SetEditingEnabled(false);
            CargarUsuarios();
        }

        // ===== UTILIDAD: habilitar/inhabilitar edición =====
        private void SetEditingEnabled(bool enabled)
        {
            // Campos
            txtNombre.Enabled = enabled;
            txtApellido.Enabled = enabled;
            txtDni.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtTelefono.Enabled = enabled;
            txtUsuario.Enabled = enabled;
            txtDireccion.Enabled = enabled;
            txtLocalidad.Enabled = enabled;
            txtProvincia.Enabled = enabled;
            txtObservaciones.Enabled = enabled;

            cboRol.Enabled = enabled;
            cboEstado.Enabled = enabled;
            cboArea.Enabled = enabled;
            cboPreguntaSeg.Enabled = enabled;
            txtRespuestaSeg.Enabled = enabled;

            // Botones Modificar/Eliminar
            button3.Enabled = enabled; // Guardar cambios
            button2.Enabled = enabled; // Eliminar
        }

        // ===== LISTADO =====
        private void CargarUsuarios(string filtro = "")
        {
            try
            {
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var da = new SqlDataAdapter(GetSqlListado(), cn))
                {
                    string f = (filtro ?? "").Trim();
                    da.SelectCommand.Parameters.AddWithValue("@f", f == "" ? "" : "%" + f + "%");

                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando usuarios:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSqlListado()
        {
            // Alias de PK para el grid: IdUsuario AS Id
            return @"
                SELECT 
                    IdUsuario AS Id,
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
                FROM dbo.Usuarios
                WHERE (@f = '' 
                       OR Nombre LIKE @f OR Apellido LIKE @f OR UsuarioLogin LIKE @f OR DNI LIKE @f)
                ORDER BY Apellido, Nombre;";
        }

        private void button1_Buscar_Click(object sender, EventArgs e)
        {
            CargarUsuarios(txtBuscar.Text.Trim());
            LimpiarCampos();
            SetEditingEnabled(false);
        }

        // ===== SELECCIÓN =====
        private void DataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.CurrentRow == null)
            {
                LimpiarCampos();
                SetEditingEnabled(false);
                return;
            }

            var cell = dataGridViewUsuarios.CurrentRow.Cells["Id"]; // alias presente en el SELECT
            if (cell?.Value == null || cell.Value == DBNull.Value)
            {
                LimpiarCampos();
                SetEditingEnabled(false);
                return;
            }

            if (!int.TryParse(cell.Value.ToString(), out int id))
            {
                LimpiarCampos();
                SetEditingEnabled(false);
                return;
            }

            CargarDetalleUsuario(id);
        }

        // ===== DETALLE =====
        private void CargarDetalleUsuario(int id)
        {
            try
            {
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = new SqlCommand("SELECT * FROM dbo.Usuarios WHERE IdUsuario=@id;", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read())
                        {
                            LimpiarCampos();
                            SetEditingEnabled(false);
                            return;
                        }

                        // Mostramos la PK real y todo lo demás
                        txtId.Text = rd["IdUsuario"].ToString();
                        txtNombre.Text = rd["Nombre"]?.ToString();
                        txtApellido.Text = rd["Apellido"]?.ToString();
                        txtDni.Text = rd["DNI"]?.ToString();
                        txtEmail.Text = rd["Email"]?.ToString();
                        txtTelefono.Text = rd["Telefono"]?.ToString();
                        txtUsuario.Text = rd["UsuarioLogin"]?.ToString();
                        txtDireccion.Text = rd["Direccion"]?.ToString();
                        txtLocalidad.Text = rd["Localidad"]?.ToString();
                        txtProvincia.Text = rd["Provincia"]?.ToString();
                        txtObservaciones.Text = rd["Observaciones"]?.ToString();

                        SetComboSafe(cboRol, rd["Rol"]?.ToString());
                        SetComboSafe(cboEstado, rd["Estado"]?.ToString());
                        SetComboSafe(cboArea, rd["Area"]?.ToString());
                        SetComboSafe(cboPreguntaSeg, rd["PreguntaSeguridad"]?.ToString());

                        txtRespuestaSeg.Text = rd["RespuestaSeguridad"]?.ToString();
                    }
                }

                // Al haber selección válida, habilito edición/botones
                SetEditingEnabled(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el detalle del usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarCampos();
                SetEditingEnabled(false);
            }
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

        // ===== GUARDAR CAMBIOS (UPDATE) =====
        private void button3_Guardar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out var id))
            {
                MessageBox.Show("Debe seleccionar un usuario.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validaciones básicas mínimas para Modificación
            var rol = (cboRol.Text ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(rol) && !ROLES_PERMITIDOS.Contains(rol))
            {
                MessageBox.Show("Rol no permitido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRol.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Apellido es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Usuario es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus(); return;
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
                    object V(string s) => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
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

                    cn.Open();
                    int n = cmd.ExecuteNonQuery();
                    if (n == 0)
                    {
                        MessageBox.Show("No se encontró el usuario a actualizar.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                MessageBox.Show("✅ Cambios guardados correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios(txtBuscar.Text.Trim());
                // Mantengo seleccionado el mismo Id si está todavía en el grid:
                ReseleccionarIdEnGrid(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReseleccionarIdEnGrid(int id)
        {
            foreach (DataGridViewRow row in dataGridViewUsuarios.Rows)
            {
                var cell = row.Cells["Id"];
                if (cell?.Value != null && int.TryParse(cell.Value.ToString(), out int gid) && gid == id)
                {
                    row.Selected = true;
                    dataGridViewUsuarios.CurrentCell = row.Cells["Id"];
                    return;
                }
            }
        }

        // ===== ELIMINAR =====
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
                    int n = cmd.ExecuteNonQuery();
                    if (n == 0)
                    {
                        MessageBox.Show("No se encontró el usuario.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                MessageBox.Show("🗑️ Usuario eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios(txtBuscar.Text.Trim());
                LimpiarCampos();
                SetEditingEnabled(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== LIMPIAR CAMPOS =====
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

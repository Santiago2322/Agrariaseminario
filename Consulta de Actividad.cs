using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Actividad : Form
    {
        private readonly string CADENA =
             @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        private int idSeleccionado = -1;

        public Consulta_de_Actividad()
        {
            InitializeComponent();
            this.AutoScroll = true;                // ✅ Scroll vertical
            this.HorizontalScroll.Enabled = true;  // ✅ Scroll horizontal
            Load += Consulta_de_Actividad_Load;
        }

        private void Consulta_de_Actividad_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureSchema();
                CargarEntornos();
                CargarActividades();

                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // ✅ Solo consulta, no se edita desde la grilla
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureSchema()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(@"
IF OBJECT_ID('dbo.Actividades','U') IS NULL
    RAISERROR('Falta la tabla dbo.Actividades.',16,1);
IF COL_LENGTH('dbo.Actividades','Id') IS NULL
    RAISERROR('dbo.Actividades debe tener columna Id (FK).',16,1);
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
    RAISERROR('Falta la tabla dbo.EntornosFormativos.',16,1);", cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarEntornos()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var da = new SqlDataAdapter(@"
SELECT 
    CASE WHEN COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL THEN IdEntorno
         ELSE Id
    END AS IdEntorno,
    Nombre
FROM dbo.EntornosFormativos
ORDER BY Nombre;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                comboEntorno.DisplayMember = "Nombre";
                comboEntorno.ValueMember = "IdEntorno";
                comboEntorno.DataSource = dt;
            }
        }

        private void CargarActividades(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA))
            using (var da = new SqlDataAdapter())
            {
                string sql = @"
SELECT a.IdActividad,
       e.Nombre AS Entorno,
       a.Nombre,
       a.Fecha,
       a.Hora,
       a.Descripcion,
       a.Responsable
FROM dbo.Actividades a
INNER JOIN dbo.EntornosFormativos e
  ON (a.Id = e.Id OR a.Id = e.IdEntorno)
WHERE 1=1";

                var cmd = new SqlCommand();
                cmd.Connection = cn;

                if (comboEntorno.SelectedValue != null)
                {
                    sql += " AND a.Id = @idEntorno";
                    cmd.Parameters.AddWithValue("@idEntorno", Convert.ToInt32(comboEntorno.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    sql += " AND (a.Nombre LIKE @f OR a.Descripcion LIKE @f OR a.Responsable LIKE @f)";
                    cmd.Parameters.AddWithValue("@f", "%" + txtFiltro.Text + "%");
                }

                sql += " ORDER BY a.Fecha DESC, a.Hora ASC;";
                cmd.CommandText = sql;

                da.SelectCommand = cmd;
                var dt = new DataTable();
                da.Fill(dt);

                grid.DataSource = dt;
                if (grid.Columns["IdActividad"] != null)
                    grid.Columns["IdActividad"].Visible = false;

                // Reafirmo solo lectura por las dudas
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
            }
        }

        // =======================================================
        // BOTONES
        // =======================================================

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarActividades(txtFiltro.Text.Trim());
        }

        // 👇 Aunque ya no haya botón "Nuevo", dejo el método por si lo usás en otro lado
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            if (comboEntorno.SelectedValue == null)
            {
                MessageBox.Show("Seleccioná un entorno.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEntorno = Convert.ToInt32(comboEntorno.SelectedValue);

            const string SQL = @"
INSERT INTO dbo.Actividades (Id, Nombre, Fecha, Hora, Descripcion, Responsable)
VALUES (@id, @n, @f, @h, @d, @r);";

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@id", idEntorno);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@f", dtpFecha.Value.Date);
                cmd.Parameters.AddWithValue("@h", string.IsNullOrWhiteSpace(txtHora.Text) ? (object)DBNull.Value : txtHora.Text.Trim());
                cmd.Parameters.AddWithValue("@d", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@r", string.IsNullOrWhiteSpace(txtResponsable.Text) ? (object)DBNull.Value : txtResponsable.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            LimpiarCampos();
            MessageBox.Show("Actividad agregada correctamente.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná una fila para modificar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string SQL = @"
UPDATE dbo.Actividades
SET Nombre=@n, Fecha=@f, Hora=@h, Descripcion=@d, Responsable=@r
WHERE IdActividad=@id;";

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@f", dtpFecha.Value.Date);
                cmd.Parameters.AddWithValue("@h", string.IsNullOrWhiteSpace(txtHora.Text) ? (object)DBNull.Value : txtHora.Text.Trim());
                cmd.Parameters.AddWithValue("@d", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@r", string.IsNullOrWhiteSpace(txtResponsable.Text) ? (object)DBNull.Value : txtResponsable.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            MessageBox.Show("Actividad modificada correctamente.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná una actividad para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro que querés eliminar esta actividad?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand("DELETE FROM dbo.Actividades WHERE IdActividad=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            LimpiarCampos();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || grid.CurrentRow == null) return;
            var fila = grid.Rows[e.RowIndex];

            idSeleccionado = int.TryParse(fila.Cells["IdActividad"]?.Value?.ToString(), out var id) ? id : -1;
            txtNombre.Text = fila.Cells["Nombre"]?.Value?.ToString();
            txtHora.Text = fila.Cells["Hora"]?.Value?.ToString();
            txtDescripcion.Text = fila.Cells["Descripcion"]?.Value?.ToString();
            txtResponsable.Text = fila.Cells["Responsable"]?.Value?.ToString();

            if (fila.Cells["Fecha"]?.Value != null && DateTime.TryParse(fila.Cells["Fecha"].Value.ToString(), out var fecha))
                dtpFecha.Value = fecha;
        }

        // =======================================================
        // HELPERS
        // =======================================================

        private void LimpiarCampos()
        {
            idSeleccionado = -1;
            txtNombre.Clear();
            txtHora.Clear();
            txtDescripcion.Clear();
            txtResponsable.Clear();
            dtpFecha.Value = DateTime.Now;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            return true;
        }
    }
}

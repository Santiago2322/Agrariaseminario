using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Actividad : Form
    {
        // 🔗 Misma conexión que el resto del proyecto
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        private int idSeleccionado = -1;

        public Consulta_de_Actividad()
        {
            InitializeComponent();
            Load += Consulta_de_Actividad_Load;

            // Asegurate en el Designer:
            //  buttonBuscar.Click   -> buttonBuscar_Click
            //  buttonEliminar.Click -> buttonEliminar_Click
            //  buttonModificar.Click-> buttonModificar_Click
            //  buttonNuevo.Click    -> buttonNuevo_Click
            //  buttonGuardar.Click  -> buttonGuardar_Click
            //  buttonCerrar.Click   -> buttonCerrar_Click
            //  dataGridView1.CellClick -> dataGridView1_CellClick
        }

        private void Consulta_de_Actividad_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTableExists();
                CargarActividades();
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar actividades: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureTableExists()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(100) NULL,
        Fecha DATETIME2 NOT NULL CONSTRAINT DF_Actividades_Fecha DEFAULT SYSUTCDATETIME()
    );
END";
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarActividades(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA))
            using (var da = new SqlDataAdapter())
            {
                string sql = @"
SELECT IdActividad, Nombre, Descripcion, Responsable, Fecha
FROM dbo.Actividades";

                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Descripcion LIKE @f OR Responsable LIKE @f";

                sql += " ORDER BY Fecha DESC, Nombre";

                da.SelectCommand = new SqlCommand(sql, cn);
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro + "%");

                var dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                if (dataGridView1.Columns["IdActividad"] != null)
                    dataGridView1.Columns["IdActividad"].Visible = false;
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            CargarActividades(textBoxFiltro.Text.Trim());
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1)
            {
                MessageBox.Show("Selecciona una actividad para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro que deseas eliminar esta actividad?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand("DELETE FROM dbo.Actividades WHERE IdActividad = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            LimpiarCampos();
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1)
            {
                MessageBox.Show("Selecciona una actividad para modificar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos()) return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(@"
UPDATE dbo.Actividades
SET Nombre=@n, Descripcion=@d, Responsable=@r
WHERE IdActividad=@id;", cn))
            {
                cmd.Parameters.AddWithValue("@n", textBoxNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@d", textBoxDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@r", textBoxResponsable.Text.Trim());
                cmd.Parameters.AddWithValue("@id", idSeleccionado);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            LimpiarCampos();
            MessageBox.Show("Actividad modificada correctamente.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.CurrentRow == null) return;

            var fila = dataGridView1.Rows[e.RowIndex];

            if (fila.Cells["IdActividad"]?.Value != null &&
                int.TryParse(fila.Cells["IdActividad"].Value.ToString(), out var id))
                idSeleccionado = id;
            else
                idSeleccionado = -1;

            textBoxNombre.Text = fila.Cells["Nombre"]?.Value?.ToString();
            textBoxDescripcion.Text = fila.Cells["Descripcion"]?.Value?.ToString();
            textBoxResponsable.Text = fila.Cells["Responsable"]?.Value?.ToString();
        }

        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarCampos()
        {
            idSeleccionado = -1;
            textBoxNombre.Clear();
            textBoxDescripcion.Clear();
            textBoxResponsable.Clear();
            textBoxNombre.Focus();
        }

        // ===== Alta rápido =====
        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(@"
INSERT INTO dbo.Actividades (Nombre, Descripcion, Responsable)
VALUES (@n, @d, @r);", cn))
            {
                cmd.Parameters.AddWithValue("@n", textBoxNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@d", textBoxDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@r", textBoxResponsable.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarActividades();
            LimpiarCampos();
            MessageBox.Show("Actividad agregada correctamente.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El campo Nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNombre.Focus();
                return false;
            }
            return true;
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Actividad : Form
    {
        private const string CADENA =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        private int idSeleccionado = -1;

        public Consulta_de_Actividad()
        {
            InitializeComponent();
            Load += Consulta_de_Actividad_Load;
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
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(100) NULL,
        Fecha DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    INSERT INTO Actividades (Nombre, Descripcion, Responsable)
    VALUES (N'Actividad de ejemplo', N'Esto es una carga inicial de prueba', N'Administrador');
END";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarActividades(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA))
            using (var da = new SqlDataAdapter())
            {
                string sql = "SELECT IdActividad, Nombre, Descripcion, Responsable, Fecha FROM Actividades";

                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Descripcion LIKE @f OR Responsable LIKE @f";

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
                MessageBox.Show("Selecciona una actividad para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro que deseas eliminar esta actividad?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = new SqlCommand("DELETE FROM Actividades WHERE IdActividad = @id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                CargarActividades();
                LimpiarCampos();
            }
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1)
            {
                MessageBox.Show("Selecciona una actividad para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(
                "UPDATE Actividades SET Nombre=@n, Descripcion=@d, Responsable=@r WHERE IdActividad=@id", cn))
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
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridView1.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(fila.Cells["IdActividad"].Value);
                textBoxNombre.Text = fila.Cells["Nombre"].Value?.ToString();
                textBoxDescripcion.Text = fila.Cells["Descripcion"].Value?.ToString();
                textBoxResponsable.Text = fila.Cells["Responsable"].Value?.ToString();
            }
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
        }
    }
}

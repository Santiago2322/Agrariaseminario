using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar : Form
    {
        // 🔗 Misma cadena del proyecto
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Consulta_de_Entornos_Formativos__Modificar_Eliminar()
        {
            InitializeComponent();
            this.AutoScroll = true;

            // Asegurate en el Designer:
            //  this.Load                        -> Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load
            //  dataGridView1.CellClick         -> dataGridView1_CellClick
            //  button1 (Modificar)             -> button1_Click
            //  button2 (Eliminar)              -> button2_Click
            //  button3 (Guardar/Agregar nuevo) -> button3_Click
            //  label3.Click (o botón Cerrar)    -> label3_Click (cierra)
        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTabla();
                CargarDatos();
                if (dataGridView1 != null)
                {
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    if (dataGridView1.Columns["Id"] != null)
                        dataGridView1.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea dbo.EntornosFormativos si no existe. No crea la BD.
        /// </summary>
        private void EnsureTabla()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id        INT IDENTITY(1,1) PRIMARY KEY,
        Nombre    NVARCHAR(120) NOT NULL,
        Tipo      NVARCHAR(80)  NOT NULL,
        Profesor  NVARCHAR(120) NOT NULL,
        Anio      NVARCHAR(20)  NOT NULL,
        Division  NVARCHAR(20)  NOT NULL,
        Grupo     NVARCHAR(40)  NOT NULL
    );
END";
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarDatos(string filtro = "")
        {
            using (var cn = new SqlConnection(CADENA))
            {
                string sql = @"SELECT Id, Nombre, Tipo, Profesor, Anio, Division, Grupo
                               FROM dbo.EntornosFormativos";
                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Tipo LIKE @f OR Profesor LIKE @f OR Anio LIKE @f OR Division LIKE @f OR Grupo LIKE @f";
                sql += " ORDER BY Nombre";

                using (var da = new SqlDataAdapter(sql, cn))
                {
                    if (!string.IsNullOrWhiteSpace(filtro))
                        da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.Trim() + "%");

                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.CurrentRow == null) return;

            var fila = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = fila.Cells["Nombre"]?.Value?.ToString();
            textBox2.Text = fila.Cells["Tipo"]?.Value?.ToString();
            textBox3.Text = fila.Cells["Profesor"]?.Value?.ToString();
            textBox4.Text = fila.Cells["Anio"]?.Value?.ToString();
            textBox6.Text = fila.Cells["Division"]?.Value?.ToString();
            textBox5.Text = fila.Cells["Grupo"]?.Value?.ToString();
        }

        private void button1_Click(object sender, EventArgs e) // Modificar
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos()) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            const string SQL = @"
UPDATE dbo.EntornosFormativos
SET Nombre=@n, Tipo=@t, Profesor=@p, Anio=@a, Division=@d, Grupo=@g
WHERE Id=@id;";

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@n", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@t", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@p", textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@a", textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@d", textBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@g", textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Registro modificado correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e) // Eliminar
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("¿Eliminar este entorno?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand("DELETE FROM dbo.EntornosFormativos WHERE Id=@id;", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Registro eliminado correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e) // Agregar (Guardar nuevo)
        {
            if (!ValidarCampos()) return;

            const string SQL = @"
INSERT INTO dbo.EntornosFormativos (Nombre, Tipo, Profesor, Anio, Division, Grupo)
VALUES (@n, @t, @p, @a, @d, @g);";

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@n", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@t", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@p", textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@a", textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@d", textBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@g", textBox5.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Nuevo entorno agregado.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("El campo Nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("El campo Tipo es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("El campo Profesor es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("El campo Año es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("El campo División es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox6.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("El campo Grupo es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return false;
            }
            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Si tu label3 es “Cerrar”, dejamos que cierre la ventana:
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}

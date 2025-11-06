using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar : Form
    {
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Consulta_de_Entornos_Formativos__Modificar_Eliminar()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.Load += Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load;

            // wire de eventos de runtime (el Designer también los setea; cualquiera de los dos está bien)
            dataGridView1.CellClick += dataGridView1_CellClick;
            button1.Click += button1_Click; // Modificar
            button2.Click += button2_Click; // Eliminar
            button3.Click += button3_Click; // Guardar/Agregar
        }

        // === Helpers compatibles C# 7.3 ===
        private static object NV(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();
        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTabla();
                CargarDatos();

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;

                if (dataGridView1.Columns["Id"] != null)
                    dataGridView1.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureTabla()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre         NVARCHAR(120) NOT NULL,
        Tipo           NVARCHAR(80)  NOT NULL,
        Profesor       NVARCHAR(120) NULL,
        Anio           NVARCHAR(20)  NULL,
        Division       NVARCHAR(20)  NULL,
        Grupo          NVARCHAR(40)  NULL,
        Observaciones  NVARCHAR(300) NULL
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
                string sql = @"
SELECT Id, Nombre, Tipo, Profesor, Anio, Division, Grupo
FROM dbo.EntornosFormativos";
                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Tipo LIKE @f OR Profesor LIKE @f OR Anio LIKE @f OR Division LIKE @f OR Grupo LIKE @f";
                sql += " ORDER BY Nombre;";

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
            textBox1.Text = (fila.Cells["Nombre"] == null || fila.Cells["Nombre"].Value == null) ? "" : fila.Cells["Nombre"].Value.ToString();
            textBox2.Text = (fila.Cells["Tipo"] == null || fila.Cells["Tipo"].Value == null) ? "" : fila.Cells["Tipo"].Value.ToString();
            textBox3.Text = (fila.Cells["Profesor"] == null || fila.Cells["Profesor"].Value == null) ? "" : fila.Cells["Profesor"].Value.ToString();
            textBox4.Text = (fila.Cells["Anio"] == null || fila.Cells["Anio"].Value == null) ? "" : fila.Cells["Anio"].Value.ToString();
            textBox6.Text = (fila.Cells["Division"] == null || fila.Cells["Division"].Value == null) ? "" : fila.Cells["Division"].Value.ToString();
            textBox5.Text = (fila.Cells["Grupo"] == null || fila.Cells["Grupo"].Value == null) ? "" : fila.Cells["Grupo"].Value.ToString();
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
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 120).Value = NV(textBox3.Text);
                cmd.Parameters.Add("@a", SqlDbType.NVarChar, 20).Value = NV(textBox4.Text);
                cmd.Parameters.Add("@d", SqlDbType.NVarChar, 20).Value = NV(textBox6.Text);
                cmd.Parameters.Add("@g", SqlDbType.NVarChar, 40).Value = NV(textBox5.Text);
                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Registro modificado correctamente.", "Confirmación",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand("DELETE FROM dbo.EntornosFormativos WHERE Id=@id;", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Registro eliminado correctamente.", "Confirmación",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e) // Agregar
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
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 120).Value = NV(textBox3.Text);
                cmd.Parameters.Add("@a", SqlDbType.NVarChar, 20).Value = NV(textBox4.Text);
                cmd.Parameters.Add("@d", SqlDbType.NVarChar, 20).Value = NV(textBox6.Text);
                cmd.Parameters.Add("@g", SqlDbType.NVarChar, 40).Value = NV(textBox5.Text);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Nuevo entorno agregado.", "Confirmación",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return true;
        }

        private void label3_Click(object sender, EventArgs e) { this.Close(); }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}

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

        private string _colPkEntornos = null;

        public Consulta_de_Entornos_Formativos__Modificar_Eliminar()
        {
            InitializeComponent();
            this.AutoScroll = true;

            // eventos
            this.Load += Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
            button1.Click += button1_Click; // Modificar
            button2.Click += button2_Click; // Eliminar
            button3.Click += button3_Click; // Cerrar
        }

        private static object NV(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();
        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {
            try
            {
                _colPkEntornos = DetectarPkEntornos();
                EnsureTabla();
                CargarDatos();

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;

                if (dataGridView1.Columns["Id"] != null)
                    dataGridView1.Columns["Id"].Visible = false;

                // desactivar botones hasta selección
                button1.Enabled = false;
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string DetectarPkEntornos()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(@"
SELECT TOP 1 CASE 
    WHEN COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL THEN 'IdEntorno'
    ELSE 'Id'
END;", cn))
            {
                cn.Open();
                var o = cmd.ExecuteScalar() as string;
                return string.IsNullOrEmpty(o) ? "Id" : o;
            }
        }

        private void EnsureTabla()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        IdEntorno     INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(120) NOT NULL,
        Tipo          NVARCHAR(80)  NOT NULL,
        Profesor      NVARCHAR(120) NULL,
        Anio          NVARCHAR(20)  NULL,
        Division      NVARCHAR(20)  NULL,
        Grupo         NVARCHAR(40)  NULL,
        Observaciones NVARCHAR(300) NULL
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
            using (var da = new SqlDataAdapter())
            {
                string sql = $@"
SELECT 
    {(_colPkEntornos == "IdEntorno" ? "IdEntorno" : "Id")} AS Id,
    Nombre, Tipo, Profesor, Anio, Division, Grupo
FROM dbo.EntornosFormativos";

                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Tipo LIKE @f OR Profesor LIKE @f OR Anio LIKE @f OR Division LIKE @f OR Grupo LIKE @f";

                sql += " ORDER BY Nombre;";

                da.SelectCommand = new SqlCommand(sql, cn);
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.Trim() + "%");

                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.CurrentRow == null) return;

            var fila = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = fila.Cells["Nombre"]?.Value?.ToString() ?? "";
            textBox2.Text = fila.Cells["Tipo"]?.Value?.ToString() ?? "";
            textBox3.Text = fila.Cells["Profesor"]?.Value?.ToString() ?? "";
            textBox4.Text = fila.Cells["Anio"]?.Value?.ToString() ?? "";
            textBox6.Text = fila.Cells["Division"]?.Value?.ToString() ?? "";
            textBox5.Text = fila.Cells["Grupo"]?.Value?.ToString() ?? "";

            // habilitar botones
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private int? IdSeleccionado()
        {
            if (dataGridView1.CurrentRow == null) return null;
            var cell = dataGridView1.CurrentRow.Cells["Id"];
            if (cell?.Value == null || cell.Value == DBNull.Value) return null;
            try { return Convert.ToInt32(cell.Value); } catch { return null; }
        }

        private void button1_Click(object sender, EventArgs e) // Modificar
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidarCampos()) return;

            var idOpt = IdSeleccionado();
            if (!idOpt.HasValue)
            {
                MessageBox.Show("No se pudo determinar el Id del registro.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = idOpt.Value;
            string colId = _colPkEntornos ?? "Id";

            string SQL = $@"
UPDATE dbo.EntornosFormativos
SET Nombre=@n, Tipo=@t, Profesor=@p, Anio=@a, Division=@d, Grupo=@g
WHERE {colId}=@id;";

            try
            {
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
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show("No se modificó ninguna fila (verificá que el registro exista).",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                CargarDatos();
                MessageBox.Show("Registro modificado correctamente.", "Confirmación",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Eliminar
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idOpt = IdSeleccionado();
            if (!idOpt.HasValue) return;
            int id = idOpt.Value;

            if (MessageBox.Show("¿Eliminar este entorno?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string colId = _colPkEntornos ?? "Id";
            string SQL = $"DELETE FROM dbo.EntornosFormativos WHERE {colId}=@id;";

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show("No se eliminó ninguna fila (puede que ya no exista).",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                CargarDatos();
                MessageBox.Show("Registro eliminado correctamente.", "Confirmación",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // limpiar campos y desactivar botones
                textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
                textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
                button1.Enabled = false;
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Cerrar
        {
            this.Close();
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

        private void label3_Click(object sender, EventArgs e) => this.Close();
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}

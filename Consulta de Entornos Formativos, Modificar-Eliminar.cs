// Consulta_de_Entornos_Formativos__Modificar_Eliminar.cs
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

            this.Load += Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit; // 🔒 bloquea edición directa
            dataGridView1.DataError += dataGridView1_DataError;         // 🎯 evita crash por formato

            button1.Click += button1_Click; // Confirmar (antes Modificar)
            button2.Click += button2_Click; // Eliminar
            button3.Click += button3_Click; // Cerrar
        }

        private static object NV(string s)
            => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();

        private static string S(object o)
            => (o == null || o == DBNull.Value) ? "" : o.ToString();

        // 🔒 Habilita/Deshabilita edición de inputs
        private void LockInputs(bool locked)
        {
            textBox1.ReadOnly = locked;
            textBox2.ReadOnly = locked;
            textBox3.ReadOnly = locked;
            textBox4.ReadOnly = locked;
            textBox5.ReadOnly = locked;
            textBox6.ReadOnly = locked;

            // Opcional: también los deshabilito visualmente
            textBox1.TabStop = !locked;
            textBox2.TabStop = !locked;
            textBox3.TabStop = !locked;
            textBox4.TabStop = !locked;
            textBox5.TabStop = !locked;
            textBox6.TabStop = !locked;
        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {
            try
            {
                _colPkEntornos = DetectarPkEntornos();
                EnsureTabla();
                CargarDatos();

                // 🔒 Grilla bloqueada
                dataGridView1.ReadOnly = true;
                dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToOrderColumns = false;

                if (dataGridView1.Columns["Id"] != null)
                    dataGridView1.Columns["Id"].Visible = false;

                // 🚫 Sin selección: inputs bloqueados y botones off
                LockInputs(true);
                button1.Enabled = false; // Confirmar
                button2.Enabled = false; // Eliminar
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
                string sql = @"
SELECT 
    {PK} AS Id,
    Nombre, Tipo, Profesor, Anio, Division, Grupo
FROM dbo.EntornosFormativos";
                sql = sql.Replace("{PK}", _colPkEntornos == "IdEntorno" ? "IdEntorno" : "Id");

                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Tipo LIKE @f OR Profesor LIKE @f OR Anio LIKE @f OR Division LIKE @f OR Grupo LIKE @f";

                sql += " ORDER BY Nombre;";

                da.SelectCommand = new SqlCommand(sql, cn);
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.Add("@f", SqlDbType.NVarChar, 120).Value = "%" + filtro.Trim() + "%";

                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.CurrentRow == null) return;

            var fila = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = fila.Cells["Nombre"] != null ? S(fila.Cells["Nombre"].Value) : "";
            textBox2.Text = fila.Cells["Tipo"] != null ? S(fila.Cells["Tipo"].Value) : "";
            textBox3.Text = fila.Cells["Profesor"] != null ? S(fila.Cells["Profesor"].Value) : "";
            textBox4.Text = fila.Cells["Anio"] != null ? S(fila.Cells["Anio"].Value) : "";
            textBox6.Text = fila.Cells["Division"] != null ? S(fila.Cells["Division"].Value) : "";
            textBox5.Text = fila.Cells["Grupo"] != null ? S(fila.Cells["Grupo"].Value) : "";

            // ✅ Al seleccionar, habilito edición y botones
            LockInputs(false);
            button1.Enabled = true; // Confirmar
            button2.Enabled = true; // Eliminar
        }

        // 🔒 Seguridad extra: impedir edición directa en la grilla
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private int? IdSeleccionado()
        {
            if (dataGridView1.CurrentRow == null) return null;
            var cell = dataGridView1.CurrentRow.Cells["Id"];
            if (cell == null || cell.Value == null || cell.Value == DBNull.Value) return null;
            try { return Convert.ToInt32(cell.Value); } catch { return null; }
        }

        private void button1_Click(object sender, EventArgs e) // Confirmar
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

            string SQL = @"
UPDATE dbo.EntornosFormativos
SET Nombre=@n, Tipo=@t, Profesor=@p, Anio=@a, Division=@d, Grupo=@g
WHERE {COL}=@id;";
            SQL = SQL.Replace("{COL}", colId);

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.Add("@n", SqlDbType.NVarChar, 120).Value = NV(textBox1.Text);
                    cmd.Parameters.Add("@t", SqlDbType.NVarChar, 80).Value = NV(textBox2.Text);
                    cmd.Parameters.Add("@p", SqlDbType.NVarChar, 120).Value = NV(textBox3.Text);
                    cmd.Parameters.Add("@a", SqlDbType.NVarChar, 20).Value = NV(textBox4.Text);
                    cmd.Parameters.Add("@d", SqlDbType.NVarChar, 20).Value = NV(textBox6.Text);
                    cmd.Parameters.Add("@g", SqlDbType.NVarChar, 40).Value = NV(textBox5.Text);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

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
                MessageBox.Show("Registro confirmado correctamente.", "Confirmación",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar: " + ex.Message, "Error",
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
            string SQL = "DELETE FROM dbo.EntornosFormativos WHERE {COL}=@id;".Replace("{COL}", colId);

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
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

                // 🔄 limpiar y volver a bloquear inputs
                textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
                textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
                button1.Enabled = false;
                button2.Enabled = false;
                LockInputs(true);
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
    }
}

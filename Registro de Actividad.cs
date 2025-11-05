using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Registro_de_Actividad : Form
    {
        // ✅ MISMA CADENA QUE EN TODO EL PROYECTO
        private const string CONN =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Registro_de_Actividad()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.Load += Registro_de_Actividad_Load;
        }

        private void Registro_de_Actividad_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaActividades();   // ← crea si no existe
                CargarGrilla();             // ← carga datos
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los registros: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Crea dbo.Actividades con columnas usadas por el proyecto
        private void EnsureTablaActividades()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades (
        IdActividad   INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(100) NOT NULL,
        Descripcion   NVARCHAR(300) NULL,
        Responsable   NVARCHAR(100) NULL,
        Fecha         DATETIME2 NOT NULL CONSTRAINT DF_Actividades_Fecha DEFAULT SYSUTCDATETIME()
    );
END";
            using (var cn = new SqlConnection(CONN))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarGrilla(string filtro = "")
        {
            using (var cn = new SqlConnection(CONN))
            {
                string sql = @"SELECT IdActividad, Fecha, Nombre, Descripcion, Responsable
                               FROM dbo.Actividades";
                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Descripcion LIKE @f OR Responsable LIKE @f";
                sql += " ORDER BY Fecha DESC";

                using (var da = new SqlDataAdapter(sql, cn))
                {
                    if (!string.IsNullOrWhiteSpace(filtro))
                        da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.Trim() + "%");

                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Oculto el ID si querés
                    if (dataGridView1.Columns["IdActividad"] != null)
                        dataGridView1.Columns["IdActividad"].Visible = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Registro guardado correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas cancelar? Los cambios no se guardarán.",
                "Confirmar cancelación", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}

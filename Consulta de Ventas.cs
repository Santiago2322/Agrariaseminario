using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Ventas : Form
    {
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Consulta_de_Ventas()
        {
            InitializeComponent();
            this.Load += Consulta_de_Ventas_Load;
            this.button1.Click += button1_Click; // Cerrar
            this.AutoScroll = true;
        }

        private void Consulta_de_Ventas_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaVentas();
                CargarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureTablaVentas()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATE            NOT NULL,
        Hora           NVARCHAR(20)    NULL,
        Cliente        NVARCHAR(150)   NULL,
        Producto       NVARCHAR(150)   NOT NULL,
        Cantidad       INT             NOT NULL,
        PrecioUnitario DECIMAL(10,2)   NOT NULL,
        Observaciones  NVARCHAR(300)   NULL
    );
END;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarVentas()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(@"
SELECT 
    IdVenta,
    Fecha,
    Hora,
    Cliente,
    Producto,
    Cantidad,
    PrecioUnitario,
    CAST(ROUND(Cantidad * PrecioUnitario, 2) AS DECIMAL(12,2)) AS Total,
    Observaciones
FROM dbo.Ventas
ORDER BY Fecha DESC, IdVenta DESC;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["IdVenta"] != null)
                    dataGridView1.Columns["IdVenta"].Visible = false;

                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) => this.Close();
    }
}

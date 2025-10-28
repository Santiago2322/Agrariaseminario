using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Ventas : Form
    {
        // USA LA MISMA CADENA QUE EN TUS OTROS FORMS
        private const string CADENA_CONEXION =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        public Consulta_de_Ventas()
        {
            InitializeComponent();
            this.Load += Consulta_de_Ventas_Load;
            this.button1.Click += button1_Click; // Cerrar
        }

        private void Consulta_de_Ventas_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaVentas();   // <-- crea la tabla si no existe
                CargarVentas();        // <-- después consulta
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea dbo.Ventas si no existe e inserta algunos datos de ejemplo si la tabla quedó vacía.
        /// </summary>
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
        Fecha          DATETIME2 NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT SYSUTCDATETIME(),
        Producto       NVARCHAR(120) NOT NULL,
        Cantidad       INT NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Total          AS (Cantidad * PrecioUnitario) PERSISTED,
        Observaciones  NVARCHAR(300) NULL
    );
END;

-- Si está vacía, cargo ejemplos
IF NOT EXISTS (SELECT 1 FROM dbo.Ventas)
BEGIN
    INSERT INTO dbo.Ventas (Producto, Cantidad, PrecioUnitario, Observaciones)
    VALUES 
    (N'Maceta 10L', 20, 1200, N'Venta local'),
    (N'Planta ornamental', 15, 3500, N'Cliente externo'),
    (N'Fertilizante 2kg', 10, 2500, N''),
    (N'Semillas de albahaca', 30, 300, N'Promo primavera');
END;
";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarVentas()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(
                "SELECT IdVenta, Fecha, Producto, Cantidad, PrecioUnitario, Total, Observaciones FROM dbo.Ventas ORDER BY Fecha DESC", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ReadOnly = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Vuelve a la Pantalla_Principal
        }
    }
}

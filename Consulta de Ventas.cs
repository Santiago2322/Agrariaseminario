using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Ventas : Form
    {
        // Conexión unificada
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Consulta_de_Ventas()
        {
            InitializeComponent();
            this.Load += Consulta_de_Ventas_Load;

            // Si tu botón "Cerrar" del diseñador se llama distinto, cambia "button1" por su nombre real
            this.button1.Click += button1_Click;

            this.AutoScroll = true;
        }

        private void Consulta_de_Ventas_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaVentas();   // crea/ajusta la tabla si no existe o le faltan columnas
                CargarVentas();        // carga la grilla
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Asegura la tabla dbo.Ventas con el ESQUEMA usado por Registro_de_una_Venta.
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
        Fecha          DATE            NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT (CONVERT(date, SYSUTCDATETIME())),
        Hora           NVARCHAR(20)    NULL,
        Cliente        NVARCHAR(150)   NULL,
        Producto       NVARCHAR(120)   NOT NULL,
        Cantidad       INT             NOT NULL,
        PrecioUnitario DECIMAL(10,2)   NOT NULL,
        Total          AS (Cantidad * PrecioUnitario) PERSISTED,
        Observaciones  NVARCHAR(300)   NULL
    );
END;

-- Columnas que podrían faltar si venías de un esquema anterior
IF COL_LENGTH('dbo.Ventas','Hora') IS NULL
    ALTER TABLE dbo.Ventas ADD Hora NVARCHAR(20) NULL;

IF COL_LENGTH('dbo.Ventas','Cliente') IS NULL
    ALTER TABLE dbo.Ventas ADD Cliente NVARCHAR(150) NULL;

IF COL_LENGTH('dbo.Ventas','Observaciones') IS NULL
    ALTER TABLE dbo.Ventas ADD Observaciones NVARCHAR(300) NULL;

-- Si no existe la columna calculada Total, la creamos (maneja el caso de que ya exista)
IF COL_LENGTH('dbo.Ventas','Total') IS NULL
BEGIN
    ALTER TABLE dbo.Ventas ADD Total AS (Cantidad * PrecioUnitario) PERSISTED;
END;

-- Datos de ejemplo si quedó vacía
IF NOT EXISTS (SELECT 1 FROM dbo.Ventas)
BEGIN
    INSERT INTO dbo.Ventas (Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Observaciones)
    VALUES
    (CONVERT(date, SYSUTCDATETIME()), N'10:00', N'Cliente local', N'Maceta 10L', 20, 1200, N'Venta mostrador'),
    (CONVERT(date, SYSUTCDATETIME()), N'11:15', N'Juan Pérez',  N'Planta ornamental', 15, 3500, N'Pedido externo'),
    (CONVERT(date, SYSUTCDATETIME()), N'12:40', N'',            N'Fertilizante 2kg', 10, 2500, N''),
    (CONVERT(date, SYSUTCDATETIME()), N'15:05', N'Ana López',   N'Semillas de albahaca', 30, 300, N'Promo primavera');
END;
";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarVentas()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(@"
SELECT 
    IdVenta, Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Total, Observaciones
FROM dbo.Ventas
ORDER BY Fecha DESC, IdVenta DESC;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                // Opcional: oculto IdVenta
                if (dataGridView1.Columns["IdVenta"] != null)
                    dataGridView1.Columns["IdVenta"].Visible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ReadOnly = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // volver a Pantalla Principal
        }
    }
}


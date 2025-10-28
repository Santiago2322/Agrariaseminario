using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Inventario : Form
    {
        // Usa exactamente la misma cadena que en el resto del proyecto
        private const string CADENA_CONEXION =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        public Inventario()
        {
            InitializeComponent();
            this.Load += Inventario_Load;
            this.button1.Click += button1_Click; // Cerrar
        }

        private void Inventario_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaInventario();  // crea tabla si falta + datos demo
                CargarInventario();       // llena el grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar inventario.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea dbo.Inventario si no existe y carga ejemplos si está vacía.
        /// </summary>
        private void EnsureTablaInventario()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre          NVARCHAR(120) NOT NULL,
        Categoria       NVARCHAR(80)  NULL,
        Stock           INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo     INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario   DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion       NVARCHAR(120) NULL,
        Observaciones   NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2  NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Inventario)
BEGIN
    INSERT INTO dbo.Inventario (Nombre, Categoria, Stock, StockMinimo, CostoUnitario, Ubicacion, Observaciones)
    VALUES
    (N'Maceta 10L',      N'Insumos',         120, 30, 1200, N'Depósito A1', N'Lote primavera'),
    (N'Sustrato 50L',    N'Insumos',          40, 15, 5200, N'Depósito B3', N'Verificar humedad'),
    (N'Planta ornamental',N'Venta',           35, 10, 3500, N'Invernadero 2', N'En floración'),
    (N'Fertilizante 2kg',N'Insumos',          18, 10, 2500, N'Depósito Químicos', N'Perecedero'),
    (N'Semillas albahaca',N'Semillas',       300,100,  300, N'Almacén Semillas', N'Nueva partida');
END;
";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarInventario()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(
                @"SELECT IdItem, Nombre, Categoria, Stock, StockMinimo, CostoUnitario, 
                         Ubicacion, Observaciones, FechaActualizacion
                  FROM dbo.Inventario
                  ORDER BY Nombre", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ReadOnly = true;

                if (dataGridView1.Columns["IdItem"] != null)
                    dataGridView1.Columns["IdItem"].Visible = false;
            }
        }

        // Botón cerrar (ya lo tenías)
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Stubs por si el Designer los tiene enganchados
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}

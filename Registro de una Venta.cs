using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Registro_de_una_Venta : Form
    {
        // 🔗 MISMA cadena que el resto del proyecto
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Registro_de_una_Venta()
        {
            InitializeComponent();
            this.AutoScroll = true;

            // (Opcional) recalcular total cuando cambian cantidad o precio
            textBox3.TextChanged += (s, e) => RecalcularTotal();
            textBox4.TextChanged += (s, e) => RecalcularTotal();
        }

        private void Registro_de_una_Venta_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablasVentasProductos();
                CargarProductos();

                // Hora por defecto (HH:mm)
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    textBox1.Text = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo inicializar el formulario de ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // ========== BOTÓN GUARDAR ====================
        // =============================================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Seleccioná un producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus(); return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida (entero > 0).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus(); return;
            }

            if (!decimal.TryParse(textBox4.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Precio inválido (número).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus(); return;
            }

            // Si Total viene vacío, lo calculo
            decimal total;
            if (!decimal.TryParse(textBox6.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out total))
                total = cantidad * precio;

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
INSERT INTO dbo.Ventas (Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Total)
VALUES (@Fecha, @Hora, @Cliente, @Producto, @Cantidad, @Precio, @Total);";

                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = DateTime.Today.Date;

                    // Hora como texto hh:mm (si querés TIME: cambia tipo en tabla y parseá)
                    cmd.Parameters.Add("@Hora", SqlDbType.NVarChar, 10).Value =
                        string.IsNullOrWhiteSpace(textBox1.Text) ? DateTime.Now.ToString("HH:mm") : textBox1.Text.Trim();

                    cmd.Parameters.Add("@Cliente", SqlDbType.NVarChar, 120).Value =
                        string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text.Trim();

                    cmd.Parameters.Add("@Producto", SqlDbType.NVarChar, 150).Value = comboBox2.Text.Trim();
                    cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = cantidad;
                    cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = Math.Round(precio, 2);
                    cmd.Parameters.Add("@Total", SqlDbType.Decimal).Value = Math.Round(total, 2);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Venta registrada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la venta:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // ========== BOTÓN CANCELAR ===================
        // =============================================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("¿Deseás cancelar el registro de venta?",
                                    "Confirmar cancelación",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);
            if (r == DialogResult.Yes) this.Close();
        }

        // =============================================
        // ========== CARGA DE PRODUCTOS ===============
        // =============================================
        private void CargarProductos()
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT NombreProducto FROM dbo.Productos ORDER BY NombreProducto;";
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var nombre = Convert.ToString(rd["NombreProducto"]);
                            comboBox2.Items.Add(nombre);
                            comboBox3.Items.Add(nombre);
                        }
                    }
                }
            }
            catch
            {
                // No bloqueo el uso si falla la carga (puede completar manualmente)
            }
        }

        // =============================================
        // ========== ESQUEMA (si falta) ===============
        // =============================================
        private void EnsureTablasVentasProductos()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos(
        IdProducto       INT IDENTITY(1,1) PRIMARY KEY,
        NombreProducto   NVARCHAR(150) NOT NULL,
        PrecioUnitario   DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock            INT NOT NULL CONSTRAINT DF_Prod_Stock DEFAULT(0),
        Unidad           NVARCHAR(30) NULL
    );

    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad)
    VALUES (N'Plantín de lechuga', 500, 100, N'unid'),
           (N'Bolsa de sustrato 25kg', 8500, 40, N'bolsa'),
           (N'Bandeja de huevos x12', 3200, 60, N'unid');
END;

IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas(
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATE NOT NULL,
        Hora           NVARCHAR(10) NULL,  -- si la querés TIME, cambiá el tipo y cómo lo guardás
        Cliente        NVARCHAR(120) NULL,
        Producto       NVARCHAR(150) NOT NULL,
        Cantidad       INT NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Total          DECIMAL(12,2) NOT NULL
    );
END;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =============================================
        // ========== UTIL =============================
        // =============================================
        private void RecalcularTotal()
        {
            if (int.TryParse(textBox3.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int cant) &&
                decimal.TryParse(textBox4.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio))
            {
                textBox6.Text = Math.Round(cant * precio, 2).ToString("0.00", CultureInfo.InvariantCulture);
            }
        }

        // Handlers ya existentes en tu código
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Campo Cliente, opcional
        }
    }
}

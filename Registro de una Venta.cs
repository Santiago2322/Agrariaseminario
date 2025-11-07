using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Registro_de_una_Venta : Form
    {
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Registro_de_una_Venta()
        {
            InitializeComponent();

            // Recalcular subtotal/total en línea 1
            txtCantidad.TextChanged += (s, e) => Recalcular();
            txtPrecio.TextChanged += (s, e) => Recalcular();

            // Recalcular subtotal/total en línea 2 (si visible)
            txtCantidad2.TextChanged += (s, e) => Recalcular();
            txtPrecio2.TextChanged += (s, e) => Recalcular();
        }

        private void Registro_de_una_Venta_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablas();
                CargarProductos();
                dtpFecha.Value = DateTime.Today;
                txtHora.Text = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo inicializar el formulario de ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                cboProducto.Items.Clear();
                cboProducto2.Items.Clear();

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
                            cboProducto.Items.Add(nombre);
                            cboProducto2.Items.Add(nombre);
                        }
                    }
                }
            }
            catch { /* no bloqueo si falla la carga */ }
        }

        private void EnsureTablas()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos(
        IdProducto INT IDENTITY(1,1) PRIMARY KEY,
        NombreProducto NVARCHAR(150) NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock INT NOT NULL CONSTRAINT DF_Prod_Stock DEFAULT(0),
        Unidad NVARCHAR(30) NULL
    );
END;

IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas(
        IdVenta INT IDENTITY(1,1) PRIMARY KEY,
        Fecha  DATE NOT NULL,
        Hora   NVARCHAR(10) NULL,
        Cliente NVARCHAR(120) NULL,
        Producto NVARCHAR(150) NOT NULL,
        Cantidad INT NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        -- Total es calculado en consulta; NO crear columna calculada aquí para simplificar
        Observaciones NVARCHAR(300) NULL
    );
END;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(cboProducto.Text))
            { MessageBox.Show("Seleccioná un producto."); cboProducto.Focus(); return; }

            if (!int.TryParse(txtCantidad.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int c1) || c1 <= 0)
            { MessageBox.Show("Cantidad inválida (entero > 0)."); txtCantidad.Focus(); return; }

            if (!decimal.TryParse(txtPrecio.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal p1) || p1 < 0)
            { MessageBox.Show("Precio inválido."); txtPrecio.Focus(); return; }

            // (opcional) segunda línea si está visible y completa
            bool linea2 = cboProducto2.Visible && !string.IsNullOrWhiteSpace(cboProducto2.Text);
            int c2 = 0; decimal p2 = 0m;
            if (linea2)
            {
                if (!int.TryParse(txtCantidad2.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out c2) || c2 <= 0)
                { MessageBox.Show("Cantidad (producto 2) inválida."); txtCantidad2.Focus(); return; }

                if (!decimal.TryParse(txtPrecio2.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out p2) || p2 < 0)
                { MessageBox.Show("Precio (producto 2) inválido."); txtPrecio2.Focus(); return; }
            }

            try
            {
                using (var cn = new SqlConnection(CADENA))
                {
                    cn.Open();
                    // INSERT línea 1 (NO se inserta columna Total → evita error de calculada/UNION)
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = @"
INSERT INTO dbo.Ventas (Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Observaciones)
VALUES (@F,@H,@Cli,@Prod,@Cant,@Precio,NULL);";
                        cmd.Parameters.Add("@F", SqlDbType.Date).Value = dtpFecha.Value.Date;
                        cmd.Parameters.Add("@H", SqlDbType.NVarChar, 10).Value = string.IsNullOrWhiteSpace(txtHora.Text) ? (object)DBNull.Value : txtHora.Text.Trim();
                        cmd.Parameters.Add("@Cli", SqlDbType.NVarChar, 120).Value = string.IsNullOrWhiteSpace(txtCliente.Text) ? (object)DBNull.Value : txtCliente.Text.Trim();
                        cmd.Parameters.Add("@Prod", SqlDbType.NVarChar, 150).Value = cboProducto.Text.Trim();
                        cmd.Parameters.Add("@Cant", SqlDbType.Int).Value = c1;
                        cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = Math.Round(p1, 2);
                        cmd.ExecuteNonQuery();
                    }

                    // INSERT línea 2 (si corresponde)
                    if (linea2)
                    {
                        using (var cmd2 = cn.CreateCommand())
                        {
                            cmd2.CommandText = @"
INSERT INTO dbo.Ventas (Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Observaciones)
VALUES (@F,@H,@Cli,@Prod,@Cant,@Precio,NULL);";
                            cmd2.Parameters.Add("@F", SqlDbType.Date).Value = dtpFecha.Value.Date;
                            cmd2.Parameters.Add("@H", SqlDbType.NVarChar, 10).Value = string.IsNullOrWhiteSpace(txtHora.Text) ? (object)DBNull.Value : txtHora.Text.Trim();
                            cmd2.Parameters.Add("@Cli", SqlDbType.NVarChar, 120).Value = string.IsNullOrWhiteSpace(txtCliente.Text) ? (object)DBNull.Value : txtCliente.Text.Trim();
                            cmd2.Parameters.Add("@Prod", SqlDbType.NVarChar, 150).Value = cboProducto2.Text.Trim();
                            cmd2.Parameters.Add("@Cant", SqlDbType.Int).Value = c2;
                            cmd2.Parameters.Add("@Precio", SqlDbType.Decimal).Value = Math.Round(p2, 2);
                            cmd2.ExecuteNonQuery();
                        }
                    }
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cancelar el registro de venta?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        private void Recalcular()
        {
            decimal s1 = 0m, s2 = 0m;

            if (int.TryParse(txtCantidad.Text.Trim(), out var c1) &&
                decimal.TryParse(txtPrecio.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var p1))
            {
                s1 = c1 * p1;
                txtSubtotal.Text = s1.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else txtSubtotal.Clear();

            if (cboProducto2.Visible &&
                int.TryParse(txtCantidad2.Text.Trim(), out var c2) &&
                decimal.TryParse(txtPrecio2.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var p2))
            {
                s2 = c2 * p2;
                txtSubtotal2.Text = s2.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else txtSubtotal2.Clear();

            txtTotal.Text = (s1 + s2).ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}

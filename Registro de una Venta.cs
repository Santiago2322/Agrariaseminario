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

            // Recalcular subtotales del editor (línea 1 y 2)
            txtCantidad.TextChanged += (s, e) => RecalcularEditor();
            txtPrecio.TextChanged += (s, e) => RecalcularEditor();
            txtCantidad2.TextChanged += (s, e) => RecalcularEditor();
            txtPrecio2.TextChanged += (s, e) => RecalcularEditor();

            // Autocompletar precio + stock al elegir producto
            cboProducto.SelectedIndexChanged += (s, e) =>
                AutocompletarPrecioYStock(cboProducto, txtPrecio, lblStock1);

            cboProducto2.SelectedIndexChanged += (s, e) =>
                AutocompletarPrecioYStock(cboProducto2, txtPrecio2, lblStock2);

            // Botón agregar líneas a la grilla
            btnAgregar.Click += btnAgregar_Click;

            // Alta rápida de producto
            btnAltaProd.Click += btnAltaProd_Click;

            // Guardar y cancelar
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void Registro_de_una_Venta_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablas();
                CargarProductos();
                PrepararGrillaLineas();

                dtpFecha.Value = DateTime.Today;
                txtHora.Text = DateTime.Now.ToString("HH:mm");

                RecalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo inicializar el formulario de ventas.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        //  TABLAS (compatibles con tu script de base Agraria)
        // =========================================================
        private void EnsureTablas()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        IdProducto     INT IDENTITY(1,1) CONSTRAINT PK_Productos PRIMARY KEY,
        NombreProducto NVARCHAR(150) NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock          INT           NOT NULL CONSTRAINT DF_Prod_Stock  DEFAULT(0),
        Unidad         NVARCHAR(30)  NULL
    );
END;

IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) CONSTRAINT PK_Ventas PRIMARY KEY,
        Fecha          DATE          NOT NULL,
        Hora           NVARCHAR(10)  NULL,
        Cliente        NVARCHAR(120) NULL,
        Producto       NVARCHAR(150) NOT NULL,
        Cantidad       INT           NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Observaciones  NVARCHAR(300) NULL
    );
END;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =========================================================
        //  PRODUCTOS (para combos)
        // =========================================================
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
                            if (!string.IsNullOrWhiteSpace(nombre))
                            {
                                cboProducto.Items.Add(nombre);
                                cboProducto2.Items.Add(nombre);
                            }
                        }
                    }
                }

                lblStock1.Text = "";
                lblStock2.Text = "";
            }
            catch
            {
                // si falla, no bloqueo el formulario
            }
        }

        // =========================================================
        //  ALTA RÁPIDA DE PRODUCTO (NOMBRE + PRECIO + STOCK)
        // =========================================================
        private void btnAltaProd_Click(object sender, EventArgs e)
        {
            string nombre = txtNuevoProd.Text.Trim();
            string sPrecio = txtNuevoPrecio.Text.Trim().Replace(',', '.');
            string sStock = txtNuevoStock.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresá el nombre del nuevo producto.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevoProd.Focus();
                return;
            }

            if (!decimal.TryParse(sPrecio, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Precio unitario inválido.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevoPrecio.Focus();
                return;
            }

            if (!int.TryParse(sStock, NumberStyles.Integer, CultureInfo.InvariantCulture, out int stock) || stock < 0)
            {
                MessageBox.Show("Stock inicial inválido (entero ≥ 0).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevoStock.Focus();
                return;
            }

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
IF EXISTS (SELECT 1 FROM dbo.Productos WHERE NombreProducto = @n)
BEGIN
    -- Actualizo precio y stock (no dejo stock negativo)
    UPDATE dbo.Productos
       SET PrecioUnitario = @p,
           Stock = CASE WHEN @s < 0 THEN 0 ELSE @s END
     WHERE NombreProducto = @n;
END
ELSE
BEGIN
    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad)
    VALUES (@n, @p, @s, N'unid');
END;";
                    cmd.Parameters.AddWithValue("@n", nombre);
                    cmd.Parameters.Add("@p", SqlDbType.Decimal).Value = Math.Round(precio, 2);
                    cmd.Parameters.Add("@s", SqlDbType.Int).Value = stock;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Producto guardado/actualizado correctamente.",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // refresco combos
                CargarProductos();

                // dejo seleccionado el nuevo producto
                cboProducto.SelectedItem = nombre;
                cboProducto2.SelectedIndex = -1;

                txtNuevoProd.Clear();
                txtNuevoPrecio.Clear();
                txtNuevoStock.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        //  STOCK / PRECIO al seleccionar producto
        // =========================================================
        private void AutocompletarPrecioYStock(ComboBox combo, TextBox destinoPrecio, Label lblStock)
        {
            var nombre = combo.Text?.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                destinoPrecio.Clear();
                lblStock.Text = "";
                return;
            }

            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT TOP 1 PrecioUnitario, Stock, ISNULL(Unidad, N'unid') AS Unidad
FROM dbo.Productos
WHERE NombreProducto = @n;";
                    cmd.Parameters.AddWithValue("@n", nombre);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            var precio = rd.GetDecimal(rd.GetOrdinal("PrecioUnitario"));
                            int stock = rd.GetInt32(rd.GetOrdinal("Stock"));
                            string unidad = rd.GetString(rd.GetOrdinal("Unidad"));

                            destinoPrecio.Text = precio.ToString("0.00", CultureInfo.InvariantCulture);
                            lblStock.Text = $"Stock disponible: {stock} {unidad}";
                        }
                        else
                        {
                            destinoPrecio.Clear();
                            lblStock.Text = "Producto sin referencia de stock.";
                        }
                    }
                }
            }
            catch
            {
                // no rompo el flujo, sólo no actualizo
            }

            RecalcularEditor();
        }

        // =========================================================
        //  GRILLA de líneas de venta
        // =========================================================
        private void PrepararGrillaLineas()
        {
            dgvLineas.Rows.Clear();
            dgvLineas.Columns.Clear();

            dgvLineas.ReadOnly = false;
            dgvLineas.AllowUserToAddRows = false;
            dgvLineas.AllowUserToDeleteRows = true;
            dgvLineas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLineas.MultiSelect = false;
            dgvLineas.RowHeadersVisible = false;
            dgvLineas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLineas.BackgroundColor = System.Drawing.Color.White;

            dgvLineas.Columns.Add("Producto", "Producto");
            dgvLineas.Columns.Add("Cantidad", "Cantidad");
            dgvLineas.Columns.Add("PrecioUnitario", "Precio Unitario");
            dgvLineas.Columns.Add("Subtotal", "Subtotal");

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "Accion",
                HeaderText = "Acción",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 90
            };
            dgvLineas.Columns.Add(colBtn);

            dgvLineas.CellContentClick -= dgvLineas_CellContentClick;
            dgvLineas.CellContentClick += dgvLineas_CellContentClick;
        }

        private void dgvLineas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                e.ColumnIndex == dgvLineas.Columns["Accion"].Index)
            {
                dgvLineas.Rows.RemoveAt(e.RowIndex);
                RecalcularTotal();
            }
        }

        // =========================================================
        //  EDITOR → AGREGA a la GRILLA (con control de stock)
        // =========================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Línea 1
            AgregarLineaEditor(cboProducto, txtCantidad, txtPrecio, lblStock1);

            // Línea 2 (si está habilitada y tiene algo)
            if (chkProd2.Checked && cboProducto2.Visible && !string.IsNullOrWhiteSpace(cboProducto2.Text))
            {
                AgregarLineaEditor(cboProducto2, txtCantidad2, txtPrecio2, lblStock2);
            }

            // Limpio el editor para poder cargar más productos cómodamente
            LimpiarEditor();
        }

        private void AgregarLineaEditor(ComboBox combo, TextBox txtCant, TextBox txtPrecio, Label lblStock)
        {
            string prod = combo.Text?.Trim();
            if (string.IsNullOrEmpty(prod))
                return;

            if (!int.TryParse(txtCant.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int cant) || cant <= 0)
            {
                MessageBox.Show("Cantidad inválida (entero > 0).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCant.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim().Replace(',', '.'), NumberStyles.Number,
                CultureInfo.InvariantCulture, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Precio inválido.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            // Stock actual en BD
            int stockDb = ObtenerStockProducto(prod, out string unidadDb);
            if (stockDb < 0)
            {
                MessageBox.Show("El producto no existe en la tabla Productos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cantidad ya cargada en la grilla para este producto
            int yaEnGrilla = 0;
            foreach (DataGridViewRow row in dgvLineas.Rows)
            {
                if (row.IsNewRow) continue;
                string prodRow = Convert.ToString(row.Cells["Producto"].Value);
                if (string.Equals(prodRow, prod, StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(Convert.ToString(row.Cells["Cantidad"].Value), out int cRow))
                        yaEnGrilla += cRow;
                }
            }

            int cantidadTotal = yaEnGrilla + cant;

            if (cantidadTotal > stockDb)
            {
                MessageBox.Show(
                    $"No hay suficiente stock para '{prod}'.\n" +
                    $"Disponible: {stockDb} {unidadDb}\n" +
                    $"Intentás cargar: {cantidadTotal}.",
                    "Stock insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Si ya existe misma combinación producto+precio, sumo cantidades
            foreach (DataGridViewRow row in dgvLineas.Rows)
            {
                if (row.IsNewRow) continue;

                string prodRow = Convert.ToString(row.Cells["Producto"].Value);
                if (!string.Equals(prodRow, prod, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!decimal.TryParse(Convert.ToString(row.Cells["PrecioUnitario"].Value).Replace(',', '.'),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precioRow))
                    continue;

                if (precioRow == precio)
                {
                    int cantRow = Convert.ToInt32(row.Cells["Cantidad"].Value);
                    cantRow += cant;
                    row.Cells["Cantidad"].Value = cantRow;
                    row.Cells["Subtotal"].Value = (cantRow * precio).ToString("0.00", CultureInfo.InvariantCulture);
                    RecalcularTotal();
                    return;
                }
            }

            // Nueva fila
            decimal subtotal = cant * precio;
            dgvLineas.Rows.Add(
                prod,
                cant,
                precio.ToString("0.00", CultureInfo.InvariantCulture),
                subtotal.ToString("0.00", CultureInfo.InvariantCulture),
                "Eliminar");

            RecalcularTotal();
        }

        private int ObtenerStockProducto(string nombre, out string unidad)
        {
            unidad = "unid";
            try
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT TOP 1 Stock, ISNULL(Unidad, N'unid') AS Unidad
FROM dbo.Productos
WHERE NombreProducto = @n;";
                    cmd.Parameters.AddWithValue("@n", nombre);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            int stock = rd.GetInt32(rd.GetOrdinal("Stock"));
                            unidad = rd.GetString(rd.GetOrdinal("Unidad"));
                            return stock;
                        }
                    }
                }
            }
            catch
            {
                // en caso de error, devuelvo -1
            }
            return -1;
        }

        // =========================================================
        //  REGLAS DE CÁLCULO
        // =========================================================
        private void RecalcularEditor()
        {
            decimal s1 = 0m, s2 = 0m;

            if (int.TryParse(txtCantidad.Text.Trim(), out var c1) &&
                decimal.TryParse(txtPrecio.Text.Trim().Replace(',', '.'),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out var p1))
            {
                s1 = c1 * p1;
                txtSubtotal.Text = s1.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else txtSubtotal.Clear();

            if (chkProd2.Checked && cboProducto2.Visible &&
                int.TryParse(txtCantidad2.Text.Trim(), out var c2) &&
                decimal.TryParse(txtPrecio2.Text.Trim().Replace(',', '.'),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out var p2))
            {
                s2 = c2 * p2;
                txtSubtotal2.Text = s2.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else txtSubtotal2.Clear();

            RecalcularTotal();
        }

        private void RecalcularTotal()
        {
            decimal total = 0m;

            foreach (DataGridViewRow row in dgvLineas.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Subtotal"].Value == null) continue;

                if (decimal.TryParse(Convert.ToString(row.Cells["Subtotal"].Value).Replace(',', '.'),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out var s))
                {
                    total += s;
                }
            }

            txtTotal.Text = total.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void LimpiarEditor()
        {
            cboProducto.SelectedIndex = -1;
            cboProducto.Text = "";
            txtCantidad.Clear();
            txtPrecio.Clear();
            txtSubtotal.Clear();
            lblStock1.Text = "";

            cboProducto2.SelectedIndex = -1;
            cboProducto2.Text = "";
            txtCantidad2.Clear();
            txtPrecio2.Clear();
            txtSubtotal2.Clear();
            lblStock2.Text = "";

            cboProducto.Focus();
        }

        // =========================================================
        //  GUARDAR / CANCELAR (INSERT VENTAS + UPDATE STOCK)
        // =========================================================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dgvLineas.Rows.Count == 0)
            {
                MessageBox.Show("Agregá al menos un producto a la grilla.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var cn = new SqlConnection(CADENA))
                {
                    cn.Open();
                    using (var tx = cn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataGridViewRow row in dgvLineas.Rows)
                            {
                                if (row.IsNewRow) continue;

                                string prod = Convert.ToString(row.Cells["Producto"].Value);
                                int cant = Convert.ToInt32(row.Cells["Cantidad"].Value);
                                decimal precio = Convert.ToDecimal(
                                    Convert.ToString(row.Cells["PrecioUnitario"].Value).Replace(',', '.'),
                                    CultureInfo.InvariantCulture);

                                // 1) Insert venta
                                using (var cmd = cn.CreateCommand())
                                {
                                    cmd.Transaction = tx;
                                    cmd.CommandText = @"
INSERT INTO dbo.Ventas (Fecha, Hora, Cliente, Producto, Cantidad, PrecioUnitario, Observaciones)
VALUES (@F,@H,@Cli,@Prod,@Cant,@Precio,NULL);";
                                    cmd.Parameters.Add("@F", SqlDbType.Date).Value = dtpFecha.Value.Date;
                                    cmd.Parameters.Add("@H", SqlDbType.NVarChar, 10).Value =
                                        string.IsNullOrWhiteSpace(txtHora.Text) ? (object)DBNull.Value : txtHora.Text.Trim();
                                    cmd.Parameters.Add("@Cli", SqlDbType.NVarChar, 120).Value =
                                        string.IsNullOrWhiteSpace(txtCliente.Text) ? (object)DBNull.Value : txtCliente.Text.Trim();
                                    cmd.Parameters.Add("@Prod", SqlDbType.NVarChar, 150).Value = prod;
                                    cmd.Parameters.Add("@Cant", SqlDbType.Int).Value = cant;
                                    cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = Math.Round(precio, 2);
                                    cmd.ExecuteNonQuery();
                                }

                                // 2) Descontar stock sin permitir negativo
                                using (var cmd2 = cn.CreateCommand())
                                {
                                    cmd2.Transaction = tx;
                                    cmd2.CommandText = @"
UPDATE dbo.Productos
   SET Stock = Stock - @c
 WHERE NombreProducto = @n
   AND Stock >= @c;";
                                    cmd2.Parameters.Add("@c", SqlDbType.Int).Value = cant;
                                    cmd2.Parameters.AddWithValue("@n", prod);

                                    int filas = cmd2.ExecuteNonQuery();
                                    if (filas == 0)
                                    {
                                        throw new InvalidOperationException(
                                            $"No hay stock suficiente para '{prod}' al momento de guardar.");
                                    }
                                }
                            }

                            tx.Commit();
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("Venta registrada correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                PrepararGrillaLineas();
                LimpiarEditor();
                RecalcularTotal();
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
            if (MessageBox.Show("¿Cancelar el registro de venta?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }
    }
}

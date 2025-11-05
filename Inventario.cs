using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Inventario : Form
    {
        private const string CADENA_CONEXION =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Inventario()
        {
            InitializeComponent();
            Load += Inventario_Load;
        }

        private void Inventario_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaInventario();
                CargarInventario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar inventario.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCerrar_Click(object sender, EventArgs e) => Close();

        // ====== SQL ======
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
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NULL,
        Unidad             NVARCHAR(30)  NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NULL,
        Observaciones      NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END;

IF COL_LENGTH('dbo.Inventario','Unidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Unidad NVARCHAR(30) NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid';

IF COL_LENGTH('dbo.Inventario','Cantidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Cantidad AS (Stock) PERSISTED;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarInventario()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var da = new SqlDataAdapter(
                @"SELECT IdItem, Nombre, Categoria, Unidad, Stock, StockMinimo, CostoUnitario, 
                         Ubicacion, Observaciones, FechaActualizacion
                  FROM dbo.Inventario
                  ORDER BY Nombre;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["IdItem"] != null)
                    dataGridView1.Columns["IdItem"].Visible = false;

                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        // ====== Botones ======
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (var dlg = new ItemDialog())
            {
                dlg.Text = "Nuevo ítem de inventario";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
INSERT INTO dbo.Inventario
(Nombre, Categoria, Unidad, Stock, StockMinimo, CostoUnitario, Ubicacion, Observaciones, FechaActualizacion)
VALUES(@Nombre,@Categoria,@Unidad,@Stock,@StockMinimo,@CostoUnitario,@Ubicacion,@Observaciones,SYSUTCDATETIME());";
                    FillParams(cmd, dlg);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                CargarInventario();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = (dataGridView1.CurrentRow.DataBoundItem as DataRowView)?.Row;
            if (row == null) return;

            using (var dlg = new ItemDialog())
            {
                // precargar
                dlg.TxtNombre.Text = row["Nombre"]?.ToString();
                dlg.TxtCategoria.Text = row["Categoria"]?.ToString();
                dlg.TxtUnidad.Text = row["Unidad"]?.ToString();
                dlg.NumStock.Value = ToDecimal(row["Stock"]);
                dlg.NumStockMin.Value = ToDecimal(row["StockMinimo"]);
                dlg.NumCosto.Value = ToDecimal(row["CostoUnitario"]);
                dlg.TxtUbicacion.Text = row["Ubicacion"]?.ToString();
                dlg.TxtObs.Text = row["Observaciones"]?.ToString();

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                int id = Convert.ToInt32(row["IdItem"]);
                using (var cn = new SqlConnection(CADENA_CONEXION))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
UPDATE dbo.Inventario
SET Nombre=@Nombre, Categoria=@Categoria, Unidad=@Unidad, Stock=@Stock,
    StockMinimo=@StockMinimo, CostoUnitario=@CostoUnitario, Ubicacion=@Ubicacion,
    Observaciones=@Observaciones, FechaActualizacion=SYSUTCDATETIME()
WHERE IdItem=@Id;";
                    FillParams(cmd, dlg);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                CargarInventario();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = (dataGridView1.CurrentRow.DataBoundItem as DataRowView)?.Row;
            if (row == null) return;

            string nombre = row["Nombre"]?.ToString();
            int id = Convert.ToInt32(row["IdItem"]);

            if (MessageBox.Show($"¿Eliminar \"{nombre}\" del inventario?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM dbo.Inventario WHERE IdItem=@Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarInventario();
        }

        // ===== Helpers =====
        private static decimal ToDecimal(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            decimal d; return decimal.TryParse(o.ToString(), out d) ? d : 0m;
        }

        private static void FillParams(SqlCommand cmd, ItemDialog dlg)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nombre", dlg.TxtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@Categoria", (object)(dlg.TxtCategoria.Text.Trim()) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Unidad", string.IsNullOrWhiteSpace(dlg.TxtUnidad.Text) ? "unid" : dlg.TxtUnidad.Text.Trim());
            cmd.Parameters.AddWithValue("@Stock", (int)dlg.NumStock.Value);
            cmd.Parameters.AddWithValue("@StockMinimo", (int)dlg.NumStockMin.Value);
            cmd.Parameters.AddWithValue("@CostoUnitario", dlg.NumCosto.Value);
            cmd.Parameters.AddWithValue("@Ubicacion", (object)(dlg.TxtUbicacion.Text.Trim()) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Observaciones", (object)(dlg.TxtObs.Text.Trim()) ?? DBNull.Value);
        }

        // ===== Diálogo simple para Agregar/Modificar =====
        private sealed class ItemDialog : Form
        {
            public TextBox TxtNombre = new TextBox();
            public TextBox TxtCategoria = new TextBox();
            public TextBox TxtUnidad = new TextBox();
            public NumericUpDown NumStock = new NumericUpDown();
            public NumericUpDown NumStockMin = new NumericUpDown();
            public NumericUpDown NumCosto = new NumericUpDown();
            public TextBox TxtUbicacion = new TextBox();
            public TextBox TxtObs = new TextBox();

            public ItemDialog()
            {
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false; MinimizeBox = false;
                Width = 520; Height = 420;
                BackColor = System.Drawing.Color.White;

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(12),
                    ColumnCount = 2,
                    RowCount = 9,
                    AutoSize = true
                };
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

                var btnOk = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, Width = 100, Height = 32 };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Width = 100, Height = 32 };

                NumStock.Minimum = 0; NumStock.Maximum = 1_000_000; NumStock.DecimalPlaces = 0;
                NumStockMin.Minimum = 0; NumStockMin.Maximum = 1_000_000; NumStockMin.DecimalPlaces = 0;
                NumCosto.Minimum = 0; NumCosto.Maximum = 1_000_000; NumCosto.DecimalPlaces = 2; NumCosto.ThousandsSeparator = true;

                AddRow("Nombre *", TxtNombre);
                AddRow("Categoría", TxtCategoria);
                AddRow("Unidad", TxtUnidad);
                AddRow("Stock", NumStock);
                AddRow("Stock mínimo", NumStockMin);
                AddRow("Costo unitario", NumCosto);
                AddRow("Ubicación", TxtUbicacion);
                AddRow("Observaciones", TxtObs);

                var pnlBtns = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
                pnlBtns.Controls.Add(btnOk); pnlBtns.Controls.Add(btnCancel);
                table.Controls.Add(pnlBtns, 0, table.RowCount - 1);
                table.SetColumnSpan(pnlBtns, 2);

                Controls.Add(table);

                AcceptButton = btnOk; CancelButton = btnCancel;

                void AddRow(string label, Control input)
                {
                    int r = table.RowCount - 1;
                    var l = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 6, 0, 0) };
                    input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    table.Controls.Add(l, 0, r);
                    table.Controls.Add(input, 1, r);
                    table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    table.RowCount++;
                }

                Shown += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(TxtUnidad.Text)) TxtUnidad.Text = "unid";
                    TxtNombre.Focus();
                };

                // Validación básica
                FormClosing += (s, e) =>
                {
                    if (DialogResult != DialogResult.OK) return;
                    if (string.IsNullOrWhiteSpace(TxtNombre.Text))
                    {
                        MessageBox.Show("El nombre es obligatorio.", "Validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                };
            }
        }
    }
}

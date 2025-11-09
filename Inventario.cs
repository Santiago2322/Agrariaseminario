using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Inventario : Form
    {
        private const string CADENA_CONEXION =
                 @"Data Source=DESKTOP-92OCSA4;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        // Modelo en memoria
        private DataTable _dtInv;
        private DataView _view;

        public Inventario()
        {
            InitializeComponent();

            // === Eventos (no cableados en Designer para evitar duplicados) ===
            Load += Inventario_Load;
            buttonCerrar.Click += buttonCerrar_Click;

            btnAgregar.Click += btnAgregar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dataGridView1.SelectionChanged += (s, e) => ActualizarEstadoBotones();

            if (cboCategorias != null)
                cboCategorias.SelectedIndexChanged += (s, e) => AplicarFiltroCategoria();
        }

        // ========================= CARGA INICIAL =========================
        private void Inventario_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaInventario();
                CargarInventario();     // llena _dtInv, _view y grilla
                CargarCategorias();     // llena combo y deja SelectedIndex = -1

                // Sin selección inicial → Agregar habilitado, Mod/Del deshabilitados
                LimpiarSeleccionGrilla();
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo preparar/cargar inventario.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCerrar_Click(object sender, EventArgs e) => Close();

        // ========================= SQL / DATOS =========================
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
        Categoria          NVARCHAR(80)  NOT NULL,
        Unidad             NVARCHAR(30)  NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NOT NULL,
        Observaciones      NVARCHAR(300) NOT NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END;

-- Compatibilidad hacia atrás (agrega columnas que puedan faltar)
IF COL_LENGTH('dbo.Inventario','Unidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Unidad NVARCHAR(30) NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid';

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
                _dtInv = new DataTable();
                da.Fill(_dtInv);
                _view = new DataView(_dtInv);
                dataGridView1.DataSource = _view;

                if (dataGridView1.Columns["IdItem"] != null)
                    dataGridView1.Columns["IdItem"].Visible = false;

                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
            }
        }

        private void CargarCategorias()
        {
            cboCategorias.Items.Clear();

            // Recolectar categorías distintas del DataTable ya cargado
            if (_dtInv != null && _dtInv.Rows.Count > 0)
            {
                var vistaCat = new DataView(_dtInv);
                DataTable dtCat = vistaCat.ToTable(true, "Categoria");
                foreach (DataRow r in dtCat.Rows)
                {
                    var c = Convert.ToString(r["Categoria"]);
                    if (!string.IsNullOrWhiteSpace(c))
                        cboCategorias.Items.Add(c);
                }
            }

            // Añadimos opción "Todas" al final para que el usuario la elija si quiere
            cboCategorias.Items.Add("(Todas)");

            // *** Importante ***: SIN selección por defecto
            cboCategorias.SelectedIndex = -1;
            cboCategorias.Text = "";
        }

        private void AplicarFiltroCategoria()
        {
            if (_view == null) return;

            string sel = cboCategorias.Text == null ? "" : cboCategorias.Text.Trim();
            if (string.IsNullOrEmpty(sel) || string.Equals(sel, "(Todas)", StringComparison.OrdinalIgnoreCase))
            {
                _view.RowFilter = "";
            }
            else
            {
                // Escapar comillas simples para RowFilter
                var seguro = sel.Replace("'", "''");
                _view.RowFilter = $"Categoria = '{seguro}'";
            }

            // Tras filtrar, limpiar selección para que Agregar quede utilizable
            LimpiarSeleccionGrilla();
            ActualizarEstadoBotones();
        }

        // ========================= BOTONES CRUD =========================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (var dlg = new ItemDialog())
            {
                dlg.Text = "Nuevo ítem de inventario";

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                // Validaciones (todos obligatorios)
                if (!ValidarDialogo(dlg)) return;

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
                AplicarFiltroCategoria(); // respeta filtro actual
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rowView = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (rowView == null) return;
            var row = rowView.Row;

            using (var dlg = new ItemDialog())
            {
                // Precargar
                dlg.TxtNombre.Text = ToStringSafe(row["Nombre"]);
                dlg.TxtCategoria.Text = ToStringSafe(row["Categoria"]);
                dlg.TxtUnidad.Text = ToStringSafe(row["Unidad"]);
                dlg.NumStock.Value = ToDecimal(row["Stock"]);
                dlg.NumStockMin.Value = ToDecimal(row["StockMinimo"]);
                dlg.NumCosto.Value = ToDecimal(row["CostoUnitario"]);
                dlg.TxtUbicacion.Text = ToStringSafe(row["Ubicacion"]);
                dlg.TxtObs.Text = ToStringSafe(row["Observaciones"]);

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                if (!ValidarDialogo(dlg)) return;

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
                AplicarFiltroCategoria();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un registro.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rowView = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (rowView == null) return;
            var row = rowView.Row;

            string nombre = ToStringSafe(row["Nombre"]);
            int id = Convert.ToInt32(row["IdItem"]);

            if (MessageBox.Show($"¿Eliminar \"{nombre}\" del inventario?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM dbo.Inventario WHERE IdItem=@Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarInventario();
            AplicarFiltroCategoria();
        }

        // ========================= HELPERS =========================
        private void ActualizarEstadoBotones()
        {
            bool haySel = (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index >= 0);

            btnAgregar.Enabled = !haySel;     // si hay selección → no agrego
            btnModificar.Enabled = haySel;
            btnEliminar.Enabled = haySel;
        }

        private void LimpiarSeleccionGrilla()
        {
            try
            {
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = null;
            }
            catch { /* ignorar si aún no está enlazada */ }
        }

        private static string ToStringSafe(object o)
        {
            return (o == null || o == DBNull.Value) ? "" : o.ToString();
        }

        private static decimal ToDecimal(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            decimal d; return decimal.TryParse(o.ToString(), out d) ? d : 0m;
        }

        private static bool ValidarDialogo(ItemDialog dlg)
        {
            if (string.IsNullOrWhiteSpace(dlg.TxtNombre.Text))
            { MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.TxtNombre.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(dlg.TxtCategoria.Text))
            { MessageBox.Show("La categoría es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.TxtCategoria.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(dlg.TxtUnidad.Text))
            { MessageBox.Show("La unidad es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.TxtUnidad.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(dlg.TxtUbicacion.Text))
            { MessageBox.Show("La ubicación es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.TxtUbicacion.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(dlg.TxtObs.Text))
            { MessageBox.Show("Las observaciones son obligatorias.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.TxtObs.Focus(); return false; }
            // Numéricos (permito 0, pero podés exigir >0 si querés)
            return true;
        }

        private static void FillParams(SqlCommand cmd, ItemDialog dlg)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nombre", dlg.TxtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@Categoria", dlg.TxtCategoria.Text.Trim());
            cmd.Parameters.AddWithValue("@Unidad", dlg.TxtUnidad.Text.Trim());
            cmd.Parameters.AddWithValue("@Stock", (int)dlg.NumStock.Value);
            cmd.Parameters.AddWithValue("@StockMinimo", (int)dlg.NumStockMin.Value);
            cmd.Parameters.AddWithValue("@CostoUnitario", dlg.NumCosto.Value);
            cmd.Parameters.AddWithValue("@Ubicacion", dlg.TxtUbicacion.Text.Trim());
            cmd.Parameters.AddWithValue("@Observaciones", dlg.TxtObs.Text.Trim());
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
                Width = 520; Height = 440;
                BackColor = System.Drawing.Color.White;
                Text = "Ítem de inventario";

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(12),
                    ColumnCount = 2,
                    RowCount = 10,
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
                AddRow("Categoría *", TxtCategoria);
                AddRow("Unidad *", TxtUnidad);
                AddRow("Stock *", NumStock);
                AddRow("Stock mínimo *", NumStockMin);
                AddRow("Costo unitario *", NumCosto);
                AddRow("Ubicación *", TxtUbicacion);
                AddRow("Observaciones *", TxtObs);

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

                FormClosing += (s, e) =>
                {
                    if (DialogResult != DialogResult.OK) return;
                    // La validación completa se hace en ValidarDialogo(..)
                };
            }
        }
    }
}

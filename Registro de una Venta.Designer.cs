using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Registro_de_una_Venta
    {
        private System.ComponentModel.IContainer components = null;

        // Contenedores
        private Panel panelMain;
        private TableLayoutPanel tlpRoot;
        private TableLayoutPanel tlpForm;
        private Panel bottomBar;

        // Controles editor
        private Label lblFecha, lblHora, lblCliente, lblProducto, lblCantidad, lblPrecio, lblSubtotal, lblTotal;
        private DateTimePicker dtpFecha;
        private TextBox txtHora, txtCliente, txtCantidad, txtPrecio, txtSubtotal, txtTotal;
        private ComboBox cboProducto;

        private CheckBox chkProd2;
        private Label lblP2_Producto, lblP2_Cantidad, lblP2_Precio, lblP2_Subtotal;
        private ComboBox cboProducto2;
        private TextBox txtCantidad2, txtPrecio2, txtSubtotal2;

        // Labels de stock
        private Label lblStock1;
        private Label lblStock2;

        // Alta rápida de producto
        private Label lblNuevoProd, lblNuevoPrecio, lblNuevoStock;
        private TextBox txtNuevoProd, txtNuevoPrecio, txtNuevoStock;
        private Button btnAltaProd;

        private Button btnAgregar, btnGuardar, btnCancelar;

        // Grilla de líneas
        private DataGridView dgvLineas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            Color verdeOscuro = Color.FromArgb(5, 80, 45);
            Color verdeMedio = Color.FromArgb(17, 105, 59);

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 860);
            this.Text = "Registro de una Venta";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(this.Registro_de_una_Venta_Load);

            // ===== Panel principal =====
            panelMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.White,
                AutoScroll = true,
                // 👇 Fuerza ancho mínimo → aparece scroll horizontal si la ventana es más chica
                AutoScrollMinSize = new Size(1400, 0)
            };
            this.Controls.Add(panelMain);

            // ===== Root layout =====
            tlpRoot = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true
            };
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // editor
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // grilla
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // bottom bar
            panelMain.Controls.Add(tlpRoot);

            // ===== Form grid (editor) =====
            tlpForm = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                AutoSize = true,
                ColumnCount = 3,
                Padding = new Padding(8, 8, 8, 16)
            };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F)); // etiquetas
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));  // inputs
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F)); // stock label
            tlpRoot.Controls.Add(tlpForm, 0, 0);

            // Helpers
            Label NewLabel(string text) => new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = verdeOscuro,
                Font = new Font("Segoe UI", 11F),
                Anchor = AnchorStyles.Left
            };
            TextBox NewTextBox(int w = 360, bool readOnly = false) => new TextBox
            {
                Width = w,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = readOnly,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            ComboBox NewCombo(int w = 480) => new ComboBox
            {
                Width = w,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            // ===== Fila: Fecha =====
            lblFecha = NewLabel("Fecha:");
            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11F),
                Anchor = AnchorStyles.Left
            };
            tlpForm.Controls.Add(lblFecha, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(dtpFecha, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Fila: Hora =====
            lblHora = NewLabel("Hora:");
            txtHora = NewTextBox(140);
            tlpForm.Controls.Add(lblHora, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtHora, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Fila: Cliente =====
            lblCliente = NewLabel("Cliente:");
            txtCliente = NewTextBox(520);
            tlpForm.Controls.Add(lblCliente, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCliente, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== ALTA RÁPIDA DE PRODUCTO =====
            lblNuevoProd = NewLabel("Nuevo producto:");
            txtNuevoProd = NewTextBox(360);
            tlpForm.Controls.Add(lblNuevoProd, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtNuevoProd, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblNuevoPrecio = NewLabel("Precio unitario:");
            txtNuevoPrecio = NewTextBox(160);
            tlpForm.Controls.Add(lblNuevoPrecio, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtNuevoPrecio, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblNuevoStock = NewLabel("Stock inicial:");
            txtNuevoStock = NewTextBox(160);
            tlpForm.Controls.Add(lblNuevoStock, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtNuevoStock, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            btnAltaProd = new Button
            {
                Text = "Agregar a catálogo",
                BackColor = verdeMedio,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Height = 32,
                Anchor = AnchorStyles.Left
            };
            btnAltaProd.FlatAppearance.BorderSize = 0;
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(btnAltaProd, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Línea 1 producto =====
            lblProducto = NewLabel("Producto:");
            cboProducto = NewCombo(520);
            lblStock1 = new Label
            {
                AutoSize = true,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Left
            };
            tlpForm.Controls.Add(lblProducto, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(cboProducto, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(lblStock1, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblCantidad = NewLabel("Cantidad:");
            txtCantidad = NewTextBox(160);
            tlpForm.Controls.Add(lblCantidad, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCantidad, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblPrecio = NewLabel("Precio Unitario:");
            txtPrecio = NewTextBox(200);
            tlpForm.Controls.Add(lblPrecio, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtPrecio, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblSubtotal = NewLabel("Subtotal:");
            txtSubtotal = NewTextBox(200, readOnly: true);
            tlpForm.Controls.Add(lblSubtotal, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtSubtotal, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Check producto 2 =====
            chkProd2 = new CheckBox
            {
                Text = "Agregar producto 2 (rápido)",
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F),
                Anchor = AnchorStyles.Left
            };
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(chkProd2, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Línea 2 producto (oculta al principio) =====
            lblP2_Producto = NewLabel("Producto 2:");
            cboProducto2 = NewCombo(520);
            lblStock2 = new Label
            {
                AutoSize = true,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Left
            };
            tlpForm.Controls.Add(lblP2_Producto, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(cboProducto2, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(lblStock2, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblP2_Cantidad = NewLabel("Cantidad 2:");
            txtCantidad2 = NewTextBox(160);
            tlpForm.Controls.Add(lblP2_Cantidad, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCantidad2, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblP2_Precio = NewLabel("Precio Unitario 2:");
            txtPrecio2 = NewTextBox(200);
            tlpForm.Controls.Add(lblP2_Precio, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtPrecio2, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            lblP2_Subtotal = NewLabel("Subtotal 2:");
            txtSubtotal2 = NewTextBox(200, readOnly: true);
            tlpForm.Controls.Add(lblP2_Subtotal, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtSubtotal2, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            void SetProd2Visible(bool v)
            {
                lblP2_Producto.Visible = cboProducto2.Visible = lblStock2.Visible = v;
                lblP2_Cantidad.Visible = txtCantidad2.Visible = v;
                lblP2_Precio.Visible = txtPrecio2.Visible = v;
                lblP2_Subtotal.Visible = txtSubtotal2.Visible = v;
            }
            SetProd2Visible(false);
            chkProd2.CheckedChanged += (s, e) => SetProd2Visible(chkProd2.Checked);

            // ===== Botón Agregar producto(s) =====
            btnAgregar = new Button
            {
                Text = "Agregar producto",
                BackColor = verdeMedio,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Height = 38,
                Anchor = AnchorStyles.Left
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(btnAgregar, 1, tlpForm.RowCount);
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 2, tlpForm.RowCount);
            tlpForm.RowCount++;

            // ===== Grilla de líneas =====
            dgvLineas = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 320,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            tlpRoot.Controls.Add(dgvLineas, 0, 1);

            // ===== Bottom bar =====
            bottomBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80
            };
            tlpRoot.Controls.Add(bottomBar, 0, 2);

            lblTotal = new Label
            {
                Text = "Total de la Compra:",
                AutoSize = true,
                ForeColor = verdeOscuro,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Anchor = AnchorStyles.Right
            };

            txtTotal = new TextBox
            {
                ReadOnly = true,
                Font = new Font("Segoe UI", 13F),
                Width = 160,
                Anchor = AnchorStyles.Right
            };

            btnGuardar = new Button
            {
                Text = "Guardar",
                BackColor = verdeMedio,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 150,
                Height = 44,
                Anchor = AnchorStyles.Right
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 150,
                Height = 44,
                Anchor = AnchorStyles.Right
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 10),
                AutoSize = true
            };
            flow.Controls.Add(lblTotal);
            flow.Controls.Add(new Panel() { Width = 10 });
            flow.Controls.Add(txtTotal);
            flow.Controls.Add(new Panel() { Width = 24 });
            flow.Controls.Add(btnGuardar);
            flow.Controls.Add(new Panel() { Width = 12 });
            flow.Controls.Add(btnCancelar);
            bottomBar.Controls.Add(flow);
        }
    }
}

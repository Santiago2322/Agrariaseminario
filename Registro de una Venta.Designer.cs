using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Registro_de_una_Venta
    {
        private System.ComponentModel.IContainer components = null;

        // Contenedores
        private Panel panelMain;              // Dock=Fill + AutoScroll
        private TableLayoutPanel tlpRoot;     // 2 filas: Content (100%) + Bottom (auto)
        private TableLayoutPanel tlpForm;     // grilla de etiquetas/inputs
        private Panel bottomBar;              // barra inferior (Dock=Fill en fila 2)

        // Controles (mismos nombres que usa tu .cs)
        private Label lblFecha, lblHora, lblCliente, lblProducto, lblCantidad, lblPrecio, lblSubtotal, lblTotal;
        private DateTimePicker dtpFecha;
        private TextBox txtHora, txtCliente, txtCantidad, txtPrecio, txtSubtotal, txtTotal;
        private ComboBox cboProducto;

        private CheckBox chkProd2;
        private Label lblP2_Producto, lblP2_Cantidad, lblP2_Precio, lblP2_Subtotal;
        private ComboBox cboProducto2;
        private TextBox txtCantidad2, txtPrecio2, txtSubtotal2;

        private Button btnGuardar, btnCancelar;

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
            this.ClientSize = new Size(1280, 860); // grande
            this.Text = "Registro de una Venta";
            this.Load += new EventHandler(this.Registro_de_una_Venta_Load);

            // ===== Panel principal =====
            panelMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32),
                BackColor = Color.White,
                AutoScroll = true
            };
            this.Controls.Add(panelMain);

            // ===== Root layout (2 filas) =====
            tlpRoot = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // contenido
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // barra inferior
            panelMain.Controls.Add(tlpRoot);

            // ===== Form grid (labels/inputs) =====
            tlpForm = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(8, 8, 8, 16)
            };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F)); // col. etiquetas
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));  // col. inputs
            tlpRoot.Controls.Add(tlpForm, 0, 0);

            // Helper creators
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
            tlpForm.Controls.Add(dtpFecha, 1, tlpForm.RowCount++);

            // ===== Fila: Hora =====
            lblHora = NewLabel("Hora:");
            txtHora = NewTextBox(140);
            tlpForm.Controls.Add(lblHora, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtHora, 1, tlpForm.RowCount++);

            // ===== Fila: Cliente =====
            lblCliente = NewLabel("Cliente:");
            txtCliente = NewTextBox(520);
            tlpForm.Controls.Add(lblCliente, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCliente, 1, tlpForm.RowCount++);

            // ===== Fila: Producto (1) =====
            lblProducto = NewLabel("Producto:");
            cboProducto = NewCombo(520);
            tlpForm.Controls.Add(lblProducto, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(cboProducto, 1, tlpForm.RowCount++);

            // ===== Fila: Cantidad (1) =====
            lblCantidad = NewLabel("Cantidad:");
            txtCantidad = NewTextBox(160);
            tlpForm.Controls.Add(lblCantidad, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCantidad, 1, tlpForm.RowCount++);

            // ===== Fila: Precio (1) =====
            lblPrecio = NewLabel("Precio Unitario:");
            txtPrecio = NewTextBox(200);
            tlpForm.Controls.Add(lblPrecio, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtPrecio, 1, tlpForm.RowCount++);

            // ===== Fila: Subtotal (1) =====
            lblSubtotal = NewLabel("Subtotal:");
            txtSubtotal = NewTextBox(200, readOnly: true);
            tlpForm.Controls.Add(lblSubtotal, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtSubtotal, 1, tlpForm.RowCount++);

            // ===== Fila: Check Producto 2 =====
            chkProd2 = new CheckBox
            {
                Text = "Agregar producto 2",
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F),
                Anchor = AnchorStyles.Left
            };
            tlpForm.Controls.Add(new Label() { AutoSize = true }, 0, tlpForm.RowCount); // celda vacía
            tlpForm.Controls.Add(chkProd2, 1, tlpForm.RowCount++);

            // ===== Sección Producto 2 (oculta inicialmente) =====
            lblP2_Producto = NewLabel("Producto:");
            cboProducto2 = NewCombo(520);
            lblP2_Cantidad = NewLabel("Cantidad:");
            txtCantidad2 = NewTextBox(160);
            lblP2_Precio = NewLabel("Precio Unitario:");
            txtPrecio2 = NewTextBox(200);
            lblP2_Subtotal = NewLabel("Subtotal:");
            txtSubtotal2 = NewTextBox(200, readOnly: true);

            void SetProd2Visible(bool v)
            {
                lblP2_Producto.Visible = cboProducto2.Visible = v;
                lblP2_Cantidad.Visible = txtCantidad2.Visible = v;
                lblP2_Precio.Visible = txtPrecio2.Visible = v;
                lblP2_Subtotal.Visible = txtSubtotal2.Visible = v;
            }

            tlpForm.Controls.Add(lblP2_Producto, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(cboProducto2, 1, tlpForm.RowCount++);
            tlpForm.Controls.Add(lblP2_Cantidad, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtCantidad2, 1, tlpForm.RowCount++);
            tlpForm.Controls.Add(lblP2_Precio, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtPrecio2, 1, tlpForm.RowCount++);
            tlpForm.Controls.Add(lblP2_Subtotal, 0, tlpForm.RowCount);
            tlpForm.Controls.Add(txtSubtotal2, 1, tlpForm.RowCount++);

            SetProd2Visible(false);
            chkProd2.CheckedChanged += (s, e) => SetProd2Visible(chkProd2.Checked);

            // ===== Bottom bar (siempre visible y alineada) =====
            bottomBar = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 80
            };
            tlpRoot.Controls.Add(bottomBar, 0, 1);

            lblTotal = new Label
            {
                Text = "Total de la Compra:",
                AutoSize = true,
                ForeColor = verdeOscuro,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Anchor = AnchorStyles.Right,
                Location = new Point(0, 0)
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
                FlatAppearance = { BorderSize = 0 },
                Width = 150,
                Height = 44,
                Anchor = AnchorStyles.Right
            };
            btnGuardar.Click += new EventHandler(this.btnGuardar_Click);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = 150,
                Height = 44,
                Anchor = AnchorStyles.Right
            };
            btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            // Usamos FlowLayout para alinear derecha sin solaparse con el scroll
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

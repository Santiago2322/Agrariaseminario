using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Actividad
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblEntorno;
        private ComboBox comboEntorno;

        private Label lblFiltro;
        private TextBox txtFiltro;
        private Button btnBuscar;

        private DataGridView grid;

        private GroupBox grpEdicion;
        private Label lblNombre;
        private TextBox txtNombre;

        private Label lblFecha;
        private DateTimePicker dtpFecha;

        private Label lblHora;
        private TextBox txtHora;

        private Label lblResponsable;
        private TextBox txtResponsable;

        private Label lblDescripcion;
        private TextBox txtDescripcion;

        // 🔴 eliminado btnNuevo
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            Color verdeOscuro = Color.FromArgb(5, 80, 45);
            Color verdeMedio = Color.FromArgb(17, 105, 59);

            this.SuspendLayout();

            // ===== FORM =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = new Font("Segoe UI", 10F);
            this.BackColor = Color.White;
            this.Text = "Consulta de Actividad";
            this.Name = "Consulta_de_Actividad";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Scroll
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(1100, 800);
            this.ClientSize = new Size(1100, 720);

            // ===== TÍTULO =====
            this.lblTitulo = new Label();
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Text = "Consulta / ABM de Actividades";
            this.lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitulo.ForeColor = verdeOscuro;
            this.lblTitulo.Location = new Point(16, 16);

            // ===== ENTORNO + FILTRO =====
            this.lblEntorno = new Label();
            this.lblEntorno.AutoSize = true;
            this.lblEntorno.Text = "Entorno:";
            this.lblEntorno.Location = new Point(16, 70);

            this.comboEntorno = new ComboBox();
            this.comboEntorno.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboEntorno.Location = new Point(88, 66);
            this.comboEntorno.Size = new Size(360, 25);

            this.lblFiltro = new Label();
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Text = "Buscar:";
            this.lblFiltro.Location = new Point(470, 70);

            this.txtFiltro = new TextBox();
            this.txtFiltro.Location = new Point(528, 66);
            this.txtFiltro.Size = new Size(320, 25);

            this.btnBuscar = new Button();
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Size = new Size(100, 28);
            this.btnBuscar.Location = new Point(860, 65);
            this.btnBuscar.BackColor = verdeMedio;
            this.btnBuscar.ForeColor = Color.White;
            this.btnBuscar.FlatStyle = FlatStyle.Flat;
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.Click += new EventHandler(this.btnBuscar_Click);

            // ===== GRID =====
            this.grid = aNewGrid(new Point(16, 110), new Size(1040, 320));
            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);

            // ===== GRUPO EDICIÓN =====
            this.grpEdicion = new GroupBox();
            this.grpEdicion.Text = "Edición / Alta rápida";
            this.grpEdicion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpEdicion.ForeColor = verdeOscuro;
            this.grpEdicion.Location = new Point(16, 446);
            this.grpEdicion.Size = new Size(1040, 210);

            // Nombre
            this.lblNombre = new Label();
            this.lblNombre.AutoSize = true;
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.Location = new Point(16, 32);

            this.txtNombre = new TextBox();
            this.txtNombre.Location = new Point(95, 28);
            this.txtNombre.Size = new Size(360, 25);

            // Fecha
            this.lblFecha = new Label();
            this.lblFecha.AutoSize = true;
            this.lblFecha.Text = "Fecha:";
            this.lblFecha.Location = new Point(480, 32);

            this.dtpFecha = new DateTimePicker();
            this.dtpFecha.Location = new Point(536, 28);
            this.dtpFecha.Size = new Size(180, 25);
            this.dtpFecha.Format = DateTimePickerFormat.Short;

            // Hora
            this.lblHora = new Label();
            this.lblHora.AutoSize = true;
            this.lblHora.Text = "Hora:";
            this.lblHora.Location = new Point(740, 32);

            this.txtHora = new TextBox();
            this.txtHora.Location = new Point(782, 28);
            this.txtHora.Size = new Size(100, 25);

            // Responsable
            this.lblResponsable = new Label();
            this.lblResponsable.AutoSize = true;
            this.lblResponsable.Text = "Responsable:";
            this.lblResponsable.Location = new Point(16, 72);

            this.txtResponsable = new TextBox();
            this.txtResponsable.Location = new Point(115, 68);
            this.txtResponsable.Size = new Size(340, 25);

            // Descripción
            this.lblDescripcion = new Label();
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Text = "Descripción:";
            this.lblDescripcion.Location = new Point(16, 112);

            this.txtDescripcion = new TextBox();
            this.txtDescripcion.Location = new Point(115, 108);
            this.txtDescripcion.Size = new Size(767, 56);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.ScrollBars = ScrollBars.Vertical;

            // ===== BOTONES (sin Nuevo) =====
            this.btnGuardar = NewBtn("Guardar", new Point(16, 664));
            this.btnModificar = NewBtn("Modificar", new Point(136, 664));
            this.btnEliminar = NewBtn("Eliminar", new Point(256, 664));
            this.btnCerrar = NewBtn("Cerrar", new Point(946, 664));
            this.btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Eventos de botones (ya definidos en el .cs)
            this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click);
            this.btnModificar.Click += new EventHandler(this.btnModificar_Click);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);
            this.btnCerrar.Click += (s, e) => this.Close();

            // ===== GRUPO: agregar controles =====
            this.grpEdicion.Controls.Add(this.lblNombre);
            this.grpEdicion.Controls.Add(this.txtNombre);
            this.grpEdicion.Controls.Add(this.lblFecha);
            this.grpEdicion.Controls.Add(this.dtpFecha);
            this.grpEdicion.Controls.Add(this.lblHora);
            this.grpEdicion.Controls.Add(this.txtHora);
            this.grpEdicion.Controls.Add(this.lblResponsable);
            this.grpEdicion.Controls.Add(this.txtResponsable);
            this.grpEdicion.Controls.Add(this.lblDescripcion);
            this.grpEdicion.Controls.Add(this.txtDescripcion);

            // ===== FORM: agregar controles =====
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEntorno);
            this.Controls.Add(this.comboEntorno);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.txtFiltro);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.grpEdicion);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnCerrar);

            this.ResumeLayout(false);
            this.PerformLayout();

            // ----- helpers locales del designer -----
            DataGridView aNewGrid(Point p, Size s)
            {
                var g = new DataGridView();
                g.Location = p;
                g.Size = s;
                g.BackgroundColor = Color.White;
                g.AllowUserToAddRows = false;
                g.AllowUserToDeleteRows = false;
                g.MultiSelect = false;
                g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                g.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                g.EnableHeadersVisualStyles = false;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                // ✅ SOLO LECTURA DESDE DISEÑO
                g.ReadOnly = true;

                return g;
            }

            Button NewBtn(string text, Point p)
            {
                var b = new Button();
                b.Text = text;
                b.Size = new Size(110, 32);
                b.Location = p;
                b.BackColor = verdeMedio;
                b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                return b;
            }
        }
    }
}

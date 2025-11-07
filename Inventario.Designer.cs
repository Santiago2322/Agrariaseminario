// Inventario.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Inventario
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button buttonCerrar;

        private ComboBox cboCategorias;
        private Label lblCategorias;
        private Label labelTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Drawing.Font fuenteTitulo = new Font("Segoe UI", 16F, FontStyle.Bold);
            System.Drawing.Font fuenteNormal = new Font("Segoe UI", 11F);
            System.Drawing.Font fuenteBtn = new Font("Segoe UI", 11F, FontStyle.Bold);

            components = new System.ComponentModel.Container();

            this.dataGridView1 = new DataGridView();
            this.btnAgregar = new Button();
            this.btnModificar = new Button();
            this.btnEliminar = new Button();
            this.buttonCerrar = new Button();

            this.cboCategorias = new ComboBox();
            this.lblCategorias = new Label();
            this.labelTitulo = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // ===== FORM =====
            this.ClientSize = new Size(980, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Text = "Inventario";

            // ===== TÍTULO =====
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = fuenteTitulo;
            this.labelTitulo.Location = new Point(24, 18);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Text = "Inventario";

            // ===== FILTRO CATEGORÍAS =====
            this.lblCategorias.AutoSize = true;
            this.lblCategorias.Font = fuenteNormal;
            this.lblCategorias.Location = new Point(28, 68);
            this.lblCategorias.Name = "lblCategorias";
            this.lblCategorias.Text = "Filtrar por categoría:";

            this.cboCategorias.Font = fuenteNormal;
            this.cboCategorias.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboCategorias.Location = new Point(180, 64);
            this.cboCategorias.Name = "cboCategorias";
            this.cboCategorias.Size = new Size(260, 28);
            // *** Sin selección por defecto ***
            this.cboCategorias.SelectedIndex = -1;

            // ===== GRID =====
            this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dataGridView1.Location = new Point(28, 110);
            this.dataGridView1.Size = new Size(920, 370);
            this.dataGridView1.Font = fuenteNormal;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = fuenteBtn };
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            // ===== BOTONES =====
            this.btnAgregar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnAgregar.BackColor = Color.FromArgb(17, 105, 59);
            this.btnAgregar.ForeColor = Color.White;
            this.btnAgregar.Font = fuenteBtn;
            this.btnAgregar.FlatStyle = FlatStyle.Flat;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.Location = new Point(28, 500);
            this.btnAgregar.Size = new Size(150, 40);
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;

            this.btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnModificar.BackColor = Color.FromArgb(17, 105, 59);
            this.btnModificar.ForeColor = Color.White;
            this.btnModificar.Font = fuenteBtn;
            this.btnModificar.FlatStyle = FlatStyle.Flat;
            this.btnModificar.FlatAppearance.BorderSize = 0;
            this.btnModificar.Location = new Point(188, 500);
            this.btnModificar.Size = new Size(150, 40);
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;

            this.btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnEliminar.BackColor = Color.FromArgb(17, 105, 59);
            this.btnEliminar.ForeColor = Color.White;
            this.btnEliminar.Font = fuenteBtn;
            this.btnEliminar.FlatStyle = FlatStyle.Flat;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.Location = new Point(348, 500);
            this.btnEliminar.Size = new Size(150, 40);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;

            this.buttonCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.buttonCerrar.BackColor = Color.FromArgb(5, 80, 45);
            this.buttonCerrar.ForeColor = Color.White;
            this.buttonCerrar.Font = fuenteBtn;
            this.buttonCerrar.FlatStyle = FlatStyle.Flat;
            this.buttonCerrar.FlatAppearance.BorderSize = 0;
            this.buttonCerrar.Location = new Point(798, 500);
            this.buttonCerrar.Size = new Size(150, 40);
            this.buttonCerrar.Text = "Cerrar";
            this.buttonCerrar.UseVisualStyleBackColor = true;

            // ===== ADD CONTROLS =====
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.lblCategorias);
            this.Controls.Add(this.cboCategorias);

            this.Controls.Add(this.dataGridView1);

            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.buttonCerrar);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

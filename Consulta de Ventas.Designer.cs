using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Ventas
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView1;
        private Button button1;
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

            this.dataGridView1 = new DataGridView();
            this.button1 = new Button();
            this.labelTitulo = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // ===== FORM =====
            this.ClientSize = new Size(920, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Text = "Consulta de Ventas";

            // ===== TÍTULO =====
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = fuenteTitulo;
            this.labelTitulo.Location = new Point(25, 20);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Text = "Consulta de Ventas";

            // ===== GRID =====
            this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dataGridView1.Location = new Point(25, 70);
            this.dataGridView1.Size = new Size(860, 420);
            this.dataGridView1.Font = fuenteNormal;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = fuenteBtn };
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            // ===== BOTÓN CERRAR =====
            this.button1.Anchor = AnchorStyles.Bottom;
            this.button1.BackColor = Color.FromArgb(5, 80, 45);
            this.button1.ForeColor = Color.White;
            this.button1.Font = fuenteBtn;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Location = new Point((this.ClientSize.Width - 150) / 2, 500);
            this.button1.Size = new Size(150, 40);
            this.button1.Text = "Cerrar";
            this.button1.UseVisualStyleBackColor = true;

            // ===== ADD CONTROLS =====
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

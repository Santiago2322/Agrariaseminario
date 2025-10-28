using System.Windows.Forms;
using System.Drawing;

namespace Proyecto_Agraria_Pacifico
{
    partial class Inventario
    {
        private System.ComponentModel.IContainer components = null;

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;

        private Button button1;
        private DataGridView dataGridView1;   // 👈 NUEVO CONTROL

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Text = "Inventario";
            this.Load += new System.EventHandler(this.Inventario_Load);

            // ===== Label 1 =====
            this.label1 = new Label();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.label1.ForeColor = Color.Black;
            this.label1.Location = new Point(20, 15);
            this.label1.Text = "Producción Vegetal:";
            this.label1.Click += new System.EventHandler(this.label1_Click);

            // ===== Label 3 =====
            this.label3 = new Label();
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.label3.ForeColor = Color.Black;
            this.label3.Location = new Point(20, 245);
            this.label3.Text = "Producción Animal:";

            // ===== Label 2 =====
            this.label2 = new Label();
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.label2.ForeColor = Color.Black;
            this.label2.Location = new Point(20, 475);
            this.label2.Text = "Industria:";
            this.label2.Click += new System.EventHandler(this.label2_Click);

            // ===== TLP 1 =====
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel1.Location = new Point(25, 55);
            this.tableLayoutPanel1.Size = new Size(840, 170);
            this.tableLayoutPanel1.BackColor = Color.White;
            this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Header Imagen
            this.label4 = new Label();
            this.label4.Text = "Imagen";
            this.label4.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.label4.ForeColor = Color.DimGray;
            this.label4.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);

            // Header Cantidad
            this.label5 = new Label();
            this.label5.Text = "Cantidad";
            this.label5.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.label5.ForeColor = Color.DimGray;
            this.label5.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);

            // ===== TLP 2 =====
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.tableLayoutPanel2.Location = new Point(25, 285);
            this.tableLayoutPanel2.Size = new Size(840, 170);
            this.tableLayoutPanel2.BackColor = Color.White;
            this.tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowCount = 0;

            // ===== TLP 3 =====
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tableLayoutPanel3.Location = new Point(25, 515);
            this.tableLayoutPanel3.Size = new Size(840, 120);
            this.tableLayoutPanel3.BackColor = Color.White;
            this.tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowCount = 0;

            // ===== NUEVO DataGridView =====
            this.dataGridView1 = new DataGridView();
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Location = new Point(25, 650);
            this.dataGridView1.Size = new Size(840, 150);
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.BorderStyle = BorderStyle.Fixed3D;

            // ===== Botón Cerrar =====
            this.button1 = new Button();
            this.button1.Text = "Cerrar";
            this.button1.Size = new Size(120, 36);
            this.button1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.button1.Location = new Point(this.ClientSize.Width - 140, this.ClientSize.Height - 50);
            this.button1.BackColor = Color.Green;
            this.button1.ForeColor = Color.White;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // ===== Add Controls =====
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.dataGridView1); // 👈 agregado
            this.Controls.Add(this.button1);
        }
    }
}

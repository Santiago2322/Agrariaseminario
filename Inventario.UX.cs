// Inventario.Designer.cs
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Inventario
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        private Label labelTitulo;
        private DataGridView dataGridView1;
        private Button button1;      // Cerrar
        private Label label1;        // opcional (sin eventos)
        private Label label2;        // opcional (sin eventos)

        /// <summary>Clean up any resources being used.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.labelTitulo = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label(); // solo para compatibilidad
            this.label2 = new System.Windows.Forms.Label(); // solo para compatibilidad

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // 
            // Inventario (Form)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 600);
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Text = "Inventario";

            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(24, 18);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(103, 25);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Inventario";

            // 
            // dataGridView1
            // 
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Location = new System.Drawing.Point(28, 60);
            this.dataGridView1.Size = new System.Drawing.Size(940, 500);
            this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // 
            // button1 (Cerrar)
            // 
            this.button1.Name = "button1";
            this.button1.Text = "Cerrar";
            this.button1.Size = new System.Drawing.Size(110, 32);
            this.button1.Location = new System.Drawing.Point(858, 575);
            this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            // NOTA: NO se engancha aquí button1_Click.
            // Tu Inventario.cs lo engancha de forma segura si existe.

            // 
            // label1 (dummy opcional, SIN eventos)
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 575);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.Text = ""; // sin uso, solo para que no falte si el diseñador lo esperaba

            // 
            // label2 (dummy opcional, SIN eventos)
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 595);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.Text = ""; // sin uso, solo compatibilidad

            // 
            // Add Controls
            // 
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}

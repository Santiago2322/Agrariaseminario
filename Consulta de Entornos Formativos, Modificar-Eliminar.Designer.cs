// Consulta_de_Entornos_Formativos__Modificar_Eliminar.Designer.cs  (DESIGNER)
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Button button1; // Modificar
        private Button button2; // Eliminar
        private Button button3; // Cerrar
        private Label label1, label2, label3, label4, label5, label6;
        private TextBox textBox1, textBox2, textBox3, textBox4, textBox5, textBox6;

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

            Font fBase = new Font("Segoe UI", 11F);
            Font fLbl = new Font("Segoe UI", 16F, FontStyle.Bold);
            Font fBtn = new Font("Segoe UI", 12F, FontStyle.Bold);
            Font fHdr = new Font("Segoe UI", 12F, FontStyle.Bold);

            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = fBase;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Consulta / Modificar / Eliminar Entornos Formativos";
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 760);

            // GRID
            dataGridView1 = new DataGridView();
            dataGridView1.Location = new Point(16, 16);
            dataGridView1.Size = new Size(1068, 340);
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = fHdr };
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // BOTONES
            button1 = new Button(); // Modificar
            button1.Text = "Modificar";
            button1.Font = fBtn;
            button1.BackColor = verdeMedio;
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Size = new Size(170, 44);
            button1.Location = new Point(280, 368);
            button1.Enabled = false;

            button2 = new Button(); // Eliminar
            button2.Text = "Eliminar";
            button2.Font = fBtn;
            button2.BackColor = verdeMedio;
            button2.ForeColor = Color.White;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.Size = new Size(170, 44);
            button2.Location = new Point(470, 368);
            button2.Enabled = false;

            // CAMPOS
            int leftLblX = 24, leftTxtX = 320, rightLblX = 560, rightTxtX = 820;
            int y1 = 438, y2 = 490, y3 = 542;

            label1 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Nombre del Entorno:", Location = new Point(leftLblX, y1) };
            label2 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Tipo de Entorno:", Location = new Point(leftLblX, y2) };
            label3 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Profesor Responsable:", Location = new Point(leftLblX, y3) };

            label4 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Año:", Location = new Point(rightLblX, y1) };
            label5 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "División:", Location = new Point(rightLblX, y2) };
            label6 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Grupo:", Location = new Point(rightLblX, y3) };

            textBox1 = new TextBox { Location = new Point(leftTxtX, y1 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };
            textBox2 = new TextBox { Location = new Point(leftTxtX, y2 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };
            textBox3 = new TextBox { Location = new Point(leftTxtX, y3 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };
            textBox4 = new TextBox { Location = new Point(rightTxtX, y1 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };
            textBox6 = new TextBox { Location = new Point(rightTxtX, y2 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };
            textBox5 = new TextBox { Location = new Point(rightTxtX, y3 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase };

            // CERRAR
            button3 = new Button();
            button3.Text = "Cerrar";
            button3.Font = fBtn;
            button3.BackColor = verdeMedio;
            button3.ForeColor = Color.White;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.Size = new Size(170, 44);
            button3.Location = new Point(914, 660);

            // Add controls
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.AddRange(new Control[]
            {
                label1, textBox1, label2, textBox2, label3, textBox3,
                label4, textBox4, label5, textBox6, label6, textBox5
            });
        }
    }
}

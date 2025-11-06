using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar
    {
        private System.ComponentModel.IContainer components = null;

        // === NOMBRES QUE ESPERA EL CODE-BEHIND ===
        private DataGridView dataGridView1;
        private Button button1; // Modificar
        private Button button2; // Eliminar
        private Button button3; // Guardar
        private Label label1;   // Nombre
        private Label label2;   // Tipo
        private Label label3;   // Profesor
        private Label label4;   // Año
        private Label label5;   // División
        private Label label6;   // Grupo
        private TextBox textBox1; // Nombre
        private TextBox textBox2; // Tipo
        private TextBox textBox3; // Profesor
        private TextBox textBox4; // Año
        private TextBox textBox6; // División
        private TextBox textBox5; // Grupo

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Paleta y fuentes
            Color verdeOscuro = Color.FromArgb(5, 80, 45);
            Color verdeMedio = Color.FromArgb(17, 105, 59);

            Font fBase = new Font("Segoe UI", 11F);
            Font fLbl = new Font("Segoe UI", 16F, FontStyle.Bold);
            Font fBtn = new Font("Segoe UI", 12F, FontStyle.Bold);
            Font fHdr = new Font("Segoe UI", 12F, FontStyle.Bold);

            // ==== FORM ====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = fBase;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Consulta / Modificar / Eliminar Entornos Formativos";
            this.Load += new EventHandler(this.Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load);

            // ==== GRID ====
            this.dataGridView1 = new DataGridView();
            this.dataGridView1.Location = new Point(16, 16);
            this.dataGridView1.Size = new Size(1068, 340);
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = fHdr
            };
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);

            // ==== BOTONES (debajo del grid) ====
            this.button1 = new Button(); // Modificar
            this.button1.Text = "Modificar";
            this.button1.Font = fBtn;
            this.button1.BackColor = verdeMedio;
            this.button1.ForeColor = Color.White;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Size = new Size(170, 44);
            this.button1.Location = new Point(280, 368);
            this.button1.Click += new EventHandler(this.button1_Click);

            this.button2 = new Button(); // Eliminar
            this.button2.Text = "Eliminar";
            this.button2.Font = fBtn;
            this.button2.BackColor = verdeMedio;
            this.button2.ForeColor = Color.White;
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.Size = new Size(170, 44);
            this.button2.Location = new Point(470, 368);
            this.button2.Click += new EventHandler(this.button2_Click);

            // ==== LAYOUT EDICIÓN (dos columnas) ====
            // Izquierda: Nombre, Tipo, Profesor
            // Derecha:  Año, División, Grupo
            int leftLblX = 24;
            int leftTxtX = 320;
            int rightLblX = 560;
            int rightTxtX = 820;
            int y1 = 438; // fila 1
            int y2 = 490; // fila 2
            int y3 = 542; // fila 3

            // Labels
            this.label1 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Nombre del Entorno:", Location = new Point(leftLblX, y1) };
            this.label2 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Tipo de Entorno:", Location = new Point(leftLblX, y2) };
            this.label3 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Profesor Responsable:", Location = new Point(leftLblX, y3) };
            this.label3.Click += new EventHandler(this.label3_Click);

            this.label4 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Año:", Location = new Point(rightLblX, y1) };
            this.label5 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "División:", Location = new Point(rightLblX, y2) };
            this.label6 = new Label { AutoSize = true, Font = fLbl, ForeColor = verdeOscuro, Text = "Grupo:", Location = new Point(rightLblX, y3) };

            // TextBoxes (los nombres que usa tu lógica)
            this.textBox1 = new TextBox { Location = new Point(leftTxtX, y1 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // Nombre
            this.textBox2 = new TextBox { Location = new Point(leftTxtX, y2 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // Tipo
            this.textBox3 = new TextBox { Location = new Point(leftTxtX, y3 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // Profesor

            this.textBox4 = new TextBox { Location = new Point(rightTxtX, y1 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // Año
            this.textBox4.TextChanged += new EventHandler(this.textBox4_TextChanged);
            this.textBox6 = new TextBox { Location = new Point(rightTxtX, y2 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // División
            this.textBox5 = new TextBox { Location = new Point(rightTxtX, y3 + 2), Size = new Size(220, 30), BorderStyle = BorderStyle.FixedSingle, Font = fBase }; // Grupo

            // Botón Guardar
            this.button3 = new Button();
            this.button3.Text = "Guardar";
            this.button3.Font = fBtn;
            this.button3.BackColor = verdeMedio;
            this.button3.ForeColor = Color.White;
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.Size = new Size(170, 44);
            this.button3.Location = new Point(914, 620);
            this.button3.Click += new EventHandler(this.button3_Click);

            // ==== ADD CONTROLS ====
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);

            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);

            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox5);

            this.Controls.Add(this.button3);
        }
    }
}

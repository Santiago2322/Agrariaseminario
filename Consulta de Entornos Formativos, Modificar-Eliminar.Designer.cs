using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Button button1, button2, button3;
        private Label label1, label2, label3, label4, label5, label6;
        private TextBox textBox1, textBox2, textBox3, textBox4, textBox6, textBox5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Paleta
            Color verdeOscuro = Color.FromArgb(5, 80, 45);
            Color verdeMedio = Color.FromArgb(17, 105, 59);
            Color fondo = Color.White;
            Color texto = Color.Black;

            this.dataGridView1 = new DataGridView();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.label1 = new Label(); this.label2 = new Label(); this.label3 = new Label();
            this.label4 = new Label(); this.label5 = new Label(); this.label6 = new Label();
            this.textBox1 = new TextBox(); this.textBox2 = new TextBox(); this.textBox3 = new TextBox();
            this.textBox4 = new TextBox(); this.textBox6 = new TextBox(); this.textBox5 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // helpers
            void StyleLabel(Label l) { l.BackColor = Color.Transparent; l.ForeColor = verdeOscuro; }
            void StyleTxt(TextBox t) { t.BackColor = fondo; t.ForeColor = texto; t.BorderStyle = BorderStyle.FixedSingle; }
            void StyleBtn(Button b) { b.BackColor = verdeMedio; b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0; }

            // dataGridView1
            this.dataGridView1.BackgroundColor = fondo;
            this.dataGridView1.GridColor = verdeOscuro;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = true;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(680, 214);
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);

            // button1 (Modificar)
            this.button1.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.button1.Location = new Point(234, 249);
            this.button1.Size = new Size(133, 52);
            this.button1.Text = "Modificar";
            this.button1.Click += new EventHandler(this.button1_Click);
            StyleBtn(this.button1);

            // button2 (Eliminar)
            this.button2.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.button2.Location = new Point(409, 249);
            this.button2.Size = new Size(133, 52);
            this.button2.Text = "Eliminar";
            this.button2.Click += new EventHandler(this.button2_Click);
            StyleBtn(this.button2);

            // button3 (Guardar)
            this.button3.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            this.button3.Location = new Point(559, 458);
            this.button3.Size = new Size(133, 52);
            this.button3.Text = "Guardar";
            this.button3.Click += new EventHandler(this.button3_Click);
            StyleBtn(this.button3);

            // Labels
            this.label1.AutoSize = true; this.label1.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new Point(22, 326); this.label1.Text = "Nombre del Entorno:"; StyleLabel(this.label1);

            this.label2.AutoSize = true; this.label2.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new Point(57, 369); this.label2.Text = "Tipo de Entorno:"; StyleLabel(this.label2);

            this.label3.AutoSize = true; this.label3.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label3.Location = new Point(8, 411); this.label3.Text = "Profesor Responsable:"; this.label3.Click += new EventHandler(this.label3_Click); StyleLabel(this.label3);

            this.label4.AutoSize = true; this.label4.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label4.Location = new Point(431, 325); this.label4.Text = "Año:"; StyleLabel(this.label4);

            this.label5.AutoSize = true; this.label5.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label5.Location = new Point(431, 368); this.label5.Text = "División:"; StyleLabel(this.label5);

            this.label6.AutoSize = true; this.label6.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label6.Location = new Point(431, 410); this.label6.Text = "Grupo:"; StyleLabel(this.label6);

            // TextBoxes
            this.textBox1.Location = new Point(234, 329); this.textBox1.Size = new Size(176, 20); StyleTxt(this.textBox1);
            this.textBox2.Location = new Point(234, 373); this.textBox2.Size = new Size(176, 20); StyleTxt(this.textBox2);
            this.textBox3.Location = new Point(234, 415); this.textBox3.Size = new Size(176, 20); StyleTxt(this.textBox3);
            this.textBox4.Location = new Point(516, 325); this.textBox4.Size = new Size(176, 20); this.textBox4.TextChanged += new EventHandler(this.textBox4_TextChanged); StyleTxt(this.textBox4);
            this.textBox6.Location = new Point(516, 374); this.textBox6.Size = new Size(176, 20); StyleTxt(this.textBox6);
            this.textBox5.Location = new Point(516, 416); this.textBox5.Size = new Size(176, 20); StyleTxt(this.textBox5);

            // Form
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = fondo;
            this.ClientSize = new Size(800, 567);
            this.Controls.AddRange(new Control[]
            {
                button3, textBox6, textBox5, textBox4, textBox3, textBox2, textBox1,
                label6, label5, label4, label3, label2, label1, button2, button1, dataGridView1
            });
            this.Name = "Consulta_de_Entornos_Formativos__Modificar_Eliminar";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Consulta / Modificar / Eliminar Entornos Formativos";
            this.Load += new EventHandler(this.Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}


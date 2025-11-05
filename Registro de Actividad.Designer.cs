using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Registro_de_Actividad
    {
        private System.ComponentModel.IContainer components = null;

        private Label label1, label2, label3, label4, label5, label6, label7, label8;
        private ComboBox comboBox1, comboBox2;
        private TextBox textBox1, textBox2, textBox3, textBox5, textBox6;
        private DataGridView dataGridView1;
        private Button button1, button4, button2;

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

            this.label1 = new Label(); this.label2 = new Label(); this.label3 = new Label();
            this.label4 = new Label(); this.label5 = new Label(); this.label6 = new Label();
            this.comboBox1 = new ComboBox(); this.textBox1 = new TextBox(); this.textBox2 = new TextBox();
            this.textBox3 = new TextBox(); this.textBox5 = new TextBox(); this.textBox6 = new TextBox();
            this.dataGridView1 = new DataGridView(); this.label7 = new Label(); this.button1 = new Button();
            this.label8 = new Label(); this.comboBox2 = new ComboBox(); this.button4 = new Button(); this.button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // helpers
            void StyleLabel(Label l) { l.BackColor = Color.Transparent; l.ForeColor = verdeOscuro; }
            void StyleTxt(TextBox t) { t.BackColor = fondo; t.ForeColor = texto; t.BorderStyle = BorderStyle.FixedSingle; }
            void StyleCombo(ComboBox c) { c.BackColor = fondo; c.ForeColor = texto; c.DropDownStyle = ComboBoxStyle.DropDownList; }
            void StyleBtn(Button b) { b.BackColor = verdeMedio; b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0; }

            // Labels
            this.label1.AutoSize = true; this.label1.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new Point(12, 9); this.label1.Text = "Seleccione Entorno Formativo:"; StyleLabel(this.label1);

            this.label2.AutoSize = true; this.label2.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new Point(12, 45); this.label2.Text = "Tipo de Entorno:"; StyleLabel(this.label2);

            this.label3.AutoSize = true; this.label3.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label3.Location = new Point(12, 81); this.label3.Text = "Profesor Responsable:"; StyleLabel(this.label3);

            this.label4.AutoSize = true; this.label4.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label4.Location = new Point(12, 117); this.label4.Text = "Año:"; StyleLabel(this.label4);

            this.label5.AutoSize = true; this.label5.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label5.Location = new Point(12, 153); this.label5.Text = "División:"; StyleLabel(this.label5);

            this.label6.AutoSize = true; this.label6.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label6.Location = new Point(12, 189); this.label6.Text = "Grupo:"; StyleLabel(this.label6);

            // Entradas
            this.comboBox1.Location = new Point(288, 12); this.comboBox1.Size = new Size(265, 21); StyleCombo(this.comboBox1);
            this.textBox1.Location = new Point(288, 48); this.textBox1.Size = new Size(265, 20); StyleTxt(this.textBox1);
            this.textBox2.Location = new Point(288, 84); this.textBox2.Size = new Size(265, 20); StyleTxt(this.textBox2);
            this.textBox3.Location = new Point(288, 120); this.textBox3.Size = new Size(120, 20); StyleTxt(this.textBox3);
            this.textBox5.Location = new Point(288, 156); this.textBox5.Size = new Size(120, 20); StyleTxt(this.textBox5);
            this.textBox6.Location = new Point(288, 192); this.textBox6.Size = new Size(120, 20); StyleTxt(this.textBox6);

            // Grid
            this.dataGridView1.Location = new Point(16, 258); this.dataGridView1.Size = new Size(537, 105);
            this.dataGridView1.BackgroundColor = fondo; this.dataGridView1.GridColor = verdeOscuro;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = texto;

            // Título de grilla
            this.label7.AutoSize = true; this.label7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.label7.Location = new Point(24, 303);
            this.label7.Text = "Grilla consulta de registro de actividad de un entorno formativo"; StyleLabel(this.label7);

            // Botón actualización
            this.button1.Font = new Font("Microsoft Sans Serif", 12F);
            this.button1.Location = new Point(161, 369); this.button1.Size = new Size(175, 47);
            this.button1.Text = "Actualización"; StyleBtn(this.button1);
            // (si querés evento: this.button1.Click += new EventHandler(this.button1_Click);)

            // Fecha/Hora
            this.label8.AutoSize = true; this.label8.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.label8.Location = new Point(12, 464); this.label8.Text = "Fecha/Hora:"; StyleLabel(this.label8);

            this.comboBox2.Location = new Point(126, 463); this.comboBox2.Size = new Size(131, 21); StyleCombo(this.comboBox2);

            // Guardar / Cancelar
            this.button4.Font = new Font("Microsoft Sans Serif", 12F);
            this.button4.Location = new Point(649, 503); this.button4.Size = new Size(139, 42);
            this.button4.Text = "Guardar"; this.button4.Click += new EventHandler(this.button4_Click); StyleBtn(this.button4);

            this.button2.Font = new Font("Microsoft Sans Serif", 12F);
            this.button2.Location = new Point(478, 503); this.button2.Size = new Size(139, 42);
            this.button2.Text = "Cancelar"; this.button2.Click += new EventHandler(this.button2_Click); StyleBtn(this.button2);

            // Form
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = fondo;
            this.ClientSize = new Size(800, 577);
            this.Controls.AddRange(new Control[]
            {
                button2, button4, comboBox2, label8, button1, label7, dataGridView1,
                textBox6, textBox5, textBox3, textBox2, textBox1, comboBox1,
                label6, label5, label4, label3, label2, label1
            });
            this.Name = "Registro_de_Actividad";
            this.Text = "Registro_de_Actividad";
            this.Load += new EventHandler(this.Registro_de_Actividad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}



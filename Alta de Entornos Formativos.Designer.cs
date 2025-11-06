using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Alta_de_Entornos_Formativos
    {
        private System.ComponentModel.IContainer components = null;

        private Label label1, label2, label3, label4, label5, label6, label7;
        private TextBox textBox1, textBox2, textBox4, textBox3, textBox5, textBox6;
        private ComboBox comboBox1;
        private Button button1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();

            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            textBox2 = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();

            button1 = new Button();

            SuspendLayout();

            // labels
            label1.AutoSize = true; label1.Font = new Font("Microsoft Sans Serif", 12F);
            label1.Location = new Point(30, 40); label1.Text = "Nombre del Entorno:";

            label2.AutoSize = true; label2.Font = new Font("Microsoft Sans Serif", 12F);
            label2.Location = new Point(30, 80); label2.Text = "Tipo de entorno:";

            label3.AutoSize = true; label3.Font = new Font("Microsoft Sans Serif", 12F);
            label3.Location = new Point(30, 120); label3.Text = "Profesor responsable:";

            label4.AutoSize = true; label4.Font = new Font("Microsoft Sans Serif", 12F);
            label4.Location = new Point(30, 160); label4.Text = "Año:";

            label5.AutoSize = true; label5.Font = new Font("Microsoft Sans Serif", 12F);
            label5.Location = new Point(210, 160); label5.Text = "División:";
            label5.Click += new EventHandler(this.label5_Click);

            label6.AutoSize = true; label6.Font = new Font("Microsoft Sans Serif", 12F);
            label6.Location = new Point(400, 160); label6.Text = "Grupo:";

            label7.AutoSize = true; label7.Font = new Font("Microsoft Sans Serif", 12F);
            label7.Location = new Point(30, 205); label7.Text = "Observaciones:";

            // inputs
            textBox1.Location = new Point(210, 40); textBox1.Size = new Size(360, 20);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; comboBox1.Location = new Point(210, 80); comboBox1.Size = new Size(360, 21);
            textBox2.Location = new Point(210, 120); textBox2.Size = new Size(360, 20);
            textBox4.Location = new Point(80, 160); textBox4.Size = new Size(110, 20);
            textBox3.Location = new Point(285, 160); textBox3.Size = new Size(100, 20);
            textBox5.Location = new Point(465, 160); textBox5.Size = new Size(105, 20);
            textBox6.Location = new Point(160, 205); textBox6.Size = new Size(410, 20);

            // botón
            button1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            button1.Location = new Point(449, 260); button1.Size = new Size(121, 40);
            button1.Text = "Guardar"; button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(this.button1_Click);

            // Form
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.ClientSize = new Size(620, 330);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Alta de Entornos Formativos";
            this.Load += new EventHandler(this.Alta_de_Entornos_Formativos_Load);

            this.Controls.Add(button1);
            this.Controls.Add(textBox6);
            this.Controls.Add(textBox5);
            this.Controls.Add(textBox3);
            this.Controls.Add(textBox4);
            this.Controls.Add(textBox2);
            this.Controls.Add(comboBox1);
            this.Controls.Add(textBox1);
            this.Controls.Add(label7);
            this.Controls.Add(label6);
            this.Controls.Add(label5);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}

using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Alta_de_Entornos_Formativos
    {
        private System.ComponentModel.IContainer components = null;

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;

        private TextBox textBox1; // Nombre
        private ComboBox comboBox1; // Tipo
        private TextBox textBox2; // Profesor
        private TextBox textBox4; // Año
        private TextBox textBox3; // División
        private TextBox textBox5; // Grupo
        private TextBox textBox6; // Observaciones

        private Button button1;   // Guardar

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alta_de_Entornos_Formativos));
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();

            this.textBox1 = new TextBox();
            this.comboBox1 = new ComboBox();
            this.textBox2 = new TextBox();
            this.textBox4 = new TextBox();
            this.textBox3 = new TextBox();
            this.textBox5 = new TextBox();
            this.textBox6 = new TextBox();

            this.button1 = new Button();
            this.SuspendLayout();

            // label1 - Nombre del Entorno
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(30, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 20);
            this.label1.Text = "Nombre del Entorno:";

            // label2 - Tipo de entorno
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(30, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 20);
            this.label2.Text = "Tipo de entorno:";

            // label3 - Profesor responsable
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(30, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 20);
            this.label3.Text = "Profesor responsable:";

            // label4 - Año
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(30, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 20);
            this.label4.Text = "Año:";

            // label5 - División
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(210, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.Text = "División:";
            this.label5.Click += new System.EventHandler(this.label5_Click);

            // label6 - Grupo
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(400, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 20);
            this.label6.Text = "Grupo:";

            // label7 - Observaciones
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(30, 205);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 20);
            this.label7.Text = "Observaciones:";

            // textBox1 - Nombre
            this.textBox1.Location = new System.Drawing.Point(210, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(360, 20);

            // comboBox1 - Tipo
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(210, 80);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(360, 21);

            // textBox2 - Profesor
            this.textBox2.Location = new System.Drawing.Point(210, 120);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(360, 20);

            // textBox4 - Año
            this.textBox4.Location = new System.Drawing.Point(80, 160);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(110, 20);

            // textBox3 - División
            this.textBox3.Location = new System.Drawing.Point(285, 160);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);

            // textBox5 - Grupo
            this.textBox5.Location = new System.Drawing.Point(465, 160);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(105, 20);

            // textBox6 - Observaciones
            this.textBox6.Location = new System.Drawing.Point(160, 205);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(410, 20);

            // button1 - Guardar
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(449, 260);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 40);
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;  // <- Colores por defecto
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // Alta_de_Entornos_Formativos (Form)
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            // Colores por defecto del formulario (coincide con otros)
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(620, 330);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Alta_de_Entornos_Formativos";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Alta de Entornos Formativos";
            this.Load += new System.EventHandler(this.Alta_de_Entornos_Formativos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}


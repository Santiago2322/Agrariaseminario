using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Consulta_de_Entornos_Formativos__Modificar_Eliminar));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Green;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(680, 214);
            this.dataGridView1.TabIndex = 0;
            // eventos
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // button1 (Modificar)
            // 
            this.button1.BackColor = System.Drawing.Color.Green;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(234, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 52);
            this.button1.TabIndex = 1;
            this.button1.Text = "Modificar";
            this.button1.UseVisualStyleBackColor = false;
            // eventos
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2 (Eliminar)
            // 
            this.button2.BackColor = System.Drawing.Color.Green;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(409, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 52);
            this.button2.TabIndex = 2;
            this.button2.Text = "Eliminar";
            this.button2.UseVisualStyleBackColor = false;
            // eventos
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3 (Guardar / Insertar)
            // 
            this.button3.BackColor = System.Drawing.Color.Green;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button3.Location = new System.Drawing.Point(559, 458);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 52);
            this.button3.TabIndex = 15;
            this.button3.Text = "Guardar";
            this.button3.UseVisualStyleBackColor = false;
            // eventos
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1 - Nombre
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(22, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre del Entorno:";
            // 
            // label2 - Tipo
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new System.Drawing.Point(57, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tipo de Entorno:";
            // 
            // label3 - Profesor
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label3.Location = new System.Drawing.Point(8, 411);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Profesor Responsable:";
            // eventos (lo tenías en tu cs)
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4 - Año
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label4.Location = new System.Drawing.Point(431, 325);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Año:";
            // 
            // label5 - División
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label5.Location = new System.Drawing.Point(431, 368);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "División:";
            // 
            // label6 - Grupo
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label6.Location = new System.Drawing.Point(431, 410);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 24);
            this.label6.TabIndex = 8;
            this.label6.Text = "Grupo:";
            // 
            // textBox1 - Nombre
            // 
            this.textBox1.BackColor = System.Drawing.Color.Green;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(234, 329);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 20);
            this.textBox1.TabIndex = 9;
            // 
            // textBox2 - Tipo
            // 
            this.textBox2.BackColor = System.Drawing.Color.Green;
            this.textBox2.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox2.Location = new System.Drawing.Point(234, 373);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(176, 20);
            this.textBox2.TabIndex = 10;
            // 
            // textBox3 - Profesor
            // 
            this.textBox3.BackColor = System.Drawing.Color.Green;
            this.textBox3.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox3.Location = new System.Drawing.Point(234, 415);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(176, 20);
            this.textBox3.TabIndex = 11;
            // 
            // textBox4 - Año
            // 
            this.textBox4.BackColor = System.Drawing.Color.Green;
            this.textBox4.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox4.Location = new System.Drawing.Point(516, 325);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(176, 20);
            this.textBox4.TabIndex = 12;
            // eventos (lo tenías en tu cs)
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox6 - División
            // 
            this.textBox6.BackColor = System.Drawing.Color.Green;
            this.textBox6.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox6.Location = new System.Drawing.Point(516, 374);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(176, 20);
            this.textBox6.TabIndex = 14;
            // 
            // textBox5 - Grupo
            // 
            this.textBox5.BackColor = System.Drawing.Color.Green;
            this.textBox5.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox5.Location = new System.Drawing.Point(516, 416);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(176, 20);
            this.textBox5.TabIndex = 13;
            // 
            // Consulta_de_Entornos_Formativos__Modificar_Eliminar (Form)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 567);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Consulta_de_Entornos_Formativos__Modificar_Eliminar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta / Modificar / Eliminar Entornos Formativos";
            // evento Load (para que ejecute EnsureSchema + CargarDatos del .cs)
            this.Load += new System.EventHandler(this.Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
    }
}

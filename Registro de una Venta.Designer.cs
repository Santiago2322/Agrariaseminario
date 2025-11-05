using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Registro_de_una_Venta
    {
        private System.ComponentModel.IContainer components = null;

        private Label label1, label2, label3, label4, label5, label6, label7, label8;
        private ComboBox comboBox1, comboBox2, comboBox3;
        private TextBox textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9;
        private Label label9, label10, label11, label12, label13;
        private CheckBox checkBox1;
        private Button btnGuardar, btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();

            this.comboBox1 = new ComboBox(); // Fecha
            this.textBox1 = new TextBox();  // Hora
            this.textBox2 = new TextBox();  // Cliente
            this.comboBox2 = new ComboBox(); // Producto 1
            this.textBox3 = new TextBox();  // Cantidad 1
            this.textBox4 = new TextBox();  // Precio 1
            this.textBox5 = new TextBox();  // Subtotal 1

            this.label9 = new Label();     // Total label
            this.checkBox1 = new CheckBox();  // Habilitar producto 2
            this.textBox6 = new TextBox();   // Total

            this.btnGuardar = new Button(); // Guardar
            this.btnCancelar = new Button(); // Cancelar

            // Segunda línea (opcional / producto 2)
            this.textBox7 = new TextBox();  // Subtotal 2
            this.textBox8 = new TextBox();  // Precio 2
            this.textBox9 = new TextBox();  // Cantidad 2
            this.comboBox3 = new ComboBox(); // Producto 2
            this.label10 = new Label();    // "Subtotal:"
            this.label11 = new Label();    // "Precio Unitario:"
            this.label12 = new Label();    // "Cantidad:"
            this.label13 = new Label();    // "Producto:"

            // ===== Paleta =====
            Color verdeOscuro = Color.FromArgb(5, 80, 45);
            Color verdeMedio = Color.FromArgb(17, 105, 59);
            Color fondo = Color.White;
            Color texto = Color.Black;

            this.SuspendLayout();

            // ====== Form ======
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = fondo;
            this.ClientSize = new Size(800, 543);
            this.Name = "Registro_de_una_Venta";
            this.Text = "Registro de una Venta";
            this.Load += new EventHandler(this.Registro_de_una_Venta_Load);

            // ===== Helpers (solo estilos) =====
            void StyleLabel(Label l) { l.BackColor = Color.Transparent; l.ForeColor = verdeOscuro; }
            void StyleTxtBox(TextBox t) { t.BackColor = fondo; t.ForeColor = texto; t.BorderStyle = BorderStyle.FixedSingle; }
            void StyleCombo(ComboBox c) { c.BackColor = fondo; c.ForeColor = texto; c.DropDownStyle = ComboBoxStyle.DropDownList; }
            void StyleButton(Button b) { b.BackColor = verdeMedio; b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0; }
            void StyleCheck(CheckBox chk) { chk.BackColor = Color.Transparent; chk.ForeColor = texto; }

            // ====== Labels principales ======
            // Fecha
            this.label1.AutoSize = true; this.label1.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new Point(12, 9); this.label1.Text = "Fecha:"; StyleLabel(this.label1);

            // Hora
            this.label2.AutoSize = true; this.label2.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new Point(12, 49); this.label2.Text = "Hora:"; StyleLabel(this.label2);

            // Cliente
            this.label3.AutoSize = true; this.label3.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label3.Location = new Point(12, 92); this.label3.Text = "Cliente:"; StyleLabel(this.label3);

            // Producto
            this.label4.AutoSize = true; this.label4.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label4.Location = new Point(12, 131); this.label4.Text = "Producto:"; StyleLabel(this.label4);

            // Cantidad
            this.label5.AutoSize = true; this.label5.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label5.Location = new Point(12, 174); this.label5.Text = "Cantidad:"; StyleLabel(this.label5);

            // Precio Unitario
            this.label6.AutoSize = true; this.label6.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label6.Location = new Point(12, 215); this.label6.Text = "Precio Unitario:"; StyleLabel(this.label6);

            // Subtotal
            this.label7.AutoSize = true; this.label7.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label7.Location = new Point(12, 258); this.label7.Text = "Subtotal:"; StyleLabel(this.label7);

            // C.+
            this.label8.AutoSize = true; this.label8.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label8.Location = new Point(414, 91); this.label8.Text = "C.+:"; StyleLabel(this.label8);

            // ====== Línea 1 ======
            this.comboBox1.Location = new Point(87, 12); this.comboBox1.Size = new Size(105, 21); StyleCombo(this.comboBox1);
            this.textBox1.Location = new Point(87, 54); this.textBox1.Size = new Size(80, 20); StyleTxtBox(this.textBox1);
            this.textBox2.Location = new Point(87, 96); this.textBox2.Size = new Size(321, 20); this.textBox2.TextChanged += new EventHandler(this.textBox2_TextChanged); StyleTxtBox(this.textBox2);
            this.comboBox2.Location = new Point(100, 134); this.comboBox2.Size = new Size(308, 21); StyleCombo(this.comboBox2);
            this.textBox3.Location = new Point(100, 178); this.textBox3.Multiline = true; this.textBox3.Size = new Size(80, 20); StyleTxtBox(this.textBox3);
            this.textBox4.Location = new Point(146, 217); this.textBox4.Size = new Size(139, 20); StyleTxtBox(this.textBox4);
            this.textBox5.Location = new Point(100, 260); this.textBox5.Size = new Size(144, 20); StyleTxtBox(this.textBox5);

            // ====== Total y acciones ======
            this.label9.AutoSize = true; this.label9.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label9.Location = new Point(509, 448); this.label9.Text = "Total de la Compra:"; StyleLabel(this.label9);

            this.checkBox1.AutoSize = true; this.checkBox1.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.checkBox1.Location = new Point(16, 319); this.checkBox1.Text = "Producto 2"; this.checkBox1.UseVisualStyleBackColor = true; StyleCheck(this.checkBox1);

            this.textBox6.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.textBox6.Location = new Point(689, 445); this.textBox6.Size = new Size(106, 29); StyleTxtBox(this.textBox6);

            this.btnGuardar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            this.btnGuardar.Location = new Point(537, 486); this.btnGuardar.Size = new Size(110, 45);
            this.btnGuardar.Text = "Guardar"; this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click); StyleButton(this.btnGuardar);

            this.btnCancelar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            this.btnCancelar.Location = new Point(653, 486); this.btnCancelar.Size = new Size(110, 45);
            this.btnCancelar.Text = "Cancelar"; this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click); StyleButton(this.btnCancelar);

            // ====== Línea 2 (opcional) ======
            this.label13.AutoSize = true; this.label13.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label13.Location = new Point(5, 350); this.label13.Text = "Producto:"; StyleLabel(this.label13);

            this.comboBox3.Location = new Point(93, 353); this.comboBox3.Size = new Size(308, 21); StyleCombo(this.comboBox3);

            this.label12.AutoSize = true; this.label12.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label12.Location = new Point(5, 393); this.label12.Text = "Cantidad:"; StyleLabel(this.label12);

            this.textBox9.Location = new Point(93, 397); this.textBox9.Multiline = true; this.textBox9.Size = new Size(80, 20); StyleTxtBox(this.textBox9);

            this.label11.AutoSize = true; this.label11.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label11.Location = new Point(5, 434); this.label11.Text = "Precio Unitario:"; StyleLabel(this.label11);

            this.textBox8.Location = new Point(139, 436); this.textBox8.Size = new Size(139, 20); StyleTxtBox(this.textBox8);

            this.label10.AutoSize = true; this.label10.Font = new Font("Microsoft Sans Serif", 14.25F);
            this.label10.Location = new Point(5, 477); this.label10.Text = "Subtotal:"; StyleLabel(this.label10);

            this.textBox7.Location = new Point(93, 479); this.textBox7.Size = new Size(144, 20); StyleTxtBox(this.textBox7);

            // ====== Add Controls ======
            this.Controls.AddRange(new Control[]
            {
                label1,label2,label3,label4,label5,label6,label7,label8,
                comboBox1,textBox1,textBox2,comboBox2,textBox3,textBox4,textBox5,
                label9,checkBox1,textBox6,btnGuardar,btnCancelar,
                label13,comboBox3,label12,textBox9,label11,textBox8,label10,textBox7
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

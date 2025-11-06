using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Alta_de_usuarios
    {
        private System.ComponentModel.IContainer components = null;

        private Label labelTitulo;
        private Label labelNombre;
        private TextBox textBoxNombre;
        private Label labelApellido;
        private TextBox textBoxApellido;
        private Label labelDNI;
        private TextBox textBoxDNI;
        private Label labelEmail;
        private TextBox textBoxEmail;
        private Label labelTelefono;
        private TextBox textBoxTelefono;
        private Label labelUsuario;
        private TextBox textBoxUsuario;
        private Label labelContrasenia;
        private TextBox textBoxContrasenia;
        private Label labelDireccion;
        private TextBox textBoxDireccion;
        private Label labelLocalidad;
        private TextBox textBoxLocalidad;
        private Label labelProvincia;
        private TextBox textBoxProvincia;
        private Label labelObservaciones;
        private TextBox textBoxObservaciones;
        private Label labelRol;
        private ComboBox comboBoxRol;
        private Label labelEstado;
        private ComboBox comboBoxEstado;
        private Label labelArea;
        private ComboBox comboBoxArea;
        private Button buttonGuardar;
        private Button buttonCancelar;

        // Seguridad
        private Label labelPreguntaSeg;
        private ComboBox comboBoxPreguntaSeg;
        private Label labelRespuestaSeg;
        private TextBox textBoxRespuestaSeg;

        // Contenedor scroll
        private Panel panelScroll;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = new Font("Segoe UI", 11F); // fuente más grande
            this.ClientSize = new System.Drawing.Size(980, 720);
            this.Name = "Alta_de_usuarios";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Alta de Usuarios";

            // Panel con scroll
            panelScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(panelScroll);

            // Título
            labelTitulo = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Location = new Point(24, 18),
                Text = "Alta de Usuarios"
            };
            panelScroll.Controls.Add(labelTitulo);

            int x1 = 28, x2 = 220, y = 80, sepY = 36, w = 320, h = 28;

            // Izquierda
            labelNombre = L("Nombre:", x1, y); panelScroll.Controls.Add(labelNombre);
            textBoxNombre = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxNombre); y += sepY;

            labelApellido = L("Apellido:", x1, y); panelScroll.Controls.Add(labelApellido);
            textBoxApellido = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxApellido); y += sepY;

            labelDNI = L("DNI:", x1, y); panelScroll.Controls.Add(labelDNI);
            textBoxDNI = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxDNI); y += sepY;

            labelEmail = L("Email:", x1, y); panelScroll.Controls.Add(labelEmail);
            textBoxEmail = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxEmail); y += sepY;

            labelTelefono = L("Teléfono:", x1, y); panelScroll.Controls.Add(labelTelefono);
            textBoxTelefono = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxTelefono); y += sepY;

            labelUsuario = L("Usuario:", x1, y); panelScroll.Controls.Add(labelUsuario);
            textBoxUsuario = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxUsuario); y += sepY;

            labelContrasenia = L("Contraseña:", x1, y); panelScroll.Controls.Add(labelContrasenia);
            textBoxContrasenia = T(x2, y - 4, w, h); textBoxContrasenia.UseSystemPasswordChar = true; panelScroll.Controls.Add(textBoxContrasenia); y += sepY;

            labelDireccion = L("Dirección:", x1, y); panelScroll.Controls.Add(labelDireccion);
            textBoxDireccion = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxDireccion); y += sepY;

            labelLocalidad = L("Localidad:", x1, y); panelScroll.Controls.Add(labelLocalidad);
            textBoxLocalidad = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxLocalidad); y += sepY;

            labelProvincia = L("Provincia:", x1, y); panelScroll.Controls.Add(labelProvincia);
            textBoxProvincia = T(x2, y - 4, w, h); panelScroll.Controls.Add(textBoxProvincia); y += sepY;

            labelObservaciones = L("Observaciones:", x1, y); panelScroll.Controls.Add(labelObservaciones);
            textBoxObservaciones = new TextBox { Location = new Point(x2, y - 4), Width = w, Height = 80, Multiline = true, BorderStyle = BorderStyle.FixedSingle };
            panelScroll.Controls.Add(textBoxObservaciones);

            // Derecha
            int x3 = 560, y2 = 80, w2 = 300;

            labelRol = L("Rol:", x3, y2); panelScroll.Controls.Add(labelRol);
            comboBoxRol = C(x3 + 160, y2 - 4, w2); panelScroll.Controls.Add(comboBoxRol); y2 += sepY;

            labelEstado = L("Estado:", x3, y2); panelScroll.Controls.Add(labelEstado);
            comboBoxEstado = C(x3 + 160, y2 - 4, w2); panelScroll.Controls.Add(comboBoxEstado); y2 += sepY;

            labelArea = L("Área:", x3, y2); panelScroll.Controls.Add(labelArea);
            comboBoxArea = C(x3 + 160, y2 - 4, w2); panelScroll.Controls.Add(comboBoxArea); y2 += sepY;

            labelPreguntaSeg = L("Pregunta de seguridad:", x3, y2); panelScroll.Controls.Add(labelPreguntaSeg);
            comboBoxPreguntaSeg = C(x3 + 160, y2 - 4, w2); panelScroll.Controls.Add(comboBoxPreguntaSeg); y2 += sepY;

            labelRespuestaSeg = L("Respuesta:", x3, y2); panelScroll.Controls.Add(labelRespuestaSeg);
            textBoxRespuestaSeg = T(x3 + 160, y2 - 4, w2, h); panelScroll.Controls.Add(textBoxRespuestaSeg); y2 += sepY;

            // Botones
            buttonGuardar = new Button
            {
                Location = new Point(x3, 460),
                Size = new Size(130, 36),
                Text = "Guardar",
                BackColor = Color.FromArgb(17, 105, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonGuardar.FlatAppearance.BorderSize = 0;

            buttonCancelar = new Button
            {
                Location = new Point(x3 + 150, 460),
                Size = new Size(130, 36),
                Text = "Cancelar",
                BackColor = Color.FromArgb(5, 80, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonCancelar.FlatAppearance.BorderSize = 0;

            panelScroll.Controls.Add(buttonGuardar);
            panelScroll.Controls.Add(buttonCancelar);

            // helpers locales
            Label L(string txt, int x, int yPos) =>
                new Label { AutoSize = true, Text = txt, Location = new Point(x, yPos) };

            TextBox T(int x, int yPos, int wdt, int hgt) =>
                new TextBox { Location = new Point(x, yPos), Width = wdt, Height = hgt, BorderStyle = BorderStyle.FixedSingle };

            ComboBox C(int x, int yPos, int wdt) =>
                new ComboBox { Location = new Point(x, yPos), Width = wdt, DropDownStyle = ComboBoxStyle.DropDownList };
        }
    }
}

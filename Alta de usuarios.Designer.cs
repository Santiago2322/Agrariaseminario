using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Alta_de_usuarios
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelScroll;   // <- contenedor con AutoScroll
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
        private Label labelPreguntaSeg;
        private ComboBox comboBoxPreguntaSeg;
        private Label labelRespuestaSeg;
        private TextBox textBoxRespuestaSeg;
        private Button buttonGuardar;
        private Button buttonCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ========== Form ==========
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(880, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alta de Usuarios";
            MinimizeBox = true; MaximizeBox = true;

            // ========== Panel con scroll ==========
            panelScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            Controls.Add(panelScroll);

            // ========== Controles ==========
            labelTitulo = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(24, 18),
                Text = "Alta de Usuarios"
            };
            panelScroll.Controls.Add(labelTitulo);

            int x1 = 28, x2 = 180, y = 60, sepY = 34, w = 260, h = 20;

            labelNombre = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Nombre:" };
            textBoxNombre = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelApellido = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Apellido:" };
            textBoxApellido = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelDNI = new Label { AutoSize = true, Location = new Point(x1, y), Text = "DNI:" };
            textBoxDNI = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelEmail = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Email:" };
            textBoxEmail = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelTelefono = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Teléfono:" };
            textBoxTelefono = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelUsuario = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Usuario:" };
            textBoxUsuario = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelContrasenia = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Contraseña:" };
            textBoxContrasenia = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h), UseSystemPasswordChar = true }; y += sepY;

            labelDireccion = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Dirección:" };
            textBoxDireccion = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelLocalidad = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Localidad:" };
            textBoxLocalidad = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelProvincia = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Provincia:" };
            textBoxProvincia = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, h) }; y += sepY;

            labelObservaciones = new Label { AutoSize = true, Location = new Point(x1, y), Text = "Observaciones:" };
            textBoxObservaciones = new TextBox { Location = new Point(x2, y - 3), Size = new Size(w, 60), Multiline = true };

            // Agrego a panel
            panelScroll.Controls.AddRange(new Control[]
            {
                labelNombre, textBoxNombre,
                labelApellido, textBoxApellido,
                labelDNI, textBoxDNI,
                labelEmail, textBoxEmail,
                labelTelefono, textBoxTelefono,
                labelUsuario, textBoxUsuario,
                labelContrasenia, textBoxContrasenia,
                labelDireccion, textBoxDireccion,
                labelLocalidad, textBoxLocalidad,
                labelProvincia, textBoxProvincia,
                labelObservaciones, textBoxObservaciones
            });

            // Columna derecha
            int x3 = 480, y2 = 60, w2 = w - 60;

            labelRol = new Label { AutoSize = true, Location = new Point(x3, y2), Text = "Rol:" };
            comboBoxRol = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(x3 + 140, y2 - 3), Size = new Size(w2, h) }; y2 += sepY;

            labelEstado = new Label { AutoSize = true, Location = new Point(x3, y2), Text = "Estado:" };
            comboBoxEstado = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(x3 + 140, y2 - 3), Size = new Size(w2, h) }; y2 += sepY;

            labelArea = new Label { AutoSize = true, Location = new Point(x3, y2), Text = "Área:" };
            comboBoxArea = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(x3 + 140, y2 - 3), Size = new Size(w2, h) }; y2 += sepY;

            labelPreguntaSeg = new Label { AutoSize = true, Location = new Point(x3, y2), Text = "Pregunta de seguridad:" };
            comboBoxPreguntaSeg = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(x3 + 140, y2 - 3), Size = new Size(w2, h) }; y2 += sepY;

            labelRespuestaSeg = new Label { AutoSize = true, Location = new Point(x3, y2), Text = "Respuesta de seguridad:" };
            textBoxRespuestaSeg = new TextBox { Location = new Point(x3 + 140, y2 - 3), Size = new Size(w2, h) }; y2 += sepY;

            panelScroll.Controls.AddRange(new Control[]
            {
                labelRol, comboBoxRol,
                labelEstado, comboBoxEstado,
                labelArea, comboBoxArea,
                labelPreguntaSeg, comboBoxPreguntaSeg,
                labelRespuestaSeg, textBoxRespuestaSeg
            });

            // Botones (abajo a la derecha)
            buttonGuardar = new Button
            {
                Location = new Point(x3, 460),
                Size = new Size(110, 32),
                Text = "Guardar",
                BackColor = Color.FromArgb(17, 105, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonGuardar.FlatAppearance.BorderSize = 0;

            buttonCancelar = new Button
            {
                Location = new Point(x3 + 120, 460),
                Size = new Size(110, 32),
                Text = "Cancelar",
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonCancelar.FlatAppearance.BorderSize = 0;

            panelScroll.Controls.Add(buttonGuardar);
            panelScroll.Controls.Add(buttonCancelar);

            // Asegura espacio de scroll al final
            var spacer = new Panel { Location = new Point(0, 520), Size = new Size(10, 10) };
            panelScroll.Controls.Add(spacer);

            // Wiring de eventos del Designer -> al code-behind
            textBoxDNI.KeyPress += new KeyPressEventHandler(this.soloNumeros_KeyPress);
            textBoxTelefono.KeyPress += new KeyPressEventHandler(this.tel_KeyPress);
        }
    }
}

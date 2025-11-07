using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Usuarios__Modificacion__Baja
    {
        private System.ComponentModel.IContainer components = null;

        private Label labelTitulo;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button button1; // Buscar
        private DataGridView dataGridViewUsuarios;

        private TextBox txtId;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDni;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private TextBox txtUsuario;

        private TextBox txtDireccion;
        private TextBox txtLocalidad;
        private TextBox txtProvincia;
        private TextBox txtObservaciones;

        private ComboBox cboRol;
        private ComboBox cboEstado;
        private ComboBox cboArea;

        private Label lblNombre;
        private Label lblApellido;
        private Label lblDni;
        private Label lblEmail;
        private Label lblTelefono;
        private Label lblUsuario;
        private Label lblDireccion;
        private Label lblLocalidad;
        private Label lblProvincia;
        private Label lblObservaciones;
        private Label lblRol;
        private Label lblEstado;
        private Label lblArea;

        // Seguridad (sin superposición)
        private Label lblPreguntaSeg;
        private ComboBox cboPreguntaSeg;
        private Label lblRespuestaSeg;
        private TextBox txtRespuestaSeg;

        private Button button2;           // Eliminar
        private Button button3;           // Guardar cambios

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ===== Fuentes =====
            Font fuenteTitulo = new Font("Segoe UI", 16F, FontStyle.Bold);
            Font fuenteLabel = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            Font fuenteNormal = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            Font fuenteBtn = new Font("Segoe UI", 10.5F, FontStyle.Bold);

            // ===== FORM =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(1120, 760);
            this.Text = "Usuarios - Consulta / Modificación / Baja";
            this.AutoScroll = true; // 👈 Scroll activado

            // ===== TÍTULO =====
            this.labelTitulo = new Label
            {
                AutoSize = true,
                Font = fuenteTitulo,
                Location = new Point(24, 18),
                Text = "Usuarios - Consulta / Modificación / Baja"
            };

            // ===== BUSCAR =====
            this.lblBuscar = new Label
            {
                AutoSize = true,
                Font = fuenteLabel,
                Location = new Point(28, 70),
                Text = "Buscar:"
            };

            this.txtBuscar = new TextBox
            {
                Font = fuenteNormal,
                Location = new Point(95, 66),
                Size = new Size(300, 28)
            };

            this.button1 = new Button
            {
                Font = fuenteBtn,
                Text = "Buscar",
                Location = new Point(405, 64),
                Size = new Size(95, 32)
            };

            // ===== GRID =====
            this.dataGridViewUsuarios = new DataGridView
            {
                Location = new Point(32, 110),
                Size = new Size(1050, 230),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            this.dataGridViewUsuarios.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle { Font = fuenteBtn };

            // ===== LAYOUT CAMPOS =====
            int xIzqLbl = 32, xIzqInp = 160;
            int xDerLbl = 560, xDerInp = 760;
            int yBase = 355;
            int sepY = 36;

            // ---- IZQUIERDA
            var lblId = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase), AutoSize = true, Text = "Id:" };
            this.txtId = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(90, 28), ReadOnly = true };

            this.lblNombre = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "Nombre:" };
            this.txtNombre = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            this.lblApellido = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "Apellido:" };
            this.txtApellido = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            this.lblDni = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "DNI:" };
            this.txtDni = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            this.lblEmail = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "Email:" };
            this.txtEmail = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            this.lblTelefono = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "Teléfono:" };
            this.txtTelefono = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            this.lblUsuario = new Label { Font = fuenteLabel, Location = new Point(xIzqLbl, yBase += sepY), AutoSize = true, Text = "Usuario:" };
            this.txtUsuario = new TextBox { Font = fuenteNormal, Location = new Point(xIzqInp, yBase - 2), Size = new Size(280, 28) };

            // ---- DERECHA
            int yDer = 355;

            this.lblDireccion = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer), AutoSize = true, Text = "Dirección:" };
            this.txtDireccion = new TextBox { Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(300, 28) };

            this.lblLocalidad = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += sepY), AutoSize = true, Text = "Localidad:" };
            this.txtLocalidad = new TextBox { Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(300, 28) };

            this.lblProvincia = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += sepY), AutoSize = true, Text = "Provincia:" };
            this.txtProvincia = new TextBox { Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(300, 28) };

            this.lblObservaciones = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += sepY), AutoSize = true, Text = "Observaciones:" };
            this.txtObservaciones = new TextBox { Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Multiline = true, Size = new Size(300, 56) };

            this.lblRol = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += 64), AutoSize = true, Text = "Rol:" };
            this.cboRol = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(200, 28) };

            this.lblEstado = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += sepY), AutoSize = true, Text = "Estado:" };
            this.cboEstado = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(200, 28) };

            this.lblArea = new Label { Font = fuenteLabel, Location = new Point(xDerLbl, yDer += sepY), AutoSize = true, Text = "Área:" };
            this.cboArea = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = fuenteNormal, Location = new Point(xDerInp, yDer - 2), Size = new Size(200, 28) };

            // ---- Seguridad (SIN superposición, con aire horizontal)
            yDer += sepY + 8;

            this.lblPreguntaSeg = new Label
            {
                Font = fuenteLabel,
                AutoSize = true,
                Location = new Point(xDerLbl, yDer),
                Text = "Pregunta de seguridad:"
            };
            this.cboPreguntaSeg = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = fuenteNormal,
                Location = new Point(xDerInp, yDer - 2),
                Size = new Size(300, 28)
            };

            this.lblRespuestaSeg = new Label
            {
                Font = fuenteLabel,
                AutoSize = true,
                Location = new Point(xDerLbl, yDer += sepY),
                Text = "Respuesta:"
            };
            this.txtRespuestaSeg = new TextBox
            {
                Font = fuenteNormal,
                Location = new Point(xDerInp, yDer - 2),
                Size = new Size(300, 28)
            };

            // ---- Botones (alineados a la columna derecha)
            this.button2 = new Button
            {
                Font = fuenteBtn,
                Text = "Eliminar",
                Size = new Size(120, 36),
                Location = new Point(xDerInp, yDer + 40),
                Enabled = false
            };

            this.button3 = new Button
            {
                Font = fuenteBtn,
                Text = "Guardar cambios",
                Size = new Size(170, 36),
                Location = new Point(xDerInp + 140, yDer + 40),
                Enabled = false
            };

            // Ajuste de alto de formulario si hiciera falta
            int bottom = yDer + 40 + 60;
            if (this.ClientSize.Height < bottom)
                this.ClientSize = new Size(this.ClientSize.Width, bottom);

            // ===== ADD CONTROLS =====
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridViewUsuarios);

            this.Controls.Add(lblId);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.lblDni);
            this.Controls.Add(this.txtDni);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);

            this.Controls.Add(this.lblDireccion);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.lblLocalidad);
            this.Controls.Add(this.txtLocalidad);
            this.Controls.Add(this.lblProvincia);
            this.Controls.Add(this.txtProvincia);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cboRol);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cboEstado);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.cboArea);

            this.Controls.Add(this.lblPreguntaSeg);
            this.Controls.Add(this.cboPreguntaSeg);
            this.Controls.Add(this.lblRespuestaSeg);
            this.Controls.Add(this.txtRespuestaSeg);

            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
        }
    }
}

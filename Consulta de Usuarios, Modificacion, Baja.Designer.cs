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
        private Button button1;
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

        // Seguridad
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

            // ===== Fuentes cómodas =====
            Font fuenteTitulo = new Font("Segoe UI", 16F, FontStyle.Bold);
            Font fuenteLabel = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            Font fuenteNormal = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            Font fuenteBtn = new Font("Segoe UI", 10.5F, FontStyle.Bold);

            // ===== FORM =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1100, 720);
            this.Text = "Usuarios - Consulta / Modificación / Baja";
            this.BackColor = Color.WhiteSmoke;

            // ===== TÍTULO =====
            this.labelTitulo = new Label();
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = fuenteTitulo;
            this.labelTitulo.Location = new Point(24, 18);
            this.labelTitulo.Text = "Usuarios - Consulta / Modificación / Baja";

            // ===== BUSCAR =====
            this.lblBuscar = new Label();
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = fuenteLabel;
            this.lblBuscar.Location = new Point(28, 70);
            this.lblBuscar.Text = "Buscar:";

            this.txtBuscar = new TextBox();
            this.txtBuscar.Font = fuenteNormal;
            this.txtBuscar.Location = new Point(95, 66);
            this.txtBuscar.Size = new Size(300, 28);

            this.button1 = new Button();
            this.button1.Font = fuenteBtn;
            this.button1.Text = "Buscar";
            this.button1.Location = new Point(405, 64);
            this.button1.Size = new Size(95, 32);

            // ===== GRID (ACHICADO) =====
            this.dataGridViewUsuarios = new DataGridView();
            this.dataGridViewUsuarios.Location = new Point(32, 110);
            this.dataGridViewUsuarios.Size = new Size(1035, 230); // ← antes 300+, ahora 230
            this.dataGridViewUsuarios.ReadOnly = true;
            this.dataGridViewUsuarios.AllowUserToAddRows = false;
            this.dataGridViewUsuarios.AllowUserToDeleteRows = false;
            this.dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUsuarios.BackgroundColor = Color.White;
            this.dataGridViewUsuarios.Font = fuenteNormal;
            this.dataGridViewUsuarios.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = fuenteBtn };

            // ===== LAYOUT CAMPOS =====
            int xIzqLbl = 32, xIzqInp = 160;
            int xDerLbl = 580, xDerInp = 705;
            int yBase = 355;              // ← subidos para que no los tape el grid
            int sepY = 36;

            // ---- IZQUIERDA
            var lblId = new Label();
            lblId.Font = fuenteLabel;
            lblId.Location = new Point(xIzqLbl, yBase);
            lblId.AutoSize = true;
            lblId.Text = "Id:";

            this.txtId = new TextBox();
            this.txtId.Font = fuenteNormal;
            this.txtId.Location = new Point(xIzqInp, yBase - 2);
            this.txtId.Size = new Size(90, 28);
            this.txtId.ReadOnly = true;

            this.lblNombre = new Label();
            this.lblNombre.Font = fuenteLabel;
            this.lblNombre.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblNombre.AutoSize = true;
            this.lblNombre.Text = "Nombre:";

            this.txtNombre = new TextBox();
            this.txtNombre.Font = fuenteNormal;
            this.txtNombre.Location = new Point(xIzqInp, yBase - 2);
            this.txtNombre.Size = new Size(280, 28);

            this.lblApellido = new Label();
            this.lblApellido.Font = fuenteLabel;
            this.lblApellido.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblApellido.AutoSize = true;
            this.lblApellido.Text = "Apellido:";

            this.txtApellido = new TextBox();
            this.txtApellido.Font = fuenteNormal;
            this.txtApellido.Location = new Point(xIzqInp, yBase - 2);
            this.txtApellido.Size = new Size(280, 28);

            this.lblDni = new Label();
            this.lblDni.Font = fuenteLabel;
            this.lblDni.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblDni.AutoSize = true;
            this.lblDni.Text = "DNI:";

            this.txtDni = new TextBox();
            this.txtDni.Font = fuenteNormal;
            this.txtDni.Location = new Point(xIzqInp, yBase - 2);
            this.txtDni.Size = new Size(280, 28);

            this.lblEmail = new Label();
            this.lblEmail.Font = fuenteLabel;
            this.lblEmail.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblEmail.AutoSize = true;
            this.lblEmail.Text = "Email:";

            this.txtEmail = new TextBox();
            this.txtEmail.Font = fuenteNormal;
            this.txtEmail.Location = new Point(xIzqInp, yBase - 2);
            this.txtEmail.Size = new Size(280, 28);

            this.lblTelefono = new Label();
            this.lblTelefono.Font = fuenteLabel;
            this.lblTelefono.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Text = "Teléfono:";

            this.txtTelefono = new TextBox();
            this.txtTelefono.Font = fuenteNormal;
            this.txtTelefono.Location = new Point(xIzqInp, yBase - 2);
            this.txtTelefono.Size = new Size(280, 28);

            this.lblUsuario = new Label();
            this.lblUsuario.Font = fuenteLabel;
            this.lblUsuario.Location = new Point(xIzqLbl, yBase += sepY);
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Text = "Usuario:";

            this.txtUsuario = new TextBox();
            this.txtUsuario.Font = fuenteNormal;
            this.txtUsuario.Location = new Point(xIzqInp, yBase - 2);
            this.txtUsuario.Size = new Size(280, 28);

            // ---- DERECHA
            int yDer = 355;

            this.lblDireccion = new Label();
            this.lblDireccion.Font = fuenteLabel;
            this.lblDireccion.Location = new Point(xDerLbl, yDer);
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Text = "Dirección:";

            this.txtDireccion = new TextBox();
            this.txtDireccion.Font = fuenteNormal;
            this.txtDireccion.Location = new Point(xDerInp, yDer - 2);
            this.txtDireccion.Size = new Size(300, 28);

            this.lblLocalidad = new Label();
            this.lblLocalidad.Font = fuenteLabel;
            this.lblLocalidad.Location = new Point(xDerLbl, yDer += sepY);
            this.lblLocalidad.AutoSize = true;
            this.lblLocalidad.Text = "Localidad:";

            this.txtLocalidad = new TextBox();
            this.txtLocalidad.Font = fuenteNormal;
            this.txtLocalidad.Location = new Point(xDerInp, yDer - 2);
            this.txtLocalidad.Size = new Size(300, 28);

            this.lblProvincia = new Label();
            this.lblProvincia.Font = fuenteLabel;
            this.lblProvincia.Location = new Point(xDerLbl, yDer += sepY);
            this.lblProvincia.AutoSize = true;
            this.lblProvincia.Text = "Provincia:";

            this.txtProvincia = new TextBox();
            this.txtProvincia.Font = fuenteNormal;
            this.txtProvincia.Location = new Point(xDerInp, yDer - 2);
            this.txtProvincia.Size = new Size(300, 28);

            this.lblObservaciones = new Label();
            this.lblObservaciones.Font = fuenteLabel;
            this.lblObservaciones.Location = new Point(xDerLbl, yDer += sepY);
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Text = "Observaciones:";

            this.txtObservaciones = new TextBox();
            this.txtObservaciones.Font = fuenteNormal;
            this.txtObservaciones.Location = new Point(xDerInp, yDer - 2);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Size = new Size(300, 56);

            this.lblRol = new Label();
            this.lblRol.Font = fuenteLabel;
            this.lblRol.Location = new Point(xDerLbl, yDer += 64);
            this.lblRol.AutoSize = true;
            this.lblRol.Text = "Rol:";

            this.cboRol = new ComboBox();
            this.cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboRol.Font = fuenteNormal;
            this.cboRol.Location = new Point(xDerInp, yDer - 2);
            this.cboRol.Size = new Size(200, 28);

            this.lblEstado = new Label();
            this.lblEstado.Font = fuenteLabel;
            this.lblEstado.Location = new Point(xDerLbl, yDer += sepY);
            this.lblEstado.AutoSize = true;
            this.lblEstado.Text = "Estado:";

            this.cboEstado = new ComboBox();
            this.cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboEstado.Font = fuenteNormal;
            this.cboEstado.Location = new Point(xDerInp, yDer - 2);
            this.cboEstado.Size = new Size(200, 28);

            this.lblArea = new Label();
            this.lblArea.Font = fuenteLabel;
            this.lblArea.Location = new Point(xDerLbl, yDer += sepY);
            this.lblArea.AutoSize = true;
            this.lblArea.Text = "Área:";

            this.cboArea = new ComboBox();
            this.cboArea.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboArea.Font = fuenteNormal;
            this.cboArea.Location = new Point(xDerInp, yDer - 2);
            this.cboArea.Size = new Size(200, 28);

            // ---- Seguridad
            yDer += sepY;
            this.lblPreguntaSeg = new Label();
            this.lblPreguntaSeg.Font = fuenteLabel;
            this.lblPreguntaSeg.AutoSize = true;
            this.lblPreguntaSeg.Location = new Point(xDerLbl, yDer);
            this.lblPreguntaSeg.Text = "Pregunta de seguridad:";

            this.cboPreguntaSeg = new ComboBox();
            this.cboPreguntaSeg.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPreguntaSeg.Font = fuenteNormal;
            this.cboPreguntaSeg.Location = new Point(xDerInp, yDer - 2);
            this.cboPreguntaSeg.Size = new Size(300, 28);

            this.lblRespuestaSeg = new Label();
            this.lblRespuestaSeg.Font = fuenteLabel;
            this.lblRespuestaSeg.AutoSize = true;
            this.lblRespuestaSeg.Location = new Point(xDerLbl, yDer += sepY);
            this.lblRespuestaSeg.Text = "Respuesta:";

            this.txtRespuestaSeg = new TextBox();
            this.txtRespuestaSeg.Font = fuenteNormal;
            this.txtRespuestaSeg.Location = new Point(xDerInp, yDer - 2);
            this.txtRespuestaSeg.Size = new Size(300, 28);

            // ---- Botones
            this.button2 = new Button();
            this.button2.Font = fuenteBtn;
            this.button2.Text = "Eliminar";
            this.button2.Size = new Size(120, 36);
            this.button2.Location = new Point(xDerInp, yDer + 40);

            this.button3 = new Button();
            this.button3.Font = fuenteBtn;
            this.button3.Text = "Guardar cambios";
            this.button3.Size = new Size(170, 36);
            this.button3.Location = new Point(xDerInp + 140, yDer + 40);

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

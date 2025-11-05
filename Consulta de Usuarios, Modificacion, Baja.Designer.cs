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

        // === NUEVOS (coinciden con el code-behind) ===
        private Label lblPreguntaSeg;
        private ComboBox cboPreguntaSeg;
        private Label lblRespuestaSeg;
        private TextBox txtRespuestaSeg;

        private Button button2;
        private Button button3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.labelTitulo = new Label();
            this.lblBuscar = new Label();
            this.txtBuscar = new TextBox();
            this.button1 = new Button();
            this.dataGridViewUsuarios = new DataGridView();

            this.txtId = new TextBox();
            this.txtNombre = new TextBox();
            this.txtApellido = new TextBox();
            this.txtDni = new TextBox();
            this.txtEmail = new TextBox();
            this.txtTelefono = new TextBox();
            this.txtUsuario = new TextBox();

            this.txtDireccion = new TextBox();
            this.txtLocalidad = new TextBox();
            this.txtProvincia = new TextBox();
            this.txtObservaciones = new TextBox();

            this.cboRol = new ComboBox();
            this.cboEstado = new ComboBox();
            this.cboArea = new ComboBox();

            this.lblNombre = new Label();
            this.lblApellido = new Label();
            this.lblDni = new Label();
            this.lblEmail = new Label();
            this.lblTelefono = new Label();
            this.lblUsuario = new Label();
            this.lblDireccion = new Label();
            this.lblLocalidad = new Label();
            this.lblProvincia = new Label();
            this.lblObservaciones = new Label();
            this.lblRol = new Label();
            this.lblEstado = new Label();
            this.lblArea = new Label();

            // === NUEVOS (UI seguridad) ===
            this.lblPreguntaSeg = new Label();
            this.cboPreguntaSeg = new ComboBox();
            this.lblRespuestaSeg = new Label();
            this.txtRespuestaSeg = new TextBox();

            this.button2 = new Button();
            this.button3 = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).BeginInit();
            this.SuspendLayout();

            // ===== FORM =====
            this.AutoScaleMode = AutoScaleMode.Font;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1050, 660);
            this.Text = "Usuarios - Consulta / Modificación / Baja";

            // ===== TÍTULO =====
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.labelTitulo.Location = new Point(24, 18);
            this.labelTitulo.Text = "Usuarios - Consulta / Modificación / Baja";

            // ===== BUSCAR =====
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new Point(28, 60);
            this.lblBuscar.Text = "Buscar:";

            this.txtBuscar.Location = new Point(90, 57);
            this.txtBuscar.Size = new Size(260, 23);

            this.button1.Text = "Buscar";
            this.button1.Location = new Point(360, 56);
            this.button1.Size = new Size(80, 25);

            // ===== GRID =====
            this.dataGridViewUsuarios.Location = new Point(28, 94);
            this.dataGridViewUsuarios.ReadOnly = true;
            this.dataGridViewUsuarios.AllowUserToAddRows = false;
            this.dataGridViewUsuarios.AllowUserToDeleteRows = false;
            this.dataGridViewUsuarios.Size = new Size(990, 260);
            this.dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // ===== IZQUIERDA =====
            var lblId = new Label();
            lblId.Location = new Point(28, 368);
            lblId.Text = "Id:";
            lblId.AutoSize = true;

            this.txtId.Location = new Point(160, 366);
            this.txtId.Size = new Size(80, 23);
            this.txtId.ReadOnly = true;

            this.lblNombre.Location = new Point(28, 398);
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.AutoSize = true;
            this.txtNombre.Location = new Point(160, 396);
            this.txtNombre.Size = new Size(260, 23);

            this.lblApellido.Location = new Point(28, 428);
            this.lblApellido.Text = "Apellido:";
            this.lblApellido.AutoSize = true;
            this.txtApellido.Location = new Point(160, 426);
            this.txtApellido.Size = new Size(260, 23);

            this.lblDni.Location = new Point(28, 458);
            this.lblDni.Text = "DNI:";
            this.lblDni.AutoSize = true;
            this.txtDni.Location = new Point(160, 456);
            this.txtDni.Size = new Size(260, 23);

            this.lblEmail.Location = new Point(28, 488);
            this.lblEmail.Text = "Email:";
            this.lblEmail.AutoSize = true;
            this.txtEmail.Location = new Point(160, 486);
            this.txtEmail.Size = new Size(260, 23);

            this.lblTelefono.Location = new Point(28, 518);
            this.lblTelefono.Text = "Teléfono:";
            this.lblTelefono.AutoSize = true;
            this.txtTelefono.Location = new Point(160, 516);
            this.txtTelefono.Size = new Size(260, 23);

            this.lblUsuario.Location = new Point(28, 548);
            this.lblUsuario.Text = "Usuario:";
            this.lblUsuario.AutoSize = true;
            this.txtUsuario.Location = new Point(160, 546);
            this.txtUsuario.Size = new Size(260, 23);

            // ===== DERECHA =====
            this.lblDireccion.Location = new Point(560, 368);
            this.lblDireccion.Text = "Dirección:";
            this.lblDireccion.AutoSize = true;
            this.txtDireccion.Location = new Point(690, 366);
            this.txtDireccion.Size = new Size(260, 23);

            this.lblLocalidad.Location = new Point(560, 398);
            this.lblLocalidad.Text = "Localidad:";
            this.lblLocalidad.AutoSize = true;
            this.txtLocalidad.Location = new Point(690, 396);
            this.txtLocalidad.Size = new Size(260, 23);

            this.lblProvincia.Location = new Point(560, 428);
            this.lblProvincia.Text = "Provincia:";
            this.lblProvincia.AutoSize = true;
            this.txtProvincia.Location = new Point(690, 426);
            this.txtProvincia.Size = new Size(260, 23);

            this.lblObservaciones.Location = new Point(560, 458);
            this.lblObservaciones.Text = "Observaciones:";
            this.lblObservaciones.AutoSize = true;
            this.txtObservaciones.Location = new Point(690, 456);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Size = new Size(260, 50);

            this.lblRol.Location = new Point(560, 518);
            this.lblRol.Text = "Rol:";
            this.lblRol.AutoSize = true;
            this.cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboRol.Location = new Point(690, 516);
            this.cboRol.Size = new Size(180, 23);

            this.lblEstado.Location = new Point(560, 548);
            this.lblEstado.Text = "Estado:";
            this.lblEstado.AutoSize = true;
            this.cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboEstado.Location = new Point(690, 546);
            this.cboEstado.Size = new Size(180, 23);

            this.lblArea.Location = new Point(560, 578);
            this.lblArea.Text = "Área:";
            this.lblArea.AutoSize = true;
            this.cboArea.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboArea.Location = new Point(690, 576);
            this.cboArea.Size = new Size(180, 23);

            // ===== SEGURIDAD (debajo de Área/Estado) =====
            int baseY = 606; // debajo de los combos
            this.lblPreguntaSeg.AutoSize = true;
            this.lblPreguntaSeg.Text = "Pregunta de seguridad:";
            this.lblPreguntaSeg.Location = new Point(560, baseY + 6);

            this.cboPreguntaSeg.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPreguntaSeg.Location = new Point(690, baseY);
            this.cboPreguntaSeg.Size = new Size(260, 23);

            this.lblRespuestaSeg.AutoSize = true;
            this.lblRespuestaSeg.Text = "Respuesta:";
            this.lblRespuestaSeg.Location = new Point(560, baseY + 40 + 6);

            this.txtRespuestaSeg.Location = new Point(690, baseY + 40);
            this.txtRespuestaSeg.Size = new Size(260, 23);

            // ===== BOTONES =====
            this.button2.Text = "Eliminar";
            this.button2.Location = new Point(690, baseY + 80);
            this.button2.Size = new Size(100, 30);

            this.button3.Text = "Guardar cambios";
            this.button3.Location = new Point(800, baseY + 80);
            this.button3.Size = new Size(160, 30);

            // Ajusto ClientSize si hace falta más alto
            if (this.ClientSize.Height < baseY + 120)
                this.ClientSize = new Size(this.ClientSize.Width, baseY + 120);

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

            // nuevos
            this.Controls.Add(this.lblPreguntaSeg);
            this.Controls.Add(this.cboPreguntaSeg);
            this.Controls.Add(this.lblRespuestaSeg);
            this.Controls.Add(this.txtRespuestaSeg);

            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

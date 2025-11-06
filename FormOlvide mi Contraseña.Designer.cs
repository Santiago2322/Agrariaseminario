using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class FormOlvide_mi_Contraseña
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblUsuario;
        private Label lblNueva;
        private Label lblConfirmar;
        private Label lblPregunta;
        private Label lblRespuesta;
        internal Label lblInfoPregunta;

        internal TextBox txtUsuario;
        internal TextBox txtNueva;
        internal TextBox txtConfirmar;
        internal ComboBox cboPregunta;
        internal TextBox txtRespuesta;

        internal Button btnRestablecer;
        internal Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ====== Fuentes y medidas cómodas ======
            Font fLbl = new Font("Segoe UI", 12F, FontStyle.Regular);
            Font fTxt = new Font("Segoe UI", 12F, FontStyle.Regular);
            Font fBtn = new Font("Segoe UI", 12F, FontStyle.Bold);

            int xLabel = 28;
            int xInput = 240;
            int wInput = 300;
            int hInput = 30;
            int y = 28;
            int dy = 46;

            // ====== Controles ======
            this.lblUsuario = new Label();
            this.lblNueva = new Label();
            this.lblConfirmar = new Label();
            this.lblPregunta = new Label();
            this.lblRespuesta = new Label();
            this.lblInfoPregunta = new Label();

            this.txtUsuario = new TextBox();
            this.txtNueva = new TextBox();
            this.txtConfirmar = new TextBox();
            this.cboPregunta = new ComboBox();
            this.txtRespuesta = new TextBox();

            this.btnRestablecer = new Button();
            this.btnCancelar = new Button();

            // ====== Form ======
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Font = fTxt;
            this.ClientSize = new Size(580, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Recuperar contraseña";

            // Sugerencias de UX
            this.AcceptButton = this.btnRestablecer;
            this.CancelButton = this.btnCancelar;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            // ====== Labels ======
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = fLbl;
            this.lblUsuario.Location = new Point(xLabel, y);
            this.lblUsuario.Text = "Usuario:";

            this.lblNueva.AutoSize = true;
            this.lblNueva.Font = fLbl;
            this.lblNueva.Location = new Point(xLabel, y += dy);
            this.lblNueva.Text = "Nueva contraseña:";

            this.lblConfirmar.AutoSize = true;
            this.lblConfirmar.Font = fLbl;
            this.lblConfirmar.Location = new Point(xLabel, y += dy);
            this.lblConfirmar.Text = "Confirmar contraseña:";

            this.lblPregunta.AutoSize = true;
            this.lblPregunta.Font = fLbl;
            this.lblPregunta.Location = new Point(xLabel, y += dy);
            this.lblPregunta.Text = "Pregunta de seguridad:";

            this.lblRespuesta.AutoSize = true;
            this.lblRespuesta.Font = fLbl;
            this.lblRespuesta.Location = new Point(xLabel, y += dy);
            this.lblRespuesta.Text = "Respuesta:";

            this.lblInfoPregunta.AutoSize = true;
            this.lblInfoPregunta.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            this.lblInfoPregunta.ForeColor = Color.DimGray;
            this.lblInfoPregunta.Location = new Point(xLabel, y + dy);
            this.lblInfoPregunta.Text = ""; // Se completa desde el code-behind si querés

            // ====== Inputs ======
            y = 24;
            this.txtUsuario.Location = new Point(xInput, y);
            this.txtUsuario.Size = new Size(wInput, hInput);
            this.txtUsuario.Font = fTxt;

            this.txtNueva.Location = new Point(xInput, y += dy);
            this.txtNueva.Size = new Size(wInput, hInput);
            this.txtNueva.Font = fTxt;
            this.txtNueva.UseSystemPasswordChar = true;

            this.txtConfirmar.Location = new Point(xInput, y += dy);
            this.txtConfirmar.Size = new Size(wInput, hInput);
            this.txtConfirmar.Font = fTxt;
            this.txtConfirmar.UseSystemPasswordChar = true;

            this.cboPregunta.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPregunta.Location = new Point(xInput, y += dy);
            this.cboPregunta.Size = new Size(wInput, hInput);
            this.cboPregunta.Font = fTxt;

            this.txtRespuesta.Location = new Point(xInput, y += dy);
            this.txtRespuesta.Size = new Size(wInput, hInput);
            this.txtRespuesta.Font = fTxt;

            // ====== Botones ======
            this.btnRestablecer.Text = "Restablecer";
            this.btnRestablecer.Font = fBtn;
            this.btnRestablecer.BackColor = Color.FromArgb(17, 105, 59);
            this.btnRestablecer.ForeColor = Color.White;
            this.btnRestablecer.FlatStyle = FlatStyle.Flat;
            this.btnRestablecer.FlatAppearance.BorderSize = 0;
            this.btnRestablecer.Size = new Size(140, 38);
            this.btnRestablecer.Location = new Point(xInput + wInput - 290, 300);
            // Si tenés estos handlers en el code-behind, quedarán conectados:
            this.btnRestablecer.Click += new System.EventHandler(this.btnRestablecer_Click);

            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Font = fBtn;
            this.btnCancelar.BackColor = Color.FromArgb(200, 50, 50);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.Size = new Size(140, 38);
            this.btnCancelar.Location = new Point(xInput + wInput - 140, 300);
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // ====== Add Controls ======
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblNueva);
            this.Controls.Add(this.lblConfirmar);
            this.Controls.Add(this.lblPregunta);
            this.Controls.Add(this.lblRespuesta);
            this.Controls.Add(this.lblInfoPregunta);

            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.txtNueva);
            this.Controls.Add(this.txtConfirmar);
            this.Controls.Add(this.cboPregunta);
            this.Controls.Add(this.txtRespuesta);

            this.Controls.Add(this.btnRestablecer);
            this.Controls.Add(this.btnCancelar);
        }
    }
}

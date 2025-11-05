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

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(560, 310);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Recuperar contraseña";

            var fontLbl = new Font("Segoe UI", 10F, FontStyle.Regular);
            int x = 210, w = 320, h = 24;

            // Labels
            this.lblUsuario.AutoSize = true; this.lblUsuario.Font = fontLbl;
            this.lblUsuario.Location = new Point(28, 30); this.lblUsuario.Text = "Usuario:";

            this.lblNueva.AutoSize = true; this.lblNueva.Font = fontLbl;
            this.lblNueva.Location = new Point(28, 70); this.lblNueva.Text = "Nueva contraseña:";

            this.lblConfirmar.AutoSize = true; this.lblConfirmar.Font = fontLbl;
            this.lblConfirmar.Location = new Point(28, 110); this.lblConfirmar.Text = "Confirmar contraseña:";

            this.lblPregunta.AutoSize = true; this.lblPregunta.Font = fontLbl;
            this.lblPregunta.Location = new Point(28, 150); this.lblPregunta.Text = "Pregunta de seguridad:";

            this.lblRespuesta.AutoSize = true; this.lblRespuesta.Font = fontLbl;
            this.lblRespuesta.Location = new Point(28, 190); this.lblRespuesta.Text = "Respuesta:";

            this.lblInfoPregunta.AutoSize = true;
            this.lblInfoPregunta.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblInfoPregunta.ForeColor = Color.DimGray;
            this.lblInfoPregunta.Location = new Point(28, 225);
            this.lblInfoPregunta.Text = "";

            // Inputs
            this.txtUsuario.Location = new Point(x, 28); this.txtUsuario.Size = new Size(w, h);
            this.txtNueva.Location = new Point(x, 68); this.txtNueva.Size = new Size(w, h); this.txtNueva.UseSystemPasswordChar = true;
            this.txtConfirmar.Location = new Point(x, 108); this.txtConfirmar.Size = new Size(w, h); this.txtConfirmar.UseSystemPasswordChar = true;

            this.cboPregunta.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPregunta.Location = new Point(x, 148); this.cboPregunta.Size = new Size(w, h);

            this.txtRespuesta.Location = new Point(x, 188); this.txtRespuesta.Size = new Size(w, h);

            // Buttons
            this.btnRestablecer.Text = "Restablecer";
            this.btnRestablecer.BackColor = SystemColors.ActiveCaption;
            this.btnRestablecer.ForeColor = SystemColors.ActiveCaptionText;
            this.btnRestablecer.Location = new Point(310, 250); this.btnRestablecer.Size = new Size(110, 32);

            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.BackColor = SystemColors.ControlLight;
            this.btnCancelar.ForeColor = SystemColors.ActiveCaptionText;
            this.btnCancelar.Location = new Point(430, 250); this.btnCancelar.Size = new Size(110, 32);

            // Add controls
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

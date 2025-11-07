using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    partial class Form1
    {
        private IContainer components = null;

        private Panel pnlLeft;
        private PictureBox pictureBox1;
        private Panel pnlRight;
        private Label lblUsuario;
        private Label lblClave;
        internal TextBox txtUsuario;
        internal TextBox txtClave;
        internal Button btnContinuar;
        internal LinkLabel lnkOlvide;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();

            this.pnlLeft = new Panel();
            this.pictureBox1 = new PictureBox();
            this.pnlRight = new Panel();
            this.lblUsuario = new Label();
            this.lblClave = new Label();
            this.txtUsuario = new TextBox();
            this.txtClave = new TextBox();
            this.btnContinuar = new Button();
            this.lnkOlvide = new LinkLabel();

            // ===== Form =====
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 480);
            this.MinimumSize = new Size(900, 480);
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.BackColor = SystemColors.Control;
            this.Font = new Font("Segoe UI", 9F);

            // ===== Left =====
            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 360;
            this.pnlLeft.BackColor = Color.White;

            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pnlLeft.Controls.Add(this.pictureBox1);

            // ===== Right =====
            this.pnlRight.Dock = DockStyle.Fill;
            this.pnlRight.BackColor = Color.FromArgb(240, 240, 240);

            var labelFont = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = labelFont;
            this.lblUsuario.Location = new Point(60, 90);
            this.lblUsuario.Text = "Usuario";

            this.lblClave.AutoSize = true;
            this.lblClave.Font = labelFont;
            this.lblClave.Location = new Point(60, 160);
            this.lblClave.Text = "Contraseña";

            var inputFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtUsuario.Font = inputFont;
            this.txtUsuario.Location = new Point(60, 125);
            this.txtUsuario.Size = new Size(320, 29);

            this.txtClave.Font = inputFont;
            this.txtClave.Location = new Point(60, 195);
            this.txtClave.Size = new Size(320, 29);
            this.txtClave.UseSystemPasswordChar = true;

            var btnFont = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnContinuar.Font = btnFont;
            this.btnContinuar.Location = new Point(60, 250);
            this.btnContinuar.Size = new Size(160, 40);
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;

            var linkFont = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            this.lnkOlvide.AutoSize = true;
            this.lnkOlvide.Font = linkFont;
            this.lnkOlvide.Location = new Point(60, 305);
            this.lnkOlvide.Text = "Olvidé mi contraseña";

            this.pnlRight.Controls.Add(this.lblUsuario);
            this.pnlRight.Controls.Add(this.txtUsuario);
            this.pnlRight.Controls.Add(this.lblClave);
            this.pnlRight.Controls.Add(this.txtClave);
            this.pnlRight.Controls.Add(this.btnContinuar);
            this.pnlRight.Controls.Add(this.lnkOlvide);

            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.ResumeLayout(false);
        }
    }
}

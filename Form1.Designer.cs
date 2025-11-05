using System; // Agregada para consistencia, aunque no es estrictamente necesaria aquí
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    // *** CORRECCIÓN CLAVE: Se añade la herencia de : Form ***
    public partial class Form1 : Form
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

        // Se sobreescribe Dispose, ahora válido porque Form1 hereda de Form
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
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = false;
            this.ClientSize = new Size(820, 420);
            this.MinimumSize = new Size(820, 420);
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.BackColor = SystemColors.Control;

            // Left
            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 330;
            this.pnlLeft.BackColor = Color.White;

            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pnlLeft.Controls.Add(this.pictureBox1);

            // Right
            this.pnlRight.Dock = DockStyle.Fill;
            this.pnlRight.BackColor = Color.FromArgb(224, 224, 224);

            // Usuario
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new Point(60, 80);
            this.lblUsuario.Text = "Usuario";

            this.txtUsuario.Location = new Point(150, 76);
            this.txtUsuario.Size = new Size(220, 22);
            this.txtUsuario.TextChanged += new System.EventHandler(this.txtUsuario_TextChanged);

            // Clave
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new Point(60, 130);
            this.lblClave.Text = "Contraseña";

            this.txtClave.Location = new Point(150, 126);
            this.txtClave.Size = new Size(220, 22);
            this.txtClave.UseSystemPasswordChar = true;
            this.txtClave.TextChanged += new System.EventHandler(this.txtClave_TextChanged);

            // Botón
            this.btnContinuar.Location = new Point(150, 180);
            this.btnContinuar.Size = new Size(120, 34);
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);

            // Link
            this.lnkOlvide.AutoSize = true;
            this.lnkOlvide.Location = new Point(150, 225);
            this.lnkOlvide.Text = "Olvidé mi contraseña";
            this.lnkOlvide.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkOlvide_LinkClicked);

            // Ensamble Right
            this.pnlRight.Controls.Add(this.lblUsuario);
            this.pnlRight.Controls.Add(this.txtUsuario);
            this.pnlRight.Controls.Add(this.lblClave);
            this.pnlRight.Controls.Add(this.txtClave);
            this.pnlRight.Controls.Add(this.btnContinuar);
            this.pnlRight.Controls.Add(this.lnkOlvide);

            // Ensamble Form
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.ResumeLayout(false);
        }
    }
}
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public class Home_Invitado : Form
    {
        public Home_Invitado()
        {
            Text = "Inicio – Invitado";
            BackColor = Color.White;

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 80, 45),
                Text = "Panel de inicio (INVITADO)\nAcceso limitado a consultas"
            };
            Controls.Add(lbl);
        }
    }
}

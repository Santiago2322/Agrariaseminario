using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public class Home_Admin : Form
    {
        public Home_Admin()
        {
            Text = "Inicio – Administrador";
            BackColor = Color.White;
            Dock = DockStyle.Fill;

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 80, 45),
                Text = "Panel de inicio (ADMIN)\nAcceso total a todas las funciones"
            };
            Controls.Add(lbl);
        }
    }
}

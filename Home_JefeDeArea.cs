using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public class Home_JefeDeArea : Form
    {
        public Home_JefeDeArea()
        {
            Text = "Inicio – Jefe de área";
            BackColor = Color.White;

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 80, 45),
                Text = "Panel de inicio (JEFE DE ÁREA)\nOperativa, consultas y ventas"
            };
            Controls.Add(lbl);
        }
    }
}

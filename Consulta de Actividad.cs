using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Actividad : Form
    {
        public Consulta_de_Actividad()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Consulta de actividades realizada correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Cierra la ventana y regresa a la principal
        }
    }
}

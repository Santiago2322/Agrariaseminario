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
    public partial class Consulta_de_Ventas : Form
    {
        public Consulta_de_Ventas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Consulta de ventas realizada correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Se cierra y regresa a la pantalla principal
        }
    }
}

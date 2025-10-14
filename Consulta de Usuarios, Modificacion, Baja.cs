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
    public partial class Consulta_de_Usuarios__Modificacion__Baja : Form
    {
        public Consulta_de_Usuarios__Modificacion__Baja()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Cambios aplicados correctamente.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Cierra y regresa a la pantalla principal
            }
        }
    }
}

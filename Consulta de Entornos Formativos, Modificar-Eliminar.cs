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
    public partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar : Form
    {
        public Consulta_de_Entornos_Formativos__Modificar_Eliminar()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Cambios en entornos formativos aplicados correctamente.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Cierra la ventana y regresa a la pantalla principal
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class Inventario : Form
    {
        public Inventario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Inventario_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Inventario actualizado correctamente.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Cierra la ventana y vuelve a la pantalla principal
            }
        }
    }
}

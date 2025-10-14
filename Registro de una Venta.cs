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
    public partial class Registro_de_una_Venta : Form
    {
        public Registro_de_una_Venta()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Registro_de_una_Venta_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Venta registrada con éxito.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // vuelve a la pantalla principal
            }
        }
    }
}

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
    public partial class Alta_de_Entornos_Formativos : Form
    {
        public Alta_de_Entornos_Formativos()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Alta_de_Entornos_Formativos_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Entorno Formativo registrado con éxito.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Se cierra y regresa a la Pantalla Principal
            }

        }
    }
}

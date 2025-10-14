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
    public partial class Alta_de_usuarios : Form
    {
        public Alta_de_usuarios()
        {
            InitializeComponent();
        }

        private void Alta_de_usuarios_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("Usuario registrado con éxito.",
                                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // vuelve a la Pantalla Principal
            }
        }
    }
}

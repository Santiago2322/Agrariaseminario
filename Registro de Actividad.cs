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
    public partial class Registro_de_Actividad : Form
    {
        public Registro_de_Actividad()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Registro_de_Actividad_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Acá podés poner la lógica para guardar en la base de datos
            // Ejemplo: GuardarDatos();

            MessageBox.Show("Registro guardado correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cerrar esta ventana y volver a la Pantalla_Principal
            this.Close();
        }
        

        

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
       "¿Seguro que deseas cancelar? Los cambios no se guardarán.",
       "Confirmar cancelación",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question
   );

            if (resultado == DialogResult.Yes)
            {
                this.Close(); // vuelve a la Pantalla_Principal
            }
        }
    }
}

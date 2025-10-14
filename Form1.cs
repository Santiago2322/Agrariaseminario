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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Aquí podrías validar usuario y contraseña antes de abrir la pantalla principal
            // Ejemplo simple (personalízalo con tu lógica real):
            if (textBox1.Text == "admin" && textBox2.Text == "1234")
            {
                // Crear instancia de Pantalla_Principal
                Pantalla_Principal principal = new Pantalla_Principal();

                // Mostrar la pantalla principal
                this.Hide();
                principal.ShowDialog();
                this.Close();

                // Ocultar el login
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormOlvide_mi_Contraseña formRecuperar = new FormOlvide_mi_Contraseña();
            formRecuperar.ShowDialog(); // abre la ventana como modal
        }
    }
}

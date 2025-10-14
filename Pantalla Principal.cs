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
    public partial class Pantalla_Principal : Form
    {
        public Pantalla_Principal()
        {
            InitializeComponent();
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            {
                Registro_de_Actividad registro = new Registro_de_Actividad();
                registro.ShowDialog(); // Abre independiente
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            {
                Alta_de_usuarios altaUsuarios = new Alta_de_usuarios();
                altaUsuarios.ShowDialog(); // se abre como ventana modal
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            {
                Consulta_de_Usuarios__Modificacion__Baja consultaUsuarios = new Consulta_de_Usuarios__Modificacion__Baja();
                consultaUsuarios.ShowDialog(); // Se abre en ventana modal
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            Alta_de_Entornos_Formativos entornos = new Alta_de_Entornos_Formativos();
            entornos.ShowDialog(); // Se abre en una ventana moda
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            {
                Consulta_de_Entornos_Formativos__Modificar_Eliminar consultaEntornos = new Consulta_de_Entornos_Formativos__Modificar_Eliminar();
                consultaEntornos.ShowDialog(); // Se abre como ventana modal
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {
            {
                Alta_de_Entornos_Formativos entornos = new Alta_de_Entornos_Formativos();
                entornos.ShowDialog(); // Se abre en una ventana modal
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Consulta_de_Actividad consultaActividad = new Consulta_de_Actividad();
            consultaActividad.ShowDialog(); // Se abre como ventana modal
        }

        private void label11_Click(object sender, EventArgs e)
        {
            {
                Registro_de_Actividad registro = new Registro_de_Actividad();
                registro.ShowDialog(); // Abre independiente
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            {
                Registro_de_una_Venta venta = new Registro_de_una_Venta();
                venta.ShowDialog();  // se abre como ventana modal
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            Inventario inv = new Inventario();
            inv.ShowDialog(); // Se abre en ventana modal
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Seguro que deseas salir de la aplicación?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                Application.Exit(); // Cierra toda la aplicación
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {
            Consulta_de_Ventas consultaVentas = new Consulta_de_Ventas();
            consultaVentas.ShowDialog(); // Abre la ventana en modo modal
        }
    }
}

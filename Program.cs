using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mostrar el login
            using (var login = new Form1())
            {
                var dr = login.ShowDialog();

                // Si cancela o falla el login → salir
                if (dr != DialogResult.OK || !login.AuthSucceeded)
                    return;

                // Mostrar mensaje de bienvenida
                MessageBox.Show("Acceso concedido. Rol: " + login.RolSeleccionado,
                                "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir Pantalla_Principal, pasando el rol
                Application.Run(new Pantalla_Principal(login.RolSeleccionado));
            }
        }
    }
}

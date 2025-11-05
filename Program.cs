using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 🔧 Previene mensajes de depuración molestos (como “NonComVisibleBaseClass”)
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string rol = "usuario";

            // 🧠 Mostrar el formulario de Login primero
            using (var login = new Form1())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;

                rol = login.RolSeleccionado; // viene del SP de la DB
            }

            // 🚀 Luego abrir la pantalla principal según el rol
            Application.Run(new Pantalla_Principal(rol));
        }
    }
}

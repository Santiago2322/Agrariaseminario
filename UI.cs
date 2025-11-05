using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public static class UI
    {
        public static void Info(string msg, string titulo = "Información") =>
            MessageBox.Show(msg, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Error(string msg, string titulo = "Error") =>
            MessageBox.Show(msg, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static bool Confirm(string msg, string titulo = "Confirmar") =>
            MessageBox.Show(msg, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public static bool Required(TextBox txt, ErrorProvider ep, string message)
        {
            if (txt == null) return true;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                if (ep != null) ep.SetError(txt, message);
                txt.BackColor = Color.MistyRose;
                try { txt.Focus(); } catch { }
                return false;
            }
            if (ep != null) ep.SetError(txt, "");
            txt.BackColor = Color.White;
            return true;
        }
    }
}

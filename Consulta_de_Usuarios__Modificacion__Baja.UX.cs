using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Usuarios__Modificacion__Baja : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var grid = FindControl<DataGridView>("dataGridView1");
            if (grid != null)
            {
                grid.DataSource = Db.Table("SELECT UsuarioId, Apellido + ', ' + Nombre AS Nombre, Usuario, Perfil FROM Usuarios ORDER BY Apellido, Nombre");
            }
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }
    }
}

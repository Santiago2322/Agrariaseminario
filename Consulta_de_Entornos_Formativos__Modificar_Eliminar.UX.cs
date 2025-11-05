using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var grid = FindControl<DataGridView>("dataGridView1");
            if (grid != null)
            {
                grid.DataSource = Db.Table("SELECT EntornoId, Nombre, Tipo, Responsable, Anio, Division, Grupo FROM Entorno ORDER BY Nombre");
            }
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }
    }
}

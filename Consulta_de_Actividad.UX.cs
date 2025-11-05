using System;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Actividad : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var grid = FindControl<DataGridView>("dataGridView1");
            if (grid != null)
            {
                grid.DataSource = Db.Table("SELECT a.ActividadId, e.Nombre AS Entorno, a.Fecha, a.Descripcion FROM Actividad a JOIN Entorno e ON e.EntornoId=a.EntornoId ORDER BY a.ActividadId DESC");
            }
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }
    }
}

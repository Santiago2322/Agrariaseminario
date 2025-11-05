using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Ventas : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var grid = FindControl<DataGridView>("dataGridView1");
            if (grid != null)
            {
                grid.DataSource = Db.Table("SELECT v.VentaId, v.Fecha, v.Cliente, v.Total FROM Venta v ORDER BY v.VentaId DESC");
            }
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }
    }
}

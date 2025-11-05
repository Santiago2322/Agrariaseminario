using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_usuarios : Form
    {
        private void Guardar_Click(object sender, System.EventArgs e)
        {
            var n = FindControl<TextBox>("txtNombre");
            var a = FindControl<TextBox>("txtApellido");
            var u = FindControl<TextBox>("txtUsuario");
            var c = FindControl<TextBox>("txtClave");
            var p = FindControl<ComboBox>("cmbPerfil");

            if (n==null || a==null || u==null || c==null) { MessageBox.Show("Complete los campos."); return; }

            Db.Exec("INSERT INTO Usuarios(Nombre,Apellido,Usuario,Clave,Perfil) VALUES(@n,@a,@u,@c,@p)",
                new SqlParameter("@n", n.Text),
                new SqlParameter("@a", a.Text),
                new SqlParameter("@u", u.Text),
                new SqlParameter("@c", c.Text),
                new SqlParameter("@p", p!=null ? (object)p.Text : (object)"Usuario"));

            MessageBox.Show("Usuario guardado.");
            this.Close();
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }
    }
}

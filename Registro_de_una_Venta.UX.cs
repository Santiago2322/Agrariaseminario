using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Registro_de_una_Venta : Form
    {
        private readonly ErrorProvider ep = new ErrorProvider();

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                var dt = Db.Table("SELECT ProductoId, Nombre FROM Producto ORDER BY Nombre");
                var cb2 = FindControl<ComboBox>("comboBox2");
                var cb3 = FindControl<ComboBox>("comboBox3");
                if (cb2 != null) { cb2.DisplayMember = "Nombre"; cb2.ValueMember = "ProductoId"; cb2.DataSource = dt.Copy(); }
                if (cb3 != null) { cb3.DisplayMember = "Nombre"; cb3.ValueMember = "ProductoId"; cb3.DataSource = dt.Copy(); }
            }
            catch { }

            HookCalc("textBox3","textBox4","textBox5");
            HookCalc("textBox9","textBox8","textBox7");

            var btnGuardar = FindButtonByText("Guardar");
            if (btnGuardar != null) btnGuardar.Click += Guardar_Click;
        }

        private T FindControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            return arr.Length > 0 ? arr[0] as T : null;
        }

        private Button FindButtonByText(string text)
        {
            foreach (Control c in this.Controls)
            {
                var b = c as Button;
                if (b != null && string.Equals(b.Text, text, StringComparison.OrdinalIgnoreCase))
                    return b;
                var inner = FindButtonByTextRecursive(c, text);
                if (inner != null) return inner;
            }
            return null;
        }
        private Button FindButtonByTextRecursive(Control parent, string text)
        {
            foreach (Control c in parent.Controls)
            {
                var b = c as Button;
                if (b != null && string.Equals(b.Text, text, StringComparison.OrdinalIgnoreCase))
                    return b;
                var inner = FindButtonByTextRecursive(c, text);
                if (inner != null) return inner;
            }
            return null;
        }

        private decimal P(Control c)
        {
            var t = c as TextBox;
            decimal v; return (t != null && decimal.TryParse((t.Text ?? "0").Replace('.', ','), out v)) ? v : 0m;
        }

        private void HookCalc(string cantName, string precioName, string subName)
        {
            var cant = FindControl<TextBox>(cantName);
            var prec = FindControl<TextBox>(precioName);
            var sub  = FindControl<TextBox>(subName);
            if (cant == null || prec == null || sub == null) return;

            EventHandler recalc = delegate(object s, EventArgs e) {
                var subtotal = P(cant) * P(prec);
                sub.Text = subtotal.ToString("0.00");
                RecalcTotal();
            };
            cant.TextChanged += recalc;
            prec.TextChanged += recalc;
        }

        private void RecalcTotal()
        {
            var t1 = FindControl<TextBox>("textBox5");
            var t2 = FindControl<TextBox>("textBox7");
            var tot = FindControl<TextBox>("textBox6");
            decimal v = 0m;
            if (t1 != null) v += P(t1);
            if (t2 != null) v += P(t2);
            if (tot != null) tot.Text = v.ToString("0.00");
        }

        private bool Required(string name, string msg)
        {
            var txt = FindControl<TextBox>(name);
            if (txt == null) return true;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                ep.SetError(txt, msg);
                txt.BackColor = System.Drawing.Color.MistyRose;
                try { txt.Focus(); } catch { }
                return false;
            }
            ep.SetError(txt, ""); txt.BackColor = System.Drawing.Color.White; return true;
        }

        private bool Positive(string name, string msg)
        {
            var txt = FindControl<TextBox>(name);
            if (txt == null) return true;
            decimal v;
            if (!decimal.TryParse((txt.Text ?? "0").Replace('.', ','), out v) || v <= 0)
            {
                ep.SetError(txt, msg);
                txt.BackColor = System.Drawing.Color.MistyRose;
                try { txt.Focus(); } catch { }
                return false;
            }
            ep.SetError(txt, ""); txt.BackColor = System.Drawing.Color.White; return true;
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;
            try
            {
                Db.InTx(delegate(SqlConnection cn, SqlTransaction tx)
                {
                    var cmdV = new SqlCommand("INSERT INTO Venta(Fecha,Cliente,Total) OUTPUT INSERTED.VentaId VALUES(GETDATE(),@cli,@tot)", cn, tx);
                    var cli = FindControl<TextBox>("textBox2");
                    var tot = FindControl<TextBox>("textBox6");
                    cmdV.Parameters.AddWithValue("@cli", cli != null ? (object)cli.Text : (object)"");
                    decimal total = tot != null ? P(tot) : 0m;
                    cmdV.Parameters.AddWithValue("@tot", total);
                    int ventaId = (int)cmdV.ExecuteScalar();

                    var cb2 = FindControl<ComboBox>("comboBox2");
                    if (cb2 != null && cb2.SelectedValue != null)
                    {
                        var d1 = new SqlCommand("INSERT INTO VentaDetalle(VentaId,ProductoId,Cantidad,PrecioUnitario) VALUES(@v,@p,@c,@pu)", cn, tx);
                        d1.Parameters.AddWithValue("@v", ventaId);
                        d1.Parameters.AddWithValue("@p", cb2.SelectedValue);
                        d1.Parameters.AddWithValue("@c", P(FindControl<TextBox>("textBox3")));
                        d1.Parameters.AddWithValue("@pu", P(FindControl<TextBox>("textBox4")));
                        d1.ExecuteNonQuery();

                        var s1 = new SqlCommand("UPDATE Producto SET Stock=Stock-@c WHERE ProductoId=@p", cn, tx);
                        s1.Parameters.AddWithValue("@c", P(FindControl<TextBox>("textBox3")));
                        s1.Parameters.AddWithValue("@p", cb2.SelectedValue);
                        s1.ExecuteNonQuery();
                    }

                    var chk = FindControl<CheckBox>("checkBox1");
                    var cb3 = FindControl<ComboBox>("comboBox3");
                    if (chk != null && chk.Checked && cb3 != null && cb3.SelectedValue != null)
                    {
                        var d2 = new SqlCommand("INSERT INTO VentaDetalle(VentaId,ProductoId,Cantidad,PrecioUnitario) VALUES(@v,@p,@c,@pu)", cn, tx);
                        d2.Parameters.AddWithValue("@v", ventaId);
                        d2.Parameters.AddWithValue("@p", cb3.SelectedValue);
                        d2.Parameters.AddWithValue("@c", P(FindControl<TextBox]("textBox9")));
                        d2.Parameters.AddWithValue("@pu", P(FindControl<TextBox]("textBox8")));
                        d2.ExecuteNonQuery();

                        var s2 = new SqlCommand("UPDATE Producto SET Stock=Stock-@c WHERE ProductoId=@p", cn, tx);
                        s2.Parameters.AddWithValue("@c", P(FindControl<TextBox]("textBox9")));
                        s2.Parameters.AddWithValue("@p", cb3.SelectedValue);
                        s2.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("Venta registrada correctamente.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Validar()
        {
            bool ok = true;
            ok &= Required("textBox2", "Cliente requerido");
            ok &= Positive("textBox3", "Cantidad 1 > 0");
            ok &= Positive("textBox4", "Precio 1 > 0");

            var chk = FindControl<CheckBox>("checkBox1");
            if (chk != null && chk.Checked)
            {
                ok &= Positive("textBox9", "Cantidad 2 > 0");
                ok &= Positive("textBox8", "Precio 2 > 0");
            }
            return ok;
        }
    }
}

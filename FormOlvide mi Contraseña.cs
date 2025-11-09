using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class FormOlvide_mi_Contraseña : Form
    {
        // 🔗 MISMA CADENA que el resto del proyecto
        private const string CONN =
            @"Data Source=DESKTOP-92OCSA4;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public FormOlvide_mi_Contraseña()
        {
            InitializeComponent();
            this.Load += FormOlvide_mi_Contraseña_Load;

            txtUsuario.Leave += txtUsuario_Leave;
            btnRestablecer.Click += btnRestablecer_Click;

            // Ya cerraba por lambda, lo dejo y además agrego el método requerido por el designer.
            btnCancelar.Click += (s, e) => this.Close();
        }

        private void FormOlvide_mi_Contraseña_Load(object sender, EventArgs e)
        {
            try { EnsureColumns(); } catch { }

            if (cboPregunta.Items.Count == 0)
            {
                cboPregunta.Items.AddRange(new object[] {
                    "¿Cuál es el nombre de tu primera mascota?",
                    "¿En qué ciudad naciste?",
                    "¿Cuál fue tu primer colegio?",
                    "¿Cuál es el segundo nombre de tu madre?"
                });
            }
        }

        // 🔧 Garantiza que existan las columnas necesarias en dbo.Usuarios
        private void EnsureColumns()
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF COL_LENGTH('dbo.Usuarios','PreguntaSeguridad') IS NULL
    ALTER TABLE dbo.Usuarios ADD PreguntaSeguridad NVARCHAR(200) NULL;
IF COL_LENGTH('dbo.Usuarios','RespuestaSeguridad') IS NULL
    ALTER TABLE dbo.Usuarios ADD RespuestaSeguridad NVARCHAR(200) NULL;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            var usuario = txtUsuario.Text.Trim();
            if (string.IsNullOrEmpty(usuario)) return;

            string pregunta = null;
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT PreguntaSeguridad FROM dbo.Usuarios WHERE UsuarioLogin=@u";
                cmd.Parameters.AddWithValue("@u", usuario);
                cn.Open();
                var r = cmd.ExecuteScalar();
                pregunta = r == null || r == DBNull.Value ? null : Convert.ToString(r);
            }

            if (!string.IsNullOrWhiteSpace(pregunta))
            {
                cboPregunta.Items.Clear();
                cboPregunta.Items.Add(pregunta);
                cboPregunta.SelectedIndex = 0;
                cboPregunta.Enabled = false;
                lblInfoPregunta.Text = "Pregunta de seguridad registrada.";
            }
            else
            {
                if (cboPregunta.Items.Count == 0)
                {
                    cboPregunta.Items.AddRange(new object[] {
                        "¿Cuál es el nombre de tu primera mascota?",
                        "¿En qué ciudad naciste?",
                        "¿Cuál fue tu primer colegio?",
                        "¿Cuál es el segundo nombre de tu madre?"
                    });
                }
                cboPregunta.Enabled = true;
                cboPregunta.SelectedIndex = -1;
                lblInfoPregunta.Text = "Este usuario no tiene pregunta registrada.";
            }
        }

        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            { MessageBox.Show("Ingresá el usuario."); txtUsuario.Focus(); return; }
            if (string.IsNullOrWhiteSpace(txtNueva.Text) || string.IsNullOrWhiteSpace(txtConfirmar.Text))
            { MessageBox.Show("Ingresá y confirmá la nueva contraseña."); txtNueva.Focus(); return; }
            if (txtNueva.Text != txtConfirmar.Text)
            { MessageBox.Show("Las contraseñas no coinciden."); txtConfirmar.Focus(); return; }
            if (string.IsNullOrWhiteSpace(cboPregunta.Text))
            { MessageBox.Show("Seleccioná la pregunta de seguridad."); cboPregunta.Focus(); return; }
            if (string.IsNullOrWhiteSpace(txtRespuesta.Text))
            { MessageBox.Show("Ingresá la respuesta de seguridad."); txtRespuesta.Focus(); return; }

            string preguntaDb = null, respuestaDb = null;
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT PreguntaSeguridad, RespuestaSeguridad
                                    FROM dbo.Usuarios WHERE UsuarioLogin=@u";
                cmd.Parameters.AddWithValue("@u", txtUsuario.Text.Trim());
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) { MessageBox.Show("Usuario no encontrado."); return; }
                    preguntaDb = rd["PreguntaSeguridad"] as string;
                    respuestaDb = rd["RespuestaSeguridad"] as string;
                }
            }

            // Validación de pregunta y respuesta
            if (!string.IsNullOrWhiteSpace(preguntaDb))
            {
                if (!string.Equals(preguntaDb?.Trim(), cboPregunta.Text.Trim(), StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(respuestaDb?.Trim(), txtRespuesta.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Pregunta o respuesta incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("El usuario no tiene pregunta/respuesta registrada. Contacte al administrador.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Actualizar contraseña
            int filas = 0;
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE dbo.Usuarios SET Contrasenia=@p WHERE UsuarioLogin=@u";
                cmd.Parameters.AddWithValue("@p", txtNueva.Text);
                cmd.Parameters.AddWithValue("@u", txtUsuario.Text.Trim());
                cn.Open();
                filas = cmd.ExecuteNonQuery();
            }

            if (filas > 0)
            {
                MessageBox.Show("Contraseña actualizada correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Agregado para satisfacer la referencia del Designer
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

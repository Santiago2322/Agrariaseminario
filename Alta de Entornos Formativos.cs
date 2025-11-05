using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_Entornos_Formativos : Form
    {
        // 🔗 MISMA CADENA QUE EL RESTO DEL PROYECTO
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Alta_de_Entornos_Formativos()
        {
            InitializeComponent();
            this.Load += Alta_de_Entornos_Formativos_Load;
            // Asegurá en el Designer: button1.Click -> button1_Click
        }

        private void Alta_de_Entornos_Formativos_Load(object sender, EventArgs e)
        {
            // Tipos de entorno predefinidos
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Productivo");
            comboBox1.Items.Add("Tecnológico");
            comboBox1.Items.Add("Didáctico");
            comboBox1.Items.Add("Otro");
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            try
            {
                EnsureTablaEntornos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error preparando esquema: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea dbo.EntornosFormativos si no existe (no crea BD).
        /// </summary>
        private void EnsureTablaEntornos()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(120) NOT NULL,
        Tipo          NVARCHAR(80)  NOT NULL,
        Profesor      NVARCHAR(120) NOT NULL,
        Anio          NVARCHAR(20)  NOT NULL,
        Division      NVARCHAR(20)  NOT NULL,
        Grupo         NVARCHAR(40)  NOT NULL,
        Observaciones NVARCHAR(300) NULL
    );
END";
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Debe completar al menos el Nombre del entorno y el Tipo.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string SQL = @"
INSERT INTO dbo.EntornosFormativos
    (Nombre, Tipo, Profesor, Anio, Division, Grupo, Observaciones)
VALUES
    (@n, @t, @p, @a, @d, @g, @o);";

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@n", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@t", comboBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@a", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@d", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@g", string.IsNullOrWhiteSpace(textBox5.Text) ? (object)DBNull.Value : textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@o", string.IsNullOrWhiteSpace(textBox6.Text) ? (object)DBNull.Value : textBox6.Text.Trim());

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("✅ Entorno Formativo registrado con éxito.",
                        "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar entorno:\n" + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Si el Designer generó este handler, lo dejamos vacío
        private void label5_Click(object sender, EventArgs e) { }
    }
}

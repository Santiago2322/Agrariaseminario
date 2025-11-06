using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_Entornos_Formativos : Form
    {
        private const string CADENA =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        public Alta_de_Entornos_Formativos()
        {
            InitializeComponent();
            this.Load += Alta_de_Entornos_Formativos_Load;
            // botón guardar lo cablea el Designer (button1.Click)
        }

        private static object NV(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();
        }

        private void Alta_de_Entornos_Formativos_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Industria – Dulces y Conservas");
            comboBox1.Items.Add("Industria – Lácteos");
            comboBox1.Items.Add("Huerta");
            comboBox1.Items.Add("Monte frutal");
            comboBox1.Items.Add("Vivero");
            comboBox1.Items.Add("Pañol");
            comboBox1.Items.Add("Cunicultura");
            comboBox1.Items.Add("Monte frutal – Donaciones");
            comboBox1.Items.Add("Animal – Cunicultura");
            comboBox1.Items.Add("Animal – Porcino");
            comboBox1.Items.Add("Animal – Apicultura");
            comboBox1.Items.Add("Animal – Parrillero");
            comboBox1.Items.Add("Animal – Ponedora");
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            try { EnsureTablaEntornos(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error preparando esquema: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureTablaEntornos()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre         NVARCHAR(120) NOT NULL,
        Tipo           NVARCHAR(80)  NOT NULL,
        Profesor       NVARCHAR(120) NULL,
        Anio           NVARCHAR(20)  NULL,
        Division       NVARCHAR(20)  NULL,
        Grupo          NVARCHAR(40)  NULL,
        Observaciones  NVARCHAR(300) NULL
    );
END";
            using (var cn = new SqlConnection(CADENA))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Guardar
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Complete al menos el Nombre del entorno y el Tipo.",
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
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 120).Value = NV(textBox2.Text);
                cmd.Parameters.Add("@a", SqlDbType.NVarChar, 20).Value = NV(textBox4.Text);
                cmd.Parameters.Add("@d", SqlDbType.NVarChar, 20).Value = NV(textBox3.Text);
                cmd.Parameters.Add("@g", SqlDbType.NVarChar, 40).Value = NV(textBox5.Text);
                cmd.Parameters.Add("@o", SqlDbType.NVarChar, 300).Value = NV(textBox6.Text);

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

        private void label5_Click(object sender, EventArgs e) { }
    }
}
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_Entornos_Formativos : Form
    {
        private const string CADENA =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        private const string CADENA_MASTER =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";

        public Alta_de_Entornos_Formativos()
        {
            InitializeComponent();
        }

        private void Alta_de_Entornos_Formativos_Load(object sender, EventArgs e)
        {
            // Cargar tipos de entornos predefinidos en el ComboBox
            comboBox1.Items.Add("Productivo");
            comboBox1.Items.Add("Tecnológico");
            comboBox1.Items.Add("Didáctico");
            comboBox1.Items.Add("Otro");

            try
            {
                EnsureSchema();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar base: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureSchema()
        {
            using (var cn = new SqlConnection(CADENA_MASTER))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "IF DB_ID(N'agraria_basedatos') IS NULL CREATE DATABASE agraria_basedatos;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(120) NOT NULL,
        Tipo NVARCHAR(80) NOT NULL,
        Profesor NVARCHAR(120) NOT NULL,
        Anio NVARCHAR(20) NOT NULL,
        Division NVARCHAR(20) NOT NULL,
        Grupo NVARCHAR(40) NOT NULL,
        Observaciones NVARCHAR(300) NULL
    );
END";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Debe completar al menos el nombre y el tipo.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
INSERT INTO EntornosFormativos 
(Nombre, Tipo, Profesor, Anio, Division, Grupo, Observaciones)
VALUES (@n, @t, @p, @a, @d, @g, @o)";
                cmd.Parameters.AddWithValue("@n", textBox1.Text);
                cmd.Parameters.AddWithValue("@t", comboBox1.Text);
                cmd.Parameters.AddWithValue("@p", textBox2.Text);
                cmd.Parameters.AddWithValue("@a", textBox4.Text);
                cmd.Parameters.AddWithValue("@d", textBox3.Text);
                cmd.Parameters.AddWithValue("@g", textBox5.Text);
                cmd.Parameters.AddWithValue("@o", textBox6.Text);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Entorno Formativo registrado con éxito.",
                        "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar entorno: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e) { }
    }
}

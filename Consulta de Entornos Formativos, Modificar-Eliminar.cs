using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Consulta_de_Entornos_Formativos__Modificar_Eliminar : Form
    {
        private const string CADENA =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        private const string CADENA_MASTER =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";

        public Consulta_de_Entornos_Formativos__Modificar_Eliminar()
        {
            InitializeComponent();
        }

        private void Consulta_de_Entornos_Formativos__Modificar_Eliminar_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureSchema();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message,
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
        Grupo NVARCHAR(40) NOT NULL
    );

    INSERT INTO dbo.EntornosFormativos (Nombre, Tipo, Profesor, Anio, Division, Grupo)
    VALUES
    (N'Huerta', N'Productivo', N'Prof. García', N'2025', N'5ºA', N'1'),
    (N'Laboratorio', N'Tecnológico', N'Prof. López', N'2025', N'5ºB', N'2');
END;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarDatos()
        {
            using (var cn = new SqlConnection(CADENA))
            using (var da = new SqlDataAdapter("SELECT * FROM EntornosFormativos", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Tipo"].Value.ToString();
                textBox3.Text = fila.Cells["Profesor"].Value.ToString();
                textBox4.Text = fila.Cells["Anio"].Value.ToString();
                textBox6.Text = fila.Cells["Division"].Value.ToString();
                textBox5.Text = fila.Cells["Grupo"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Modificar
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE EntornosFormativos
                                    SET Nombre=@n, Tipo=@t, Profesor=@p, Anio=@a, Division=@d, Grupo=@g
                                    WHERE Id=@id";
                cmd.Parameters.AddWithValue("@n", textBox1.Text);
                cmd.Parameters.AddWithValue("@t", textBox2.Text);
                cmd.Parameters.AddWithValue("@p", textBox3.Text);
                cmd.Parameters.AddWithValue("@a", textBox4.Text);
                cmd.Parameters.AddWithValue("@d", textBox6.Text);
                cmd.Parameters.AddWithValue("@g", textBox5.Text);
                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Registro modificado correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e) // Eliminar
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("¿Eliminar este entorno?", "Confirmar", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var cn = new SqlConnection(CADENA))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM EntornosFormativos WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                CargarDatos();
                MessageBox.Show("Registro eliminado correctamente.",
                    "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Guardar nuevos cambios
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;

            using (var cn = new SqlConnection(CADENA))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO EntornosFormativos
                                    (Nombre, Tipo, Profesor, Anio, Division, Grupo)
                                    VALUES (@n, @t, @p, @a, @d, @g)";
                cmd.Parameters.AddWithValue("@n", textBox1.Text);
                cmd.Parameters.AddWithValue("@t", textBox2.Text);
                cmd.Parameters.AddWithValue("@p", textBox3.Text);
                cmd.Parameters.AddWithValue("@a", textBox4.Text);
                cmd.Parameters.AddWithValue("@d", textBox6.Text);
                cmd.Parameters.AddWithValue("@g", textBox5.Text);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarDatos();
            MessageBox.Show("Nuevo entorno agregado.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cambios en entornos formativos aplicados correctamente.",
                            "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }
    }
}

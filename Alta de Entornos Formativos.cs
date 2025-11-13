using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Alta_de_Entornos_Formativos : Form
    {
        private const string CADENA =
              @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";

        // ---- Límites de longitudes (suaves) ----
        private const int MAX_NOMBRE = 120;
        private const int MAX_TIPO = 80;
        private const int MAX_PROFESOR = 120;
        private const int MAX_ANIO = 20;
        private const int MAX_DIVISION = 20;
        private const int MAX_GRUPO = 40;
        private const int MAX_OBS = 300;

        // ErrorProvider para marcar campos
        private readonly ErrorProvider ep = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

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
            // Sugerencias de "Tipo"
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

            // MaxLength (si los TextBox vienen del diseñador con nombres textBox1..6)
            SafeSetMaxLength(textBox1, MAX_NOMBRE);     // Nombre
            SafeSetMaxLength(textBox2, MAX_PROFESOR);   // Profesor
            SafeSetMaxLength(textBox4, MAX_ANIO);       // Año
            SafeSetMaxLength(textBox3, MAX_DIVISION);   // División
            SafeSetMaxLength(textBox5, MAX_GRUPO);      // Grupo
            SafeSetMaxLength(textBox6, MAX_OBS);        // Observaciones

            // Validación reactiva simple (limpia errores al escribir/cambiar)
            WireReactiveValidation();

            try { EnsureTablaEntornos(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error preparando esquema: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SafeSetMaxLength(TextBox tb, int max)
        {
            if (tb != null) tb.MaxLength = max;
        }

        private void WireReactiveValidation()
        {
            void onChange(object s, EventArgs e) => ValidarFormulario(false);

            if (textBox1 != null) textBox1.TextChanged += onChange; // Nombre
            if (comboBox1 != null) comboBox1.SelectedIndexChanged += onChange; // Tipo
            if (textBox2 != null) textBox2.TextChanged += onChange; // Profesor
            if (textBox4 != null) textBox4.TextChanged += onChange; // Año
            if (textBox3 != null) textBox3.TextChanged += onChange; // División
            if (textBox5 != null) textBox5.TextChanged += onChange; // Grupo
            if (textBox6 != null) textBox6.TextChanged += onChange; // Observaciones

            // Opcional: restringir "Año" a dígitos y guiones (ej: "2025", "3ro", etc. lo dejo libre por si usás "3°")
            // Si querés solo dígitos, descomentá:
            // if (textBox4 != null) textBox4.KeyPress += (s, e) =>
            // {
            //     if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            // };
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

        // =============== VALIDACIÓN CENTRAL =================
        // Devuelve true si todo OK. Si showMessages = true, muestra aviso y enfoca el primer error.
        private bool ValidarFormulario(bool showMessages)
        {
            // limpiar errores previos
            ep.SetError(textBox1, "");
            ep.SetError(comboBox1, "");
            ep.SetError(textBox2, "");
            ep.SetError(textBox4, "");
            ep.SetError(textBox3, "");
            ep.SetError(textBox5, "");
            ep.SetError(textBox6, "");

            Control primerError = null;
            void Err(Control c, string msg)
            {
                ep.SetError(c, msg);
                if (primerError == null) primerError = c;
            }

            // Requeridos (no vacíos ni solo espacios)
            if (IsBlank(textBox1)) Err(textBox1, "Requerido.");                       // Nombre
            if (comboBox1 == null || string.IsNullOrWhiteSpace(comboBox1.Text))       // Tipo
                Err(comboBox1, "Requerido.");
            if (IsBlank(textBox2)) Err(textBox2, "Requerido.");                       // Profesor
            if (IsBlank(textBox4)) Err(textBox4, "Requerido.");                       // Año
            if (IsBlank(textBox3)) Err(textBox3, "Requerido.");                       // División
            if (IsBlank(textBox5)) Err(textBox5, "Requerido.");                       // Grupo
            if (IsBlank(textBox6)) Err(textBox6, "Requerido.");                       // Observaciones

            bool ok = (primerError == null);

            if (!ok && showMessages)
            {
                MessageBox.Show("Revisá los campos marcados.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try { primerError?.Focus(); } catch { /* ignore */ }
            }

            return ok;
        }

        private bool IsBlank(TextBox tb)
        {
            return tb == null || string.IsNullOrWhiteSpace(tb.Text);
        }

        // =============== GUARDAR =================
        private void button1_Click(object sender, EventArgs e) // Guardar
        {
            // Validación de todos los campos obligatorios
            if (!ValidarFormulario(true)) return;

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

        // (Si lo usabas para cerrar con clic, lo dejo vacío para no romper)
        private void label5_Click(object sender, EventArgs e) { }
    }
}

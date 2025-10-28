using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Form1 : Form
    {
        private const string CADENA_CONEXION =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=agraria_basedatos;Integrated Security=True;TrustServerCertificate=True;";

        private const string CADENA_MASTER =
            @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();
            // (opcional) ocultá el password si no lo hiciste en el designer
            try { textBox2.UseSystemPasswordChar = true; } catch { /* ignore */ }

            try
            {
                EnsureLocalDbUp("MSSQLLocalDB");
                InitializeDatabase();
                EnsureAdminUserUpsert();   // <- crea o corrige admin
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al preparar la base de datos:\n" + ex.Message,
                    "Error de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- LocalDB helper ----------
        private void EnsureLocalDbUp(string instanceName)
        {
            bool Run(string args)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "sqllocaldb",
                        Arguments = args,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    using (var p = Process.Start(psi))
                    {
                        p.WaitForExit(8000);
                        return p.ExitCode == 0;
                    }
                }
                catch { return false; }
            }

            if (!Run("i " + instanceName)) // info -> si no existe, creamos
            {
                if (!Run("create " + instanceName))
                    throw new Exception("No se pudo crear LocalDB. Instalar SQL Server Express LocalDB.");
            }
            // arrancar
            if (!Run("start " + instanceName))
            {
                Run("stop " + instanceName);
                Run("delete " + instanceName);
                if (!Run("create " + instanceName) || !Run("start " + instanceName))
                    throw new Exception("No se pudo iniciar LocalDB.");
            }
        }

        // ---------- Crear BD/tabla si faltan ----------
        private void InitializeDatabase()
        {
            using (var cn = new SqlConnection(CADENA_MASTER))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "IF DB_ID(N'agraria_basedatos') IS NULL CREATE DATABASE agraria_basedatos;";
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        IdUsuario     INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(80)  NOT NULL,
        Apellido      NVARCHAR(80)  NOT NULL,
        DNI           VARCHAR(12)   NOT NULL UNIQUE,
        Email         NVARCHAR(120) NOT NULL,
        Telefono      NVARCHAR(40)  NULL,
        UsuarioLogin  NVARCHAR(60)  NOT NULL UNIQUE,
        Contrasenia   NVARCHAR(200) NOT NULL,
        Direccion     NVARCHAR(150) NULL,
        Localidad     NVARCHAR(80)  NULL,
        Provincia     NVARCHAR(80)  NULL,
        Observaciones NVARCHAR(300) NULL,
        Rol           NVARCHAR(40)  NULL,
        Estado        NVARCHAR(20)  NULL,
        Area          NVARCHAR(60)  NULL,
        FechaAlta     DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaAlta DEFAULT SYSUTCDATETIME()
    );
END";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ---------- Asegurar/Corregir usuario admin ----------
        private void EnsureAdminUserUpsert()
        {
            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'admin')
BEGIN
    INSERT INTO dbo.Usuarios
      (Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin, Contrasenia, Rol, Estado, Area, Observaciones)
    VALUES
      (N'Administrador', N'Sistema', '99999999', N'admin@demo.com', N'555-0000',
       N'admin', N'1234', N'Administrador', N'Activo', N'Administración', N'Usuario inicial (auto)')
END
ELSE
BEGIN
    UPDATE dbo.Usuarios
       SET Contrasenia = N'1234',
           Estado      = N'Activo',
           Rol         = N'Administrador'
     WHERE UsuarioLogin = N'admin';
END";
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ---------- UI vacía por ahora ----------
        private void label2_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        // ---------- Login ----------
        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = (textBox1.Text ?? "").Trim();
            string pass = (textBox2.Text ?? "");

            if (usuario.Length == 0 || pass.Length == 0)
            {
                MessageBox.Show("Debe ingresar usuario y contraseña.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rolUsuario = null;
            string sql = @"
SELECT Rol
FROM dbo.Usuarios
WHERE LTRIM(RTRIM(UsuarioLogin)) = LTRIM(RTRIM(@u))
  AND Contrasenia = @p
  AND Estado = N'Activo';";

            using (var cn = new SqlConnection(CADENA_CONEXION))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@p", pass);

                try
                {
                    cn.Open();
                    object o = cmd.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                        rolUsuario = Convert.ToString(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error de Conexión en Login:\n" + ex.Message,
                        "Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(rolUsuario))
            {
                // Login OK
                // Si tu constructor de Pantalla_Principal recibe rol:
                try
                {
                    var principal = new Pantalla_Principal(rolUsuario);
                    this.Hide();
                    principal.ShowDialog();
                }
                finally
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos, o el usuario no está activo.",
                                "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Implementá si usás recuperación de contraseña
        }
    }
}

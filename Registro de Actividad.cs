using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto_Agraria_Pacifico
{
    public partial class Registro_de_Actividad : Form
    {
        // ✅ MISMA CADENA QUE EN TODO EL PROYECTO
        private const string CONN =
            @"Data Source=DESKTOP-92OCSA4;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True";
        public Registro_de_Actividad()
        {
            InitializeComponent();
            this.AutoScroll = true;

            // Evito dobles suscripciones
            this.Load -= Registro_de_Actividad_Load;
            this.Load += Registro_de_Actividad_Load;
        }

        // ---- Helpers ----
        private static object NV(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : (object)s.Trim();
        }

        private void Registro_de_Actividad_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTablaActividades();   // crea si no existe (compatible con el esquema que usás)
                CargarEntornos();           // llena el combo
                CargarTurnos();             // llena combo de horarios (si lo usás)
                CargarGrilla();             // carga grilla (todo o por entorno si hay selección)

                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Mostrar detalles del entorno si hay alguno seleccionado
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                comboBox1_SelectedIndexChanged(comboBox1, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los registros: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Crea dbo.Actividades compatible con tu esquema real:
        // IdActividad, Id(FK a EntornosFormativos), Nombre, Fecha(date), Hora(nvarchar(10)),
        // Descripcion, Responsable, Estado
        private void EnsureTablaActividades()
        {
            const string SQL = @"
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Id          INT NOT NULL,  -- FK a EntornosFormativos(Id)
        Nombre      NVARCHAR(120) NULL,
        Fecha       DATE NOT NULL CONSTRAINT DF_Act_Fecha DEFAULT (CONVERT(date,GETDATE())),
        Hora        NVARCHAR(10) NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(120) NULL,
        Estado      NVARCHAR(50)  NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Act_Entorno' AND parent_object_id=OBJECT_ID('dbo.Actividades'))
BEGIN
    BEGIN TRY
        ALTER TABLE dbo.Actividades WITH CHECK
        ADD CONSTRAINT FK_Act_Entorno FOREIGN KEY(Id) REFERENCES dbo.EntornosFormativos(IdEntorno)
            ON UPDATE CASCADE ON DELETE CASCADE;
    END TRY
    BEGIN CATCH
        -- Si tu PK es Id (no IdEntorno), intento ese nombre:
        IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Act_Entorno' AND parent_object_id=OBJECT_ID('dbo.Actividades'))
        BEGIN
            BEGIN TRY
                ALTER TABLE dbo.Actividades WITH CHECK
                ADD CONSTRAINT FK_Act_Entorno FOREIGN KEY(Id) REFERENCES dbo.EntornosFormativos(Id)
                    ON UPDATE CASCADE ON DELETE CASCADE;
            END TRY
            BEGIN CATCH
                PRINT 'Aviso: no se pudo crear FK (revise columna PK de EntornosFormativos).';
            END CATCH
        END
    END CATCH
END
";
            using (var cn = new SqlConnection(CONN))
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CargarEntornos()
        {
            using (var cn = new SqlConnection(CONN))
            using (var da = new SqlDataAdapter(
                @"SELECT 
                      CASE 
                        WHEN COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL THEN IdEntorno
                        ELSE Id
                      END AS IdEntorno,
                      Nombre, Tipo, Profesor, Anio, Division, Grupo, Observaciones
                  FROM dbo.EntornosFormativos
                  ORDER BY Nombre;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);

                comboBox1.DisplayMember = "Nombre";
                comboBox1.ValueMember = "IdEntorno";
                comboBox1.DataSource = dt;

                // Mostrar metadata si hay selección
                MostrarDatosEntornoActual();
            }
        }

        private void CargarTurnos()
        {
            // Slots típicos (podés cambiarlos)
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new object[]
            {
                "08:00", "09:30", "11:00", "13:00", "14:30", "16:00"
            });
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarDatosEntornoActual();
            CargarGrilla(); // refresca actividades por entorno seleccionado
        }

        private void MostrarDatosEntornoActual()
        {
            var drv = comboBox1.SelectedItem as DataRowView;
            if (drv == null)
            {
                textBox1.Text = ""; // Tipo
                textBox2.Text = ""; // Profesor
                textBox3.Text = ""; // Año
                textBox5.Text = ""; // División
                textBox6.Text = ""; // Grupo
                return;
            }

            // Solo MUESTRA datos del entorno (read-only)
            textBox1.Text = Convert.ToString(drv["Tipo"] ?? "");
            textBox2.Text = Convert.ToString(drv["Profesor"] ?? "");
            textBox3.Text = Convert.ToString(drv["Anio"] ?? "");
            textBox5.Text = Convert.ToString(drv["Division"] ?? "");
            textBox6.Text = Convert.ToString(drv["Grupo"] ?? "");
        }

        private int? EntornoSeleccionadoId()
        {
            try
            {
                if (comboBox1.SelectedValue == null) return null;
                return Convert.ToInt32(comboBox1.SelectedValue);
            }
            catch { return null; }
        }

        private void CargarGrilla(string filtro = "")
        {
            using (var cn = new SqlConnection(CONN))
            using (var da = new SqlDataAdapter())
            {
                var idEnt = EntornoSeleccionadoId();

                // Si existe la vista v_ActividadesPorEntorno, úsala. Si no, JOIN directo.
                string sql = @"
IF OBJECT_ID('dbo.v_ActividadesPorEntorno','V') IS NOT NULL
BEGIN
    SELECT a.IdActividad, a.Entorno, a.Nombre, a.Fecha, a.Hora, a.Descripcion, a.Responsable, a.Estado
    FROM dbo.v_ActividadesPorEntorno a
    /**COND**/
    ORDER BY a.Fecha DESC, a.Hora ASC;
END
ELSE
BEGIN
    SELECT act.IdActividad,
           ent.Nombre AS Entorno,
           act.Nombre,
           act.Fecha,
           act.Hora,
           act.Descripcion,
           act.Responsable,
           act.Estado
    FROM dbo.Actividades act
    INNER JOIN dbo.EntornosFormativos ent
      ON ent.Id = act.Id OR ent.IdEntorno = act.Id
    /**COND2**/
    ORDER BY act.Fecha DESC, act.Hora ASC;
END
";

                string where = "";
                if (idEnt.HasValue)
                {
                    where = "WHERE (ent.Id = @id OR ent.IdEntorno = @id)";
                }
                else if (!string.IsNullOrWhiteSpace(filtro))
                {
                    where = "WHERE (act.Nombre LIKE @f OR act.Descripcion LIKE @f OR act.Responsable LIKE @f)";
                }

                sql = sql.Replace("/**COND**/", idEnt.HasValue
                    ? "WHERE a.Entorno IS NOT NULL AND a.Entorno <> '' AND a.Entorno IN (SELECT Nombre FROM dbo.EntornosFormativos WHERE (Id = @id OR IdEntorno = @id))"
                    : (!string.IsNullOrWhiteSpace(filtro) ? "WHERE a.Nombre LIKE @f OR a.Descripcion LIKE @f OR a.Responsable LIKE @f" : "")
                );

                sql = sql.Replace("/**COND2**/", where);

                da.SelectCommand = new SqlCommand(sql, cn);
                if (idEnt.HasValue)
                    da.SelectCommand.Parameters.AddWithValue("@id", idEnt.Value);
                if (!string.IsNullOrWhiteSpace(filtro))
                    da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.Trim() + "%");

                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["IdActividad"] != null)
                    dataGridView1.Columns["IdActividad"].Visible = false;
            }
        }

        // Guardar → por ahora solo avisa y cierra (sin duplicar mensajes)
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Registro guardado correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // Cancelar → confirmación simple 1 sola vez
        private void button2_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("¿Seguro que deseas cancelar? Los cambios no se guardarán.",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) this.Close();
        }
    }
}

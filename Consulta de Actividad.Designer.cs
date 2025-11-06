namespace Proyecto_Agraria_Pacifico
{
    partial class Consulta_de_Actividad
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            // Fuentes “grandes y legibles”
            System.Drawing.Font fuenteTitulo = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            System.Drawing.Font fuenteLabel = new System.Drawing.Font("Segoe UI", 11F);
            System.Drawing.Font fuenteCtrl = new System.Drawing.Font("Segoe UI", 11F);
            System.Drawing.Font fuenteBtn = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBoxFiltro = new System.Windows.Forms.TextBox();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.textBoxResponsable = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonModificar = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.buttonCerrar = new System.Windows.Forms.Button();
            this.buttonNuevo = new System.Windows.Forms.Button(); // NUEVO
            this.buttonGuardar = new System.Windows.Forms.Button(); // NUEVO

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // ===== FORM =====
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta de Actividad";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(780, 540);   // más grande

            // ===== BUSCAR =====
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Size = new System.Drawing.Size(70, 28);
            this.label1.Text = "Buscar:";
            this.label1.Font = fuenteLabel;

            this.textBoxFiltro.Location = new System.Drawing.Point(95, 16);
            this.textBoxFiltro.Size = new System.Drawing.Size(380, 28);
            this.textBoxFiltro.Font = fuenteCtrl;

            this.buttonBuscar.Location = new System.Drawing.Point(485, 14);
            this.buttonBuscar.Size = new System.Drawing.Size(110, 32);
            this.buttonBuscar.Text = "Buscar";
            this.buttonBuscar.Font = fuenteBtn;
            this.buttonBuscar.UseVisualStyleBackColor = true;
            this.buttonBuscar.Click += new System.EventHandler(this.buttonBuscar_Click);

            // ===== GRID =====
            this.dataGridView1.Location = new System.Drawing.Point(20, 60);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(736, 250); // un poco más alto/ancho
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Font = fuenteCtrl;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Font = fuenteBtn };
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);

            // ===== CAMPOS (más grandes) =====
            int xLbl = 20, xTxt = 140, sepY = 38;
            int yBase = 330;

            // Nombre
            this.label2.Location = new System.Drawing.Point(xLbl, yBase);
            this.label2.Size = new System.Drawing.Size(110, 28);
            this.label2.Text = "Nombre:";
            this.label2.Font = fuenteLabel;

            this.textBoxNombre.Location = new System.Drawing.Point(xTxt, yBase - 2);
            this.textBoxNombre.Size = new System.Drawing.Size(320, 28);
            this.textBoxNombre.Font = fuenteCtrl;

            // Descripción
            this.label3.Location = new System.Drawing.Point(xLbl, yBase + sepY);
            this.label3.Size = new System.Drawing.Size(110, 28);
            this.label3.Text = "Descripción:";
            this.label3.Font = fuenteLabel;

            this.textBoxDescripcion.Location = new System.Drawing.Point(xTxt, yBase + sepY - 2);
            this.textBoxDescripcion.Size = new System.Drawing.Size(320, 28);
            this.textBoxDescripcion.Font = fuenteCtrl;

            // Responsable
            this.label4.Location = new System.Drawing.Point(xLbl, yBase + 2 * sepY);
            this.label4.Size = new System.Drawing.Size(110, 28);
            this.label4.Text = "Responsable:";
            this.label4.Font = fuenteLabel;

            this.textBoxResponsable.Location = new System.Drawing.Point(xTxt, yBase + 2 * sepY - 2);
            this.textBoxResponsable.Size = new System.Drawing.Size(320, 28);
            this.textBoxResponsable.Font = fuenteCtrl;

            // ===== BOTONES (más grandes) =====
            // Columna izquierda (Modificar/Eliminar/Cerrar)
            this.buttonModificar.Location = new System.Drawing.Point(485, yBase - 2);
            this.buttonModificar.Size = new System.Drawing.Size(120, 34);
            this.buttonModificar.Text = "Modificar";
            this.buttonModificar.Font = fuenteBtn;
            this.buttonModificar.UseVisualStyleBackColor = true;
            this.buttonModificar.Click += new System.EventHandler(this.buttonModificar_Click);

            this.buttonEliminar.Location = new System.Drawing.Point(485, yBase + sepY - 2);
            this.buttonEliminar.Size = new System.Drawing.Size(120, 34);
            this.buttonEliminar.Text = "Eliminar";
            this.buttonEliminar.Font = fuenteBtn;
            this.buttonEliminar.UseVisualStyleBackColor = true;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);

            this.buttonCerrar.Location = new System.Drawing.Point(485, yBase + 2 * sepY - 2);
            this.buttonCerrar.Size = new System.Drawing.Size(120, 34);
            this.buttonCerrar.Text = "Cerrar";
            this.buttonCerrar.Font = fuenteBtn;
            this.buttonCerrar.UseVisualStyleBackColor = true;
            this.buttonCerrar.Click += new System.EventHandler(this.buttonCerrar_Click);

            // Columna derecha (Nuevo/Guardar)
            this.buttonNuevo.Location = new System.Drawing.Point(625, yBase - 2);
            this.buttonNuevo.Size = new System.Drawing.Size(130, 34);
            this.buttonNuevo.Text = "Nuevo";
            this.buttonNuevo.Font = fuenteBtn;
            this.buttonNuevo.UseVisualStyleBackColor = true;
            this.buttonNuevo.Click += new System.EventHandler(this.buttonNuevo_Click);

            this.buttonGuardar.Location = new System.Drawing.Point(625, yBase + sepY - 2);
            this.buttonGuardar.Size = new System.Drawing.Size(130, 34);
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.Font = fuenteBtn;
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);

            // ===== ADD CONTROLS =====
            this.Controls.Add(this.buttonGuardar);
            this.Controls.Add(this.buttonNuevo);
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.buttonModificar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxResponsable);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.textBoxNombre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonBuscar);
            this.Controls.Add(this.textBoxFiltro);
            this.Controls.Add(this.dataGridView1);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBoxFiltro;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.TextBox textBoxResponsable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonModificar;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.Button buttonCerrar;

        // NUEVOS:
        private System.Windows.Forms.Button buttonNuevo;
        private System.Windows.Forms.Button buttonGuardar;
    }
}

namespace Jump
{
    partial class frmCrearPlanos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnContraer = new System.Windows.Forms.Button();
            this.btnExpandir = new System.Windows.Forms.Button();
            this.btnSeleccionarNinguno = new System.Windows.Forms.Button();
            this.btnSeleccionarTodos = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbPlanoIndividual = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gbxVistas = new System.Windows.Forms.GroupBox();
            this.trvVistas = new System.Windows.Forms.TreeView();
            this.gbxTipoPlano = new System.Windows.Forms.GroupBox();
            this.lstTipoPlano = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbxVistas.SuspendLayout();
            this.gbxTipoPlano.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnContraer);
            this.panel2.Controls.Add(this.btnExpandir);
            this.panel2.Controls.Add(this.btnSeleccionarNinguno);
            this.panel2.Controls.Add(this.btnSeleccionarTodos);
            this.panel2.Controls.Add(this.btnAceptar);
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 324);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(628, 34);
            this.panel2.TabIndex = 2;
            // 
            // btnContraer
            // 
            this.btnContraer.Location = new System.Drawing.Point(294, 3);
            this.btnContraer.Name = "btnContraer";
            this.btnContraer.Size = new System.Drawing.Size(90, 28);
            this.btnContraer.TabIndex = 11;
            this.btnContraer.Text = "Contraer";
            this.btnContraer.UseVisualStyleBackColor = true;
            this.btnContraer.Click += new System.EventHandler(this.btnContraer_Click);
            // 
            // btnExpandir
            // 
            this.btnExpandir.Location = new System.Drawing.Point(198, 3);
            this.btnExpandir.Name = "btnExpandir";
            this.btnExpandir.Size = new System.Drawing.Size(90, 28);
            this.btnExpandir.TabIndex = 10;
            this.btnExpandir.Text = "Expandir";
            this.btnExpandir.UseVisualStyleBackColor = true;
            this.btnExpandir.Click += new System.EventHandler(this.btnExpandir_Click);
            // 
            // btnSeleccionarNinguno
            // 
            this.btnSeleccionarNinguno.Location = new System.Drawing.Point(102, 3);
            this.btnSeleccionarNinguno.Name = "btnSeleccionarNinguno";
            this.btnSeleccionarNinguno.Size = new System.Drawing.Size(90, 28);
            this.btnSeleccionarNinguno.TabIndex = 9;
            this.btnSeleccionarNinguno.Text = "Ninguno";
            this.btnSeleccionarNinguno.UseVisualStyleBackColor = true;
            this.btnSeleccionarNinguno.Click += new System.EventHandler(this.btnSeleccionarNinguno_Click);
            // 
            // btnSeleccionarTodos
            // 
            this.btnSeleccionarTodos.Location = new System.Drawing.Point(6, 3);
            this.btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            this.btnSeleccionarTodos.Size = new System.Drawing.Size(90, 28);
            this.btnSeleccionarTodos.TabIndex = 8;
            this.btnSeleccionarTodos.Text = "Todos";
            this.btnSeleccionarTodos.UseVisualStyleBackColor = true;
            this.btnSeleccionarTodos.Click += new System.EventHandler(this.btnSeleccionarTodos_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(433, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(90, 28);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(529, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 28);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbPlanoIndividual);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 294);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 24);
            this.panel1.TabIndex = 3;
            // 
            // chbPlanoIndividual
            // 
            this.chbPlanoIndividual.AutoSize = true;
            this.chbPlanoIndividual.Checked = true;
            this.chbPlanoIndividual.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPlanoIndividual.Location = new System.Drawing.Point(9, 3);
            this.chbPlanoIndividual.Name = "chbPlanoIndividual";
            this.chbPlanoIndividual.Size = new System.Drawing.Size(165, 17);
            this.chbPlanoIndividual.TabIndex = 0;
            this.chbPlanoIndividual.Text = "Crear un plano por cada vista";
            this.chbPlanoIndividual.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.gbxVistas, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbxTipoPlano, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(628, 285);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // gbxVistas
            // 
            this.gbxVistas.Controls.Add(this.trvVistas);
            this.gbxVistas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxVistas.Location = new System.Drawing.Point(3, 3);
            this.gbxVistas.Name = "gbxVistas";
            this.gbxVistas.Size = new System.Drawing.Size(308, 279);
            this.gbxVistas.TabIndex = 0;
            this.gbxVistas.TabStop = false;
            this.gbxVistas.Text = "Vistas";
            // 
            // trvVistas
            // 
            this.trvVistas.CheckBoxes = true;
            this.trvVistas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvVistas.Location = new System.Drawing.Point(3, 16);
            this.trvVistas.Name = "trvVistas";
            this.trvVistas.Size = new System.Drawing.Size(302, 260);
            this.trvVistas.TabIndex = 14;
            this.trvVistas.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvVistas_NodeMouseClick);
            // 
            // gbxTipoPlano
            // 
            this.gbxTipoPlano.Controls.Add(this.lstTipoPlano);
            this.gbxTipoPlano.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxTipoPlano.Location = new System.Drawing.Point(317, 3);
            this.gbxTipoPlano.Name = "gbxTipoPlano";
            this.gbxTipoPlano.Size = new System.Drawing.Size(308, 279);
            this.gbxTipoPlano.TabIndex = 1;
            this.gbxTipoPlano.TabStop = false;
            this.gbxTipoPlano.Text = "Tipo de plano";
            // 
            // lstTipoPlano
            // 
            this.lstTipoPlano.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTipoPlano.FormattingEnabled = true;
            this.lstTipoPlano.Location = new System.Drawing.Point(3, 16);
            this.lstTipoPlano.Name = "lstTipoPlano";
            this.lstTipoPlano.Size = new System.Drawing.Size(302, 260);
            this.lstTipoPlano.TabIndex = 0;
            // 
            // frmCrearPlanos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "frmCrearPlanos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCrearPlanos";
            this.Load += new System.EventHandler(this.frmCrearPlanos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCrearPlanos_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gbxVistas.ResumeLayout(false);
            this.gbxTipoPlano.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chbPlanoIndividual;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gbxVistas;
        private System.Windows.Forms.GroupBox gbxTipoPlano;
        private System.Windows.Forms.ListBox lstTipoPlano;
        private System.Windows.Forms.TreeView trvVistas;
        private System.Windows.Forms.Button btnContraer;
        private System.Windows.Forms.Button btnExpandir;
        private System.Windows.Forms.Button btnSeleccionarNinguno;
        private System.Windows.Forms.Button btnSeleccionarTodos;
    }
}
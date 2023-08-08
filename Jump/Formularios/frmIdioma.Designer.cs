namespace Jump
{
    partial class frmIdioma
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
            this.gbxSeleccionIdioma = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pcbxBandera = new System.Windows.Forms.PictureBox();
            this.lstIdioma = new System.Windows.Forms.ListBox();
            this.lstPaises = new System.Windows.Forms.ListBox();
            this.txtTituloIdioma = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbxSeleccionIdioma.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbxBandera)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gbxSeleccionIdioma, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTituloIdioma, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gbxSeleccionIdioma
            // 
            this.gbxSeleccionIdioma.Controls.Add(this.tableLayoutPanel2);
            this.gbxSeleccionIdioma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxSeleccionIdioma.Location = new System.Drawing.Point(3, 43);
            this.gbxSeleccionIdioma.Name = "gbxSeleccionIdioma";
            this.gbxSeleccionIdioma.Size = new System.Drawing.Size(578, 275);
            this.gbxSeleccionIdioma.TabIndex = 5;
            this.gbxSeleccionIdioma.TabStop = false;
            this.gbxSeleccionIdioma.Text = "Idiomas disponibles";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutPanel2.Controls.Add(this.pcbxBandera, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lstIdioma, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lstPaises, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(572, 255);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // pcbxBandera
            // 
            this.pcbxBandera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pcbxBandera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbxBandera.Location = new System.Drawing.Point(391, 3);
            this.pcbxBandera.Name = "pcbxBandera";
            this.pcbxBandera.Size = new System.Drawing.Size(178, 249);
            this.pcbxBandera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbxBandera.TabIndex = 5;
            this.pcbxBandera.TabStop = false;
            // 
            // lstIdioma
            // 
            this.lstIdioma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstIdioma.FormattingEnabled = true;
            this.lstIdioma.ItemHeight = 15;
            this.lstIdioma.Location = new System.Drawing.Point(3, 3);
            this.lstIdioma.Name = "lstIdioma";
            this.lstIdioma.Size = new System.Drawing.Size(188, 249);
            this.lstIdioma.TabIndex = 2;
            // 
            // lstPaises
            // 
            this.lstPaises.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPaises.FormattingEnabled = true;
            this.lstPaises.ItemHeight = 15;
            this.lstPaises.Location = new System.Drawing.Point(197, 3);
            this.lstPaises.Name = "lstPaises";
            this.lstPaises.Size = new System.Drawing.Size(188, 249);
            this.lstPaises.TabIndex = 3;
            this.lstPaises.SelectedIndexChanged += new System.EventHandler(this.lstPaises_SelectedIndexChanged);
            // 
            // txtTituloIdioma
            // 
            this.txtTituloIdioma.AutoSize = true;
            this.txtTituloIdioma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTituloIdioma.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTituloIdioma.Location = new System.Drawing.Point(3, 0);
            this.txtTituloIdioma.Name = "txtTituloIdioma";
            this.txtTituloIdioma.Size = new System.Drawing.Size(578, 40);
            this.txtTituloIdioma.TabIndex = 3;
            this.txtTituloIdioma.Text = "Seleccione el idioma";
            this.txtTituloIdioma.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 324);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(578, 34);
            this.panel1.TabIndex = 6;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(485, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 28);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(389, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(90, 28);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // frmIdioma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmIdioma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jump";
            this.Load += new System.EventHandler(this.frmIdioma_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCerrar_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbxSeleccionIdioma.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbxBandera)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label txtTituloIdioma;
        private System.Windows.Forms.GroupBox gbxSeleccionIdioma;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pcbxBandera;
        private System.Windows.Forms.ListBox lstIdioma;
        private System.Windows.Forms.ListBox lstPaises;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
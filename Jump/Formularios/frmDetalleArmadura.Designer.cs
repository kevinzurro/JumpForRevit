namespace Jump
{
    partial class frmDetalleArmadura
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbEtiquetaArmadura = new System.Windows.Forms.CheckBox();
            this.chbEtiquetaLongitud = new System.Windows.Forms.CheckBox();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbEtiquetaLongitud = new System.Windows.Forms.ComboBox();
            this.cmbEtiquetaArmadura = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 121);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.06977F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(478, 75);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbEtiquetaArmadura);
            this.panel1.Controls.Add(this.chbEtiquetaLongitud);
            this.panel1.Controls.Add(this.lbl2);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 69);
            this.panel1.TabIndex = 0;
            // 
            // chbEtiquetaArmadura
            // 
            this.chbEtiquetaArmadura.AutoSize = true;
            this.chbEtiquetaArmadura.Checked = true;
            this.chbEtiquetaArmadura.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbEtiquetaArmadura.Location = new System.Drawing.Point(24, 10);
            this.chbEtiquetaArmadura.Margin = new System.Windows.Forms.Padding(0);
            this.chbEtiquetaArmadura.Name = "chbEtiquetaArmadura";
            this.chbEtiquetaArmadura.Size = new System.Drawing.Size(127, 17);
            this.chbEtiquetaArmadura.TabIndex = 18;
            this.chbEtiquetaArmadura.Text = "Etiqueta de armadura";
            this.chbEtiquetaArmadura.UseVisualStyleBackColor = true;
            // 
            // chbEtiquetaLongitud
            // 
            this.chbEtiquetaLongitud.AutoSize = true;
            this.chbEtiquetaLongitud.Checked = true;
            this.chbEtiquetaLongitud.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbEtiquetaLongitud.Location = new System.Drawing.Point(24, 44);
            this.chbEtiquetaLongitud.Margin = new System.Windows.Forms.Padding(0);
            this.chbEtiquetaLongitud.Name = "chbEtiquetaLongitud";
            this.chbEtiquetaLongitud.Size = new System.Drawing.Size(154, 17);
            this.chbEtiquetaLongitud.TabIndex = 19;
            this.chbEtiquetaLongitud.Text = "Longitud parcial de la barra";
            this.chbEtiquetaLongitud.UseVisualStyleBackColor = true;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(3, 45);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(16, 13);
            this.lbl2.TabIndex = 17;
            this.lbl2.Text = "2.";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(3, 11);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(16, 13);
            this.lbl1.TabIndex = 16;
            this.lbl1.Text = "1.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbEtiquetaLongitud);
            this.panel3.Controls.Add(this.cmbEtiquetaArmadura);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(194, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(281, 69);
            this.panel3.TabIndex = 1;
            // 
            // cmbEtiquetaLongitud
            // 
            this.cmbEtiquetaLongitud.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEtiquetaLongitud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEtiquetaLongitud.FormattingEnabled = true;
            this.cmbEtiquetaLongitud.Location = new System.Drawing.Point(3, 40);
            this.cmbEtiquetaLongitud.Name = "cmbEtiquetaLongitud";
            this.cmbEtiquetaLongitud.Size = new System.Drawing.Size(272, 21);
            this.cmbEtiquetaLongitud.TabIndex = 1;
            // 
            // cmbEtiquetaArmadura
            // 
            this.cmbEtiquetaArmadura.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEtiquetaArmadura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEtiquetaArmadura.FormattingEnabled = true;
            this.cmbEtiquetaArmadura.Location = new System.Drawing.Point(3, 6);
            this.cmbEtiquetaArmadura.Name = "cmbEtiquetaArmadura";
            this.cmbEtiquetaArmadura.Size = new System.Drawing.Size(272, 21);
            this.cmbEtiquetaArmadura.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEjecutar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 84);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 34);
            this.panel2.TabIndex = 1;
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEjecutar.Location = new System.Drawing.Point(385, 1);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(90, 30);
            this.btnEjecutar.TabIndex = 0;
            this.btnEjecutar.Text = "Seleccionar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            this.btnEjecutar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // frmDetalleArmadura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 121);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1000, 160);
            this.MinimumSize = new System.Drawing.Size(500, 160);
            this.Name = "frmDetalleArmadura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jump";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDetalleArmadura_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chbEtiquetaArmadura;
        private System.Windows.Forms.CheckBox chbEtiquetaLongitud;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbEtiquetaArmadura;
        private System.Windows.Forms.ComboBox cmbEtiquetaLongitud;
    }
}
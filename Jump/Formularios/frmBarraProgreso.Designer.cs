namespace Jump
{
    partial class frmBarraProgreso
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
            this.pbrBarraProgreso = new System.Windows.Forms.ProgressBar();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbrBarraProgreso
            // 
            this.pbrBarraProgreso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbrBarraProgreso.Location = new System.Drawing.Point(12, 35);
            this.pbrBarraProgreso.Name = "pbrBarraProgreso";
            this.pbrBarraProgreso.Size = new System.Drawing.Size(460, 32);
            this.pbrBarraProgreso.TabIndex = 0;
            // 
            // lblProgreso
            // 
            this.lblProgreso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgreso.AutoSize = true;
            this.lblProgreso.Location = new System.Drawing.Point(12, 12);
            this.lblProgreso.Name = "lblProgreso";
            this.lblProgreso.Size = new System.Drawing.Size(115, 13);
            this.lblProgreso.TabIndex = 1;
            this.lblProgreso.Text = "Procesando elementos";
            // 
            // frmBarraProgreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 82);
            this.ControlBox = false;
            this.Controls.Add(this.lblProgreso);
            this.Controls.Add(this.pbrBarraProgreso);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 120);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 120);
            this.Name = "frmBarraProgreso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jump";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbrBarraProgreso;
        private System.Windows.Forms.Label lblProgreso;
    }
}
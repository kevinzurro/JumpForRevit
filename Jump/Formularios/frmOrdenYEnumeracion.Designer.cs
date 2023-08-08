﻿namespace Jump
{
    partial class frmOrdenYEnumeracion
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
            this.gbxSeleccion = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbCategorias = new System.Windows.Forms.ComboBox();
            this.rbtnConjuntoDeLaLista = new System.Windows.Forms.RadioButton();
            this.rbtnElementosSeleccionados = new System.Windows.Forms.RadioButton();
            this.rbtnTodos = new System.Windows.Forms.RadioButton();
            this.lstElementos = new System.Windows.Forms.ListBox();
            this.gbxEnumeracion = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSufijo = new System.Windows.Forms.TextBox();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.txtIncremento = new System.Windows.Forms.TextBox();
            this.lblSufijo = new System.Windows.Forms.Label();
            this.txtPrefijo = new System.Windows.Forms.TextBox();
            this.lblNumeroInicial = new System.Windows.Forms.Label();
            this.lblIncremento = new System.Windows.Forms.Label();
            this.txtNumeroInicial = new System.Windows.Forms.TextBox();
            this.lstParametros = new System.Windows.Forms.ListBox();
            this.gbxPreview = new System.Windows.Forms.GroupBox();
            this.lblParametroElegido = new System.Windows.Forms.Label();
            this.lblVistaPrevia = new System.Windows.Forms.Label();
            this.gbxOrden = new System.Windows.Forms.GroupBox();
            this.rbtn42 = new System.Windows.Forms.RadioButton();
            this.rbtn41 = new System.Windows.Forms.RadioButton();
            this.rbtn32 = new System.Windows.Forms.RadioButton();
            this.rbtn31 = new System.Windows.Forms.RadioButton();
            this.rbtn22 = new System.Windows.Forms.RadioButton();
            this.rbtn21 = new System.Windows.Forms.RadioButton();
            this.rbtn12 = new System.Windows.Forms.RadioButton();
            this.rbtn11 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbxSeleccion.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbxEnumeracion.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbxPreview.SuspendLayout();
            this.gbxOrden.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gbxSeleccion, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbxEnumeracion, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbxOrden, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gbxSeleccion
            // 
            this.gbxSeleccion.Controls.Add(this.tableLayoutPanel2);
            this.gbxSeleccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxSeleccion.Location = new System.Drawing.Point(3, 3);
            this.gbxSeleccion.Name = "gbxSeleccion";
            this.gbxSeleccion.Size = new System.Drawing.Size(321, 415);
            this.gbxSeleccion.TabIndex = 12;
            this.gbxSeleccion.TabStop = false;
            this.gbxSeleccion.Text = "Categoría";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lstElementos, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(315, 395);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbCategorias);
            this.panel2.Controls.Add(this.rbtnConjuntoDeLaLista);
            this.panel2.Controls.Add(this.rbtnElementosSeleccionados);
            this.panel2.Controls.Add(this.rbtnTodos);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(7);
            this.panel2.Size = new System.Drawing.Size(309, 134);
            this.panel2.TabIndex = 0;
            // 
            // cmbCategorias
            // 
            this.cmbCategorias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategorias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategorias.FormattingEnabled = true;
            this.cmbCategorias.Location = new System.Drawing.Point(0, 7);
            this.cmbCategorias.Name = "cmbCategorias";
            this.cmbCategorias.Size = new System.Drawing.Size(309, 23);
            this.cmbCategorias.TabIndex = 3;
            this.cmbCategorias.SelectionChangeCommitted += new System.EventHandler(this.cmbCategorias_SelectedIndexChanged);
            // 
            // rbtnConjuntoDeLaLista
            // 
            this.rbtnConjuntoDeLaLista.AutoSize = true;
            this.rbtnConjuntoDeLaLista.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnConjuntoDeLaLista.Location = new System.Drawing.Point(2, 97);
            this.rbtnConjuntoDeLaLista.Margin = new System.Windows.Forms.Padding(6);
            this.rbtnConjuntoDeLaLista.Name = "rbtnConjuntoDeLaLista";
            this.rbtnConjuntoDeLaLista.Size = new System.Drawing.Size(139, 19);
            this.rbtnConjuntoDeLaLista.TabIndex = 2;
            this.rbtnConjuntoDeLaLista.TabStop = true;
            this.rbtnConjuntoDeLaLista.Text = "Elementos de la lista";
            this.rbtnConjuntoDeLaLista.UseVisualStyleBackColor = true;
            // 
            // rbtnElementosSeleccionados
            // 
            this.rbtnElementosSeleccionados.AutoSize = true;
            this.rbtnElementosSeleccionados.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnElementosSeleccionados.Location = new System.Drawing.Point(2, 67);
            this.rbtnElementosSeleccionados.Margin = new System.Windows.Forms.Padding(6);
            this.rbtnElementosSeleccionados.Name = "rbtnElementosSeleccionados";
            this.rbtnElementosSeleccionados.Size = new System.Drawing.Size(166, 19);
            this.rbtnElementosSeleccionados.TabIndex = 1;
            this.rbtnElementosSeleccionados.TabStop = true;
            this.rbtnElementosSeleccionados.Text = "Elementos seleccionados";
            this.rbtnElementosSeleccionados.UseVisualStyleBackColor = true;
            // 
            // rbtnTodos
            // 
            this.rbtnTodos.AutoSize = true;
            this.rbtnTodos.Checked = true;
            this.rbtnTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnTodos.Location = new System.Drawing.Point(2, 37);
            this.rbtnTodos.Margin = new System.Windows.Forms.Padding(6);
            this.rbtnTodos.Name = "rbtnTodos";
            this.rbtnTodos.Size = new System.Drawing.Size(59, 19);
            this.rbtnTodos.TabIndex = 0;
            this.rbtnTodos.TabStop = true;
            this.rbtnTodos.Text = "Todos";
            this.rbtnTodos.UseVisualStyleBackColor = true;
            // 
            // lstElementos
            // 
            this.lstElementos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstElementos.FormattingEnabled = true;
            this.lstElementos.ItemHeight = 15;
            this.lstElementos.Location = new System.Drawing.Point(3, 143);
            this.lstElementos.Name = "lstElementos";
            this.lstElementos.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstElementos.Size = new System.Drawing.Size(309, 249);
            this.lstElementos.TabIndex = 1;
            // 
            // gbxEnumeracion
            // 
            this.gbxEnumeracion.Controls.Add(this.tableLayoutPanel3);
            this.gbxEnumeracion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxEnumeracion.Location = new System.Drawing.Point(560, 3);
            this.gbxEnumeracion.Name = "gbxEnumeracion";
            this.gbxEnumeracion.Size = new System.Drawing.Size(321, 415);
            this.gbxEnumeracion.TabIndex = 11;
            this.gbxEnumeracion.TabStop = false;
            this.gbxEnumeracion.Text = "Enumeración";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lstParametros, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.gbxPreview, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(315, 395);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtSufijo);
            this.panel3.Controls.Add(this.lblPrefijo);
            this.panel3.Controls.Add(this.txtIncremento);
            this.panel3.Controls.Add(this.lblSufijo);
            this.panel3.Controls.Add(this.txtPrefijo);
            this.panel3.Controls.Add(this.lblNumeroInicial);
            this.panel3.Controls.Add(this.lblIncremento);
            this.panel3.Controls.Add(this.txtNumeroInicial);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(7);
            this.panel3.Size = new System.Drawing.Size(309, 144);
            this.panel3.TabIndex = 0;
            // 
            // txtSufijo
            // 
            this.txtSufijo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSufijo.Location = new System.Drawing.Point(104, 103);
            this.txtSufijo.Name = "txtSufijo";
            this.txtSufijo.Size = new System.Drawing.Size(195, 21);
            this.txtSufijo.TabIndex = 15;
            this.txtSufijo.TextChanged += new System.EventHandler(this.txtPrefijo_TextChanged);
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijo.Location = new System.Drawing.Point(10, 15);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(42, 15);
            this.lblPrefijo.TabIndex = 8;
            this.lblPrefijo.Text = "Prefijo";
            // 
            // txtIncremento
            // 
            this.txtIncremento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIncremento.Location = new System.Drawing.Point(104, 73);
            this.txtIncremento.Name = "txtIncremento";
            this.txtIncremento.Size = new System.Drawing.Size(195, 21);
            this.txtIncremento.TabIndex = 13;
            this.txtIncremento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.verificarSoloNumero);
            // 
            // lblSufijo
            // 
            this.lblSufijo.AutoSize = true;
            this.lblSufijo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSufijo.Location = new System.Drawing.Point(10, 105);
            this.lblSufijo.Name = "lblSufijo";
            this.lblSufijo.Size = new System.Drawing.Size(38, 15);
            this.lblSufijo.TabIndex = 14;
            this.lblSufijo.Text = "Sufijo";
            // 
            // txtPrefijo
            // 
            this.txtPrefijo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrefijo.Location = new System.Drawing.Point(104, 13);
            this.txtPrefijo.Name = "txtPrefijo";
            this.txtPrefijo.Size = new System.Drawing.Size(195, 21);
            this.txtPrefijo.TabIndex = 9;
            this.txtPrefijo.TextChanged += new System.EventHandler(this.txtPrefijo_TextChanged);
            // 
            // lblNumeroInicial
            // 
            this.lblNumeroInicial.AutoSize = true;
            this.lblNumeroInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroInicial.Location = new System.Drawing.Point(10, 45);
            this.lblNumeroInicial.Name = "lblNumeroInicial";
            this.lblNumeroInicial.Size = new System.Drawing.Size(87, 15);
            this.lblNumeroInicial.TabIndex = 10;
            this.lblNumeroInicial.Text = "Número inicial";
            // 
            // lblIncremento
            // 
            this.lblIncremento.AutoSize = true;
            this.lblIncremento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncremento.Location = new System.Drawing.Point(10, 75);
            this.lblIncremento.Name = "lblIncremento";
            this.lblIncremento.Size = new System.Drawing.Size(69, 15);
            this.lblIncremento.TabIndex = 12;
            this.lblIncremento.Text = "Incremento";
            // 
            // txtNumeroInicial
            // 
            this.txtNumeroInicial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNumeroInicial.Location = new System.Drawing.Point(104, 43);
            this.txtNumeroInicial.Name = "txtNumeroInicial";
            this.txtNumeroInicial.Size = new System.Drawing.Size(195, 21);
            this.txtNumeroInicial.TabIndex = 11;
            this.txtNumeroInicial.TextChanged += new System.EventHandler(this.txtPrefijo_TextChanged);
            this.txtNumeroInicial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.verificarSoloNumero);
            // 
            // lstParametros
            // 
            this.lstParametros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParametros.FormattingEnabled = true;
            this.lstParametros.ItemHeight = 15;
            this.lstParametros.Location = new System.Drawing.Point(3, 223);
            this.lstParametros.Name = "lstParametros";
            this.lstParametros.Size = new System.Drawing.Size(309, 169);
            this.lstParametros.TabIndex = 2;
            this.lstParametros.SelectedIndexChanged += new System.EventHandler(this.lstParametros_SelectedIndexChanged);
            // 
            // gbxPreview
            // 
            this.gbxPreview.Controls.Add(this.lblParametroElegido);
            this.gbxPreview.Controls.Add(this.lblVistaPrevia);
            this.gbxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxPreview.Location = new System.Drawing.Point(3, 153);
            this.gbxPreview.Name = "gbxPreview";
            this.gbxPreview.Size = new System.Drawing.Size(309, 64);
            this.gbxPreview.TabIndex = 3;
            this.gbxPreview.TabStop = false;
            this.gbxPreview.Text = "Vista Previa";
            // 
            // lblParametroElegido
            // 
            this.lblParametroElegido.AutoSize = true;
            this.lblParametroElegido.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParametroElegido.Location = new System.Drawing.Point(9, 25);
            this.lblParametroElegido.Name = "lblParametroElegido";
            this.lblParametroElegido.Size = new System.Drawing.Size(65, 15);
            this.lblParametroElegido.TabIndex = 3;
            this.lblParametroElegido.Text = "Parámetro";
            // 
            // lblVistaPrevia
            // 
            this.lblVistaPrevia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVistaPrevia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVistaPrevia.Location = new System.Drawing.Point(99, 25);
            this.lblVistaPrevia.Name = "lblVistaPrevia";
            this.lblVistaPrevia.Size = new System.Drawing.Size(200, 17);
            this.lblVistaPrevia.TabIndex = 2;
            this.lblVistaPrevia.Text = "Enumeración";
            this.lblVistaPrevia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxOrden
            // 
            this.gbxOrden.Controls.Add(this.rbtn42);
            this.gbxOrden.Controls.Add(this.rbtn41);
            this.gbxOrden.Controls.Add(this.rbtn32);
            this.gbxOrden.Controls.Add(this.rbtn31);
            this.gbxOrden.Controls.Add(this.rbtn22);
            this.gbxOrden.Controls.Add(this.rbtn21);
            this.gbxOrden.Controls.Add(this.rbtn12);
            this.gbxOrden.Controls.Add(this.rbtn11);
            this.gbxOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxOrden.Location = new System.Drawing.Point(330, 3);
            this.gbxOrden.Name = "gbxOrden";
            this.gbxOrden.Size = new System.Drawing.Size(224, 415);
            this.gbxOrden.TabIndex = 9;
            this.gbxOrden.TabStop = false;
            this.gbxOrden.Text = "Orden";
            // 
            // rbtn42
            // 
            this.rbtn42.AutoSize = true;
            this.rbtn42.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn42.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn42.Image = global::Jump.Iconos_e_Imagenes.Iconos._4_2;
            this.rbtn42.Location = new System.Drawing.Point(124, 292);
            this.rbtn42.Name = "rbtn42";
            this.rbtn42.Size = new System.Drawing.Size(94, 80);
            this.rbtn42.TabIndex = 7;
            this.rbtn42.TabStop = true;
            this.rbtn42.Text = "                     ";
            this.rbtn42.UseVisualStyleBackColor = true;
            // 
            // rbtn41
            // 
            this.rbtn41.AutoSize = true;
            this.rbtn41.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn41.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn41.Image = global::Jump.Iconos_e_Imagenes.Iconos._4_1;
            this.rbtn41.Location = new System.Drawing.Point(13, 292);
            this.rbtn41.Name = "rbtn41";
            this.rbtn41.Size = new System.Drawing.Size(94, 80);
            this.rbtn41.TabIndex = 6;
            this.rbtn41.TabStop = true;
            this.rbtn41.Text = "                     ";
            this.rbtn41.UseVisualStyleBackColor = true;
            // 
            // rbtn32
            // 
            this.rbtn32.AutoSize = true;
            this.rbtn32.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn32.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn32.Image = global::Jump.Iconos_e_Imagenes.Iconos._3_2;
            this.rbtn32.Location = new System.Drawing.Point(124, 206);
            this.rbtn32.Name = "rbtn32";
            this.rbtn32.Size = new System.Drawing.Size(94, 80);
            this.rbtn32.TabIndex = 5;
            this.rbtn32.TabStop = true;
            this.rbtn32.Text = "                     ";
            this.rbtn32.UseVisualStyleBackColor = true;
            // 
            // rbtn31
            // 
            this.rbtn31.AutoSize = true;
            this.rbtn31.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn31.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn31.Image = global::Jump.Iconos_e_Imagenes.Iconos._3_1;
            this.rbtn31.Location = new System.Drawing.Point(13, 206);
            this.rbtn31.Name = "rbtn31";
            this.rbtn31.Size = new System.Drawing.Size(94, 80);
            this.rbtn31.TabIndex = 4;
            this.rbtn31.TabStop = true;
            this.rbtn31.Text = "                     ";
            this.rbtn31.UseVisualStyleBackColor = true;
            // 
            // rbtn22
            // 
            this.rbtn22.AutoSize = true;
            this.rbtn22.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn22.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn22.Image = global::Jump.Iconos_e_Imagenes.Iconos._2_2;
            this.rbtn22.Location = new System.Drawing.Point(124, 120);
            this.rbtn22.Name = "rbtn22";
            this.rbtn22.Size = new System.Drawing.Size(94, 80);
            this.rbtn22.TabIndex = 3;
            this.rbtn22.TabStop = true;
            this.rbtn22.Text = "                     ";
            this.rbtn22.UseVisualStyleBackColor = true;
            // 
            // rbtn21
            // 
            this.rbtn21.AutoSize = true;
            this.rbtn21.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn21.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn21.Image = global::Jump.Iconos_e_Imagenes.Iconos._2_1;
            this.rbtn21.Location = new System.Drawing.Point(13, 120);
            this.rbtn21.Name = "rbtn21";
            this.rbtn21.Size = new System.Drawing.Size(94, 80);
            this.rbtn21.TabIndex = 2;
            this.rbtn21.TabStop = true;
            this.rbtn21.Text = "                     ";
            this.rbtn21.UseVisualStyleBackColor = true;
            // 
            // rbtn12
            // 
            this.rbtn12.AutoSize = true;
            this.rbtn12.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn12.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn12.Image = global::Jump.Iconos_e_Imagenes.Iconos._1_2;
            this.rbtn12.Location = new System.Drawing.Point(124, 34);
            this.rbtn12.Name = "rbtn12";
            this.rbtn12.Size = new System.Drawing.Size(94, 80);
            this.rbtn12.TabIndex = 1;
            this.rbtn12.TabStop = true;
            this.rbtn12.Text = "                     ";
            this.rbtn12.UseVisualStyleBackColor = true;
            // 
            // rbtn11
            // 
            this.rbtn11.AutoSize = true;
            this.rbtn11.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbtn11.Checked = true;
            this.rbtn11.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbtn11.Image = global::Jump.Iconos_e_Imagenes.Iconos._1_1;
            this.rbtn11.Location = new System.Drawing.Point(13, 34);
            this.rbtn11.Name = "rbtn11";
            this.rbtn11.Size = new System.Drawing.Size(94, 80);
            this.rbtn11.TabIndex = 0;
            this.rbtn11.TabStop = true;
            this.rbtn11.Text = "                     ";
            this.rbtn11.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(560, 424);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(321, 34);
            this.panel1.TabIndex = 13;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(228, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 28);
            this.btnCancelar.TabIndex = 15;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(132, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(90, 28);
            this.btnAceptar.TabIndex = 14;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // frmOrdenYEnumeracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "frmOrdenYEnumeracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orden y Enumeracion";
            this.Load += new System.EventHandler(this.frmOrdenYEnumeracion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCerrar_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbxSeleccion.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbxEnumeracion.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gbxPreview.ResumeLayout(false);
            this.gbxPreview.PerformLayout();
            this.gbxOrden.ResumeLayout(false);
            this.gbxOrden.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbxEnumeracion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSufijo;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.TextBox txtIncremento;
        private System.Windows.Forms.Label lblSufijo;
        private System.Windows.Forms.TextBox txtPrefijo;
        private System.Windows.Forms.Label lblNumeroInicial;
        private System.Windows.Forms.Label lblIncremento;
        private System.Windows.Forms.TextBox txtNumeroInicial;
        private System.Windows.Forms.ListBox lstParametros;
        private System.Windows.Forms.GroupBox gbxPreview;
        private System.Windows.Forms.Label lblParametroElegido;
        private System.Windows.Forms.Label lblVistaPrevia;
        private System.Windows.Forms.GroupBox gbxOrden;
        private System.Windows.Forms.RadioButton rbtn42;
        private System.Windows.Forms.RadioButton rbtn41;
        private System.Windows.Forms.RadioButton rbtn32;
        private System.Windows.Forms.RadioButton rbtn31;
        private System.Windows.Forms.RadioButton rbtn22;
        private System.Windows.Forms.RadioButton rbtn21;
        private System.Windows.Forms.RadioButton rbtn12;
        private System.Windows.Forms.RadioButton rbtn11;
        private System.Windows.Forms.GroupBox gbxSeleccion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbtnConjuntoDeLaLista;
        private System.Windows.Forms.RadioButton rbtnElementosSeleccionados;
        private System.Windows.Forms.RadioButton rbtnTodos;
        private System.Windows.Forms.ListBox lstElementos;
        private System.Windows.Forms.ComboBox cmbCategorias;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancelar;
    }
}
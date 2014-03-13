namespace Assembly
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components = null;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.textArchivo = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblFecha = new System.Windows.Forms.ToolStripStatusLabel();
			this.calendario = new System.Windows.Forms.MonthCalendar();
			this.groupArchivo = new System.Windows.Forms.GroupBox();
			this.cmdEnsamblar = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblNumerado = new System.Windows.Forms.Label();
			this.groupSalida = new System.Windows.Forms.GroupBox();
			this.textS19 = new System.Windows.Forms.RichTextBox();
			this.Ver = new System.Windows.Forms.Label();
			this.textCodigoMaquina = new System.Windows.Forms.RichTextBox();
			this.cmbSalida = new System.Windows.Forms.ComboBox();
			this.textSalida = new System.Windows.Forms.RichTextBox();
			this.abrirArchivo = new System.Windows.Forms.OpenFileDialog();
			this.tabErroresSintaxis = new System.Windows.Forms.TabPage();
			this.listErroresSintaxis = new System.Windows.Forms.ListBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabErroresCODOP = new System.Windows.Forms.TabPage();
			this.listErroresCODOP = new System.Windows.Forms.ListBox();
			this.tabErroresOperando = new System.Windows.Forms.TabPage();
			this.listErroresOperando = new System.Windows.Forms.ListBox();
			this.tabErroresDirectiva = new System.Windows.Forms.TabPage();
			this.listErroresDirectiva = new System.Windows.Forms.ListBox();
			this.tabErroresPostEnsamblado = new System.Windows.Forms.TabPage();
			this.listErroresPostEnsamblado = new System.Windows.Forms.ListBox();
			this.abrirTABOP = new System.Windows.Forms.OpenFileDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.abrirArchivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdCambiarTABOP = new System.Windows.Forms.Button();
			this.lblTABOP = new System.Windows.Forms.Label();
			this.cmbInfoCODOPS = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.groupArchivo.SuspendLayout();
			this.groupSalida.SuspendLayout();
			this.tabErroresSintaxis.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabErroresCODOP.SuspendLayout();
			this.tabErroresOperando.SuspendLayout();
			this.tabErroresDirectiva.SuspendLayout();
			this.tabErroresPostEnsamblado.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textArchivo
			// 
			this.textArchivo.AcceptsTab = true;
			this.textArchivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.textArchivo.AutoWordSelection = true;
			this.textArchivo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.textArchivo.BulletIndent = 1;
			this.textArchivo.DetectUrls = false;
			this.textArchivo.EnableAutoDragDrop = true;
			this.textArchivo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textArchivo.ForeColor = System.Drawing.Color.Black;
			this.textArchivo.HideSelection = false;
			this.textArchivo.Location = new System.Drawing.Point(46, 55);
			this.textArchivo.Name = "textArchivo";
			this.textArchivo.ReadOnly = true;
			this.textArchivo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.textArchivo.ShowSelectionMargin = true;
			this.textArchivo.Size = new System.Drawing.Size(298, 319);
			this.textArchivo.TabIndex = 0;
			this.textArchivo.TabStop = false;
			this.textArchivo.Text = "";
			this.textArchivo.WordWrap = false;
			this.textArchivo.VScroll += new System.EventHandler(this.TextArchivoVScroll);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.lblFecha});
			this.statusStrip1.Location = new System.Drawing.Point(0, 580);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(784, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblFecha
			// 
			this.lblFecha.Name = "lblFecha";
			this.lblFecha.Size = new System.Drawing.Size(16, 17);
			this.lblFecha.Text = "...";
			// 
			// calendario
			// 
			this.calendario.Location = new System.Drawing.Point(310, 182);
			this.calendario.Name = "calendario";
			this.calendario.TabIndex = 2;
			this.calendario.Visible = false;
			// 
			// groupArchivo
			// 
			this.groupArchivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.groupArchivo.Controls.Add(this.cmdEnsamblar);
			this.groupArchivo.Controls.Add(this.label2);
			this.groupArchivo.Controls.Add(this.label1);
			this.groupArchivo.Controls.Add(this.lblNumerado);
			this.groupArchivo.Controls.Add(this.textArchivo);
			this.groupArchivo.Location = new System.Drawing.Point(12, 48);
			this.groupArchivo.Name = "groupArchivo";
			this.groupArchivo.Size = new System.Drawing.Size(360, 399);
			this.groupArchivo.TabIndex = 5;
			this.groupArchivo.TabStop = false;
			this.groupArchivo.Text = "Archivo";
			// 
			// cmdEnsamblar
			// 
			this.cmdEnsamblar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdEnsamblar.Location = new System.Drawing.Point(269, 20);
			this.cmdEnsamblar.Name = "cmdEnsamblar";
			this.cmdEnsamblar.Size = new System.Drawing.Size(75, 23);
			this.cmdEnsamblar.TabIndex = 3;
			this.cmdEnsamblar.Text = "Ensamblar";
			this.cmdEnsamblar.UseVisualStyleBackColor = true;
			this.cmdEnsamblar.Click += new System.EventHandler(this.CmdAnalizarClick);
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.SystemColors.Control;
			this.label2.Location = new System.Drawing.Point(6, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 37);
			this.label2.TabIndex = 2;
			this.label2.Text = "label1";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.ForeColor = System.Drawing.SystemColors.Control;
			this.label1.Location = new System.Drawing.Point(6, 355);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 29);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// lblNumerado
			// 
			this.lblNumerado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.lblNumerado.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.lblNumerado.ForeColor = System.Drawing.Color.SteelBlue;
			this.lblNumerado.Location = new System.Drawing.Point(6, 57);
			this.lblNumerado.Name = "lblNumerado";
			this.lblNumerado.Size = new System.Drawing.Size(34, 329);
			this.lblNumerado.TabIndex = 1;
			this.lblNumerado.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10\r\n11\r\n12\r\n13\r\n14\r\n15\r\n16\r\n17\r\n18\r\n19\r\n20\r\n21\r\n22\r\n23" +
			"\r\n24\r\n25\r\n26";
			this.lblNumerado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupSalida
			// 
			this.groupSalida.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupSalida.Controls.Add(this.textS19);
			this.groupSalida.Controls.Add(this.Ver);
			this.groupSalida.Controls.Add(this.textCodigoMaquina);
			this.groupSalida.Controls.Add(this.cmbSalida);
			this.groupSalida.Controls.Add(this.textSalida);
			this.groupSalida.Controls.Add(this.calendario);
			this.groupSalida.Location = new System.Drawing.Point(412, 103);
			this.groupSalida.Name = "groupSalida";
			this.groupSalida.Size = new System.Drawing.Size(360, 344);
			this.groupSalida.TabIndex = 5;
			this.groupSalida.TabStop = false;
			this.groupSalida.Text = "Salida";
			// 
			// textS19
			// 
			this.textS19.AcceptsTab = true;
			this.textS19.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textS19.AutoWordSelection = true;
			this.textS19.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.textS19.BulletIndent = 1;
			this.textS19.DetectUrls = false;
			this.textS19.EnableAutoDragDrop = true;
			this.textS19.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textS19.ForeColor = System.Drawing.Color.Black;
			this.textS19.Location = new System.Drawing.Point(36, 53);
			this.textS19.Name = "textS19";
			this.textS19.ReadOnly = true;
			this.textS19.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.textS19.ShowSelectionMargin = true;
			this.textS19.Size = new System.Drawing.Size(298, 260);
			this.textS19.TabIndex = 12;
			this.textS19.TabStop = false;
			this.textS19.Text = "";
			this.textS19.Visible = false;
			this.textS19.WordWrap = false;
			// 
			// Ver
			// 
			this.Ver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Ver.Location = new System.Drawing.Point(36, 24);
			this.Ver.Name = "Ver";
			this.Ver.Size = new System.Drawing.Size(69, 23);
			this.Ver.TabIndex = 11;
			this.Ver.Text = "Ver...";
			this.Ver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textCodigoMaquina
			// 
			this.textCodigoMaquina.AcceptsTab = true;
			this.textCodigoMaquina.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textCodigoMaquina.AutoWordSelection = true;
			this.textCodigoMaquina.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.textCodigoMaquina.BulletIndent = 1;
			this.textCodigoMaquina.DetectUrls = false;
			this.textCodigoMaquina.EnableAutoDragDrop = true;
			this.textCodigoMaquina.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textCodigoMaquina.ForeColor = System.Drawing.Color.Black;
			this.textCodigoMaquina.Location = new System.Drawing.Point(36, 53);
			this.textCodigoMaquina.Name = "textCodigoMaquina";
			this.textCodigoMaquina.ReadOnly = true;
			this.textCodigoMaquina.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.textCodigoMaquina.ShowSelectionMargin = true;
			this.textCodigoMaquina.Size = new System.Drawing.Size(298, 260);
			this.textCodigoMaquina.TabIndex = 3;
			this.textCodigoMaquina.TabStop = false;
			this.textCodigoMaquina.Text = "";
			this.textCodigoMaquina.WordWrap = false;
			// 
			// cmbSalida
			// 
			this.cmbSalida.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbSalida.FormattingEnabled = true;
			this.cmbSalida.Items.AddRange(new object[] {
									"Salida",
									"Código Máquina",
									"S19"});
			this.cmbSalida.Location = new System.Drawing.Point(111, 26);
			this.cmbSalida.Name = "cmbSalida";
			this.cmbSalida.Size = new System.Drawing.Size(223, 21);
			this.cmbSalida.TabIndex = 10;
			this.cmbSalida.SelectedIndexChanged += new System.EventHandler(this.CmbSalidaSelectedIndexChanged);
			// 
			// textSalida
			// 
			this.textSalida.AcceptsTab = true;
			this.textSalida.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textSalida.AutoWordSelection = true;
			this.textSalida.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.textSalida.BulletIndent = 1;
			this.textSalida.DetectUrls = false;
			this.textSalida.EnableAutoDragDrop = true;
			this.textSalida.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textSalida.ForeColor = System.Drawing.Color.Black;
			this.textSalida.Location = new System.Drawing.Point(36, 53);
			this.textSalida.Name = "textSalida";
			this.textSalida.ReadOnly = true;
			this.textSalida.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.textSalida.ShowSelectionMargin = true;
			this.textSalida.Size = new System.Drawing.Size(298, 260);
			this.textSalida.TabIndex = 0;
			this.textSalida.TabStop = false;
			this.textSalida.Text = "";
			this.textSalida.WordWrap = false;
			// 
			// abrirArchivo
			// 
			this.abrirArchivo.Filter = "Archivo de texto | *.txt";
			this.abrirArchivo.Title = "Abrir archivo...";
			// 
			// tabErroresSintaxis
			// 
			this.tabErroresSintaxis.Controls.Add(this.listErroresSintaxis);
			this.tabErroresSintaxis.Location = new System.Drawing.Point(4, 22);
			this.tabErroresSintaxis.Name = "tabErroresSintaxis";
			this.tabErroresSintaxis.Padding = new System.Windows.Forms.Padding(3);
			this.tabErroresSintaxis.Size = new System.Drawing.Size(776, 98);
			this.tabErroresSintaxis.TabIndex = 0;
			this.tabErroresSintaxis.Text = "Errores de Sintaxis";
			this.tabErroresSintaxis.UseVisualStyleBackColor = true;
			// 
			// listErroresSintaxis
			// 
			this.listErroresSintaxis.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listErroresSintaxis.FormattingEnabled = true;
			this.listErroresSintaxis.Location = new System.Drawing.Point(3, 3);
			this.listErroresSintaxis.Name = "listErroresSintaxis";
			this.listErroresSintaxis.ScrollAlwaysVisible = true;
			this.listErroresSintaxis.Size = new System.Drawing.Size(770, 92);
			this.listErroresSintaxis.TabIndex = 0;
			this.listErroresSintaxis.SelectedIndexChanged += new System.EventHandler(this.ListErroresSintaxisSelectedIndexChanged);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabErroresSintaxis);
			this.tabControl.Controls.Add(this.tabErroresCODOP);
			this.tabControl.Controls.Add(this.tabErroresOperando);
			this.tabControl.Controls.Add(this.tabErroresDirectiva);
			this.tabControl.Controls.Add(this.tabErroresPostEnsamblado);
			this.tabControl.Location = new System.Drawing.Point(0, 453);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(784, 124);
			this.tabControl.TabIndex = 3;
			// 
			// tabErroresCODOP
			// 
			this.tabErroresCODOP.Controls.Add(this.listErroresCODOP);
			this.tabErroresCODOP.Location = new System.Drawing.Point(4, 22);
			this.tabErroresCODOP.Name = "tabErroresCODOP";
			this.tabErroresCODOP.Padding = new System.Windows.Forms.Padding(3);
			this.tabErroresCODOP.Size = new System.Drawing.Size(776, 98);
			this.tabErroresCODOP.TabIndex = 1;
			this.tabErroresCODOP.Text = "Errores de CODOP";
			this.tabErroresCODOP.UseVisualStyleBackColor = true;
			// 
			// listErroresCODOP
			// 
			this.listErroresCODOP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listErroresCODOP.FormattingEnabled = true;
			this.listErroresCODOP.Location = new System.Drawing.Point(3, 3);
			this.listErroresCODOP.Name = "listErroresCODOP";
			this.listErroresCODOP.ScrollAlwaysVisible = true;
			this.listErroresCODOP.Size = new System.Drawing.Size(770, 92);
			this.listErroresCODOP.TabIndex = 1;
			this.listErroresCODOP.SelectedIndexChanged += new System.EventHandler(this.ListErroresCODOPSelectedIndexChanged);
			// 
			// tabErroresOperando
			// 
			this.tabErroresOperando.Controls.Add(this.listErroresOperando);
			this.tabErroresOperando.Location = new System.Drawing.Point(4, 22);
			this.tabErroresOperando.Name = "tabErroresOperando";
			this.tabErroresOperando.Padding = new System.Windows.Forms.Padding(3);
			this.tabErroresOperando.Size = new System.Drawing.Size(776, 98);
			this.tabErroresOperando.TabIndex = 2;
			this.tabErroresOperando.Text = "Errores de Operando";
			this.tabErroresOperando.UseVisualStyleBackColor = true;
			// 
			// listErroresOperando
			// 
			this.listErroresOperando.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listErroresOperando.FormattingEnabled = true;
			this.listErroresOperando.Location = new System.Drawing.Point(3, 3);
			this.listErroresOperando.Name = "listErroresOperando";
			this.listErroresOperando.ScrollAlwaysVisible = true;
			this.listErroresOperando.Size = new System.Drawing.Size(770, 92);
			this.listErroresOperando.TabIndex = 2;
			this.listErroresOperando.SelectedIndexChanged += new System.EventHandler(this.ListErroresOperandoSelectedIndexChanged);
			// 
			// tabErroresDirectiva
			// 
			this.tabErroresDirectiva.Controls.Add(this.listErroresDirectiva);
			this.tabErroresDirectiva.Location = new System.Drawing.Point(4, 22);
			this.tabErroresDirectiva.Name = "tabErroresDirectiva";
			this.tabErroresDirectiva.Padding = new System.Windows.Forms.Padding(3);
			this.tabErroresDirectiva.Size = new System.Drawing.Size(776, 98);
			this.tabErroresDirectiva.TabIndex = 3;
			this.tabErroresDirectiva.Text = "Errores de Directiva";
			this.tabErroresDirectiva.UseVisualStyleBackColor = true;
			// 
			// listErroresDirectiva
			// 
			this.listErroresDirectiva.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listErroresDirectiva.FormattingEnabled = true;
			this.listErroresDirectiva.Location = new System.Drawing.Point(3, 3);
			this.listErroresDirectiva.Name = "listErroresDirectiva";
			this.listErroresDirectiva.ScrollAlwaysVisible = true;
			this.listErroresDirectiva.Size = new System.Drawing.Size(770, 92);
			this.listErroresDirectiva.TabIndex = 3;
			this.listErroresDirectiva.SelectedIndexChanged += new System.EventHandler(this.ListErroresdirectivaSelectedIndexChanged);
			// 
			// tabErroresPostEnsamblado
			// 
			this.tabErroresPostEnsamblado.Controls.Add(this.listErroresPostEnsamblado);
			this.tabErroresPostEnsamblado.Location = new System.Drawing.Point(4, 22);
			this.tabErroresPostEnsamblado.Name = "tabErroresPostEnsamblado";
			this.tabErroresPostEnsamblado.Padding = new System.Windows.Forms.Padding(3);
			this.tabErroresPostEnsamblado.Size = new System.Drawing.Size(776, 98);
			this.tabErroresPostEnsamblado.TabIndex = 4;
			this.tabErroresPostEnsamblado.Text = "Errores Post Ensamblado";
			this.tabErroresPostEnsamblado.UseVisualStyleBackColor = true;
			// 
			// listErroresPostEnsamblado
			// 
			this.listErroresPostEnsamblado.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listErroresPostEnsamblado.FormattingEnabled = true;
			this.listErroresPostEnsamblado.Location = new System.Drawing.Point(3, 3);
			this.listErroresPostEnsamblado.Name = "listErroresPostEnsamblado";
			this.listErroresPostEnsamblado.ScrollAlwaysVisible = true;
			this.listErroresPostEnsamblado.Size = new System.Drawing.Size(770, 92);
			this.listErroresPostEnsamblado.TabIndex = 4;
			// 
			// abrirTABOP
			// 
			this.abrirTABOP.FileName = "TABOP.txt";
			this.abrirTABOP.Filter = "Archivo de texto|*.txt";
			this.abrirTABOP.Title = "Carga Archivo TABOP";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.archivoToolStripMenuItem,
									this.toolStripMenuItem1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(784, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// archivoToolStripMenuItem
			// 
			this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.abrirArchivoToolStripMenuItem,
									this.toolStripMenuItem2,
									this.salirToolStripMenuItem});
			this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
			this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.archivoToolStripMenuItem.Text = "Archivo";
			// 
			// abrirArchivoToolStripMenuItem
			// 
			this.abrirArchivoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("abrirArchivoToolStripMenuItem.Image")));
			this.abrirArchivoToolStripMenuItem.Name = "abrirArchivoToolStripMenuItem";
			this.abrirArchivoToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
			this.abrirArchivoToolStripMenuItem.Text = "Abrir archivo";
			this.abrirArchivoToolStripMenuItem.Click += new System.EventHandler(this.CargarArchivoToolStripMenuItemClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(139, 6);
			// 
			// salirToolStripMenuItem
			// 
			this.salirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("salirToolStripMenuItem.Image")));
			this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
			this.salirToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
			this.salirToolStripMenuItem.Text = "Salir";
			this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirToolStripMenuItemClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.acercaDeToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
			this.toolStripMenuItem1.Text = "?";
			// 
			// acercaDeToolStripMenuItem
			// 
			this.acercaDeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("acercaDeToolStripMenuItem.Image")));
			this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
			this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.acercaDeToolStripMenuItem.Text = "Acerca de...";
			this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.AcercaDeToolStripMenuItemClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cmdCambiarTABOP);
			this.groupBox1.Controls.Add(this.lblTABOP);
			this.groupBox1.Location = new System.Drawing.Point(412, 48);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 49);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "TABOP";
			// 
			// cmdCambiarTABOP
			// 
			this.cmdCambiarTABOP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCambiarTABOP.Image = ((System.Drawing.Image)(resources.GetObject("cmdCambiarTABOP.Image")));
			this.cmdCambiarTABOP.Location = new System.Drawing.Point(310, 20);
			this.cmdCambiarTABOP.Name = "cmdCambiarTABOP";
			this.cmdCambiarTABOP.Size = new System.Drawing.Size(32, 23);
			this.cmdCambiarTABOP.TabIndex = 1;
			this.cmdCambiarTABOP.UseVisualStyleBackColor = true;
			this.cmdCambiarTABOP.Click += new System.EventHandler(this.CmdCambiarTABOPClick);
			// 
			// lblTABOP
			// 
			this.lblTABOP.Location = new System.Drawing.Point(18, 20);
			this.lblTABOP.Name = "lblTABOP";
			this.lblTABOP.Size = new System.Drawing.Size(286, 23);
			this.lblTABOP.TabIndex = 0;
			this.lblTABOP.Text = "Archivo";
			// 
			// cmbInfoCODOPS
			// 
			this.cmbInfoCODOPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbInfoCODOPS.FormattingEnabled = true;
			this.cmbInfoCODOPS.Location = new System.Drawing.Point(579, 27);
			this.cmbInfoCODOPS.Name = "cmbInfoCODOPS";
			this.cmbInfoCODOPS.Size = new System.Drawing.Size(191, 21);
			this.cmbInfoCODOPS.TabIndex = 8;
			this.cmbInfoCODOPS.SelectedIndexChanged += new System.EventHandler(this.CmbInfoCODOPSSelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(458, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(115, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Ver información de...";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(784, 602);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbInfoCODOPS);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupSalida);
			this.Controls.Add(this.groupArchivo);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(800, 640);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FishAssembly";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupArchivo.ResumeLayout(false);
			this.groupSalida.ResumeLayout(false);
			this.tabErroresSintaxis.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabErroresCODOP.ResumeLayout(false);
			this.tabErroresOperando.ResumeLayout(false);
			this.tabErroresDirectiva.ResumeLayout(false);
			this.tabErroresPostEnsamblado.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.RichTextBox textS19;
		private System.Windows.Forms.ListBox listErroresPostEnsamblado;
		private System.Windows.Forms.TabPage tabErroresPostEnsamblado;
		private System.Windows.Forms.ComboBox cmbSalida;
		private System.Windows.Forms.RichTextBox textCodigoMaquina;
		private System.Windows.Forms.Label Ver;
		private System.Windows.Forms.ListBox listErroresDirectiva;
		private System.Windows.Forms.TabPage tabErroresDirectiva;
		private System.Windows.Forms.ListBox listErroresOperando;
		private System.Windows.Forms.TabPage tabErroresOperando;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbInfoCODOPS;
		private System.Windows.Forms.Button cmdCambiarTABOP;
		private System.Windows.Forms.Label lblTABOP;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem abrirArchivoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.OpenFileDialog abrirTABOP;
		private System.Windows.Forms.ListBox listErroresCODOP;
		private System.Windows.Forms.TabPage tabErroresCODOP;
		private System.Windows.Forms.ListBox listErroresSintaxis;
		private System.Windows.Forms.RichTextBox textSalida;
		private System.Windows.Forms.Button cmdEnsamblar;
		private System.Windows.Forms.Label lblNumerado;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog abrirArchivo;
		private System.Windows.Forms.GroupBox groupSalida;
		private System.Windows.Forms.GroupBox groupArchivo;
		private System.Windows.Forms.TabPage tabErroresSintaxis;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.MonthCalendar calendario;
		private System.Windows.Forms.ToolStripStatusLabel lblFecha;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.RichTextBox textArchivo;
	}
}

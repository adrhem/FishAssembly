namespace Assembly
{
	partial class Acerca_de___
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Acerca_de___));
			this.lblNombre = new System.Windows.Forms.Label();
			this.lblEmail = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.cmdOK = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblNombre
			// 
			this.lblNombre.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNombre.Location = new System.Drawing.Point(139, 87);
			this.lblNombre.Name = "lblNombre";
			this.lblNombre.Size = new System.Drawing.Size(183, 23);
			this.lblNombre.TabIndex = 0;
			this.lblNombre.Text = "Julio Adrián Hernández Méndez";
			this.lblNombre.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// lblEmail
			// 
			this.lblEmail.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblEmail.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEmail.ForeColor = System.Drawing.Color.SteelBlue;
			this.lblEmail.Location = new System.Drawing.Point(139, 110);
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.Size = new System.Drawing.Size(183, 13);
			this.lblEmail.TabIndex = 0;
			this.lblEmail.Text = "bin.cat.service@gmail.com";
			this.lblEmail.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.lblEmail.Click += new System.EventHandler(this.LblEmailClick);
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(12, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(310, 23);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "Taller de Programación de Sistemas";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(12, 110);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(56, 23);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "Aceptar";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.CmdOKClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
			this.pictureBox1.Location = new System.Drawing.Point(15, 36);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// Acerca_de___
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 142);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.lblEmail);
			this.Controls.Add(this.lblNombre);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(350, 180);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(350, 180);
			this.Name = "Acerca_de___";
			this.Opacity = 0.98D;
			this.Text = "Acerca de...";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.Label lblNombre;
	}
}

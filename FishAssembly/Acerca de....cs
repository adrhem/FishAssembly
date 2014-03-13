using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Assembly
{
	public partial class Acerca_de___ : Form
	{
		public Acerca_de___()
		{
			InitializeComponent();
		}
		
		void CmdOKClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void LblEmailClick(object sender, EventArgs e)
		{
			
   			Process.Start("mailto: " + lblEmail.Text); 		
		}
	}
}

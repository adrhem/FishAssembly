/*
 * Julio Adrián Hernández Méndez
 * Código: 208769265
 */
// Assembly for the Architecture HC12
// Copyright (C)2013
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Text;

namespace Assembly
{
	public partial class MainForm : Form
	{
		Int32 dir_inic, contloc; //Dirección inicial y contador de localidades.
		String variante; //Variable que s eutiliza para guardar el nombre de la variante del IDX.
		String rutaTemporal = "", rutaTABSIM = "";
		List<CODOP> codops = new List<CODOP>(); //Variable que guarda referencias de los CODOPS recuperados.
		List<referenciaModoDir> referencias = new List<referenciaModoDir>(); //Este guarda los modos de direccionamiento para el 2° paso del ensamblador
		bool seCargoArchivo = false, seCargoTABOP = true;
		//Estos son cudros de texto que guardan la información durante el programa.
		RichTextBox tabop = new RichTextBox(), temporal = new RichTextBox(), tabsim = new RichTextBox();
		int endsEncontrados = 0, orgsEncontrados = 0;
		bool ensambladoCorrecto;
		public MainForm()
		{
			InitializeComponent();
			
			//La etiquera numerada para mostrar el número de linea
			lblNumerado.Font = new Font(textArchivo.Font.FontFamily, 
                              textArchivo.Font.Size);
			lblNumerado.Location = new Point(6,56);
			tabop.LoadFile("TABOP.txt", RichTextBoxStreamType.PlainText);
		   	lblTABOP.Text = "TABOP.txt";
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			muestraFecha();
		}
		
		void muestraFecha()
		{
			lblFecha.Text = "Hoy es " + daDia((int)calendario.TodayDate.DayOfWeek) + ", " +
							calendario.TodayDate.Day + "/" +
							calendario.TodayDate.Month + "/" +
							calendario.TodayDate.Year ;				
		}
		
		String daDia(int dia)
		{
			String cadena = "";
			switch(dia)
			{
				case 1:
					cadena = "Lunes";
					break;
				case 2:
					cadena = "Martes";
					break;
				case 3:
					cadena = "Miércoles";
					break;
				case 4:
					cadena = "Jueves";
					break;
				case 5:
					cadena = "Viernes";
					break;
				case 6:
					cadena = "Sábado";
					break;
				case 7:
					cadena = "Domingo";
					break;
			}
			
			return cadena;
		}
		
		
		void AcercaDeToolStripMenuItemClick(object sender, EventArgs e)
		{
			Acerca_de___ x = new Acerca_de___();
			x.ShowDialog();
		}
		
		void SalirToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void CargarArchivoToolStripMenuItemClick(object sender, EventArgs e)
		{
			cargaArchivo();
		}
		
		void cargaArchivo()
		{
			if(abrirArchivo.ShowDialog() == DialogResult.OK)
			{
			   	textArchivo.LoadFile(abrirArchivo.FileName, RichTextBoxStreamType.PlainText);
			   	groupArchivo.Text = Path.GetFileName(abrirArchivo.FileName);
			   	seCargoArchivo = true;
			}
			else
				seCargoArchivo = false;
		}
		
		void cargaTABOP()
		{
			if(!seCargoTABOP && abrirTABOP.ShowDialog() == DialogResult.OK)
			{
			   	tabop.LoadFile(abrirTABOP.FileName, RichTextBoxStreamType.PlainText);
			   	lblTABOP.Text = Path.GetFileName(abrirTABOP.FileName);
			   	seCargoTABOP = true;
			}
		}
		
		//Esta función es un escuchador del evento scroll del archivo principal
		void TextArchivoVScroll(object sender, EventArgs e)
		{
		    int d = textArchivo.GetPositionFromCharIndex(0).Y % 
		                              (textArchivo.Font.Height + 1);
		    lblNumerado.Location = new Point(6, 56 + d);
		
		    actualizarlblNumerado();
		}
		
		//Esta función cambia los valores que tiene el scroll
		void actualizarlblNumerado()
		{
		    Point pos = new Point(0, 0);
		    int firstIndex = textArchivo.GetCharIndexFromPosition(pos);
		    int firstLine = textArchivo.GetLineFromCharIndex(firstIndex);
		
		    pos.X = ClientRectangle.Width;
		    pos.Y = ClientRectangle.Height;
		    int lastIndex = textArchivo.GetCharIndexFromPosition(pos);
		    int lastLine = textArchivo.GetLineFromCharIndex(lastIndex);

		    pos = textArchivo.GetPositionFromCharIndex(lastIndex);

		    lblNumerado.Text = "";
		    for (int i = firstLine; i <= lastLine + 1; i++)
		    {
		        lblNumerado.Text += i + 1 + "\n";
		    }
		
		}
		
		//Función que determina si se trata de una directiva, ya que son menos y nos ahora el trabajo
		//para no buscar todos los codops disponibles.
		bool esDirectiva(bool esPrimeraLinea, int linea,String etiqueta,String codop, String operando)
		{
			if(esPrimeraLinea && codop.ToUpper() == "EQU")
			{
				dir_inic = 0;
				contloc = dir_inic;
			}
			
			else if(esPrimeraLinea && codop.ToUpper() != "ORG")
			{
				aplicaErrorDirectiva("La primera línea, diferente de comentario, deber ser ORG. Se tomará la dirección inicial en 0.");
				dir_inic = 0;
				contloc = dir_inic;
			}
			
			bool directiva = false;;
			switch(codop.ToUpper())
			{
				case "DB":
				case "DC.B":
				case "FCB":
					if(evaluaOperandoDirectiva(codop.ToUpper(),255,linea,codop,operando))
					{
						guardaReferencia(codop.ToUpper(),"DIRECTIVA");
						contloc = contloc + 1;
					}
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". " + codop.ToUpper() + ". Error, no se aumentará el CONTLOC.");
					directiva = true;
					break;
					
				case "DW":
				case "DC.W":
				case "FDB":
					if(evaluaOperandoDirectiva(codop.ToUpper(),65535,linea,codop,operando))
					{
						guardaReferencia(codop.ToUpper(),"DIRECTIVA");
						contloc = contloc + 2;
					}
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". " + codop.ToUpper() + ". Error, no se aumentará el CONTLOC.");
					directiva = true;
					break;
				case "FCC":
					if(operando.Length >= 2)
					{
						if(operando.ToCharArray()[0] == '"' && operando.ToCharArray()[operando.Length - 1] == '"')
						{
							guardaReferencia(codop.ToUpper(),"DIRECTIVA");
							contloc = contloc + (operando.Length - 2);
						}
						else
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". " + codop.ToUpper() + ". La primer y ultima posición del operando deben ser el caracter \".");
					}
					else
						aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". " + codop.ToUpper() + ". La longitud del operando debe ser mayor o igual a 2.");
					directiva = true;
					break;
					
				case "DS":
				case "DS.B":
				case "RMB":
					if(evaluaOperandoDirectiva(codop.ToUpper(),65535,linea,codop,operando))
						contloc = contloc + retornaNumero(operando);
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". " + codop.ToUpper() + ". Error, no se aumentará el CONTLOC.");
					directiva = true;
					directiva = true;
					break;
					
				case "DS.W":
				case "RMW":
					if(evaluaOperandoDirectiva(codop.ToUpper(),65535,linea,codop,operando))
						contloc = contloc + 2*retornaNumero(operando);
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". " + codop.ToUpper() + ". Error, no se aumentará el CONTLOC.");
					directiva = true;
					directiva = true;
					break;
				
				case "EQU":
					if(etiqueta != "" && operando != "")
						evaluaOperandoDirectiva(codop.ToUpper(),65535,linea,codop,operando);
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". EQU. El operando y la etiqueta deben ser diferentes de null.");
					directiva = true;
					break;
				case "END":
					if(operando!="")
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". El operando del END debe ser nulo.");
					endsEncontrados++;
					directiva = true;
					break;
				case "ORG":
					orgsEncontrados++;
					if(esPrimeraLinea)
						evaluaOperandoORG(linea,etiqueta,operando);
					else
						aplicaErrorDirectiva("Linea " + linea.ToString("000") + ". Se ignorará este ORG ya que no está en la primer linea.");
					directiva = true;
					break;
			}
			
			return directiva;
		}
		
		//Este es el escuchador del evento analizar
		void CmdAnalizarClick(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			Ensamblar();
			textSalida.Text += "\n Total de Bytes Ensamblados: " + (contloc - dir_inic).ToString();
			if(ensambladoCorrecto && seCargoArchivo){
				leeArchivoTemporal();
				generaS19();
			}
				
			this.Cursor = Cursors.Default;
		}
		
		void generaS19()
		{
			generaS0();
			generaS1();
			generaS9();
			guardaS19();
		}
		
		void generaS0()
		{
			BaseObjeto s0 = new BaseObjeto();
			s0.direccion_inicial = "0000";
			byte[] asciiBytes = Encoding.ASCII.GetBytes(abrirArchivo.FileName);
			int i = 0;
			s0.datos = asciiBytes[i].ToString("X");
			for(i = 1 ; i < asciiBytes.Length ; i++)
				s0.datos += asciiBytes[i].ToString("X");
			s0.datos += "0A";
			s0.longitud = calculaLongitud(s0);
			s0.checksum = calculaChecksum(s0);
			textS19.Text += "S0" + s0.to_string();
		}
		
		void generaS1()
		{
			int longitud_restante = 16;
			BaseObjeto s1 = null;
			int i;
			List<LineaCodigoMaquina> lineas = obtieneLineasCodigoMaquina();
			bool esDirectivaDeIncremento = false;
			foreach(LineaCodigoMaquina linea in lineas)
			{				
				switch(linea.codop.ToUpper())
				{
					case "ORG":
						s1 = new BaseObjeto();
						s1.direccion_inicial = linea.contloc;
						break;
					case "DS":
					case "DS.B":
					case "RMB":
					case "DS.W":
					case "RMW":
					case "END":
						esDirectivaDeIncremento = true;
						if(s1.datos != "")
						{
							s1.longitud = calculaLongitud(s1);
							s1.checksum = calculaChecksum(s1);
							textS19.Text+= "S1" + s1.to_string();
							s1 = new BaseObjeto();
						}
						else
							s1 = new BaseObjeto();
						
						break;
					case "EQU":
						break;
					default:
						if(esDirectivaDeIncremento)
						{
							s1 = new BaseObjeto();
							s1.direccion_inicial = linea.contloc;
							longitud_restante = 16;
						}
						esDirectivaDeIncremento = false;
						if(longitud_restante == 0)
						{
							if(s1.datos != "")
							{
								s1.longitud = calculaLongitud(s1);
								s1.checksum = calculaChecksum(s1);
								textS19.Text+= "S1" + s1.to_string();
								s1 = new BaseObjeto();
								s1.direccion_inicial = linea.contloc;
								longitud_restante = 16;
							}
							else
								s1 = new BaseObjeto();
								
						}
						
						for( i = 0; longitud_restante > 0 && i < linea.codigo_maquina.Length;i = i + 2)
						{
							s1.datos+= linea.codigo_maquina.Substring(i,2);
							longitud_restante--;
						}
						
						//Esta parte se valida, ya que a veces no termina toda la linea de codigo máquina y es
						// por si queda espacio para capturlo
						if(longitud_restante == 0 && i < linea.codigo_maquina.Length)
						{
							s1.longitud = calculaLongitud(s1);
							s1.checksum = calculaChecksum(s1);
							textS19.Text+= "S1" + s1.to_string();
							s1 = new BaseObjeto();
							s1.direccion_inicial = (Convert.ToInt32(linea.contloc,16) + i/2).ToString("X4");
							longitud_restante = 16;
							while( longitud_restante > 0 && i < linea.codigo_maquina.Length)
							{
								s1.datos += linea.codigo_maquina.Substring(i,2);
								longitud_restante--;
								i = i + 2;
							}
						}
						break;
				}
			}
			
		}
		
		//Esta función carga toda la información del archivo temporal guardandola en arreglos
		//Para facilitar el uso en la generación del código objeto
		List<LineaCodigoMaquina> obtieneLineasCodigoMaquina()
		{
			int i, j;
			String c = "";
			String linea = "";
			LineaCodigoMaquina linea_actual;
			List<LineaCodigoMaquina> lineas = new List<LineaCodigoMaquina>();
			i = 0;
			while( i < textCodigoMaquina.Lines.Length - 1 )
			{
				linea_actual = new LineaCodigoMaquina();
				linea = textCodigoMaquina.Lines[i];
				j = 0;
				linea_actual.contloc = linea.Substring(j,4);
				j = 5;
				c = linea.Substring(j,1);
				while(j < linea.Length && c!= "	")
				{
					linea_actual.etiqueta += c;
					j++;
					c = linea.Substring(j,1);
				}
				j++;
				c = linea.Substring(j,1);
				while(j < linea.Length && c!= "	")
				{
					linea_actual.codop += c;
					j++;
					c = linea.Substring(j,1);
				}
				j++;
				c = linea.Substring(j,1);
				while(j < linea.Length && c!= "	")
				{
					linea_actual.operando += c;
					j++;
					c = linea.Substring(j,1);
				}
				while(c == "	")
				{
					j++;
					c = linea.Substring(j,1);
				}
				linea_actual.codigo_maquina += linea.Substring(j,linea.Length-j);		
				lineas.Add(linea_actual);
				i++; 
			}
			
			return lineas;
		}
		
		void generaS9()
		{
			BaseObjeto s0 = new BaseObjeto();
			
			s0.direccion_inicial = "0000";
			s0.datos = "";
			s0.longitud = "03";
			s0.checksum = calculaChecksum(s0);
			textS19.Text += "S9" + s0.to_string();
		}
		
		void guardaS19()
		{
			String pathS19 = Path.GetDirectoryName(abrirArchivo.FileName) + "\\" + Path.GetFileNameWithoutExtension(abrirArchivo.FileName) + ".S19";
			System.IO.StreamWriter archivo_s19 = new StreamWriter(pathS19);
			foreach(String i in textS19.Lines)
				archivo_s19.WriteLine(i);
			archivo_s19.Close();
			
		}
		
		String calculaLongitud(BaseObjeto x)
		{
			String longitud = "";
			String total = x.direccion_inicial + x.datos;
			longitud = (total.Length/2 + 1).ToString("X2");
			return longitud;
		}
		
		String calculaChecksum(BaseObjeto x)
		{
			String checksum = "";
			String total = x.longitud + x.direccion_inicial + x.datos;
			int suma = 0;
			
			for(int i = 0; i < total.Length ; i=i+2)
				suma += Convert.ToInt32(total.Substring(i,2),16);
			
			suma = 65535 - suma;
			
			checksum = suma.ToString("X2");
			checksum = checksum.Substring(checksum.Length-2,2);
			return checksum;
		}
		
		void inicializar()
		{
			//Limpia en caso d que haya sido cargado otro archivo.
			//Es decir, para que no cargue los errores del archivo anterior
			ensambladoCorrecto = true;
			codops.Clear();
			listErroresSintaxis.Items.Clear();
			listErroresCODOP.Items.Clear();
			listErroresOperando.Items.Clear();
			listErroresDirectiva.Items.Clear();
			listErroresPostEnsamblado.Items.Clear();
			cmbInfoCODOPS.Items.Clear();
			referencias.Clear();
			cmbSalida.SelectedIndex = 0;
		}
		
		void Ensamblar()
		{	
			inicializar();
			if(seCargoArchivo && seCargoTABOP) //Primero verifica si se cargó el archivo
			{
				List<String> etiquetas = new List<String>();
				int i, j, k;
				Char c;
				String etiqueta = "", codop = "", operando = "";
				textSalida.Text = "";
				textCodigoMaquina.Text = "";
				textS19.Text = "";
				temporal.Text = "";
				tabsim.Text = "";
				temporal.AcceptsTab = true;
				tabsim.AcceptsTab = true;
				bool continuaLinea = true;
				bool yaEstaCargado;
				bool esPrimeraLinea = true;
				endsEncontrados = 0;
				orgsEncontrados = 0;
				
				for ( i = 0 ; i < textArchivo.Lines.Length ; i++ )
				{
					yaEstaCargado = false;
					c = ' ';
					etiqueta = "";
					codop = "";
					operando = "";
					j=0;
					if ( textArchivo.Lines[i].Length > 0 )
					{
						c = textArchivo.Lines[i].ToCharArray()[j];
						continuaLinea = true;
						
						if ( c == ';' )
						{
							textSalida.Text += "Comentario\n\n";
						}
						
						else 
						{
							//Este While captura la etiqueta
							while((!Char.IsWhiteSpace(c) && c != (char)9) && continuaLinea)
							{
								etiqueta += c.ToString();
								j++;
								if( j < textArchivo.Lines[i].ToCharArray().Length )
									c = textArchivo.Lines[i].ToCharArray()[j];
								else
									continuaLinea = false;

							}
							
							//Este while quita los espacios en blancos y tabuladores
							while((Char.IsWhiteSpace(c) || c == (char)9) && continuaLinea)
							{
								j++;
								if( j < textArchivo.Lines[i].ToCharArray().Length )
									c = textArchivo.Lines[i].ToCharArray()[j];
								else
									continuaLinea = false;
							}	
							
							
							//Este while captura el Codop
							while((!Char.IsWhiteSpace(c) && c != (char)9) && continuaLinea)
							{
								codop += c.ToString();
								j++;
								if( j < textArchivo.Lines[i].ToCharArray().Length )
									c = textArchivo.Lines[i].ToCharArray()[j];
								else
									continuaLinea = false;
							}
							
							//Este while quita los espacios en blancos y tabuladores
							while((Char.IsWhiteSpace(c) || c == (char)9) && continuaLinea)
							{
								j++;
								if( j < textArchivo.Lines[i].ToCharArray().Length )
									c = textArchivo.Lines[i].ToCharArray()[j];
								else
									continuaLinea = false;
							}
							
							//Este while captura el operando
							while(c != '\0' && continuaLinea)
							{
								operando += c.ToString();
								j++;
								if( j < textArchivo.Lines[i].ToCharArray().Length )
									c = textArchivo.Lines[i].ToCharArray()[j];
								else
									continuaLinea = false;
							}
							// En dado caso que la linea no esté vacía
							if(!(etiqueta == "" && codop == "" && operando == "")) 
							{
								int guardaCONTLOC;
								
								guardaCONTLOC = contloc;
								
								bool llevaOperando = false;
								evaluarEtiqueta(etiqueta,i+1);
								evaluarCODOP(codop,i+1);
								
								imprimeEnSalida(etiqueta, codop, operando);
								
								if(!esDirectiva(esPrimeraLinea,i + 1,etiqueta,codop,operando))
								{
									for(k = 0; k < codops.Count && !yaEstaCargado; k++)
										if(codops[k].nombre.ToUpper() == codop.ToUpper())
											yaEstaCargado = true;
									if(yaEstaCargado)
										llevaOperando = encuentraErroresOperando(codop,operando,i+1, k-1);
									else
										llevaOperando = CODOPLlevaOperando(codop,operando,i+1);
									
									if(operando == ""){
										
										int l = 0;
										while( l < codops.Count && codops[l].nombre.ToUpper() != codop.ToUpper())
											l++;
										
										if(l < codops.Count && codops[l].nombre.ToUpper() == codop.ToUpper())
											imprimeModoDireccionamiento(l,0);
										
										contloc = contloc + codops[l].info[0].b_total;
										textSalida.Text += "\n";
									}
										
									else
									{
										if(llevaOperando)
											evaluarOperando(codop, operando,i+1);
										else
											textSalida.Text += "\n";
									}
								}
								if(codop.ToUpper() != "EQU")
									esPrimeraLinea = false;
								
								if(contloc>65535)
									aplicaErrorDirectiva("CONTLOC. El contador de localidades se desboró.");
								
								if(codop.ToUpper() == "ORG")
									guardaCONTLOC = dir_inic;
								if(etiqueta != "")
								{
									int z = 0;
									while(z < etiquetas.Count && etiquetas[z]!= etiqueta)
										z++;
									if(z < etiquetas.Count)
										aplicaErrorDirectiva("Linea " + (i+1).ToString("000") + ". No puede haber etiquetas repetidas.");
									else
										etiquetas.Add(etiqueta);
								}
							
								operacionConArchivos(guardaCONTLOC,etiqueta,codop,operando);
							}//Nulos
						}
					}//Enf Lines[i] > 0
					
				}
				
				aplicaErroresEND_ORG();
				renombraTabs();
				guardaArchivos();
			}
			else
				MessageBox.Show("Debe cargarse un archivo y el TABOP para analizarlo.","Advertencia",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		
		}
		
		void leeArchivoTemporal()
		{
			String valor = "", etiqueta = "", codop = "", operando = "";
			String linea = "";
			Char c;
			int i;
			
			try
			{
				using (StreamReader archivo = new StreamReader(rutaTemporal)) 
				{
				    while (archivo.Peek() >= 0) 
				    {
				    	valor = "";
				    	etiqueta = "";
				    	codop = "";
				    	operando = "";
				    
				        linea = archivo.ReadLine();
				        
				        if(linea.Length > 0)
				        {
				        	valor = linea.Substring(0,4);
				        	
					        //Es 5 porque lee el hexadecimal de 4 Bytes y el tabulador.
					        i = 5;
					        c = linea.ToCharArray()[i];
					       	while(c != (char)9 && i < linea.Length)
							{
								etiqueta += c.ToString();
								i++;
								if( i < linea.Length )
									c = linea.ToCharArray()[i];
								else
									break;
							}
					       	
					       	i++;
					       	c = linea.ToCharArray()[i];
					       	
					       	while(c != (char)9 && i < linea.Length)
							{
								codop += c.ToString();
								i++;
								if( i < linea.Length )
									c = linea.ToCharArray()[i];
								else
									break;
							}
					       	
					       	i++;
					       	c = linea.ToCharArray()[i];
					       	
					       	while(c != '\0' && i < linea.Length)
							{
								operando += c.ToString();
								i++;
								if( i < linea.Length )
									c = linea.ToCharArray()[i];
								else
									break;
							}
					       	
					       	generaCodigoMaquina(valor,etiqueta,codop,operando);
				        }
				    }
				}
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.Message,"Advertencia",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
			if(listErroresPostEnsamblado.Items.Count > 0)
			{
				tabErroresPostEnsamblado.Text = "Errores Post Ensamblado (" + listErroresPostEnsamblado.Items.Count + ")";
				ensambladoCorrecto = false;
			}
			else
			{
				tabErroresPostEnsamblado.Text = "Errores Post Ensamblado (0)";
				listErroresPostEnsamblado.Items.Add("¡Correcto!");
			}
			
			textCodigoMaquina.SaveFile(rutaTemporal,RichTextBoxStreamType.UnicodePlainText);
		}


		void generaCodigoMaquina(String valor,String etiqueta,String codop,String operando)
		{
			int i;
			String codigo_maquina = "", formato = "";
			i = 0;
			int bytes = 0;
			Char c;
			textCodigoMaquina.Text += valor + "\t" + etiqueta + "\t" + codop + "\t" + operando;
			//Esto lo hace solo para que se vea estético
			if(operando.Length>=6)
				textCodigoMaquina.Text += "\t";	
			else
				textCodigoMaquina.Text += "\t\t";	
			//Aquí busca las referencias previamente guardadas para evitar volver a leer todo el archivo
			while(i < referencias.Count && referencias[i].contloc.ToUpper() != valor.ToUpper())
				i++;
			if(i<referencias.Count && (referencias[i].contloc.ToUpper() == valor.ToUpper() && (referencias[i].codop.ToUpper() == codop.ToUpper())))
			{
				//Primero evalua si no se trata de una directiva
				if(referencias[i].modo_direccionamiento!="DIRECTIVA")
				{
					codigo_maquina = codops[referencias[i].iCODOP].info[referencias[i].iModoDir].codigo_maquina;
					bytes = codops[referencias[i].iCODOP].info[referencias[i].iModoDir].b_calcular;
					if(bytes>1)
						formato = "X" + (bytes*2).ToString();
					else
						formato = "X2";
					switch(codops[referencias[i].iCODOP].info[referencias[i].iModoDir].modo_direccionamiento.ToUpper())
					{
						case "INHERENTE":
							break;
						case "DIRECTO":
							codigo_maquina += retornaNumero(operando).ToString(formato);
							break;
						case "EXTENDIDO":
							c = operando.ToCharArray()[0];
							if(Char.IsLetter(c) || c=='_')
							{
								bool encontroResultado = false;
								String etiquetabusca = "", linea = "";
								int x;
								using (StreamReader archivo = new StreamReader(rutaTABSIM)) 
								{
								    while (archivo.Peek() >= 0 && !encontroResultado) 
								    {
								    	etiquetabusca = "";
								        linea = archivo.ReadLine();
								        x = 0;
								        if(x<linea.Length)
								        	c = linea.ToCharArray(x,1)[0];
								        while(x < linea.Length && c!=(char)9)
								        {
								        	etiquetabusca += c.ToString();
								        	x++;
								        	if(x<linea.Length)
								        		c = linea.ToCharArray(x,1)[0];
								        }
								        x++;
								        if(etiquetabusca.ToUpper() == operando.ToUpper())
								        {
								        	encontroResultado = true;
								        	codigo_maquina+=  linea.Substring(x);
								        }
								    }
								}
								
								if(!encontroResultado)
									listErroresPostEnsamblado.Items.Add("No se encontró la la etiqueta "+ operando.ToUpper() +" en el TABSIM");
							}
							else
								codigo_maquina += retornaNumero(operando).ToString(formato);
							break;
						case "INMEDIATO":
							codigo_maquina += retornaNumero(operando.Substring(1)).ToString(formato);
							break;
						case "IDX":
							switch(referencias[i].variante)
							{
								case "Indizado de 5 bits":
									codigo_maquina += xbIndizado5bits(operando);
									break;
								case "Indizado de Acumulador":
									codigo_maquina += xbIndizadoAcumulador(operando);
									break;
								case "Indizado Pre Incremento":
									codigo_maquina += xbIndizadoPre_Inc(operando);
									break;
								case "Indizado Pre Decremento":
									codigo_maquina += xbIndizadoPre_Dec(operando);
									break;
								case "Indizado Post Incremento":
									codigo_maquina += xbIndizadoPost_Inc(operando);
									break;
								case "Indizado Post Decremento":
									codigo_maquina += xbIndizadoPost_Dec(operando);
									break;
							}
							break;
						case "IDX1":
							codigo_maquina += xbIDX1(operando);
							break;
						case "IDX2":
							codigo_maquina += xbIDX2(operando);
							break;
						case "[D,IDX]":
							codigo_maquina += xbIndizadoAcumuladorD(operando);
							break;
						case "[IDX2]":
							codigo_maquina += xb16BitsIndirecto(operando);
							break;
						case "REL":
							codigo_maquina += calculaDesplazamiento(referencias[i].contloc,operando,
							                  codops[referencias[i].iCODOP].info[referencias[i].iModoDir].b_total,
							                  bytes);
							break;
						default:
							break;
					}
					
					textCodigoMaquina.Text += codigo_maquina;
				}
				//En caso contrario que no se trate de una directiva
				else if(referencias[i].modo_direccionamiento == "DIRECTIVA")
				{
					switch(referencias[i].codop.ToUpper())
					{
						case "DB":
						case "DC.B":
						case "FCB":
							codigo_maquina = retornaNumero(operando).ToString("X2");
							if(codigo_maquina.Length>2)
								codigo_maquina = codigo_maquina.Substring(codigo_maquina.Length-2,2);
							break;
							
						case "DW":
						case "DC.W":
						case "FDB":
							codigo_maquina = retornaNumero(operando).ToString("X4");
							if(codigo_maquina.Length>4)
								codigo_maquina = codigo_maquina.Substring(codigo_maquina.Length-4,4);
							break;
							
						case "FCC":
							operando = operando.Substring(1,operando.Length-2);
							byte[] asciiBytes = Encoding.ASCII.GetBytes(operando);
							int z = 0;
							codigo_maquina = asciiBytes[z].ToString("X");
							for(z = 1 ; z < asciiBytes.Length ; z++)
								codigo_maquina += asciiBytes[z].ToString("X");
							
							break;
					}
					
					textCodigoMaquina.Text += codigo_maquina;
				}
					
			}
			
			else
				textCodigoMaquina.Text += "No Encontrado";
			
			textCodigoMaquina.Text += "\n";
		}
		
		String calculaDesplazamiento(String contloc_actual, String operando, int total_bytes, int bytes_calcular)
		{
			String desplazamiento = "", linea;
			int i=0;
			Char c = (Char)9;
			String etiqueta = "";
			String valor_etiqueta = "";
			bool encontroResultado = false;
			int desplazamiento_num = 0;
			try
			{
				using (StreamReader archivo = new StreamReader(rutaTABSIM)) 
				{
				    while (archivo.Peek() >= 0 && !encontroResultado) 
				    {
				    	etiqueta = "";
				    	valor_etiqueta = "";
				        linea = archivo.ReadLine();
				        i = 0;
				        if(i<linea.Length)
				        	c = linea.ToCharArray(i,1)[0];
				        while(i < linea.Length && c!=(char)9)
				        {
				        	etiqueta += c.ToString();
				        	i++;
				        	if(i<linea.Length)
				        		c = linea.ToCharArray(i,1)[0];
				        }
				        
				        if(etiqueta.ToUpper() == operando.ToUpper())
				        {
				        	encontroResultado = true;
				        	i++;
				        	valor_etiqueta = linea.Substring(i);
				        	desplazamiento_num = retornaNumero("$" + valor_etiqueta) - 
				        						(retornaNumero("$" + contloc_actual) + total_bytes);
				        	switch(bytes_calcular)
				        	{
				        		case 1:
				        			if(desplazamiento_num >= -128 && desplazamiento_num < 128)
				        			{
				        				desplazamiento += desplazamiento_num.ToString("X2");
				        				desplazamiento = desplazamiento.Substring(desplazamiento.Length-2);
				        			}
				        				
				        			else
				        				listErroresPostEnsamblado.Items.Add("El desplazamiento del operando " + etiqueta + " debe estar entre -128 y 127");
				        			
				        			break;
				        		case 2:
				        			if(desplazamiento_num >= -32768 && desplazamiento_num < 32768)
				        			{
				        				desplazamiento += desplazamiento_num.ToString("X4");
				        				desplazamiento = desplazamiento.Substring(desplazamiento.Length-4);
				        			}
				        			else
				        				listErroresPostEnsamblado.Items.Add("El desplazamiento del operando " + etiqueta + " debe estar entre -32768 y 32767");
				        			break;
				        	}
				        }
				    }
				}
				
				
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.Message,"Advertencia",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
			finally
			{
				if(!encontroResultado)
					listErroresPostEnsamblado.Items.Add("No se encontró la la etiqueta "+ operando.ToUpper() +" en el TABSIM");
			}
			return desplazamiento;
		}
		
		//retorna el valor en bytes que tiene cada registro
		String valorRegistro(String registro)
		{
			String valor = "";
			switch(registro.ToUpper())
			{
				case "X":
					valor = "00";
					break;
				case "Y":
					valor = "01";
					break;
				case "SP":
					valor = "10";
					break;
				case "PC":
					valor = "11";
					break;
				case "A":
					valor = "00";
					break;
				case "B":
					valor = "01";
					break;
				case "D":
					valor = "10";
					break;
			}
			
			return valor;
		}
		
		//Transformade binario a hexadecimal
		String binaryToHex(String binary)
		{
			String val = "";
			switch(binary)
			{
				case "0000": val = "0"; break;
				case "0001": val = "1"; break;
				case "0010": val = "2"; break;
				case "0011": val = "3"; break;
				case "0100": val = "4"; break;
				case "0101": val = "5"; break;
				case "0110": val = "6"; break;
				case "0111": val = "7"; break;
				case "1000": val = "8"; break;
				case "1001": val = "9"; break;
				case "1010": val = "A"; break;
				case "1011": val = "B"; break;
				case "1100": val = "C"; break;
				case "1101": val = "D"; break;
				case "1110": val = "E"; break;
				case "1111": val = "F"; break;
			}
			return val;
		}
		
		//Retorna el xb si se trata d eun indizado de 5 bits
		String xbIndizado5bits(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			if(i < operando.Length && operando.Substring(i,1)!=",")
			{
				while(i < operando.Length && operando.Substring(i,1)!=",")
				{
					numeroStr += operando.Substring(i,1);
					i++;
				}
				numero = Int32.Parse(numeroStr);
			}
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			xb += valorRegistro(registro) + "0";
			numeroStr = Convert.ToString(numero,2);
			if(numeroStr.Length>5)
				numeroStr = numeroStr.Substring(numeroStr.Length-5,5);
			else
			{
				numeroStr = numeroStr.PadLeft(5, '0');
			}
			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un indizado de acumulador
		String xbIndizadoAcumulador(String operando)
		{
			String xb = "";
			int i = 0;
			String registro2 = "", registro1 = "";
			
			registro1 += operando.Substring(i,1);
			i++;
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro2 += operando.Substring(i,1);
				i++;
			}			
			xb += "111" + valorRegistro(registro2) + "1" + valorRegistro(registro1);
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un indizado pre incremento
		String xbIndizadoPre_Inc(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			while(i < operando.Length && operando.Substring(i,1)!=",")
			{
				numeroStr += operando.Substring(i,1);
				i++;
			}
			numero = Int32.Parse(numeroStr);
			i++;
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += valorRegistro(registro) + "10";
			numeroStr = Convert.ToString(numero-1,2);
			if(numeroStr.Length>4)
				numeroStr = numeroStr.Substring(numeroStr.Length-4,4);
			else
			{
				numeroStr = numeroStr.PadLeft(4, '0');
			}
			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un indizado pre decremento
		String xbIndizadoPre_Dec(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			while(i < operando.Length && operando.Substring(i,1)!=",")
			{
				numeroStr += operando.Substring(i,1);
				i++;
			}
			numero = Int32.Parse(numeroStr);
			i++;
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += valorRegistro(registro) + "10";
			numeroStr = Convert.ToString(numero*(-1),2);
			if(numeroStr.Length>4)
				numeroStr = numeroStr.Substring(numeroStr.Length-4,4);
			else
			{
				numeroStr = numeroStr.PadLeft(4, '0');
			}
			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un indizado post incremento
		String xbIndizadoPost_Inc(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			while(i < operando.Length && operando.Substring(i,1)!=",")
			{
				numeroStr += operando.Substring(i,1);
				i++;
			}
			numero = Int32.Parse(numeroStr);
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="+")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += valorRegistro(registro) + "11";
			numeroStr = Convert.ToString(numero-1,2);
			if(numeroStr.Length>4)
				numeroStr = numeroStr.Substring(numeroStr.Length-4,4);
			else
			{
				numeroStr = numeroStr.PadLeft(4, '0');
			}
			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un indizado post decremento
		String xbIndizadoPost_Dec(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			while(i < operando.Length && operando.Substring(i,1)!=",")
			{
				numeroStr += operando.Substring(i,1);
				i++;
			}
			numero = Int32.Parse(numeroStr);
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="-")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += valorRegistro(registro) + "11";
			numeroStr = Convert.ToString(numero*(-1),2);
			if(numeroStr.Length>4)
				numeroStr = numeroStr.Substring(numeroStr.Length-4,4);
			else
			{
				numeroStr = numeroStr.PadLeft(4, '0');
			}
			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		
		//Retorna el xb si se trata de un IDX1
		String xbIDX1(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			if(i < operando.Length && operando.Substring(i,1)!=",")
			{
				while(i < operando.Length && operando.Substring(i,1)!=",")
				{
					numeroStr += operando.Substring(i,1);
					i++;
				}
				numero = Int32.Parse(numeroStr);
			}
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += "111" + valorRegistro(registro) + "00";
			if(numero>=0)
				xb += "0";
			else
				xb += "1";
			
			numeroStr = Convert.ToString(numero,2);
			if(numeroStr.Length>8)
				numeroStr = numeroStr.Substring(numeroStr.Length-8,8);
			else
				numeroStr = numeroStr.PadLeft(8, '0');

			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4)) + binaryToHex(xb.Substring(8,4)) + binaryToHex(xb.Substring(12,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un IDX2
		String xbIDX2(String operando)
		{
			String xb = "";
			int i = 0;
			int numero = 0;
			String registro = "", numeroStr = "";
			if(i < operando.Length && operando.Substring(i,1)!=",")
			{
				while(i < operando.Length && operando.Substring(i,1)!=",")
				{
					numeroStr += operando.Substring(i,1);
					i++;
				}
				numero = Int32.Parse(numeroStr);
			}
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += "111" + valorRegistro(registro) + "010";
			
			numeroStr = Convert.ToString(numero,2);
			if(numeroStr.Length>16)
				numeroStr = numeroStr.Substring(numeroStr.Length-16,16);
			else
				numeroStr = numeroStr.PadLeft(16, '0');

			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4)) + 
				 binaryToHex(xb.Substring(8,4)) + binaryToHex(xb.Substring(12,4)) +
				 binaryToHex(xb.Substring(16,4)) + binaryToHex(xb.Substring(20,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un [IDX2]
		String xb16BitsIndirecto(String operando)
		{
			String xb = "";
			int i = 1;
			int numero = 0;
			String registro = "", numeroStr = "";
			
			while(i < operando.Length && operando.Substring(i,1)!=",")
			{
				numeroStr += operando.Substring(i,1);
				i++;
			}
			numero = Int32.Parse(numeroStr);
			
			i++;
			while(i < operando.Length && operando.Substring(i,1)!="]")
			{
				registro += operando.Substring(i,1);
				i++;
			}			
			
			xb += "111" + valorRegistro(registro) + "011";
			
			numeroStr = Convert.ToString(numero,2);
			if(numeroStr.Length>16)
				numeroStr = numeroStr.Substring(numeroStr.Length-16,16);
			else
				numeroStr = numeroStr.PadLeft(16, '0');

			xb += numeroStr;
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4)) + 
				 binaryToHex(xb.Substring(8,4)) + binaryToHex(xb.Substring(12,4)) +
				 binaryToHex(xb.Substring(16,4)) + binaryToHex(xb.Substring(20,4));
			return xb;
		}
		
		//Retorna el xb si se trata de un [D,IDX]
		String xbIndizadoAcumuladorD(String operando)
		{
			String xb = "";
			int i = 3;
			String registro2 = "";

			while(i < operando.Length && operando.Substring(i,1)!="]")
			{
				registro2 += operando.Substring(i,1);
				i++;
			}			
			xb += "111" + valorRegistro(registro2) + "111";
			xb = binaryToHex(xb.Substring(0,4)) + binaryToHex(xb.Substring(4,4));
			return xb;
		}
		
		//Esta función determina como va a guarddar ciertos valores en el TABSIM y el temporal respectivamente
		void operacionConArchivos(int contloc, String etiqueta, String codop, String operando)
		{
			String temporalLine = "";
			String TABSIMLine = "";
			codop = codop.ToUpper();
			if(etiqueta != "")
			{
				if(codop == "EQU")
					TABSIMLine = etiqueta + "\t" + retornaNumero(operando).ToString("X4") + "\n";
				
				else
					TABSIMLine = etiqueta + "\t" + contloc.ToString("X4") + "\n";
			}
			
			if(etiqueta == "")
				etiqueta = "null";
			if(operando == "")
				operando = "null";
			
			if(codop!= "EQU")
				temporalLine = contloc.ToString("X4") + "\t" + etiqueta + "\t" + codop + "\t" + operando + "\n";
			
			else
				temporalLine = retornaNumero(operando).ToString("X4") + "\t" + etiqueta + "\t" + codop + "\t" + operando + "\n";
			
			temporal.Text += temporalLine;
			tabsim.Text += TABSIMLine;
		}
		
		//Esta función guarda los archivos temporal y TABSIM (Si lo hay)
		void guardaArchivos()
		{
			
			if(temporal.Text != "")
			{
				rutaTemporal =  Path.GetDirectoryName(abrirArchivo.FileName) + "\\" +	
           		Path.GetFileNameWithoutExtension(abrirArchivo.FileName) + "_temp.txt";
				
				StreamWriter SW = new System.IO.StreamWriter(rutaTemporal ,
           		false, Encoding.ASCII);
				for(int i = 0 ; i < temporal.Lines.Length ; i++)
					SW.WriteLine(temporal.Lines[i]);
      			SW.Close();
			}
				
			if(tabsim.Text != "")
			{
				rutaTABSIM = Path.GetDirectoryName(abrirArchivo.FileName) + "\\" +				                                             
					Path.GetFileNameWithoutExtension(abrirArchivo.FileName) + "_TABSIM.txt";
					
				StreamWriter SW = new System.IO.StreamWriter( rutaTABSIM, false, Encoding.ASCII);
      			for(int i = 0 ; i < tabsim.Lines.Length ; i++)
					SW.WriteLine(tabsim.Lines[i]);
      			SW.Close();
			
			}			
		}
		
		//Esta función aplica los errores en un listtextbox sobre el END y ORG
		void aplicaErroresEND_ORG()
		{
			if(endsEncontrados==0)
			{
				aplicaErrorSintaxis("No se encontró END.");
				aplicaErrorDirectiva("No se encontró END.");
			}
		
			else if (endsEncontrados>1)
			{
				aplicaErrorSintaxis("Hay más de un END en el programa");
				aplicaErrorDirectiva("Hay más de un END en el programa");
			}
			
			if(orgsEncontrados==0)
				aplicaErrorDirectiva("No se encontró ORG, la dirección inicial por default fue 0.");
		
			else if (orgsEncontrados>1)
				aplicaErrorDirectiva("Hay más de un ORG en el programa. Sólo se tomará en cuenta el primero.");
			
		}
		
		//Esta función imprime ciertos valores en el RichTextBox de salida
		void imprimeEnSalida(String etiqueta, String codop, String operando)
		{
			if(etiqueta == "")
				textSalida.Text += "Etiqueta: " + "null" + "\n";
			else
				textSalida.Text += "Etiqueta: " + etiqueta + "\n";
			
			if(codop == "")
				textSalida.Text += "CODOP: " + "null" + "\n";
			else
				textSalida.Text += "CODOP: " + codop + "\n";
			
			if(operando == "")
				textSalida.Text += "Operando: " + "null" + "\n\n";
			else
				textSalida.Text += "Operando: " + operando + "\n\n";
		}
		
		//Esta funcion renombra las pestañas para mostrar cuántos errores tiene.
		void renombraTabs()
		{
			if(listErroresSintaxis.Items.Count > 0)
			{
				tabErroresSintaxis.Text = "Errores de Sintaxis (" + listErroresSintaxis.Items.Count + ")";
				ensambladoCorrecto = false;
			}
			else
			{
				tabErroresSintaxis.Text = "Errores de Sintaxis (0)";
				listErroresSintaxis.Items.Add("¡Correcto!");
			}
			
			if(listErroresCODOP.Items.Count > 0)
			{
				tabErroresCODOP.Text = "Errores de CODOP (" + listErroresCODOP.Items.Count + ")";
				ensambladoCorrecto = false;
			}
			else
			{
				tabErroresCODOP.Text = "Errores de CODOP (0)";
				listErroresCODOP.Items.Add("¡Correcto!");
			}
			
			if(listErroresOperando.Items.Count > 0)
			{
				tabErroresOperando.Text = "Errores de Operando (" + listErroresOperando.Items.Count + ")";
				ensambladoCorrecto = false;
			}
			else
			{
				tabErroresOperando.Text = "Errores de Operando (0)";
				listErroresOperando.Items.Add("¡Correcto!");
			}
			
			if(listErroresDirectiva.Items.Count > 0)
			{
				tabErroresDirectiva.Text = "Errores de Directiva (" + listErroresDirectiva.Items.Count + ")";
				ensambladoCorrecto = false;
			}
			else
			{
				tabErroresDirectiva.Text = "Errores de Directiva (0)";
				listErroresDirectiva.Items.Add("¡Correcto!");
			}
	
		}
		
		
		void aplicaErrorSintaxis(String error)
		{
			listErroresSintaxis.Items.Add(error);
		}
		
		void aplicaErrorCODOP(String error)
		{
			listErroresCODOP.Items.Add(error);
		}
		
		void aplicaErrorOperando(String error)
		{
			listErroresOperando.Items.Add(error);
		}
		
		void aplicaErrorDirectiva(String error)
		{
			listErroresDirectiva.Items.Add(error);
		}
		
		//Esta función evalua la etiqueta, mostrando sus posibles errores y agregandolos al listtextbox
		void evaluarEtiqueta(String etiqueta, int linea)
		{
			Char c;
			int i = 0;
			bool hayCaracteresEspeciales = false;
			if(etiqueta.Length > 0)
			{
				c = etiqueta.ToCharArray()[i];
				if( !Char.IsLetter(c))
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", Etiqueta. Las etiquetas deben comenzar con letra.");
				
				for(i = 1; i < etiqueta.Length ; i++)
				{
					c = etiqueta.ToCharArray()[i];
					if( !Char.IsLetterOrDigit(c) && c != '_' )
						hayCaracteresEspeciales = true;
				}
				if(hayCaracteresEspeciales)
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", Etiqueta. Los caracteres válidos en las etiquetas son letras, digitos (0..9) y el guión bajo.");
					
				if(etiqueta.Length > 8)
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", Etiqueta. La longitud máxima de una etiqueta es de ocho caracteres.");
			}
		}
		
		//Esta función evalua la sintaxis de los CODOPS, mostrando sus posibles errores
		void evaluarCODOP(String codop, int linea)
		{
			Char c;
			int i = 0;
			int cantidadPuntos = 0;
			bool hayCaracteresEspeciales = false;
			if(codop.Length > 0)
			{
				c = codop.ToCharArray()[i];
				if( !Char.IsLetter(c))
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", CODOP. Los cógidos de operación deben comenzar con letra.");
				
				for(i = 1; i < codop.Length ; i++)
				{
					c = codop.ToCharArray()[i];
					if( !Char.IsLetter(c) && c!= '.')
						hayCaracteresEspeciales = true;

					if( c == '.' )
						cantidadPuntos++;
				}
				if(hayCaracteresEspeciales)
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", CODOP. Los caracteres válidos en los códigos de operación son letra y el carácter del punto.");
				
				if(cantidadPuntos>1)
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", CODOP. Los códigos de operación no pueden tener más de un punto.");
				
				if(codop.Length > 5)
					aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", CODOP. La longitud máxima de los códigos de operación es de cinco caracteres.");
			}
			else
				aplicaErrorSintaxis("Linea " + linea.ToString("000") + ", CODOP. Siempre debe haber un código de operación.");
		}
		
		//Esta funcion evalua si el CODOP es o no inherente, además de guardarlos en referencias para 
		//Posteriormente mostrarlos en un MessageBox.
		bool CODOPLlevaOperando(String codop, String operando, int linea)
		{
			
			String[] valores = {"","","","","","",""};
			int i;
			int j;
			int iValor;
			bool continuaBuscando = true, continuaLinea = true;
			bool CODOPLlevaOperando = false, encontroCODOP = false;
			bool hayMas = false;
			Char c = ' ';
			String nombre;
			String lector;
			CODOP aux_codop = new CODOP();
			InformacionCODOP aux_info = new InformacionCODOP();
			List<InformacionCODOP> infos = new List<InformacionCODOP>();
			
			if(codop.ToUpper() == "ORG" || codop.ToUpper() == "END")
				continuaBuscando = false;
			
			for ( i = 0 ; i < tabop.Lines.Length && continuaBuscando ; i++ )
			{
				
				nombre = "";
				continuaLinea = true;
				for ( j = 0 ; continuaBuscando && j < tabop.Lines[i].Length && continuaLinea; j++ )
				{
					c = tabop.Lines[i].ToCharArray()[j];
					if(c!='|')
						nombre += c.ToString();
					else
					{
						
						if(nombre.ToUpper() == codop.ToUpper()) //Lo encontró
						{
							if(hayMas)
							{
								for(int k = 0; k < 7 ; k++)
								{
									valores[k] += ", ";
								}
							}
							hayMas = true;
							encontroCODOP = true;
							continuaLinea = false;
							//Comienza a capturar los demás valores
							j++;
							iValor=1;
							valores[0] = nombre;
							aux_codop.nombre = nombre;
							aux_info = new InformacionCODOP();
							do
							{
								lector = "";
								do
								{
									c = tabop.Lines[i].ToCharArray()[j];
									if(c!='|')
									{
										lector += c.ToString();
										valores[iValor] += c.ToString();
									}
									j++;
								}while(c!='|' && j < tabop.Lines[i].Length);
								
								switch(iValor)
								{
									case 2:
										aux_info.modo_direccionamiento = lector.ToString();
										break;
									case 3:
										aux_info.codigo_maquina = lector.ToString();
										break;
									case 4:
										aux_info.b_calculados = Int32.Parse(lector.ToString());
										break;
									case 5:
										aux_info.b_calcular = Int32.Parse(lector.ToString());
										break;
									case 6:
										aux_info.b_total = Int32.Parse(lector.ToString());
									break;
								}
								iValor++;
							}while( iValor < 7 && j < tabop.Lines[i].Length );
							//Termina de capturar los valores
							
							infos.Add(aux_info);
							
							if(valores[1] == "0") 
							{
								CODOPLlevaOperando = false;
								aux_codop.lleva_operando = false;
							}
							else
							{
								CODOPLlevaOperando = true;
								aux_codop.lleva_operando = true;
							}
							
							
						}
						else
						{
							continuaLinea = false;
							hayMas = false;
						}
						
						if(aux_info.modo_direccionamiento == "Inherente")
							hayMas = false;
						
						if(!hayMas && encontroCODOP)
						{
							continuaBuscando = false;
							String op = valores[1]=="0"?"No":"Si";
							aux_codop.cadena = "CODOP: " + valores[0] + 
								                "\nOperando: "+ op  +
								                "\nModo de direccionamiento: " + valores[2] +
												"\nCódigo máquina: " + valores[3] + 
												"\nTotal de Bytes calculados: " + valores[4] + 
												"\nTotal de Bytes por calcular: " + valores[5] + 
												"\nSuma total de Bytes: " + valores[6];
							aux_codop.info = new List<InformacionCODOP>(infos);
							codops.Add(aux_codop);
							cmbInfoCODOPS.Items.Add(aux_codop.nombre);
						}
					}	
				}
				
				if(!hayMas)
				{
					do
					{
						i++;
						if( (i) < tabop.Lines.Length)
							c = tabop.Lines[i].ToCharArray()[0];
						else
						{
							continuaBuscando = false;
							continuaLinea = false;
						}
					}while(c!='¿' && i < tabop.Lines.Length);
				}
				
			}
			
			if(!encontroCODOP)
				aplicaErrorCODOP("Linea " + linea.ToString("000") + ". El CODOP '" + codop + "' no existe.");
			else
			{
				if(CODOPLlevaOperando && operando == "")
					aplicaErrorCODOP("Linea " + linea.ToString("000") + ".El CODOP '" + codop.ToUpper() +"' lleva operando y no lo tiene.");
				else if(!CODOPLlevaOperando && operando != "")
					aplicaErrorCODOP("Linea " + linea.ToString("000") + ".El CODOP '" + codop.ToUpper() +"' no lleva operando y el operando es '" + operando +"'.");
			}
			
			return CODOPLlevaOperando;
		}
		
		//Esa función evalua si el CODOP lleva operando, es como la función de arriba, a diferencia que
		//En esta función ya están guardados previamente.
		bool encuentraErroresOperando(String codop, String operando, int linea, int i)
		{
			if(codops[i].lleva_operando && operando == "")
				aplicaErrorCODOP("Linea " + linea.ToString("000") + ".El CODOP '" + codop.ToUpper() +"' lleva operando y no lo tiene.");
			else if(!codops[i].lleva_operando && operando != "")
				aplicaErrorCODOP("Linea " + linea.ToString("000") + ".El CODOP '" + codop.ToUpper() +"' no lleva operando y el operando es '" + operando +"'.");
			return codops[i].lleva_operando;
		}
		
		//Esta función es la que determina que modo de dirección le corresponde al CODO dado
		//Además de mandar a llamar otras funciones en dado caso que no sea el MD correspondiente
		//Si encuentra errores los muestra.
		void evaluarOperando(String codop, String operando, int linea)
		{
			List <String> errores = new List<String>();
			int i , j;
			bool esCorrectoMD = false;
			i = 0;
			while( i < codops.Count && codops[i].nombre.ToUpper() != codop.ToUpper())
				i++;
			
			for(j = 0 ; j < codops[i].info.Count && !esCorrectoMD ; j++)
				switch(codops[i].info[j].modo_direccionamiento.ToUpper())
				{
					case "INMEDIATO":
						if( operando.Length > 0 && operando.ToCharArray()[0] == '#')
						{
							esCorrectoMD = esInmediato(operando, errores, i, j);
								if(esCorrectoMD)
									imprimeModoDireccionamientoInmediato(i,j);
						}
						break;
					case "DIRECTO":
						if( operando.Length > 0 && (	(operando.ToCharArray()[0] == '@') 
						                           || 	(operando.ToCharArray()[0] == '$') 
						                           ||	(operando.ToCharArray()[0] == '%') 
						                           || 	(Char.IsDigit(operando.ToCharArray()[0]))))
						{
							esCorrectoMD = esDirecto(operando, errores, i, j);
							if(esCorrectoMD)
								imprimeModoDireccionamiento(i,j);
						}
							break;
					case "EXTENDIDO":
						if( operando.Length > 0 && (	(operando.ToCharArray()[0] == '@') 
						                           || 	(operando.ToCharArray()[0] == '$') 
						                           ||	(operando.ToCharArray()[0] == '%') 
						                           || 	(Char.IsLetterOrDigit(operando.ToCharArray()[0]))
						                          ))
						{
							esCorrectoMD = esExtendido(operando, errores);
							if(esCorrectoMD)
								imprimeModoDireccionamiento(i,j);
						}
						break;
					case "IDX":
						if(operando.Length > 0 && (	operando.ToCharArray()[0] == '-' 	||
						                          	operando.ToCharArray()[0] == ',' 	||
						                           	operando.ToCharArray()[0] == 'a'	||
						                           	operando.ToCharArray()[0] == 'b' 	||
						                           	operando.ToCharArray()[0] == 'd' 	||
						                           	operando.ToCharArray()[0] == 'A' 	||
						                           	operando.ToCharArray()[0] == 'B' 	||
						                           	operando.ToCharArray()[0] == 'D' 	||
						                           	Char.IsDigit(operando.ToCharArray()[0]))
						                           )
							esCorrectoMD = esIDX_(operando, errores,i,j);
						break;
					case "IDX1":
						if( operando.Length > 0 && (	Char.IsDigit(operando.ToCharArray()[0])
						                            ||	(operando.ToCharArray()[0] == '-')
						                           ))
						{
							esCorrectoMD = esModoIndizado9Bits(operando, errores);
							if(esCorrectoMD)
								imprimeModoDireccionamiento(i,j);
						}
						break;
					case "IDX2":
						if( operando.Length > 0 && Char.IsDigit(operando.ToCharArray()[0]))
						{
							esCorrectoMD = esModoIndizado16Bits(operando, errores);
							if(esCorrectoMD)
								imprimeModoDireccionamiento(i,j);
						}
						break;
					case "[D,IDX]":
						if( operando.Length > 1 && operando.ToCharArray()[0] == '[' && operando.ToCharArray()[1].ToString().ToUpper() == "D")
						{
							esCorrectoMD = esModoIndizadoDeAcumuladorIndirecto(operando, errores, i, j);
								if(esCorrectoMD)
									imprimeModoDireccionamiento(i,j);
						}
						break;
					case "[IDX2]":
						if( operando.Length > 0 && operando.ToCharArray()[0] == '[')
						{
							esCorrectoMD = esModoIndizadoIndirecto16Bits(operando, errores, i, j);
								if(esCorrectoMD)
									imprimeModoDireccionamiento(i,j);
						}
						break;
					case "REL":
						if( operando.Length > 0 && Char.IsLetter(operando.ToCharArray()[0]))
						{
							esCorrectoMD = esRelativo(operando, errores);
								if(esCorrectoMD)
									imprimeModoDireccionamientoInmediato(i,j);
						}
						break;
					default:
						break;
				}
			
			if(!esCorrectoMD)
				for(i = 0; i < errores.Count ; i++)
					aplicaErrorOperando("Linea " + linea.ToString("000") + ", Operando. " + errores[i]);
			else
				contloc = contloc + codops[i].info[j-1].b_total;
				
			textSalida.Text += "\n";
		}
		
		//Esta función guarda las referencias del TABOP, para usarlas en la generación del código máquina
		//MessageBox. La variante de esta es que aplica cuando el MD es diferente de inherente e IDX
		void guardaReferencia(int i ,int  j)
		{
			referenciaModoDir x = new referenciaModoDir();
			x.contloc = contloc.ToString("X4");
			x.codop = codops[i].nombre;
			x.modo_direccionamiento = codops[i].info[j].modo_direccionamiento;
			x.iCODOP = i;
			x.iModoDir = j;
			referencias.Add(x);
		}
		
		//Esta función guarda las referencias del TABOP, para usarlas en la generación del código máquina
		//MessageBox. La variante de esta es que aplica solo cuando es Inherente
		void guardaReferencia(String codop, String modo_direccionamiento)
		{
			referenciaModoDir x = new referenciaModoDir();
			x.contloc = contloc.ToString("X4");
			x.codop = codop;
			x.modo_direccionamiento = modo_direccionamiento;
			referencias.Add(x);
		}
		
		//Esta función guarda las referencias del TABOP, para usarlas en la generación del código máquina
		//MessageBox. La variante de esta es que aplica solo cuando el MD es IDX
		void guardaReferencia(int i ,int  j, String var)
		{
			referenciaModoDir x = new referenciaModoDir();
			x.contloc = contloc.ToString("X4");
			x.codop = codops[i].nombre;
			x.modo_direccionamiento = codops[i].info[j].modo_direccionamiento;
			x.variante = var;
			x.iCODOP = i;
			x.iModoDir = j;
			referencias.Add(x);
		}
		
		//Imprime el MD de direccionamiento en el RichTextBox Salida, variante para cuando el MD es diferente
		//de Inherente e inmediato
		void imprimeModoDireccionamiento(int i, int j)
		{
			textSalida.Text+= "-->" +codops[i].info[j].modo_direccionamiento + " de " + codops[i].info[j].b_total.ToString() + " Byte(s).\n";
			
			guardaReferencia(i, j);
		}
		
		//Imprime el MD de direccionamiento en el RichTextBox Salida, variante para cuando el MD es inmediato
		void imprimeModoDireccionamientoInmediato(int i, int j)
		{
			textSalida.Text+= "-->" +codops[i].info[j].modo_direccionamiento + " de " + (codops[i].info[j].b_calcular*8).ToString() + " bit(s)" + ", de " + codops[i].info[j].b_total.ToString() + " Byte(s).\n";
			guardaReferencia(i,j);
		}
		
		//Imprime el MD de direccionamiento en el RichTextBox Salida, variante para cuando el MD es idx
		void imprimeModoDireccionamientoIDX(int i, int j, String variante)
		{
			textSalida.Text+= "-->" +codops[i].info[j].modo_direccionamiento +"("+ variante +")" +  " de " + codops[i].info[j].b_total.ToString() + " Byte(s).\n";		
			guardaReferencia(i,j,variante);
		}
		
		//Función que determina si un operando es base binaria
		bool esBinario(String cadena)
		{
			int i;
			Char c;
			bool esValido = false;
			i = 0;
			if(i < cadena.Length)
				esValido = true;
			for( i = 0 ; i < cadena.Length && esValido ; i++ )
			{
				c = cadena.ToCharArray()[i];
				
				switch(c)
				{
					case '0':
					case '1':
						break;
					default:
						esValido = false;
						break;
				}
			}
		
			return esValido;
		
		}
		
		//Función que determina si un operando es base octal
		bool esOctal(String cadena)
		{
			int i;
			Char c;
			bool esValido = false;
			i = 0;
			if(i < cadena.Length)
				esValido = true;
			for( i = 0 ; i < cadena.Length && esValido ; i++ )
			{
				c = cadena.ToCharArray()[i];
				
				switch(c)
				{
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
						break;
					default:
						esValido = false;
						break;
				}
			}
		
			return esValido;
		
		}
		
		//Función que determina si un operando es base hexadecimal
		bool esHexadecimal(String cadena)
		{
			int i;
			Char c;
			bool esValido = false;
			
			i = 0;
			if(i < cadena.Length)
				esValido = true;
			
			for( i = 0 ; i < cadena.Length && esValido ; i++ )
			{
				c = cadena.ToCharArray()[i];
				
				switch(c)
				{
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
						break;
					default:
						esValido = false;
						break;
				}
			}
		
			return esValido;
		
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento inmediato
		bool esInmediato(String operando, List<String> errores, int iCodop, int iModoDir)
		{
			bool esInmediato = false, esDecimal = false, esDecimalValido = false;
			String cifra = "";
			Char c;
			int i = 0, numero = -1, numeroDecimal = -1;
			c = operando.ToCharArray()[i];
			if(c=='#' && i < operando.Length)
			{
				i = 1;
				if(i<operando.Length)
				{
					c = operando.ToCharArray()[i];
					switch(c){
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							esDecimal = true;
							do
							{
								c =  operando.ToCharArray()[i];
								cifra += c.ToString();
								i++;
							}while(i < operando.Length);
							try
							{
								numeroDecimal = Int32.Parse(cifra);
								esDecimalValido = true;
							}
							catch
							{
								errores.Add("Inmediato. La base decimal sólo acepta los caracteres numéricos del 0 al 9.");
								esDecimalValido = false;
							}
							finally
							{
								if(esDecimalValido)
									switch(codops[iCodop].info[iModoDir].b_calcular)
									{
										case 1:
											if(!(numeroDecimal>=0 && numeroDecimal<=255)){
												errores.Add("Inmediato. Para números de 8 bits, el rango debe ser entre 0 y 255");
												esInmediato = false;
											}
											else
												esInmediato = true;
											break;
										case 2:
											if(!(numeroDecimal>=0 && numeroDecimal<=65535)){
												errores.Add("Inmediato. Para números de 16 bits, el rango debe ser entre 0 y 65535");
												esInmediato = false;
											}
											else
												esInmediato = true;
											break;
									}
							}
								
							break;
							
						case '$':
							i++;
							while(i < operando.Length)
							{
								c = operando.ToCharArray()[i];
								cifra += c.ToString();
								i++;
							}
							if(cifra.Length > 0)
								if(!(esInmediato = esHexadecimal(cifra)))
							   		errores.Add("Inmediato. La base hexadecimal sólo acepta los caracteres numéricos del 0 al 9 y las letras a..f, A..F.");
								else
								{
									numero = Convert.ToInt32(cifra,16);
									esInmediato = true;
								}
							else
								errores.Add("Inmediato. Después del símbolo \"$\" no puede estar vacío.");
							break;
						case '@':
							i++;
							while(i < operando.Length)
							{
								c = operando.ToCharArray()[i];
								cifra += c.ToString();
								i++;
							}
							if(cifra.Length>0)
								if(!(esInmediato = esOctal(cifra)))
							   		errores.Add("Inmediato. La base octal sólo acepta los caracteres numéricos del 0 al 7.");
								else
								{
									numero = Convert.ToInt32(cifra,8);
									esInmediato = true;
								}
							else
								errores.Add("Inmediato. Después del símbolo \"@\" no puede estar vacío.");
							break;
						case '%':
							i++;
							while(i < operando.Length)
							{
								c = operando.ToCharArray()[i];
								cifra += c.ToString();
								i++;
							}
							if(cifra.Length > 0)
								if(!(esInmediato = esBinario(cifra)))
							   		errores.Add("Inmediato. La base binaria sólo acepta los caracteres numéricos 0 y 1.");
								else
								{
									numero = Convert.ToInt32(cifra,2);
									esInmediato = true;
								}
							else
								errores.Add("Inmediato. Después del símbolo \"%\" no puede estar vacio.");
							
							break;
						default:
							errores.Add("Inmediato. Caracter no valido para modo de direccionamiento Inmediato.");
							break;
					}
					if(esInmediato && !esDecimal)
						switch(codops[iCodop].info[iModoDir].b_calcular)
						{
							case 1:
								if(!(numero>=0 && numero<=255)){
									errores.Add("Inmediato. Para números de 8 bits, el rango debe ser entre 0 y 255");
									esInmediato = false;
								}
								else
									esInmediato = true;
								break;
							case 2:
								if(!(numero>=0 && numero<=65535)){
									errores.Add("Inmediato. Para números de 16 bits, el rango debe ser entre 0 y 65535");
									esInmediato = false;
								}
								else
									esInmediato = true;
								break;
						}
				}
				else
					errores.Add("Inmediato. Después del símbolo de \"#\" no puede estar vacío.");
			}
			else
				errores.Add("Inmediato. El caracter de inicio en el modo de direccionamiento Inmediato debe ser \"#\"");
			
			return esInmediato;
		}
		
		//Función que determina si el operanto de la directiva ORG está correctamente escrita
		void evaluaOperandoORG(int linea, String etiqueta, String operando)
		{
			bool esCorrecto = false, esDecimalValido = false;
			String cifra = "";
			Char c;
			int i = 0, numero = -1;
			
			if(etiqueta != "")
				aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. La etiqueta debe ser nula");
			
			if(i<operando.Length)
			{
				c = operando.ToCharArray()[i];
				switch(c){
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						do
						{
							c =  operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}while(i < operando.Length);
						try
						{
							numero = Int32.Parse(cifra);
							esDecimalValido = true;
						}
						catch
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. La base decimal sólo acepta los caracteres numéricos del 0 al 9. Se tomará el valor 0.");
							esDecimalValido = false;
							esCorrecto = false;
						}
						finally
						{
							if(esDecimalValido)
								esCorrecto = true;
						}
							
						break;
						
					case '$':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esHexadecimal(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. La base hexadecimal sólo acepta los caracteres numéricos del 0 al 9 y las letras a..f, A..F. Se tomará el valor 0.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,16);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. Después del símbolo \"$\" no puede estar vacío. Se tomará el valor 0.");
							esCorrecto = false;
						}
						break;
					case '@':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esOctal(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. La base octal sólo acepta los caracteres numéricos del 0 al 7. Se tomará el valor 0.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,8);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. Después del símbolo \"@\" no puede estar vacío. Se tomará el valor 0.");
							esCorrecto = false;
						}
						break;
					case '%':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esBinario(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. La base binaria sólo acepta los caracteres numéricos 0 y 1. Se tomará el valor 0.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,2);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. Después del símbolo \"%\" no puede estar vacío. Se tomará el valor 0.");
							esCorrecto = false;
						}
						break;
					default:
						aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. Caracter no valido para el el operando de la directiva. Se tomará el valor 0.");
						esCorrecto = false;
						break;
				}
				if(esCorrecto)
				{
					if(numero >= 0 && numero <= 65535)
					{
						dir_inic = numero;
						contloc = dir_inic;
						esCorrecto = true;
					}
					else
					{
						aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. El rango válido para el operando de la directiva ORG es 0 a 65535. Se tomará el valor 0.");
						esCorrecto = false;
					}
				}
			}
			else
			{
				aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". ORG. El operando del ORG no puede estar vacio. Se tomará el valor 0.");
				esCorrecto = false;
			}
			
			if(!esCorrecto)
			{
				dir_inic = 0;
				contloc = dir_inic;
			}
		}
		
		//Función que retorna el valor decimal de cualquier base dada, añadiendo el simbolo
		//$, %, @ y digito para hexadecimal, binario, octal y decimal respectivamente
		int retornaNumero(String operando)
		{
			int numero = -1;
			if(operando.Length > 0)
			{
				String cifra = "";
				Char c;
				int i = 0;
				c = operando.ToCharArray()[i];
				switch(c){
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						do
						{
							c =  operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}while(i < operando.Length);
						
						numero = Int32.Parse(cifra);
						break;
						
					case '$':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						numero = Convert.ToInt32(cifra,16);
						break;
					case '@':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						numero = Convert.ToInt32(cifra,8);
						break;
						
					case '%':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						numero = Convert.ToInt32(cifra,2);
						break;
				}
			}
			
			return numero;
		}
		
		//Evalua el operando en una linea, si está bien escrito, si corresponde a su base dada
		//Además si se encuentra en el rango dependiendo del tipo de CODOP.
		//Aplica errores en caso que no concuerde.
		bool evaluaOperandoDirectiva(String nombre, int rango,int linea, String codop, String operando)
		{
			bool esCorrecto = false, esDecimalValido = false;
			String cifra = "";
			Char c;
			int i = 0, numero = -1;
						
			if(i<operando.Length)
			{
				c = operando.ToCharArray()[i];
				switch(c){
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						do
						{
							c =  operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}while(i < operando.Length);
						try
						{
							numero = Int32.Parse(cifra);
							esDecimalValido = true;
						}
						catch
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+ nombre +". La base decimal sólo acepta los caracteres numéricos del 0 al 9.");
							esDecimalValido = false;
							esCorrecto = false;
						}
						finally
						{
							if(esDecimalValido)
								esCorrecto = true;
						}
							
						break;
						
					case '$':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esHexadecimal(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+ nombre +". La base hexadecimal sólo acepta los caracteres numéricos del 0 al 9 y letras a..f y A..F. Se tomará el valor 0.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,16);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". Después del símbolo \"$\" no puede estar vacío.");
							esCorrecto = false;
						}
						break;
					case '@':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esOctal(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". La base octal sólo acepta los caracteres numéricos del 0 al 7.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,8);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". Después del símbolo \"@\" no puede estar vacío.");
							esCorrecto = false;
						}
						break;
					case '%':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!esBinario(cifra))
							{
						   		aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". La base binaria sólo acepta los caracteres numéricos 0 y 1.");
								esCorrecto = false;
							}
						   	else
							{
								numero = Convert.ToInt32(cifra,2);
								esCorrecto = true;
							}
						else
						{
							aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". Después del símbolo \"%\" no puede estar vacío.");
							esCorrecto = false;
						}
						break;
					default:
						aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". Caracter no valido para el el operando de la directiva.");
						esCorrecto = false;
						break;
				}
				if(esCorrecto)
				{
					if(numero >= 0 && numero <=rango)
						esCorrecto = true;
					else
					{
						aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". " + nombre+ ". El rango válido para el operando de la directiva "+nombre+" es 0 a "+rango.ToString() +".");
						esCorrecto = false;
					}
				}
			}
			else
			{
				aplicaErrorDirectiva("Linea "+ linea.ToString("000") + ". "+nombre+". El operando del "+nombre+" no puede estar vacio.");
				esCorrecto = false;
			}
			
			return esCorrecto;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento directo
		bool esDirecto(String operando,List<String> errores, int iCodop, int iModoDir)
		{
			bool esDirecto = false, esDecimal = false, esDecimalValido = false;
			String cifra = "";
			Char c;
			int i = 0, numero = -1, numeroDecimal = -1;
			if(i<operando.Length)
			{
				c = operando.ToCharArray()[i];
				switch(c){
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						esDecimal = true;
						do
						{
							c =  operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}while(i < operando.Length);
						try
						{
							numeroDecimal = Int32.Parse(cifra);
							esDecimalValido = true;
						}
						catch
						{
							errores.Add("Directo. La base decimal sólo acepta los caracteres numéricos del 0 al 9.");
							esDecimalValido = false;
						}
						finally
						{
							if(esDecimalValido)
								switch(codops[iCodop].info[iModoDir].b_calcular)
								{
									case 1:
										if(!(numeroDecimal>=0 && numeroDecimal<=255)){
											errores.Add("Directo. Para números de 8 bits, el rango debe ser entre 0 y 255");
											esDirecto = false;
										}
										else
											esDirecto = true;
										break;
									case 2:
										if(!(numeroDecimal>=0 && numeroDecimal<=65535)){
											errores.Add("Directo. Para números de 16 bits, el rango debe ser entre 0 y 65535");
											esDirecto = false;
										}
										else
											esDirecto = true;
										break;
								}
						}
							
						break;
						
					case '$':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!(esDirecto = esHexadecimal(cifra)))
						   		errores.Add("Directo. La base hexadecimal sólo acepta los caracteres numéricos del 0 al 9 y las letras a..f, A..F.");
							else
							{
								numero = Convert.ToInt32(cifra,16);
								esDirecto = true;
							}
						else
							errores.Add("Directo. Después del símbolo \"$\" no puede estar vacío.");
						break;
					case '@':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length>0)
							if(!(esDirecto = esOctal(cifra)))
						   		errores.Add("Directo. La base octal sólo acepta los caracteres numéricos del 0 al 7.");
							else
							{
								numero = Convert.ToInt32(cifra,8);
								esDirecto = true;
							}
						else
							errores.Add("Directo. Después del símbolo \"@\" no puede estar vacío.");
						break;
					case '%':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!(esDirecto = esBinario(cifra)))
						   		errores.Add("Directo. La base binaria sólo acepta los caracteres numéricos 0 y 1.");
							else
							{
								numero = Convert.ToInt32(cifra,2);
								esDirecto = true;
							}
						else
							errores.Add("Directo. Después del símbolo \"%\" no puede estar vacio.");
						
						break;
					default:
						errores.Add("Directo. Caracter no valido para modo de direccionamiento Inmediato.");
						break;
				}
				if(esDirecto && !esDecimal)
					switch(codops[iCodop].info[iModoDir].b_calcular)
					{
						case 1:
							if(!(numero>=0 && numero<=255)){
								errores.Add("Directo. Para números de 8 bits, el rango debe ser entre 0 y 255");
								esDirecto = false;
							}
							else
								esDirecto = true;
							break;
						case 2:
							if(!(numero>=0 && numero<=65535)){
								errores.Add("Directo. Para números de 16 bits, el rango debe ser entre 0 y 65535");
								esDirecto = false;
							}
							else
								esDirecto = true;
							break;
					}
			}
			else
				errores.Add("Directo. Para el modo de direccionamiento Directo, no puede estar vacío.");
			
			return esDirecto;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento extendido
		bool esExtendido(String operando,List<String> errores)
		{
			bool esExtendido = false, esEtiqueta = false, esDecimal = false, esDecimalValido = false;
			String cifra = "";
			Char c;
			int i = 0, numero = -1, numeroDecimal = -1;
			if(i<operando.Length)
			{
				c = operando.ToCharArray()[i];
				switch(c){
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						esDecimal = true;
						do
						{
							c =  operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}while(i < operando.Length);
						try
						{
							numeroDecimal = Int32.Parse(cifra);
							esDecimalValido = true;
						}
						catch
						{
							errores.Add("Extendido. La base decimal sólo acepta los caracteres numéricos del 0 al 9.");
							esDecimalValido = false;
						}
						finally
						{
							if(esDecimalValido)
							{
								if(!(numeroDecimal>=256 && numeroDecimal<=65535)){
									errores.Add("Extendido. El rango para los números extendidos es entre 256 y 65535");
									esExtendido = false;
								}
								else
									esExtendido = true;
							}
								
								
						}
							
						break;
						
					case '$':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!(esExtendido = esHexadecimal(cifra)))
						   		errores.Add("Extendido. La base hexadecimal sólo acepta los caracteres numéricos del 0 al 9 y las letras a..f, A..F.");
							else
							{
								numero = Convert.ToInt32(cifra,16);
								esExtendido = true;
							}
						else
							errores.Add("Extendido. Después del símbolo \"$\" no puede estar vacío.");
						break;
					case '@':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length>0)
							if(!(esExtendido = esOctal(cifra)))
						   		errores.Add("Extendido. La base octal sólo acepta los caracteres numéricos del 0 al 7.");
							else
							{
								numero = Convert.ToInt32(cifra,8);
								esExtendido = true;
							}
						else
							errores.Add("Extendido. Después del símbolo \"@\" no puede estar vacío.");
						break;
					case '%':
						i++;
						while(i < operando.Length)
						{
							c = operando.ToCharArray()[i];
							cifra += c.ToString();
							i++;
						}
						if(cifra.Length > 0)
							if(!(esExtendido = esBinario(cifra)))
						   		errores.Add("Extendido. La base binaria sólo acepta los caracteres numéricos 0 y 1.");
							else
							{
								numero = Convert.ToInt32(cifra,2);
								esExtendido = true;
							}
						else
							errores.Add("Extendido. Después del símbolo \"%\" no puede estar vacio.");
						
						break;
					default:
						esEtiqueta = true;
						esExtendido = esComoEtiqueta(operando,errores, "Extendido");
						break;
				}
				if(esExtendido && !esDecimal && !esEtiqueta)
				{
					if(!(numero>=256 && numero<=65535)){
						errores.Add("Extendido. El rango para los números extendidos es entre 256 y 65535");
						esExtendido = false;
					}
					else
						esExtendido = true;
				}
					
			}
			else
				errores.Add("Extendido. Para el modo de direccionamiento Extendido, no puede estar vacío.");
			return esExtendido;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento extendido, 
		//con la variante de etiqueta
		bool esComoEtiqueta(String etiqueta, List<String> errores, String prefijo)
		{
			Char c;
			int i = 0;
			bool hayCaracteresEspeciales = false;
			bool esCorrecto = true;
			c = etiqueta.ToCharArray()[i];
			if( !Char.IsLetter(c))
			{
				errores.Add(prefijo + ". Las etiquetas deben comenzar con letra.");
				esCorrecto = false;
			}
			
			for(i = 1; i < etiqueta.Length ; i++)
			{
				c = etiqueta.ToCharArray()[i];
				if( !Char.IsLetterOrDigit(c) && c != '_' )
					hayCaracteresEspeciales = true;
			}
			if(hayCaracteresEspeciales)
			{
				errores.Add(prefijo + ". Los caracteres válidos en las etiquetas son letras, digitos (0..9) y el guión bajo.");
				esCorrecto = false;
			}
			if(etiqueta.Length > 8)
			{
				errores.Add(prefijo + ". La longitud máxima de una etiqueta es de ocho caracteres.");
				esCorrecto = false;
			}
			
			return esCorrecto;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento IDX
		//Solo que evalua si es uno de 5 bits, después si es acumulador o si es AutoPostDecrementoIncremento
		bool esIDX_(String operando,List<String> errores, int i, int j) //IDX
		{
			bool esIDX = true;
			variante = "";
			
			esIDX = esModoIndizado5Bits(operando,errores);
			
			if(!esIDX)
			{	
				esIDX = esIndizadoAcumulador(operando,errores);
				
				if(!esIDX)
					esIDX = esAutoPrePostDecrementoIncremento(operando,errores,variante);
				else
					variante = "Indizado de Acumulador";
			}
			else
				variante = "Indizado de 5 bits";
			
			if(esIDX)
				imprimeModoDireccionamientoIDX(i,j, variante);
			
			return esIDX;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento Indizado de 5 bits
		bool esModoIndizado5Bits(String operando,List<String> errores)
		{
			bool esIDX = true;
			
			bool encontroComa = false;
			Char c;
			String registro = "";
			int i, numero = -1;
			String cifra = "";
			i = 0;
			
			if(i< operando.Length)
			{
				c = operando.ToCharArray()[i];
			
				if(c == '-')
				{
					cifra += c.ToString();
					i++;
					if(i>operando.Length)
					{
						errores.Add("IDX. Error de sintaxis para Indizado de 5 bits, " +
							            "debe ser un número entre -16 a 15, " +
							            "seguido de la coma y un registro de computadora (X, Y, SP o PC)");
						return false;
					}
				}
				
				do{
					c = operando.ToCharArray()[i];
					switch(c)
					{		
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							cifra += c.ToString();
							break;
						case ',':
							encontroComa = true;
							i++;
							while(i<operando.Length)
							{
								c = operando.ToCharArray()[i];
								registro += c;
								i++;
							}
							break;
						default:
							esIDX = false;
							break;
					}
					i++;
				}
				while(!encontroComa && esIDX && i< operando.Length);
				
				if(!encontroComa || !esIDX ){
					errores.Add("IDX. Error de sintaxis para Indizado de 5 bits, " +
							            "debe ser un número entre -16 a 15, " +
							            "seguido de la coma y un registro de computadora (X, Y, SP o PC)");
					esIDX = false;
				}
				else if(encontroComa && esIDX)
				{
					try
					{
						if(cifra == "")
							cifra = "0";
						numero = Int32.Parse(cifra);
					}
					catch
					{
						errores.Add("IDX. El modo Indizado de 5 bits solo acepta el formato decimal (0..9)");
						esIDX = false;
					}
					finally
					{
						if(!(numero >= -16 && numero<=15) )
						{
							errores.Add("IDX. El modo indizado de 5 bits debe contener un número entre -16 y 15.");
							esIDX = false;
						}
					}
				}
				registro = registro.ToUpper();
				if(!(registro == "X" || registro == "Y" || registro == "SP" || registro == "PC"))
				{
					errores.Add("IDX. El modo indizado de 5 bits debe tener un registro valido (X, Y, SP ó PC)");
					esIDX = false;
				}
			}
			else
			{
				esIDX = false;
				errores.Add("IDX. En el modo indizado de 5 bits no puede estar vacio");
			}
			
			return esIDX;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento AutoPrePostIncrementoDecremento
		bool esAutoPrePostDecrementoIncremento(String operando,List<String> errores, String var)
		{
			bool esIDX = true, esNumeroCorrecto = false;
			bool errorDeSintaxis = false;
			Char c;
			String registro = "";
			int i;
			int numero = -1;
			i = 0;
			
			if(i < operando.Length)
			{
				try
				{
					numero = Int32.Parse(operando.ToCharArray()[i].ToString());
					esNumeroCorrecto = true;
				}
				catch
				{
					esNumeroCorrecto = false;
					errores.Add("IDX. El modo Indizado Auto/Post Incremento/Decremento debe contener un número en la primer posición, admeás de ser decimal (0..9).");
					esIDX = false;
				}
				finally
				{
					if(esNumeroCorrecto)
					{
						if(numero>= 1 && numero<=8)
						{
							i++;
							c = operando.ToCharArray()[i];
							if(c == ',' && (i+1)<operando.Length)
							{
								i++;
								c = operando.ToCharArray()[i];
								i++;
								switch(c)
								{
									case '+':
										if(i < operando.Length)
										{
											c = operando.ToCharArray()[i];
											if(( c == 's' || c == 'S') && ((i+1) < operando.Length))
												registro = (operando.ToCharArray()[i].ToString()
												            + operando.ToCharArray()[i+1].ToString()).ToString().ToUpper();
											else
												registro = c.ToString().ToUpper();
											
											var = new String(("Indizado Pre Incremento").ToCharArray());
										}
										else
											errorDeSintaxis = true;
										break;
									case '-':
										if(i < operando.Length)
										{
											c = operando.ToCharArray()[i];
											if(( c == 's' || c == 'S') && ((i+1) < operando.Length))
												registro = (operando.ToCharArray()[i].ToString()
												            + operando.ToCharArray()[i+1].ToString()).ToString().ToUpper();
											else
												registro = c.ToString().ToUpper();
											
											var = new String(("Indizado Pre Decremento").ToCharArray());
										}
										else
											errorDeSintaxis = true;
										break;
									case 's':
									case 'S':
										if(i<operando.Length && (i+1) < operando.Length)
										{
											registro = (c.ToString() + operando.ToCharArray()[i]).ToString().ToUpper();
											i++;
											c = operando.ToCharArray()[i];
											if(c == '+')
												var = new String(("Indizado Post Incremento").ToCharArray());
											else if(c == '-')
												var = new String(("Indizado Post Decremento").ToCharArray());
											else
											{
												esIDX = false;
												errores.Add("IDX. " +
												            "En el modo de direccionamiento Post/Pre Incremento/decremento, después del registro debe existir el signo + o -");
											}
										}
										else
											errorDeSintaxis = true;
										break;
									case 'x':
									case 'X':
										if(i<operando.Length)
										{
											registro = c.ToString().ToUpper();
											c = operando.ToCharArray()[i];
											if(c == '+')
												var = "Indizado Post Incremento";
											else if(c == '-')
												var = "Indizado Post Decremento";
											else
											{
												esIDX = false;
												errores.Add("IDX. En el modo de direccionamiento Indizado Post Incremento/decremento, después del registro debe existir el signo + o -");
											}
										}
										else
											errorDeSintaxis = true;
										break;
									case 'y':
									case 'Y':
										if(i<operando.Length)
										{
											registro = c.ToString().ToUpper();
											c = operando.ToCharArray()[i];
											if(c == '+')
												var = "Indizado Post Incremento";
											else if(c == '-')
												var = "Indizado Post Decremento";
											else
											{
												esIDX = false;
												errores.Add("IDX. En el modo de direccionamiento Indizado Post Incremento/decremento, después del registro debe existir el signo + o -");
											}
										}
										else
											errorDeSintaxis = true;
										break;
									default:
										errores.Add("IDX. En el modo Indizado Auto/Post Incremento/Decremento debe contener un número del 1 al 9, la coma," +
										            "seguido de el signo de \"+\" o \"-\" seguido del registro X,Y o SP; o bien el registro X, Y o SP " +
										            "seguido del signo de \"+\" o \"-\".");
										esIDX = false;
										break;
								}
								
								if(errorDeSintaxis)
								{
									esIDX = false;
									errores.Add("IDX. En el modo Indizado Auto Pre/Post Incremento/Decremento debe contener un número del 1 al 9, la coma," +
										            "seguido de el signo de \"+\" o \"-\" seguido del registro X,Y o SP; o bien el registro X, Y o SP " +
										            "seguido del signo de \"+\" o \"-\".");
								}
								
								if(!(registro == "X" || registro == "Y" || registro =="SP"))
								{
									esIDX = false;
									errores.Add("IDX. En el modo Indizado Auto Pre/Post Incremento/Decremento los registros válidos después de la coma son X, Y o SP");
								}
								
							}
							else if ( c!= ',')
							{
								errores.Add("IDX. El modo Indizado Auto Pre/Post Incremento/Decremento debe contener una \",\".");
								esIDX = false;
							}
							else
							{
								errores.Add("IDX. En el modo Indizado Auto Pre/Post Incremento/Decremento debe contener un número del 1 al 9, la coma," +
										            "seguido de el signo de \"+\" o \"-\" seguido del registro X,Y o SP; o bien el registro X, Y o SP " +
										            "seguido del signo de \"+\" o \"-\".");
								esIDX = false;
							}
						}
						else
						{
							esIDX = false;
							errores.Add("IDX. En el modo Indizado Auto Pre/Post Incremento/Decremento el número debe estar entre 1 y 8");
						}
					}
				}
			}
			else
			{
				errores.Add("IDX. El modo Indizado Auto/Post Incremento/Decremento debe contener algo.");
				esIDX = false;
			}
			variante = var;
			return esIDX;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento indizado de acumulador
		bool esIndizadoAcumulador(String operando,List<String> errores)
		{
			bool esIDX = true;
			Char c;
			String registro2 = "";
			String registro1 = "";
			int i;
			i = 0;
			
			
			if(i < operando.Length)
			{	
				registro1 = operando.ToCharArray()[i].ToString().ToUpper();
				
				if(registro1 == "D" || registro1 == "A" || registro1 == "B")
				{
					i++;
					c = operando.ToCharArray()[i];
					if(c == ',')
					{
						i++;
						registro2 = operando.ToCharArray()[i].ToString().ToUpper();
						if ((i + 1)< operando.Length && (registro2 != "X" || registro2 != "Y"))
						{
						    i++;
							registro2 += operando.ToCharArray()[i].ToString().ToUpper();
						}
						    
						if(!(registro2 == "X" || registro2 == "Y" || registro2 == "SP" || registro2 == "PC"))
						{
							errores.Add("IDX. El modo indizado de acumulador debe tener un registro después de la coma válido (X, Y, SP ó PC).");
							esIDX = false;
						}
					}
					else
					{
						esIDX = false;
						errores.Add("IDX. En el modo indizado de acumulador debe contener una coma.");
					}
				}
				else
				{
					esIDX = false;
					errores.Add("IDX. En el modo indizado de acumulador debe iniciar con el registro \"A\", \"B\" o \"D\".");
				}
			}
			else
			{
				esIDX = false;
				errores.Add("IDX. En el modo indizado de acumulador no puede estar vacío.");
			}
			return esIDX;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento indizado de 9 bits
		bool esModoIndizado9Bits(String operando,List<String> errores)//IDX1
		{
			bool esIDX1 = true;
			bool encontroComa = false;
			Char c;
			String registro = "";
			int i, numero = -1;
			String cifra = "";
			i = 0;
			
			if(i< operando.Length)
			{
				c = operando.ToCharArray()[i];
			
				if(c == '-')
				{
					cifra += c.ToString();
					i++;
					if(i>operando.Length)
					{
						errores.Add("IDX1. Error de sintaxis para Indizado de 9 bits, " +
							            "debe ser un número entre -256 a -17 o de 15 a 255, " +
							            "seguido de la coma y un registro de computadora (X, Y, SP o PC)");
						return false;
					}
				}
				
				do{
					c = operando.ToCharArray()[i];
					switch(c)
					{		
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							cifra += c.ToString();
							break;
						case ',':
							encontroComa = true;
							i++;
							while(i<operando.Length)
							{
								c = operando.ToCharArray()[i];
								registro += c;
								i++;
							}
							break;
						default:
							esIDX1 = false;
							break;
					}
					i++;
				}
				while(!encontroComa && esIDX1 && i< operando.Length);
				
				if(!encontroComa || !esIDX1 ){
					errores.Add("IDX1. Error de sintaxis para Indizado de 9 bits, " +
							            "debe ser un número entre -256 a -17 o de 15 a 255, " +
							            "seguido de la coma y un registro de computadora (X, Y, SP o PC)");
					esIDX1 = false;
				}
				else if(encontroComa && esIDX1)
				{
					try
					{
						numero = Int32.Parse(cifra);
					}
					catch
					{
						errores.Add("IDX1. El modo Indizado de 9 bits solo acepta el formato decimal (0..9)");
						esIDX1 = false;
					}
					finally
					{
						if(!((numero >= -256 && numero <= -17) || (numero >= 15 && numero <= 255)))
						{
							errores.Add("IDX1. El modo indizado de 9 bits debe contener un número entre -256 a -17 o de 15 a 255");
							esIDX1 = false;
						}
					}
				}
				registro = registro.ToUpper();
				if(registro == "")
				{
					esIDX1 = false;
					errores.Add("IDX1. El modo indizado de 9 bits debe tener un registro");
				}
				else if(!(registro == "X" || registro == "Y" || registro == "SP" || registro == "PC"))
				{
					errores.Add("IDX1. El modo indizado de 9 bits debe tener un registro valido (X, Y, SP ó PC)");
					esIDX1 = false;
				}
			}
			else
			{
				esIDX1 = false;
				errores.Add("IDX1. En el modo indizado de 9 bits no puede estar vacio");
			}
			
			return esIDX1;
		}
		
		
		//Función que determina si un operando corresponde al modo de direccionamiento indizado de 16 bits
		bool esModoIndizado16Bits(String operando,List<String> errores)//IDX2
		{
			bool esIDX2 = true;
			bool encontroComa = false;
			Char c;
			String registro = "";
			int i, numero = -1;
			String cifra = "";
			i = 0;
			
			if(i < operando.Length)
			{	
				do{
					c = operando.ToCharArray()[i];
					switch(c)
					{		
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							cifra += c.ToString();
							break;
						case ',':
							encontroComa = true;
							i++;
							while(i<operando.Length)
							{
								c = operando.ToCharArray()[i];
								registro += c;
								i++;
							}
							break;
						default:
							esIDX2 = false;
							break;
					}
					i++;
				}
				while(!encontroComa && esIDX2 && i< operando.Length);
				
				if(!encontroComa || !esIDX2 ){
					errores.Add("IDX2. Error de sintaxis para Indizado de 16 bits, " +
							            "debe ser un número entre 0 y 65535, " +
							            "seguido de la coma y un registro de computadora (X, Y, SP o PC).");
					esIDX2 = false;
				}
				else if(encontroComa && esIDX2)
				{
					try
					{
						numero = Int32.Parse(cifra);
					}
					catch
					{
						errores.Add("IDX2. El modo Indizado de 16 bits solo acepta el formato decimal (0..9).");
						esIDX2 = false;
					}
					finally
					{
						if(!(numero >= 0 && numero <= 65535))
						{
							errores.Add("IDX2. El modo indizado de 16 bits debe contener un número entre 0 y 65535.");
							esIDX2 = false;
						}
					}
				}
				registro = registro.ToUpper();
				if(registro == ""){
					errores.Add("IDX2. El modo indizado de 16 bits debe tener un registro");
					esIDX2 = false;
				}
					
				else if(!(registro == "X" || registro == "Y" || registro == "SP" || registro == "PC"))
				{
					errores.Add("IDX2. El modo indizado de 16 bits debe tener un registro valido (X, Y, SP ó PC).");
					esIDX2 = false;
				}
			}
			else
			{
				esIDX2 = false;
				errores.Add("IDX2. " +
				            "En el modo indizado de 16 bits no puede estar vacío.");
			}
			
			return esIDX2;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento indizado indirecto
		//de 16 bits
		bool esModoIndizadoIndirecto16Bits(String operando,List<String> errores, int iCodop, int iModoDir)//[IDX2]
		{
			bool esModoIndizadoIndirecto = true;
			bool encontroComa = false;
			Char c;
			String registro = "";
			int i, numero = -1;
			String cifra = "";
			i = 0;
			if(i < operando.Length)
			{	
				c = operando.ToCharArray()[i];
				if(c == '[' && (i+1) < operando.Length)
				{
					i++;
					do{
						c = operando.ToCharArray()[i];
						switch(c)
						{		
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
								cifra += c.ToString();
								break;
							case ',':
								encontroComa = true;
								i++;
								while(i<operando.Length-1)
								{
									c = operando.ToCharArray()[i];
									registro += c;
									i++;
								}
								break;
							default:
								esModoIndizadoIndirecto = false;
								break;
						}
						i++;
					}
					while(!encontroComa && esModoIndizadoIndirecto && i< operando.Length);
					
					if(!encontroComa || !esModoIndizadoIndirecto ){
						errores.Add("[IDX2]. Error de sintaxis para Indizado indirecto de 16 bits, " +
								            "Debe comenzar con [, debe ser un número entre 0 y 65535, " +
								            "seguido de la coma y un registro de computadora (X, Y, SP o PC). y terminando con ]");
						esModoIndizadoIndirecto = false;
					}
					else if(encontroComa && esModoIndizadoIndirecto)
					{
						try
						{
							numero = Int32.Parse(cifra);
						}
						catch
						{
							errores.Add("[IDX2]. El modo Indizado indirecto de 16 bits solo acepta el formato decimal (0..9).");
							esModoIndizadoIndirecto = false;
						}
						finally
						{
							if(!(numero >= 0 && numero <= 65535))
							{
								errores.Add("[IDX2]. El modo indizado indirecto de 16 bits debe contener un número entre 0 y 65535.");
								esModoIndizadoIndirecto = false;
							}
						}
					}
					registro = registro.ToUpper();
					if(registro == "")
					{
						esModoIndizadoIndirecto = false;
						errores.Add("IDX1. El modo indizado de 9 bits debe tener un registro");
					}
					else if(!(registro == "X" || registro == "Y" || registro == "SP" || registro == "PC"))
					{
						errores.Add("[IDX2]. El modo indizado indirecto de 16 bits debe tener un registro valido (X, Y, SP ó PC).");
						esModoIndizadoIndirecto = false;
					}
					
					if(operando.ToCharArray()[operando.Length-1] != ']')
					{
						errores.Add("[IDX2]. El modo indizado indirecto de 16 bits debe terminar con el caracter \"]\".");
						esModoIndizadoIndirecto = false;
					}
				}
				else if(c != '[' || (i+1) > operando.Length)
				{
					errores.Add("[IDX2]. Error de sintaxis para Indizado indirecto de 16 bits, " +
								            "Debe comenzar con [, debe ser un número entre 0 y 65535, " +
								            "seguido de la coma y un registro de computadora (X, Y, SP o PC). y terminando con ]");
					esModoIndizadoIndirecto = false;
				}
				else
				{
					errores.Add("[IDX2]. Error de sintaxis para Indizado indirecto de 16 bits, " +
								            "Debe comenzar con [, debe ser un número entre 0 y 65535, " +
								            "seguido de la coma y un registro de computadora (X, Y, SP o PC). y terminando con ]");
					esModoIndizadoIndirecto = false;
				}
					
			}
			else
			{
				esModoIndizadoIndirecto = false;
				errores.Add("[IDX2]. En el modo indizado indirecto de 16 bits no puede estar vacío.");
			}
			return esModoIndizadoIndirecto;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento indizado
		//de acumulador indirecto
		bool esModoIndizadoDeAcumuladorIndirecto(String operando,List<String> errores, int iCodop, int iModoDir)//[D,IDX]
		{
			bool esModoIndDeAcuInd = true;
			Char c;
			String registro = "";
			int i;
			i = 0;
			
			
			if(i < operando.Length)
			{	
				c = operando.ToCharArray()[i];
				
				if(c == '[' && (i+1) < operando.Length)
				{
					i++;
					c = operando.ToCharArray()[i];
					if(c.ToString().ToUpper() == "D")
					{
						i++;
						c = operando.ToCharArray()[i];
						if(c == ',')
						{
							i++;
							registro = operando.ToCharArray()[i].ToString().ToUpper();
							if ((i + 1)< operando.Length && !(registro == "X" || registro == "Y"))
							{
							    i++;
								registro += operando.ToCharArray()[i].ToString().ToUpper();
							}
							if(!(registro == "X" || registro == "Y" || registro == "SP" || registro == "PC"))
							{
								errores.Add("[D,IDX]. El modo indizado de acumulador indirecto debe tener un registro valido (X, Y, SP ó PC).");
								esModoIndDeAcuInd = false;
							}
							
							if(operando.ToCharArray()[operando.Length-1] != ']')
							{
								errores.Add("[D,IDX]. El modo indizado de acumulador indirecto debe terminar con el caracter \"]\".");
								esModoIndDeAcuInd = false;
							}
						}
						else
						{
							esModoIndDeAcuInd = false;
							errores.Add("[D,IDX]. En el modo indizado de acumulador indirecto debe contener una coma.");
						}
					}
					else
					{
						esModoIndDeAcuInd = false;
						errores.Add("[D,IDX]. En el modo indizado de acumulador indirecto debe contener el registro \"D\".");
					}
				}
				else if( c!='[')
				{
					esModoIndDeAcuInd = false;
					errores.Add("[D,IDX]. En el modo indizado de acumulador indirecto debe comenzar con el caracter \"[\".");
				}
				else if (c== '[' || (i+1) > operando.Length)
				{
					esModoIndDeAcuInd = false;
					errores.Add("[D,IDX]. El modo de indizado de acumulador indirecto debe contener [, seguido de registro D" +
					            " una ',' y un registro valido (X,Y,SP,PC) y terminar con un \"]\"");
				}
					
			}
			else
			{
				esModoIndDeAcuInd = false;
				errores.Add("[D,IDX]. En el modo indizado de acumulador indirecto no puede estar vacío.");
			}
			return esModoIndDeAcuInd;
		}
		
		//Función que determina si un operando corresponde al modo de direccionamiento relativo
		bool esRelativo(String operando,List<String> errores)
		{
			return esComoEtiqueta(operando, errores, "REL");
		}
		
		//Función de evento, manda a llamarlo cuando se hace click en algun elemento del listtextbox 
		//Errores de Sintaxis, para colorear la linea del error
		void ListErroresSintaxisSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				int linea = Int32.Parse(
					listErroresSintaxis.Items[listErroresSintaxis.SelectedIndex].ToString().ToCharArray(6,1)[0].ToString() +
					listErroresSintaxis.Items[listErroresSintaxis.SelectedIndex].ToString().ToCharArray(7,1)[0].ToString() +
					listErroresSintaxis.Items[listErroresSintaxis.SelectedIndex].ToString().ToCharArray(8,1)[0].ToString()
					) - 1;
				int start = textArchivo.GetFirstCharIndexFromLine(linea);
				int lenght = textArchivo.Lines[linea].Length;
				textArchivo.Select(start,lenght);
				
			}
			catch
			{
				textArchivo.Select(0,0);
			}
		}
		
		//Función para cambiar la ruta del tabop, en caso que se desee cambiar por otro
		void CmdCambiarTABOPClick(object sender, EventArgs e)
		{
			seCargoTABOP = false;
			cargaTABOP();
		}
		
		//Función de evento, manda a llamarlo cuando se hace click en algun elemento del listtextbox 
		//Errores de Códigos de Operación, para colorear la linea del error
		void ListErroresCODOPSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				int linea = Int32.Parse(
					listErroresCODOP.Items[listErroresCODOP.SelectedIndex].ToString().ToCharArray(6,1)[0].ToString() +
					listErroresCODOP.Items[listErroresCODOP.SelectedIndex].ToString().ToCharArray(7,1)[0].ToString() +
					listErroresCODOP.Items[listErroresCODOP.SelectedIndex].ToString().ToCharArray(8,1)[0].ToString()
					) - 1;
				int start = textArchivo.GetFirstCharIndexFromLine(linea);
				int lenght = textArchivo.Lines[linea].Length;
				textArchivo.Select(start,lenght);
				
			}
			catch
			{
				textArchivo.Select(0,0);
			}
		}
		
		//Función de evento, manda a llamarlo cuando se hace click en algun elemento del listtextbox 
		//Errores de Sintaxis de Operando, para colorear la linea del error
		void ListErroresOperandoSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				int linea = Int32.Parse(
					listErroresOperando.Items[listErroresOperando.SelectedIndex].ToString().ToCharArray(6,1)[0].ToString() +
					listErroresOperando.Items[listErroresOperando.SelectedIndex].ToString().ToCharArray(7,1)[0].ToString() +
					listErroresOperando.Items[listErroresOperando.SelectedIndex].ToString().ToCharArray(8,1)[0].ToString()
					) - 1;
				int start = textArchivo.GetFirstCharIndexFromLine(linea);
				int lenght = textArchivo.Lines[linea].Length;
				textArchivo.Select(start,lenght);
				
			}
			catch
			{
				textArchivo.Select(0,0);
			}
		}
		
		//Función de evento, manda a llamarlo cuando se hace click en algun elemento del listtextbox 
		//Errores de Directivas, para colorear la linea del error
		void ListErroresdirectivaSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				int linea = Int32.Parse(
					listErroresDirectiva.Items[listErroresDirectiva.SelectedIndex].ToString().ToCharArray(6,1)[0].ToString() +
					listErroresDirectiva.Items[listErroresDirectiva.SelectedIndex].ToString().ToCharArray(7,1)[0].ToString() +
					listErroresDirectiva.Items[listErroresDirectiva.SelectedIndex].ToString().ToCharArray(8,1)[0].ToString()
					) - 1;
				int start = textArchivo.GetFirstCharIndexFromLine(linea);
				int lenght = textArchivo.Lines[linea].Length;
				textArchivo.Select(start,lenght);
				
			}
			catch
			{
				textArchivo.Select(0,0);
			}
		}
		
		//Función de evento para ver la información de los CODOPS que se utilizaron para ensamblar
		//Al cambiar el item del combo, muestra un MessageBox
		void CmbInfoCODOPSSelectedIndexChanged(object sender, EventArgs e)
		{
			int i = 0;
			
			while(i < codops.Count && codops[i].nombre.ToUpper() != cmbInfoCODOPS.Items[cmbInfoCODOPS.SelectedIndex].ToString().ToUpper())
				i++;
			
			MessageBox.Show(codops[i].cadena,codops[i].nombre,MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		
		//Función que muestra la información solicitada en el COMBO salida, es decir
		//Muestra la información del análisis, el archivo temporal o el archivo S19 generado
		void CmbSalidaSelectedIndexChanged(object sender, EventArgs e)
		{
			switch(cmbSalida.SelectedIndex)
			{
				case 0:
					textSalida.Visible = true;
					textCodigoMaquina.Visible = false;
					textS19.Visible = false;
					break;
					
				case 1:
					textSalida.Visible = false;
					textCodigoMaquina.Visible = true;
					textS19.Visible = false;
					break;
					
				case 2:
					textS19.Visible = true;
					textSalida.Visible = false;
					textCodigoMaquina.Visible = false;
					break;
			}
		}
	} // Class
	
	partial class CODOP{
		public String nombre;
		public bool lleva_operando;
		public List<InformacionCODOP> info;
		public String cadena;
		
	}
	partial class InformacionCODOP{
		public String modo_direccionamiento;
		public String codigo_maquina;
		public int b_calculados;
		public int b_calcular;
		public int b_total;
	}
	partial class referenciaModoDir{
		public String contloc;
		public String codop;
		public String modo_direccionamiento;
		public String variante;
		public int iCODOP;
		public int iModoDir;
	}
	partial class BaseObjeto{
		public String longitud;
		public String direccion_inicial;
		public String datos;
		public String checksum;
		
		public BaseObjeto(){
			this.longitud = "";
			this.direccion_inicial = "";
			this.datos = "";
			this.checksum = "";
		}
		public String to_string()
		{
			String val = "";
			val = this.longitud + this.direccion_inicial + this.datos + this.checksum + "\n";
			return val;
		}
	}
	
	partial class LineaCodigoMaquina{
		public String contloc;
		public String etiqueta;
		public String codop;
		public String operando;
		public String codigo_maquina;
		
		public String to_string()
		{
			return this.contloc + "_" + this.etiqueta + "_" + this.codop + "_" + this.operando + "_" + this.codigo_maquina;
		}
	}
} //Namespace

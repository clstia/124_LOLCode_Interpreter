using System;
using System.Collections;
using System.Text.RegularExpressions;
using Gtk;

namespace Milestone_1
{
	public class SyntaxAnalyzer
	{
		private Gtk.ListStore symbolTableModel;
		private Hashtable iHasAHT;
		private ArrayList variableList;
		private String[] codeInput;
		private Gtk.Window theWindow;
		private Gtk.TextView theConsole;
		private String[] current;
		private char[] space_del = { ' ' };

		public SyntaxAnalyzer (String[] codeInput, Gtk.Window theWindow, Gtk.TextView theConsole)
		{
			Milestone_1.RegexList getHas = new Milestone_1.RegexList ();
			iHasAHT = getHas.initVarHT ();

			this.symbolTableModel = new Gtk.ListStore (typeof(String), typeof(String), typeof(String));
			this.codeInput = codeInput;
			this.theWindow = theWindow;
			this.theConsole = theConsole;
			this.variableList = new ArrayList ();
		}

		public void syntaxAnalyzer ()
		{
			/*// check HAI
			if (!Regex.IsMatch (codeInput [0], @"^HAI$")) {
				this.errorMessage ("Y NO HAI");
			}
			// check KTHXBYE
			if (!Regex.IsMatch (codeInput [codeInput.Length - 1], @"^KTHXBYE$")) {
				this.errorMessage ("Y U NO SAI BAI");
			}*/

			// go through all initializations
			this.allInits ();

			// go through all visible
			//this.allVisible ();

			// go through all gimmeh
			//this.allGimmeh ();

			// all arithmetic
			//this.allMath ();

			// all if-else
			//this.ifElse ();



		}

		public Gtk.ListStore getSymbolModel ()
		{
			return this.symbolTableModel;
		}

		private void allMath ()
		{
			foreach (String line in codeInput) {

			}
		}


		private void ifElse ()
		{
			
		}

		private void allGimmeh ()
		{
			foreach (String line in codeInput) {
				if (Regex.IsMatch (line, @"^GIMMEH [a-zA-Z][a-zA-Z0-9_]*$")) {
					current = line.Split (space_del);
					if (current.Length == 2) {
						if (this.checkIfVarExists (current [1])) {
							String consoleText = this.theConsole.Buffer.Text;
							if (consoleText == "") {
								consoleText = String.Concat (consoleText, "GIMMEH ", current [1].Trim ().ToString (), ": ");
							} else {
								consoleText = String.Concat (consoleText, "\nGIMMEH ", current [1].Trim ().ToString (), ": ");
							}
							this.theConsole.Buffer.Text = consoleText;
							// set consoleTextView as editable
							this.theConsole.Editable = true;
							this.theConsole.Buffer.PlaceCursor (this.theConsole.Buffer.EndIter);
							// get the last line of the buffer
							consoleText = this.theConsole.Buffer.Text;
							char[] new_line = { '\n' };
							String[] temp = consoleText.Split (new_line);
							String inputLine = temp [temp.Length - 1];
							// get input
							temp = inputLine.Split (space_del);
							// this is a string input
							if (temp.Length > 3) {
								String input = "";
								for (int i = 2; i < temp.Length; i++) {
									input = String.Concat (input, temp [i], " "); 
								}
								this.alterValue (current [1], input.Trim ());
							} else {
								this.alterValue (current [1], temp [2].Trim ());
							}
						} else {
							this.errorMessage ("NO VAR");
						}
					} else {
						this.errorMessage ("TOO MUCH SENPAI~~");
					}
				}
			}
		}

		private void alterValue (String varName, String newValue)
		{
			Gtk.TreeIter iterator;
			symbolTableModel.GetIterFirst (out iterator);
			for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
				if (symbolTableModel.GetValue (iterator, 1).ToString () == varName) {
					if (Regex.IsMatch (newValue, "^\".*\"$")) {
						symbolTableModel.SetValue (iterator, 0, "YARN");
					}
					if (Regex.IsMatch (newValue, "^(-)?[0-9]+$")) {
						symbolTableModel.SetValue (iterator, 0, "NUMBR");
					}
					if (Regex.IsMatch (newValue, @"^(-)?[0-9]+\.[0-9]+$"))
					{
						symbolTableModel.SetValue (iterator, 0, "NUMBAR");
					}
					if (Regex.IsMatch (newValue, @"(WIN|FAIL)")) {
						symbolTableModel.SetValue (iterator, 0, "TROOF");
					}
					symbolTableModel.SetValue (iterator, 2, newValue);
					return;
				}
			}
		}

		private void allVisible ()
		{
			foreach (String line in codeInput) {
				if (Regex.IsMatch (line, @"^VISIBLE .*")) {
					current = line.Split (space_del);
					// use counter controlled loop instead of data for more control
					String toPrint = "";
					for (int i = 1; i < current.Length; i++) {
						if (Regex.IsMatch (current [i].Trim (), "(^\".*\"$|^\".*$|^.*\")")) {
							toPrint = String.Concat (toPrint, " ", current [i].Trim ());
						} else {
							String x = current[i].Trim () ;
							if (this.checkIfVarExists (current [i].Trim ())) {
								Gtk.TreeIter iterator;
								symbolTableModel.GetIterFirst (out iterator);
								int j;
								for (j = 0; j < symbolTableModel.IterNChildren (); j++) {
									x = symbolTableModel.GetValue (iterator, 1).ToString ();
									if (x == current [i]) {
										toPrint = String.Concat (toPrint, " ", symbolTableModel.GetValue (iterator, 2).ToString ());
									}
								}
							} else {
								//String err = ;  
								this.errorMessage (String.Concat ("Variable ", x.ToString (), " Does Not Exist"));
								return;
							}
						}
					}
					this.printToConsole (toPrint);
				}
			}
		}

		private void printToConsole (String toPrint)
		{
			String consoleOutput = "";
			if (this.theConsole.Buffer.Text == "") {
				this.theConsole.Buffer.Text = toPrint.Replace ("\"", "");
			} else {
				consoleOutput = String.Concat (this.theConsole.Buffer.Text, "\n");
				consoleOutput = String.Concat (consoleOutput, toPrint.Replace ("\"", ""));
				this.theConsole.Buffer.Text = consoleOutput;
			}
		}

		private Boolean checkIfVarExists (String varName)
		{
			foreach (String var in variableList) {
				if (var == varName) {
					return true;
				}
			}
			return false;
		}

		private void allInits ()
		{
			foreach (String line in codeInput) {
				foreach (String pattern in iHasAHT.Keys) {
					if (Regex.IsMatch (line, pattern)) {
						switch (iHasAHT [pattern].ToString ()) {
						case "NOOB":
							current = line.Split (space_del);	
							if (this.checkIfAlreadyDeclared (current [3]))
								this.variableList.Add (current [3]);
							else
								break;
							symbolTableModel.AppendValues (iHasAHT[pattern], current[3], "");
							break;
						default:
							current = line.Split (space_del);
							if (this.checkIfAlreadyDeclared (current [3]))
								this.variableList.Add (current [3]);
							else
								break;
							symbolTableModel.AppendValues (iHasAHT[pattern], current[3],  current[5]);
							break;
						}
					}
				}
			}
		}

		private Boolean checkIfAlreadyDeclared (String varName)
		{
			foreach (String var in variableList) {
				if (var == varName) {
					this.errorMessage ("Variable already exists");
					return false;
				}
			}
			return true;
		}

		private void errorMessage (String message)
		{
			MessageDialog err = new MessageDialog (this.theWindow, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, message);
			err.Title = "Error!";
			err.Run ();
			err.Destroy ();
		}
	}
}


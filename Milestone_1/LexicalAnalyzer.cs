using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milestone_1
{
	public class LexicalAnalyzer
	{
		private Hashtable regexHT, varHT, reservedHT;
		private Gtk.ListStore lexemeModel;
		private Stack delimiterStack = new Stack ();
		private String current = null, commentString = "";

		public LexicalAnalyzer ()
		{
			Milestone_1.RegexList list = new Milestone_1.RegexList ();
			this.regexHT = list.initRegexHT ();
			this.varHT = list.initVarHT ();
			this.reservedHT = list.initReservedHT ();
			lexemeModel = new Gtk.ListStore (typeof(String), typeof(String));
		}

		public void fillLexemes (String[] codeInput)
		{
			char[] space_delimiter = { ' ' };
			// gets 1 line of code from codeInput
			foreach (String line in codeInput)
			{
				// break 1 line into words
				String[] brokenLine = line.Split (space_delimiter);
				// for each word in line
				foreach (String word in brokenLine) 
				{
					if (current == null) {
						current = word.Trim();
					} 
					else {
						current = String.Concat (current, " ", word).Trim();
					}

					// compare current statement to regex list
					foreach (String pattern in regexHT.Keys) 
					{
						if (Regex.IsMatch (current, pattern)) {
							switch (regexHT [pattern].ToString ()) {
							case "Single Line Comment":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								// push single line comment flag to stack
								delimiterStack.Push ("slc");
								// append lexeme for single line comment
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "Start of Block Comment":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// push block comment flag to stack
								delimiterStack.Push ("sbc");
								// append lexeme for start of block comment
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "End of Block Comment":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;

								// if comment string is not blank, append comment string
								if (commentString.Trim ().Length != 0)
									lexemeModel.AppendValues (commentString, "Comment Content");
								// append lexeme
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								// remove flag
								delimiterStack.Pop ();
								// clear comment string
								commentString = null;
								break;
							case "Variable Declaration":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								// append lexeme
								lexemeModel.AppendValues ("I HAS A", regexHT[pattern].ToString ());
								// append variable name
								String[] temp = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp[3], "Variable Name");
								break;
							case "YARN":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;

								// append string lexeme
								char[] quote = { '"' };
								current = current.TrimStart (quote);
								current = current.TrimEnd (quote);
								lexemeModel.AppendValues ("\"", "String Delimiter");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								lexemeModel.AppendValues ("\"", "String Delimiter");
								break;
							case "String Concatenation":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;

								// concatenate the arguments of smoosh into one value
								String[] temp3 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp3 [0], regexHT[pattern].ToString ());
								for (int i = 1; i < temp3.Length; i++) {
									foreach (String var in varHT.Keys) {
										if (Regex.IsMatch (temp3 [i], var)) {
											switch (varHT [var].ToString ()) {
											case "YARN":
												char[] quote2 = { '"' };
												temp3 [i] = temp3 [i].TrimStart (quote2);
												temp3 [i] = temp3 [i].TrimEnd (quote2);
												lexemeModel.AppendValues ("\"", "String Delimiter");
												lexemeModel.AppendValues (temp3 [i], varHT [var].ToString ());
												lexemeModel.AppendValues ("\"", "String Delimiter");
												break;
											default:
												lexemeModel.AppendValues (temp3 [i], varHT [var].ToString ());
												break;
											}
										}
									}
								}
								break;
							case "Standard Input":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								
								String[] temp4 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp4[0], regexHT[pattern].ToString ());
								lexemeModel.AppendValues (temp4[1], "Variable Name");
								break;
							case "Standard Output":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;

								// this will get every argument for VISIBLE
								String[] temp5 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp5 [0], regexHT [pattern].ToString ());
								for (int i = 1; i < temp5.Length; i++) {
									foreach (String var in varHT.Keys) {
										if (Regex.IsMatch (temp5 [i], var)) {
											switch (varHT [var].ToString ()) {
											case "YARN":
												char[] quote2 = { '"' };
												temp5 [i] = temp5 [i].TrimStart (quote2);
												temp5 [i] = temp5 [i].TrimEnd (quote2);
												lexemeModel.AppendValues ("\"", "String Delimiter");
												lexemeModel.AppendValues (temp5 [i], varHT [var].ToString ());
												lexemeModel.AppendValues ("\"", "String Delimiter");
												break;
											default:
												lexemeModel.AppendValues (temp5 [i], varHT [var].ToString ());
												break;
											}
										}
									}
								}
								break;
							case "Max":
							case "Min":
							case "Boolean AND":
							case "Boolean OR":
							case "Boolean XOR":
							case "Boolean NOT":
							case "N-Arity Boolean AND":
							case "N-Arity Boolean OR":
							case "Equality":
							case "Inequality":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								lexemeModel.AppendValues ("IT", "Implicit Variable");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "NOOB":
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								int counter = 0;
								foreach (String reserved in reservedHT.Keys) {
									if (Regex.IsMatch (current, reserved)) {
										break;
									} else {
										counter++;
									}
								}
								if (counter == reservedHT.Count)
									lexemeModel.AppendValues (current, "Variable Name");
								else
									continue;
								break;
							default:
								// if a single line comment is declared before hand
								if (this.checkSLC ())
									break;
								// if a multi-line comment is declared before hand
								if (this.checkMLC ()) 
									break;
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							}
							current = null;
							break;
						}
					}
					if (this.checkSLC ())
						continue;

					if (this.checkMLC ()) 
						continue;
				}
				// add BTW comment string here
				if (this.checkSLC ()) {
					if (commentString.Trim ().Length != 0)
						lexemeModel.AppendValues (commentString.Trim (), "Comment String");
					delimiterStack.Pop (); 
					commentString = "";
				}
			}
		}

		// if a "BTW" is detected, this will catch all incoming words until the whole line is read
		private Boolean checkSLC ()
		{
			if (delimiterStack.Count > 0) {
				if (delimiterStack.Peek ().ToString () == "slc") {
					commentString = String.Concat (commentString, " ", current);
					current = null;
					return true;
				}
			}
			return false;
		}

		// if an "OBTW" is detected, this will catch all incoming inputs until "TLDR"
		private Boolean checkMLC ()
		{
			if (delimiterStack.Count > 0) {
				if (delimiterStack.Peek ().ToString () == "sbc") {
					commentString = String.Concat (commentString, " ", current);
					current = null;
					return true;
				}
			}
			return false;
		}

		public Gtk.ListStore getModel ()
		{
			return lexemeModel;
		}
	}
}


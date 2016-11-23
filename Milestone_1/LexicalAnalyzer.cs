using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milestone_1
{
	public class LexicalAnalyzer
	{
		private Hashtable regexHT, varHT;
		private Gtk.ListStore lexemeModel;
		private Stack delimiterStack = new Stack ();
		private String current = null, commentString = null;

		public LexicalAnalyzer ()
		{
			Milestone_1.RegexList list = new Milestone_1.RegexList ();
			this.regexHT = list.initRegexHT ();
			this.varHT = list.initVarHT ();
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
						current = String.Concat (current, " ", word);
					}

					Console.WriteLine (current); 

					// compare current statement to regex list
					foreach (String pattern in regexHT.Keys) 
					{
						if (Regex.IsMatch (current, pattern)) {
							switch (regexHT [pattern].ToString ()) {
							case "Single Line Comment":
								if (this.checkMLC ()) break;
								String[] comment = current.Split (space_delimiter);
								lexemeModel.AppendValues (comment [0], regexHT [pattern].ToString ());
								lexemeModel.AppendValues (comment [1], "Comment String");
								break;
							case "Start of Block Comment":
								if (this.checkMLC ()) break;
								delimiterStack.Push ("sbc");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "End of Block Comment":
								lexemeModel.AppendValues (commentString, "Comment Content");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								delimiterStack.Pop ();
								commentString = null;
								break;
							case "Variable Declaration":
								lexemeModel.AppendValues ("I HAS A", regexHT[pattern].ToString ());
								String[] temp = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp[3], "NOOB");
								break;
							case "YARN":
								char[] quote = { '"' };
								current = current.TrimStart (quote);
								current = current.TrimEnd (quote);
								lexemeModel.AppendValues ("\"", "String Delimiter");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								lexemeModel.AppendValues ("\"", "String Delimiter");
								break;
							case "Assignment Statement":
								String[] temp2 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp2[0], "NOOB");
								lexemeModel.AppendValues ("R", regexHT[pattern].ToString ());
								break;
							case "String Concatenation":
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
								String[] temp4 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp4[0], regexHT[pattern].ToString ());
								lexemeModel.AppendValues (temp4[1], "NOOB");
								break;
							case "Standard Output":
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
							default:
								if (this.checkMLC ()) break;
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							}
							current = null;
							break;
						}
					}
					if (this.checkMLC ()) break;
				}
			}
		}
	
		// if an "OBTW" is detected, this will catch all incoming inputs until "TLDR"
		private Boolean checkMLC ()
		{
			if (delimiterStack.Count > 0) {
				if (delimiterStack.Peek ().ToString () == "sbc") {
					commentString = String.Concat (commentString, " ", current);
					Console.WriteLine ("current comment string - {0}", commentString); 
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


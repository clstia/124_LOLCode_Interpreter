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
		private String current = null, commentString = null;

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
					// always trim whitespace
					if (current == null) {
						current = word.Trim();
					} 
					else {
						current = String.Concat (current, " ", word).Trim();
					}

					Console.WriteLine (current); 

					// compare current statement to regex list
					foreach (String pattern in regexHT.Keys) 
					{
						if (Regex.IsMatch (current, pattern)) {
							switch (regexHT [pattern].ToString ()) {
							case "Single Line Comment":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;
								// use space as delimiter to split the string
								String[] comment = current.Split (space_delimiter);
								// store the keyword
								//lexemeModel.AppendValues (comment [0], regexHT [pattern].ToString ());
								// derive the whole comment string
								//String tempComment = ""; // temporary string for comment
								//for (int i = 1; i < comment.Length; i++) {
								//	tempComment = String.Concat (tempComment, " ", comment[i]);
								//}
								// append whole comment string
								//lexemeModel.AppendValues (tempComment, "Comment String");
								break;
							case "Start of Block Comment":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;
								
								delimiterStack.Push ("sbc");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "End of Block Comment":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								if (commentString.Trim().Length != 0)
									lexemeModel.AppendValues (commentString, "Comment Content");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								delimiterStack.Pop ();
								commentString = null;
								break;
							case "Variable Declaration":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;

								lexemeModel.AppendValues ("I HAS A", regexHT[pattern].ToString ());
								String[] temp = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp[3], "NOOB");
								break;
							case "YARN":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;


								char[] quote = { '"' };
								current = current.TrimStart (quote);
								current = current.TrimEnd (quote);
								lexemeModel.AppendValues ("\"", "String Delimiter");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								lexemeModel.AppendValues ("\"", "String Delimiter");
								break;
							case "String Concatenation":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;


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
							case "Standard Input":// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;

								String[] temp4 = current.Split (space_delimiter);
								lexemeModel.AppendValues (temp4[0], regexHT[pattern].ToString ());
								lexemeModel.AppendValues (temp4[1], "NOOB");
								break;
							case "Standard Output":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;
								
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
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;
								
								lexemeModel.AppendValues ("IT", "Implicit Variable");
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							case "NOOB":
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
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
									lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								else
									continue;
								break;
							default:
								// this will check if there is a single line comment declared previously
								if (this.checkSLC ())
									break;

								// this will check if there are multiline comments declared previously
								if (this.checkMLC ())
									break;
								
								lexemeModel.AppendValues (current, regexHT [pattern].ToString ());
								break;
							}
							current = null;
							break;
						}
					}
					if (this.checkMLC ()) 
						continue;
					if (this.checkSLC ())
						continue;
				}

				// after reading the whole line, check if that line has single line comment
				if (delimiterStack.Count > 0) {
					if (delimiterStack.Peek ().ToString () == "slc") {
						delimiterStack.Pop ();
						if (commentString.Trim().Length != 0)
							lexemeModel.AppendValues (commentString, "Comment Content");
						commentString = null;
					}
				}
			}
		}
	
		// if a "BTW" is detected, this will catch all incoming outputs until the whole line is read
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
					Console.WriteLine ("current comment string - {0} - length - {1}", commentString.Trim(), commentString.Trim().Length); 
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


using System;
using System.Collections;
using System.Text.RegularExpressions;
using Gtk;

namespace Milestone_1
{
	public class SyntaxAnalyzer_v2
	{
		// UI Elements
		private ListStore symbolTableModel; // symbol table model
		private TextView theConsole; // refers to the console in the main window
		private Window theParent; // refers to the parent window

		// Syntax Analyzer Essentials
		private String[] codeInput; // holds the whole source code
		private char[] space_del = { ' ' }; // space delimiter

		// Constructor
		public SyntaxAnalyzer_v2 (String[] codeInput, Window theParent, TextView theConsole)
		{
			this.codeInput = codeInput;
			this.symbolTableModel = new ListStore (typeof(String), typeof(String), typeof(String));
			this.theParent = theParent;
			this.theConsole = theConsole;
		}

		public void analyzeSyntax ()
		{
			int lineNumber = 1;
			Hashtable regexList = new Milestone_1.RegexList ().initSyntaxHT ();

			foreach (String line in codeInput) {
				// if no 'HAI'
				if (lineNumber == 1) {
					if (!Regex.IsMatch (line.Trim (), @"^HAI$")) {
						this.errorMessage ("There is no code starting keyword");
						break;
					}
				}
				// if no 'KTHXBYE'
				if (lineNumber == codeInput.Length) {
					if (!Regex.IsMatch (line.Trim (), @"^KTHXBYE$")) {
						this.errorMessage ("There is no code terminating keyword");
						break;
					}
				}

				// other lines
				if (lineNumber > 1 && lineNumber < codeInput.Length) {
					foreach (String pattern in regexList.Keys) {
						if (Regex.IsMatch (line, pattern)) {
							switch (regexList [pattern].ToString ()) {
							case "String Concatenation":
								this.concatAll (line.Split (space_del));
								break;
							case "Standard Output":
								this.visibleFunction (line.Split (space_del));
								break;
							case "Standard Input":
								this.gimmehFunction (line.Split (space_del)); 
								break;
							case "Variable Declaration":
								this.varDecFunction (line);
								break;
							case "Assignment Statement":
								this.assignValue (line.Split (space_del)); 
								break;
							case "Arithmetic":
								this.arithmeticFunction (line.Split (space_del));
								break;
							case "Boolean":
								this.booleanFunction (line.Split (space_del));
								break;
							case "Compare":
								this.compareFunction (line.Split (space_del));
								break;
							}
						}
					}
				}

				// increment lineNumber
				lineNumber++;
			}
		}

		// procedure for comparing stuff
		private void compareFunction (String[] line)
		{
			String argument = "";

			switch (line.Length) {
			// both saem
			case 5:
				if (Regex.IsMatch (line [2], "\".*\"") && Regex.IsMatch (line [4], "\".*\"")) {
					// if line[2] is further in the dictionary
					if (String.Compare (line [2], line [4]) == 0)
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else if (Regex.IsMatch (line [2], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [4], "(-)?[0-9]+(\\.[0-9]+)?")) {
					if (int.Parse (line [2]) == int.Parse (line [4]))
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else if (Regex.IsMatch (line [2], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [4], "(-)?[0-9]+\\.[0-9]+")) {
					if (float.Parse (line [2], System.Globalization.CultureInfo.InvariantCulture) == float.Parse (line [4], System.Globalization.CultureInfo.InvariantCulture))
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else {
					this.errorMessage ("Different Data Types");
					return;
				}
				break;
			// diffrint
			case 4:
				if (Regex.IsMatch (line [1], "\".*\"") && Regex.IsMatch (line [3], "\".*\"")) {
					// if line[2] is further in the dictionary
					if (String.Compare (line [1], line [3]) != 0)
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else if (Regex.IsMatch (line [1], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [3], "(-)?[0-9]+(\\.[0-9]+)?")) {
					if (int.Parse (line [1]) != int.Parse (line [3]))
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else if (Regex.IsMatch (line [1], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [3], "(-)?[0-9]+\\.[0-9]+")) {
					if (float.Parse (line [1], System.Globalization.CultureInfo.InvariantCulture) != float.Parse (line [3], System.Globalization.CultureInfo.InvariantCulture))
						this.findIt ("WIN", "TROOF");
					else
						this.findIt ("FAIL", "TROOF");
				} else {
					this.errorMessage ("Different Data Types");
					return;
				}
				break;
			// both saem biggr/smallr
			case 9:
				if (String.Compare (line [2], line [6]) == 0) {
					switch (line[4])
					{
					case "BIGGR":
						if (Regex.IsMatch (line [6], "\".*\"") && Regex.IsMatch (line [8], "\".*\"")) {
							// if line[6] is further in the dictionary
							if (String.Compare (line [6], line [8]) == 1)
								argument = line [6];
							else
								argument = line [8];
						} else if (Regex.IsMatch (line [6], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [8], "(-)?[0-9]+(\\.[0-9]+)?")) {
							if (int.Parse (line [6]) > int.Parse (line [8]))
								argument = line [6];
							else
								argument = line [8];
						} else if (Regex.IsMatch (line [6], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [8], "(-)?[0-9]+\\.[0-9]+")) {
							if (float.Parse (line [6], System.Globalization.CultureInfo.InvariantCulture) > float.Parse (line [8], System.Globalization.CultureInfo.InvariantCulture))
								argument = line [6];
							else
								argument = line [8];
						} else {
							this.errorMessage ("Different Data Types");
							return;
						}
						break;
					case "SMALLR":
						if (Regex.IsMatch (line [6], "\".*\"") && Regex.IsMatch (line [8], "\".*\"")) {
							// if line[6] is further in the dictionary
							if (String.Compare (line [6], line [8]) == -1)
								argument = line [6];
							else
								argument = line [8];
						} else if (Regex.IsMatch (line [6], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [8], "(-)?[0-9]+(\\.[0-9]+)?")) {
							if (int.Parse (line [6]) < int.Parse (line [8]))
								argument = line [6];
							else
								argument = line [8];
						} else if (Regex.IsMatch (line [6], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [8], "(-)?[0-9]+\\.[0-9]+")) {
							if (float.Parse (line [6], System.Globalization.CultureInfo.InvariantCulture) < float.Parse (line [8], System.Globalization.CultureInfo.InvariantCulture))
								argument = line [6];
							else
								argument = line [8];
						} else {
							this.errorMessage ("Different Data Types");
							return;
						}
						break;
					}
					if (Regex.IsMatch (line [2], "\".*\"") && Regex.IsMatch (argument, "\".*\"")) {
						// if line[2] is further in the dictionary
						if (String.Compare (line [2], argument) == 0)
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					} else if (Regex.IsMatch (line [2], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (argument, "(-)?[0-9]+(\\.[0-9]+)?")) {
						if (int.Parse (line [2]) == int.Parse (argument))
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					} else if (Regex.IsMatch (line [2], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (argument, "(-)?[0-9]+\\.[0-9]+")) {
								if (float.Parse (line [2], System.Globalization.CultureInfo.InvariantCulture) == float.Parse (argument, System.Globalization.CultureInfo.InvariantCulture))
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					}
				}
				break;
			// diffrint biggr/smallr
			case 8:
				if (String.Compare (line [1], line [5]) == 0) {
					switch (line[3])
					{
					case "BIGGR":
						if (Regex.IsMatch (line [5], "\".*\"") && Regex.IsMatch (line [7], "\".*\"")) {
							// if line[6] is further in the dictionary
							if (String.Compare (line [5], line [7]) == 1)
								argument = line [5];
							else
								argument = line [7];
						} else if (Regex.IsMatch (line [5], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [7], "(-)?[0-9]+(\\.[0-9]+)?")) {
							if (int.Parse (line [5]) > int.Parse (line [7]))
								argument = line [5];
							else
								argument = line [7];
						} else if (Regex.IsMatch (line [5], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [7], "(-)?[0-9]+\\.[0-9]+")) {
							if (float.Parse (line [5], System.Globalization.CultureInfo.InvariantCulture) > float.Parse (line [7], System.Globalization.CultureInfo.InvariantCulture))
								argument = line [5];
							else
								argument = line [7];
						} else {
							this.errorMessage ("Different Data Types");
							return;
						}
						break;
					case "SMALLR":
						if (Regex.IsMatch (line [5], "\".*\"") && Regex.IsMatch (line [7], "\".*\"")) {
							// if line[6] is further in the dictionary
							if (String.Compare (line [5], line [7]) == -1)
								argument = line [5];
							else
								argument = line [7];
						} else if (Regex.IsMatch (line [5], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (line [7], "(-)?[0-9]+(\\.[0-9]+)?")) {
							if (int.Parse (line [5]) < int.Parse (line [7]))
								argument = line [5];
							else
								argument = line [7];
						} else if (Regex.IsMatch (line [5], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (line [7], "(-)?[0-9]+\\.[0-9]+")) {
							if (float.Parse (line [5], System.Globalization.CultureInfo.InvariantCulture) < float.Parse (line [7], System.Globalization.CultureInfo.InvariantCulture))
								argument = line [5];
							else
								argument = line [7];
						} else {
							this.errorMessage ("Different Data Types");
							return;
						}
						break;
					}
					if (Regex.IsMatch (line [1], "\".*\"") && Regex.IsMatch (argument, "\".*\"")) {
						// if line[2] is further in the dictionary
						if (String.Compare (line [1], argument) == 0)
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					} else if (Regex.IsMatch (line [1], "(-)?[0-9]+(\\.[0-9]+)?") && Regex.IsMatch (argument, "(-)?[0-9]+(\\.[0-9]+)?")) {
						if (int.Parse (line [1]) == int.Parse (argument))
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					} else if (Regex.IsMatch (line [1], "(-)?[0-9]+\\.[0-9]+") && Regex.IsMatch (argument, "(-)?[0-9]+\\.[0-9]+")) {
						if (float.Parse (line [1], System.Globalization.CultureInfo.InvariantCulture) == float.Parse (argument, System.Globalization.CultureInfo.InvariantCulture))
							this.findIt ("WIN", "TROOF");
						else
							this.findIt ("FAIL", "TROOF");
						return;
					}
				}
				break;
			}
		}

		private void findIt (String value, String type)
		{
			TreeIter iterator;
			symbolTableModel.GetIterFirst (out iterator);

			if (this.checkIfVariableExists ("IT")) {
				for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
					if (symbolTableModel.GetValue (iterator, 1).ToString () == "IT") {
						symbolTableModel.SetValue (iterator, 0, type);
						symbolTableModel.SetValue (iterator, 2, value);
						break;
					}
				}
			} else {
				symbolTableModel.AppendValues (type, "IT", value);
			}
		}

		// procedure for boolean functions
		private void booleanFunction (String[] line)
		{
			Stack function = new Stack ();

			foreach (String param in line) {
				if (Regex.IsMatch (param, "^(OF|AN|MKAY)$")) {
					continue;
				} else if (Regex.IsMatch (param, "^(WIN|FAIL|BOTH|EITHER|NOT|WON|ANY|ALL)$")) {
					function.Push (param);
				} else {
					// get the immediate value
					if (!this.checkIfReservedWord (param)) {
						if (this.checkIfVariableExists (param.Trim ())) {
							TreeIter iterator;
							symbolTableModel.GetIterFirst (out iterator);
							for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
								if (symbolTableModel.GetValue (iterator, 1).ToString () == param) {
									function.Push (symbolTableModel.GetValue (iterator, 2).ToString ());
									break;
								}
							}
						} else {
							this.errorMessage (String.Concat (param, " does not exist BOOL"));
						}
					} else {
						this.errorMessage (String.Concat (param, " is a Reserved Word"));
					}
				}
			}

			Stack evaluation = new Stack ();
			Boolean op2, op1;
			
			foreach (String param in function) {
				if (Regex.IsMatch (param, "^WIN$")) {
					evaluation.Push (true); 
				} else if (Regex.IsMatch (param, "^FAIL$")) {
					evaluation.Push (false);
				} else if (Regex.IsMatch (param, "^NOT$")) {
					if (Boolean.Parse (evaluation.Pop ().ToString ()))
						evaluation.Push (false);
					else
						evaluation.Push (true);
				} else{
					switch (param) {
					case "ALL":
					case "BOTH":
						op2 = Boolean.Parse (evaluation.Pop ().ToString ());
						op1 = Boolean.Parse (evaluation.Pop ().ToString ());
						evaluation.Push (op1 && op2);
						break;
					case "ANY":
					case "EITHER":
						op2 = Boolean.Parse (evaluation.Pop ().ToString ());
						op1 = Boolean.Parse (evaluation.Pop ().ToString ());
						evaluation.Push (op1 || op2);
						break;
					case "WON":
						op2 = Boolean.Parse (evaluation.Pop ().ToString ());
						op1 = Boolean.Parse (evaluation.Pop ().ToString ());
						evaluation.Push (op1 ^ op2); 
						break;
					}
				}
			}

			// implicit it
			if (this.checkIfVariableExists ("IT")) {
				TreeIter iterator;
				symbolTableModel.GetIterFirst (out iterator);
				for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
					if (symbolTableModel.GetValue (iterator, 1).ToString () == "IT") {
						symbolTableModel.SetValue (iterator, 0, "TROOF");
						symbolTableModel.SetValue (iterator, 2, evaluation.Pop ().ToString ());
						break;
					}
				}
			} else {
				symbolTableModel.AppendValues ("TROOF", "IT", evaluation.Pop ().ToString ());
			}
		}

		// procedure for string concatenation
		private void concatAll (String[] parts)
		{
			String concat = "";
			char[] quote = { '"' };
			for (int i = 1; i < parts.Length; i++) {
				parts [i] = parts [i].TrimEnd (quote).TrimStart (quote);
				if (this.checkIfVariableExists (parts [i])) {
					TreeIter iterator;
					symbolTableModel.GetIterFirst (out iterator);
					for (int j = 0; j < symbolTableModel.IterNChildren (); j++)
					{
						if (symbolTableModel.GetValue (iterator, 1).ToString () == parts [j]) {
							concat = String.Concat (concat, " ", symbolTableModel.GetValue (iterator, 2).ToString ());
							break;
						}
					}
				} else {
					concat = String.Concat (concat.Trim (), " ", parts [i].ToString ().Trim ());
				}
			}

			Console.WriteLine (concat);

			// implicit it
			if (this.checkIfVariableExists ("IT")) {
				TreeIter iterator;
				symbolTableModel.GetIterFirst (out iterator);
				for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
					if (symbolTableModel.GetValue (iterator, 1).ToString () == "IT") {
						symbolTableModel.SetValue (iterator, 0, "YARN");
						concat = String.Concat ("\"", concat, "\"");
						symbolTableModel.SetValue (iterator, 2, concat);
						break;
					}
				}
			} else {
				concat = String.Concat ("\"", concat, "\"");
				symbolTableModel.AppendValues ("YARN", "IT", concat);
			}
		}

		// procedure for arithmetic operations
		private void arithmeticFunction (String[] equation)
		{
			// create evaluation stack
			Stack parameters = new Stack ();

			// push needed parameters to stack
			foreach (String temp in equation) {
				// for process keywords
				if (Regex.IsMatch (temp, @"^(SUM|PRODUKT|MOD|QUOSHUNT|DIFF)")) {
					parameters.Push (temp);
					continue;
				}

				// for literals
				if (Regex.IsMatch (temp, "(-)?[0-9]+(\\.[0-9]+)?")) {
					parameters.Push (temp);
					continue;
				}

				// for strings
				if (Regex.IsMatch (temp, "\".*\"")) {
					this.errorMessage (String.Concat (temp, " is a YARN. Cannot process YARN."));
				}


				// for variables
				if (Regex.IsMatch (temp, @"^[a-zA-Z][a-zA-Z0-9_]*$")) {
					// ignore "OF" and "AN"
					if (Regex.IsMatch (temp, "^(OF|AN)$")) {
						continue;
					}

					if (this.checkIfReservedWord (temp)) {
						this.errorMessage (String.Concat (temp, " is a Reserved Word. You can not use this."));
					}
					if (!this.checkIfVariableExists (temp)) {
						this.errorMessage (String.Concat (temp, " is not yet declared."));
					} else {
						// traverse symbol table
						TreeIter iterator;
						symbolTableModel.GetIterFirst (out iterator);
						for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
							if (symbolTableModel.GetValue (iterator, 1).ToString () == temp) {
								if (!Regex.IsMatch (symbolTableModel.GetValue (iterator, 2).ToString (), "^\".*\"$")) {
									parameters.Push (symbolTableModel.GetValue (iterator, 2).ToString ());
								} else {
									this.errorMessage (String.Concat (symbolTableModel.GetValue (iterator, 2).ToString (), " is a YARN. Cannot process YARN."));
									return;
								}
							}
						}
					}
				}
			}

			// create data structure for stack evaluation
			Stack operandList = new Stack ();

			try {
				// evaluate the stack
				foreach (String operand in parameters) {
					if (Regex.IsMatch (operand, @"(-)?[0-9]+(\.[0-9]+)?")) {
						operandList.Push (operand);
					} else {
						// initially convert to float to easily handle floats if ever needed
						float op2 = float.Parse (operandList.Pop ().ToString (), System.Globalization.CultureInfo.InvariantCulture);
						float op1 = float.Parse (operandList.Pop ().ToString (), System.Globalization.CultureInfo.InvariantCulture);
						switch (operand)
						{
						case "SUM":
							operandList.Push (op1 + op2);
							break;
						case "PRODUKT":
							operandList.Push (op1 * op2);
							break;
						case "DIFF":
							operandList.Push (op1 - op2);
							break;
						case "QUOSHUNT":
							operandList.Push (op1 / op2);
							break;
						case "MOD":
							operandList.Push (op1 % op2);
							break;
						}
					}
				}
			} catch (Exception e)
			{
				this.errorMessage ("Arithmetic Error");
				Console.WriteLine (e.StackTrace);
				return;
			} finally {
				try {
					String finalAnswer = operandList.Pop ().ToString ();
					if (Regex.IsMatch (finalAnswer, "^(-)?[0-9]+$"))
					{
						if (this.checkIfVariableExists ("IT")) {
							TreeIter iterator;
							symbolTableModel.GetIterFirst (out iterator);
							for (int i = 0; i < symbolTableModel.IterNChildren (); i++)
							{
								if (symbolTableModel.GetValue (iterator, 1).ToString () == "IT")
								{
									symbolTableModel.SetValue (iterator, 2, finalAnswer);
								}
							}
						} else {
							symbolTableModel.AppendValues ("NUMBR", "IT", finalAnswer);
						}
					} else {
						if (this.checkIfVariableExists ("IT")) {
							TreeIter iterator;
							symbolTableModel.GetIterFirst (out iterator);
							for (int i = 0; i < symbolTableModel.IterNChildren (); i++)
							{
								if (symbolTableModel.GetValue (iterator, 1).ToString () == "IT")
								{
									symbolTableModel.SetValue (iterator, 2, finalAnswer);
								}
							}
						} else {
							symbolTableModel.AppendValues ("NUMBAR", "IT", finalAnswer);
						}
					}
				}
				catch (Exception e2) {
					this.errorMessage ("Arithmetic Error 2");
					Console.WriteLine (e2.StackTrace);
				}
			}
		}

		// procedure for assignment statement
		private void assignValue (String[] declaration)
		{
			// check if variable is reserved word
			if (this.checkIfReservedWord (declaration [0])) {
				this.errorMessage (String.Concat (declaration [0], " is a Reserved Word. You can not use this."));
				return;
			}

			// check if variable is already declared
			if (!this.checkIfVariableExists (declaration [0])) {
				this.errorMessage (String.Concat (declaration [0], " is not yet declared. Declare it first."));
				return;
			}

			// compile new value
			String newValue = "";
			for (int i = 2; i < declaration.Length; i++) {
				newValue = String.Concat (newValue.Trim (), " ", declaration [i].Trim ()).Trim ();
			}

			// replace value inside symbol table
			TreeIter iterator;
			symbolTableModel.GetIterFirst (out iterator);
			// traverse symbol table
			for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
				// find target variable
				if (symbolTableModel.GetValue (iterator, 1).ToString () == declaration[0]) {
					// get variable type
					if (Regex.IsMatch (newValue, "^\".*\"$")) {
						symbolTableModel.SetValue (iterator, 0, "YARN");
					}
					if (Regex.IsMatch (newValue, "^(-)?[0-9]+$")) {
						symbolTableModel.SetValue (iterator, 0, "NUMBR");
					}
					if (Regex.IsMatch (newValue, @"^(-)?[0-9]+\.[0-9]+$")) {
						symbolTableModel.SetValue (iterator, 0, "NUMBAR");
					}
					if (Regex.IsMatch (newValue, "^(WIN|FAIL)$")) {
						symbolTableModel.SetValue (iterator, 0, "TROOF");
					}
					// add new value
					symbolTableModel.SetValue (iterator, 2, newValue.Trim ());
				}
			}
		}

		// procedure for variable declaration
		private void varDecFunction (String declaration)
		{
			// rely on another regex list for definition of declarations
			Hashtable varDecPatterns = new Milestone_1.RegexList ().initIHasHT ();
			foreach (String pattern in varDecPatterns.Keys) {
				if (Regex.IsMatch (declaration, pattern)) {
					// tokenize declaration statement
					String[] param = declaration.Split (space_del);

					// check first if the variable is a reserved word
					if (this.checkIfReservedWord (param [3])) {
						this.errorMessage (String.Concat (param [3], " is a Reserved Word. You can not use this."));
						return;
					}

					// if this is already in the symbol table, spawn error message
					if (this.checkIfVariableExists (param [3])) {
						this.errorMessage (String.Concat (param [3], " is already declared"));
						return;
					}

					// switch serves as filter of what variable is being added
					switch (varDecPatterns [pattern].ToString ()) {
					// noob has no initial value
					case "NOOB":
						symbolTableModel.AppendValues (varDecPatterns [pattern].ToString ().Trim (), param [3].Trim (), "");
						break;
					// variables initialized by expressions are also special cases since preprocessing of value is needed
					case "Expression Defined":
						break;
					// yarns need to be concatenated properly since they are separated by spaces
					case "YARN":
						String stringValue = "";
						for (int i = 5; i < param.Length; i++) {
							stringValue = String.Concat (stringValue.Trim (), " ", param [i].Trim ()); 
						}
						symbolTableModel.AppendValues (varDecPatterns [pattern].ToString ().Trim (), param [3].Trim (), stringValue.Trim ());
						break;
					// troofs, numbrs, and numbars have simple values compared to the others
					default:
						symbolTableModel.AppendValues (varDecPatterns [pattern].ToString ().Trim (), param [3].Trim (), param [5].Trim ());
						break;
					}
				}
			}
		}

		// procedure for input
		private void gimmehFunction (String[] inputArguments)
		{

		}

		// procedure for printing
		private void visibleFunction (String[] printArguments)
		{
			// temporary variable for printing
			String toPrint = "";

			// if there are no supplied arguments
			if (printArguments.Length == 1) {
				this.errorMessage ("There is nothing to print here");
				return;
			}

			// traverse the entire printArugments array
			for (int i = 1; i < printArguments.Length; i++) {
				// if this certain word is a variable, get value
				if (this.checkIfVariableExists (printArguments [i])) {
					// define a tree iterator
					TreeIter iterator;
					// get value of iterator of said tree
					symbolTableModel.GetIterFirst (out iterator);
					for (int j = 0; j < symbolTableModel.IterNChildren (); j++) {
						String temp = symbolTableModel.GetValue (iterator, 1).ToString ();
						if (temp == printArguments [i]) {
							toPrint = String.Concat (toPrint, " ", symbolTableModel.GetValue (iterator, 2).ToString ());
						}
					}
				}
				// if the argument is an undeclared variable, issue error
				else 
				{
					if (Regex.IsMatch (printArguments [i], "^[a-zA-Z][a-zA-Z0-9_]*$")) {
						this.errorMessage (String.Concat (printArguments[i], " does not exist"));
						return;
					} else {
						toPrint = String.Concat (toPrint, " ", printArguments [i].Trim ());
					}
				}
			}
			// after all the processing, print said statement
			this.printToConsole (toPrint);
		}

		// checks if a variable is a reserved word. returns true if it is a reserved word.
		private Boolean checkIfReservedWord (String variableName)
		{
			Hashtable reservedWords = new Milestone_1.RegexList ().initReservedHT ();
			foreach (String word in reservedWords.Keys) {
				if (Regex.IsMatch (variableName, word)) {
					return true;
				}
			}
			return false;
		}

		// checks if a variable is already declared. returns true if it is already declared
		private Boolean checkIfVariableExists (String variableName)
		{
			TreeIter iterator;
			symbolTableModel.GetIterFirst (out iterator);
			for (int i = 0; i < symbolTableModel.IterNChildren (); i++) {
				if (symbolTableModel.GetValue (iterator, 1).ToString ().Equals (variableName.Trim ())) {
					return true;
				}
			}
			return false;

		}

		// this procedure will print a line of text to the console
		private void printToConsole (String toPrint)
		{
			String consoleOutput = "";
			// if the console is empty
			if (this.theConsole.Buffer.Text == "") {
				this.theConsole.Buffer.Text = toPrint.Replace ("\"", "");
			} 
			// if the console is not empty
			else {
				consoleOutput = String.Concat (this.theConsole.Buffer.Text, "\n");
				consoleOutput = String.Concat (consoleOutput, toPrint.Replace ("\"", ""));
				this.theConsole.Buffer.Text = consoleOutput;
			}
		}

		// this is a pop-up window that displays a certain error message
		private void errorMessage (String message)
		{
			MessageDialog err = new MessageDialog (this.theParent, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, message);
			err.Title = "Error!";
			err.Run ();
			err.Destroy ();
		}

		// return symbol table model
		public ListStore getSymbolModel ()
		{
			return this.symbolTableModel;
		}
	}
}


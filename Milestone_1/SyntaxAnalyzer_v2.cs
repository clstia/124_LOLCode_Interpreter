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

		// Constructor
		public SyntaxAnalyzer_v2 (String[] codeInput, Window theParent, TextView theConsole)
		{
			this.codeInput = codeInput;
			this.symbolTableModel = new ListStore (typeof(String), typeof(String), typeof(String));
			this.theParent = theParent;
			this.theConsole = theConsole;
		}
	}
}


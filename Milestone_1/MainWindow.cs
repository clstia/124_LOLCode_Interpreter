using System;
using Gtk;
using System.IO;

public partial class MainWindow: Gtk.Window
{
	private String fileName, prevFileName = "";
	private StreamReader sr;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		this.initTreeViews ();
	}

	// initialize the column headers to the tree views for Lexemes and Symbol Table
	private void initTreeViews ()
	{
		lexemeTreeView.AppendColumn ("Lexeme", new Gtk.CellRendererText(), "text", 0);
		lexemeTreeView.AppendColumn ("Value", new Gtk.CellRendererText(), "text", 1);

		symbolTreeView.AppendColumn ("Type", new Gtk.CellRendererText (), "text", 0);
		symbolTreeView.AppendColumn ("Symbol", new Gtk.CellRendererText(), "text", 1);
		symbolTreeView.AppendColumn ("Value", new Gtk.CellRendererText (), "text", 2);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExecuteButtonClicked (object sender, EventArgs e)
	{
		if (fileName != null) 
		{
			this.readFile (); 
		}

		this.interpretCode ();
	}

	// get filename of target file
	protected void OnOpenFileButtonSelectionChanged (object sender, EventArgs e)
	{
		fileName = openFileButton.Filename.ToString (); 
	}

	private void readFile ()
	{
		if (this.fileName != this.prevFileName) 
		{
			String inputBuffer = "", nextLine, nextLine2;

			// create stream reader
			try
			{
				sr = new StreamReader (fileName);
				// read file while not EOF
				while ((nextLine = sr.ReadLine ()) != null)
				{
					nextLine2 = String.Concat (nextLine, "\n");
					inputBuffer = String.Concat (inputBuffer, nextLine2); 
				}
				// add read file to text view for code
				codeTextView.Buffer.Text = inputBuffer.Trim();
			}
			catch (IOException io_exception) 
			{
				Console.WriteLine (io_exception.StackTrace); 
			}
			finally 
			{
				// set current filename to prev filename in anticipation of next code input
				// or when code file input is not changed but text view input is changed
				this.prevFileName = this.fileName;
				// close stream reader
				sr.Close (); 
			}
		}
	}

	private void interpretCode ()
	{
		char[] delimiter = { '\n' };
		Milestone_1.LexicalAnalyzer fillLexemes = new Milestone_1.LexicalAnalyzer ();
		fillLexemes.fillLexemes (codeTextView.Buffer.Text.Split (delimiter));
		lexemeTreeView.Model = fillLexemes.getModel ();
	}

}

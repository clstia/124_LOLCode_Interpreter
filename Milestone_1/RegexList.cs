using System;
using System.Collections;

namespace Milestone_1
{
	public class RegexList : Hashtable
	{
		public Hashtable regexHT;
		public Hashtable varHT;

		public RegexList ()
		{
			
		}

		public Hashtable initVarHT ()
		{
			varHT = new Hashtable ();

			// variables
			varHT.Add ("^\".*\"$", "YARN");
			varHT.Add (@"^(-)?[0-9]+$", "NUMBR");
			varHT.Add (@"^(-)?[0-9]+\.[0-9]+$", "NUMBAR");
			varHT.Add (@"^(WIN|FAIL)$", "TROOF");
			varHT.Add (@"^[a-zA-Z][a-zA-Z0-9_]+$", "NOOB");

			return varHT;
		}

		public Hashtable initRegexHT ()
		{
			regexHT = new Hashtable ();

			// start and end of code
			regexHT.Add (@"^HAI$", "Start of Code"); // m1 done
			regexHT.Add (@"^KTHXBYE$", "End of Code"); // m1 done

			// comments
			regexHT.Add (@"^BTW .*", "Single Line Comment"); // m1 done
			regexHT.Add (@"^OBTW$", "Start of Block Comment"); // m1 done
			regexHT.Add (@"^TLDR$", "End of Block Comment"); // m1 done

			// standard input and output
			regexHT.Add (@"^VISIBLE .*", "Standard Output"); // m1 done
			regexHT.Add (@"^GIMMEH [a-zA-Z][a-zA-Z0-9_]*", "Standard Input"); // m1 done

			// variable operations
			regexHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]*$", "Variable Declaration"); // m1 done 
			regexHT.Add ("^ITZ", "Initialization"); // m1 done
			regexHT.Add (@"^[a-zA-Z][a-zA-Z0-9_]* R$", "Assignment Statement"); // m1 done
			regexHT.Add (@"^SMOOSH .*", "String Concatenation");  // m1 done

			// control flow structures
			regexHT.Add (@"^O RLY?$", "Start of IF Block");
			regexHT.Add (@"^YA RLY$", "IF Clause");
			regexHT.Add (@"^NO WAI$", "ELSE Clause");
			regexHT.Add (@"^WTF?$", "Start of CASE Block");
			regexHT.Add (@"^OMG", "CASE Clause");
			regexHT.Add (@"^GTFO$", "BREAK Clause");
			regexHT.Add (@"^OMGWTF$", "DEFAULT Clause");
			regexHT.Add (@"^OIC$", "End of Control Block");

			// argument separator
			regexHT.Add (@"^AN$", "Argument Separator"); // m1 done

			// arithmetic operations
			regexHT.Add (@"^SUM OF$", "Addition"); // m1 done
			regexHT.Add (@"^DIFF OF$", "Subtraction"); // m1 done
			regexHT.Add (@"^PRODUKT OF$", "Multiplication"); // m1 done
			regexHT.Add (@"^QUOSHUNT OF$", "Division"); // m1 done
			regexHT.Add (@"^MOD OF$", "Modulo Division"); // m1 done

			// comparative operations
			regexHT.Add (@"^BIGGR OF", "Max");
			regexHT.Add (@"^SMALLR OF", "Min");
			regexHT.Add (@"^BOTH OF", "Boolean AND");
			regexHT.Add (@"^EITHER OF", "Boolean OR");
			regexHT.Add (@"^WON OF", "Boolean XOR");
			regexHT.Add (@"^NOT", "Boolean NOT");
			regexHT.Add (@"^ANY OF", "N-Arity Boolean OR");
			regexHT.Add (@"^ALL OF", "N-Arity Boolean AND");
			regexHT.Add (@"^BOTH SAEM", "Equality");
			regexHT.Add (@"^DIFFRINT", "Inequality"); 

			// variables
			regexHT.Add ("^\".*\"$", "YARN"); // m1 done
			regexHT.Add (@"^(-)?[0-9]+$", "NUMBR"); // m1 done
			regexHT.Add (@"^(-)?[0-9]+\.[0-9]+$", "NUMBAR"); // m1 done
			regexHT.Add (@"^(WIN|FAIL)$", "TROOF"); // m1 done

			return regexHT;
		}
	}
}


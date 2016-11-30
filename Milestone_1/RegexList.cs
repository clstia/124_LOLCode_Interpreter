using System;
using System.Collections;

namespace Milestone_1
{
	public class RegexList : Hashtable
	{
		public Hashtable genericHT;

		public RegexList ()
		{
			
		}

		public Hashtable initArithmetic ()
		{
			genericHT = new Hashtable ();
			return genericHT;
		}

		public Hashtable initIHasHT ()
		{
			genericHT = new Hashtable ();

			// I HAS A
			genericHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]*$", "NOOB");
			genericHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]* ITZ (WIN|FAIL)", "TROOF");
			genericHT.Add ("^I HAS A [a-zA-Z][a-zA-Z0-9_]* ITZ \".*\"$", "YARN");
			genericHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]* ITZ (-)?[0-9]+$", "NUMBR");
			genericHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]* ITZ (-)?[0-9]+\.[0-9]+$", "NUMBAR"); 
			// add init with expression

			return genericHT;
		}

		public Hashtable initReservedHT ()
		{
			genericHT = new Hashtable ();

			genericHT.Add (@"^HAI$", "Reserved"); 
			genericHT.Add (@"^KTHXBYE$", "Reserved");
			genericHT.Add (@"^BTW$", "Reserved");
			genericHT.Add (@"^OBTW$", "Reserved");
			genericHT.Add (@"^TLDR$", "Reserved");
			genericHT.Add (@"^VISIBLE$", "Reserved");
			genericHT.Add (@"^GIMMEH$", "Reserved");
			genericHT.Add (@"^I$", "Reserved");
			genericHT.Add (@"^HAS$", "Reserved");
			genericHT.Add (@"^A$", "Reserved");
			genericHT.Add (@"^ITZ$", "Reserved");
			genericHT.Add (@"^R$", "Reserved");
			genericHT.Add (@"^SMOOSH$", "Reserved");
			genericHT.Add (@"^O$", "Reserved");
			genericHT.Add (@"^RLY$", "Reserved");
			genericHT.Add (@"^YA$", "Reserved");
			genericHT.Add (@"^NO$", "Reserved");
			genericHT.Add (@"^WAI$", "Reserved");
			genericHT.Add (@"^WTF$", "Reserved");
			genericHT.Add (@"^OMG$", "Reserved");
			genericHT.Add (@"^GTFO$", "Reserved");
			genericHT.Add (@"^OMGWTF$", "Reserved");
			genericHT.Add (@"^OIC$", "Reserved");
			genericHT.Add (@"^IT$", "Reserved");
			genericHT.Add (@"^AN$", "Reserved");
			genericHT.Add (@"^OF$", "Reserved");
			genericHT.Add (@"^SUM$", "Reserved");
			genericHT.Add (@"^DIFF$", "Reserved");
			genericHT.Add (@"^PRODUKT$", "Reserved");
			genericHT.Add (@"^QUOSHUNT$", "Reserved");
			genericHT.Add (@"^MOD$", "Reserved");
			genericHT.Add (@"^BIGGR$", "Reserved");
			genericHT.Add (@"^SMALLR$", "Reserved");
			genericHT.Add (@"^BOTH$", "Reserved");
			genericHT.Add (@"^EITHER$", "Reserved");
			genericHT.Add (@"^ANY$", "Reserved");
			genericHT.Add (@"^ALL$", "Reserved");
			genericHT.Add (@"^NOT$", "Reserved");
			genericHT.Add (@"^WON$", "Reserved");
			genericHT.Add (@"^SAEM$", "Reserved");
			genericHT.Add (@"^DIFFRINT$", "Reserved");

			return genericHT;
		}

		public Hashtable initVarHT ()
		{
			genericHT = new Hashtable ();

			// variables
			genericHT.Add ("^\".*\"$", "YARN");
			genericHT.Add (@"^(-)?[0-9]+$", "NUMBR");
			genericHT.Add (@"^(-)?[0-9]+\.[0-9]+$", "NUMBAR");
			genericHT.Add (@"^(WIN|FAIL)$", "TROOF");
			genericHT.Add (@"^[a-zA-Z][a-zA-Z0-9_]+$", "NOOB");

			return genericHT;
		}

		public Hashtable initRegexHT ()
		{
			genericHT = new Hashtable ();

			// comparative operations
			genericHT.Add (@"^BIGGR OF$", "Max"); // m1 done. need to detect arguments
			genericHT.Add (@"^SMALLR OF$", "Min"); // m1 done . need to detect arguments
			genericHT.Add (@"^BOTH OF$", "Boolean AND"); // m1 done . need to detect arguments
			genericHT.Add (@"^EITHER OF$", "Boolean OR"); // m1 done . need to detect arguments
			genericHT.Add (@"^WON OF$", "Boolean XOR"); // m1 done . need to detect arguments
			genericHT.Add (@"^NOT$", "Boolean NOT"); // m1 done . need to detect arguments
			genericHT.Add (@"^ANY OF$", "N-Arity Boolean OR"); // m1 done . need to detect arguments
			genericHT.Add (@"^ALL OF$", "N-Arity Boolean AND"); // m1 done . need to detect arguments
			genericHT.Add (@"^BOTH SAEM$", "Equality"); // m1 done . need to detect arguments
			genericHT.Add (@"^DIFFRINT$", "Inequality");  // m1 done . need to detect arguments

			// start and end of code
			genericHT.Add (@"^HAI$", "Start of Code"); // m1 done
			genericHT.Add (@"^KTHXBYE$", "End of Code"); // m1 done

			// comments
			genericHT.Add (@"^BTW .*", "Single Line Comment"); // m1 done
			genericHT.Add (@"^OBTW$", "Start of Block Comment"); // m1 done
			genericHT.Add (@"^TLDR$", "End of Block Comment"); // m1 done

			// standard input and output
			genericHT.Add (@"^VISIBLE", "Standard Output"); // m1 done
			genericHT.Add (@"^GIMMEH [a-zA-Z][a-zA-Z0-9_]*", "Standard Input"); // m1 done

			// variable operations
			genericHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]*$", "Variable Declaration"); // m1 done 
			genericHT.Add (@"^ITZ", "Initialization"); // m1 done
			genericHT.Add (@"^R$", "Assignment Statement"); // m1 done
			genericHT.Add (@"^SMOOSH .*", "String Concatenation");  // m1 done

			// control flow structures
			genericHT.Add (@"^O RLY\?$", "Start of IF Block"); //m1 done
			genericHT.Add (@"^YA RLY$", "IF Clause"); // m1 done
			genericHT.Add (@"^NO WAI$", "ELSE Clause"); // m1 done
			genericHT.Add (@"^WTF\?$", "Start of CASE Block"); // m1 done
			genericHT.Add (@"^OMG$", "CASE Clause"); // m1 done
			genericHT.Add (@"^GTFO$", "BREAK Clause"); // m1 done
			genericHT.Add (@"^OMGWTF$", "DEFAULT Clause"); // m1 done
			genericHT.Add (@"^OIC$", "End of Control Block"); // m1 done

			// argument separator
			genericHT.Add (@"^AN$", "Argument Separator"); // m1 done

			// arithmetic operations
			genericHT.Add (@"^SUM OF$", "Addition"); // m1 done
			genericHT.Add (@"^DIFF OF$", "Subtraction"); // m1 done
			genericHT.Add (@"^PRODUKT OF$", "Multiplication"); // m1 done
			genericHT.Add (@"^QUOSHUNT OF$", "Division"); // m1 done
			genericHT.Add (@"^MOD OF$", "Modulo Division"); // m1 done

			// variables
			genericHT.Add ("^\".*\"$", "YARN"); // m1 done
			genericHT.Add (@"^(-)?[0-9]+$", "NUMBR"); // m1 done
			genericHT.Add (@"^(-)?[0-9]+\.[0-9]+$", "NUMBAR"); // m1 done
			genericHT.Add (@"^(WIN|FAIL)$", "TROOF"); // m1 done
			genericHT.Add (@"^[a-zA-Z][a-zA-Z0-9_]*$", "NOOB");

			return genericHT;
		}
	}
}


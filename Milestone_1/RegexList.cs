using System;
using System.Collections;

namespace Milestone_1
{
	public class RegexList : Hashtable
	{
		public Hashtable regexHT;
		public Hashtable varHT;
		public Hashtable reservedHT;

		public RegexList ()
		{
			
		}

		public Hashtable initReservedHT ()
		{
			reservedHT = new Hashtable ();

			reservedHT.Add (@"^HAI$", "Reserved"); 
			reservedHT.Add (@"^KTHXBYE$", "Reserved");
			reservedHT.Add (@"^BTW$", "Reserved");
			reservedHT.Add (@"^OBTW$", "Reserved");
			reservedHT.Add (@"^TLDR$", "Reserved");
			reservedHT.Add (@"^VISIBLE$", "Reserved");
			reservedHT.Add (@"^GIMMEH$", "Reserved");
			reservedHT.Add (@"^I$", "Reserved");
			reservedHT.Add (@"^HAS$", "Reserved");
			reservedHT.Add (@"^A$", "Reserved");
			reservedHT.Add (@"^ITZ$", "Reserved");
			reservedHT.Add (@"^R$", "Reserved");
			reservedHT.Add (@"^SMOOSH$", "Reserved");
			reservedHT.Add (@"^O$", "Reserved");
			reservedHT.Add (@"^RLY$", "Reserved");
			reservedHT.Add (@"^YA$", "Reserved");
			reservedHT.Add (@"^NO$", "Reserved");
			reservedHT.Add (@"^WAI$", "Reserved");
			reservedHT.Add (@"^WTF$", "Reserved");
			reservedHT.Add (@"^OMG$", "Reserved");
			reservedHT.Add (@"^GTFO$", "Reserved");
			reservedHT.Add (@"^OMGWTF$", "Reserved");
			reservedHT.Add (@"^OIC$", "Reserved");
			reservedHT.Add (@"^IT$", "Reserved");
			reservedHT.Add (@"^AN$", "Reserved");
			reservedHT.Add (@"^OF$", "Reserved");
			reservedHT.Add (@"^SUM$", "Reserved");
			reservedHT.Add (@"^DIFF$", "Reserved");
			reservedHT.Add (@"^PRODUKT$", "Reserved");
			reservedHT.Add (@"^QUOSHUNT$", "Reserved");
			reservedHT.Add (@"^MOD$", "Reserved");
			reservedHT.Add (@"^BIGGR$", "Reserved");
			reservedHT.Add (@"^SMALLR$", "Reserved");
			reservedHT.Add (@"^BOTH$", "Reserved");
			reservedHT.Add (@"^EITHER$", "Reserved");
			reservedHT.Add (@"^ANY$", "Reserved");
			reservedHT.Add (@"^ALL$", "Reserved");
			reservedHT.Add (@"^NOT$", "Reserved");
			reservedHT.Add (@"^WON$", "Reserved");
			reservedHT.Add (@"^SAEM$", "Reserved");
			reservedHT.Add (@"^DIFFRINT$", "Reserved");

			return reservedHT;
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

			// comparative operations
			regexHT.Add (@"^BIGGR OF$", "Max"); // m1 done. need to detect arguments
			regexHT.Add (@"^SMALLR OF$", "Min"); // m1 done . need to detect arguments
			regexHT.Add (@"^BOTH OF$", "Boolean AND"); // m1 done . need to detect arguments
			regexHT.Add (@"^EITHER OF$", "Boolean OR"); // m1 done . need to detect arguments
			regexHT.Add (@"^WON OF$", "Boolean XOR"); // m1 done . need to detect arguments
			regexHT.Add (@"^NOT$", "Boolean NOT"); // m1 done . need to detect arguments
			regexHT.Add (@"^ANY OF$", "N-Arity Boolean OR"); // m1 done . need to detect arguments
			regexHT.Add (@"^ALL OF$", "N-Arity Boolean AND"); // m1 done . need to detect arguments
			regexHT.Add (@"^BOTH SAEM$", "Equality"); // m1 done . need to detect arguments
			regexHT.Add (@"^DIFFRINT$", "Inequality");  // m1 done . need to detect arguments

			// start and end of code
			regexHT.Add (@"^HAI$", "Start of Code"); // m1 done
			regexHT.Add (@"^KTHXBYE$", "End of Code"); // m1 done

			// comments
			regexHT.Add (@"^BTW .*", "Single Line Comment"); // m1 done
			regexHT.Add (@"^OBTW$", "Start of Block Comment"); // m1 done
			regexHT.Add (@"^TLDR$", "End of Block Comment"); // m1 done

			// standard input and output
			regexHT.Add (@"^VISIBLE", "Standard Output"); // m1 done
			regexHT.Add (@"^GIMMEH [a-zA-Z][a-zA-Z0-9_]*", "Standard Input"); // m1 done

			// variable operations
			regexHT.Add (@"^I HAS A [a-zA-Z][a-zA-Z0-9_]*$", "Variable Declaration"); // m1 done 
			regexHT.Add (@"^ITZ", "Initialization"); // m1 done
			regexHT.Add (@"^R$", "Assignment Statement"); // m1 done
			regexHT.Add (@"^SMOOSH .*", "String Concatenation");  // m1 done

			// control flow structures
			regexHT.Add (@"^O RLY\?$", "Start of IF Block"); //m1 done
			regexHT.Add (@"^YA RLY$", "IF Clause"); // m1 done
			regexHT.Add (@"^NO WAI$", "ELSE Clause"); // m1 done
			regexHT.Add (@"^WTF\?$", "Start of CASE Block"); // m1 done
			regexHT.Add (@"^OMG$", "CASE Clause"); // m1 done
			regexHT.Add (@"^GTFO$", "BREAK Clause"); // m1 done
			regexHT.Add (@"^OMGWTF$", "DEFAULT Clause"); // m1 done
			regexHT.Add (@"^OIC$", "End of Control Block"); // m1 done

			// argument separator
			regexHT.Add (@"^AN$", "Argument Separator"); // m1 done

			// arithmetic operations
			regexHT.Add (@"^SUM OF$", "Addition"); // m1 done
			regexHT.Add (@"^DIFF OF$", "Subtraction"); // m1 done
			regexHT.Add (@"^PRODUKT OF$", "Multiplication"); // m1 done
			regexHT.Add (@"^QUOSHUNT OF$", "Division"); // m1 done
			regexHT.Add (@"^MOD OF$", "Modulo Division"); // m1 done

			// variables
			regexHT.Add ("^\".*\"$", "YARN"); // m1 done
			regexHT.Add (@"^(-)?[0-9]+$", "NUMBR"); // m1 done
			regexHT.Add (@"^(-)?[0-9]+\.[0-9]+$", "NUMBAR"); // m1 done
			regexHT.Add (@"^(WIN|FAIL)$", "TROOF"); // m1 done
			regexHT.Add (@"^[a-zA-Z][a-zA-Z0-9_]*$", "NOOB");

			return regexHT;
		}
	}
}


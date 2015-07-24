using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVitamins.XmlDocumentation.Compatibility
{
	class Program
	{
		static void Main(string[] arguments)
		{
			var args = new ParsedArguments(arguments);

			if (args.Params.Count > 0)
			{
				string inputFile = args.Params.FirstOrDefault();
				var util = new XmlDocumentSweeper(inputFile);

				string outputFile = args.GetNamedParam("output");
				if (string.IsNullOrWhiteSpace(outputFile) && args.HasSwitch("replace"))
					outputFile = inputFile;

				if (!string.IsNullOrWhiteSpace(outputFile))
					util.Transform(outputFile);
				else
					Console.WriteLine(util.Transform());
			}
			else
				usage();
		}

		static void usage()
		{
			Console.WriteLine(@"xmldoc-markdown-compat.exe ""path\to\a\file.xml"" [/replace] [/output:""path\to\a\new.xml""]

e.g.
xmldoc-markdown-compat.exe file.xml /replace

  overwrites file.xml with the new version

xmldoc-markdown-compat.exe file.xml /output:new.xml

  saves changes to new.xml

xmldoc-markdown-compat.exe file.xml

  outputs the changes to console
");
		}
	}
}

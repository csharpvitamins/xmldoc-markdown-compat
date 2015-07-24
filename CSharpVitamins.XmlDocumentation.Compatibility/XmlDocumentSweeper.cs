using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CSharpVitamins.XmlDocumentation.Compatibility
{
	public class XmlDocumentSweeper
	{
		static string[] defaultElements = new[]
		{
			"summary",
			"remarks",
			"param",
			"response"
		};

		public XmlDocumentSweeper(string targetFile = null, string[] targetElements = null)
		{
			this.TargetFile = targetFile;
			this.TargetElements = targetElements ?? defaultElements;
		}

		/// <summary>
		/// The file to transform
		/// </summary>
		public string TargetFile { get; set; }

		/// <summary>
		/// The elements local node names to convert
		/// </summary>
		public string[] TargetElements { get; set; }

		/// <summary>
		/// Saves the transformed document to the given path
		/// </summary>
		/// <param name="outputFile"></param>
		public void Transform(string outputFile)
		{
			applyTransform(x => x.Save(outputFile));
		}

		/// <summary>
		/// Returns transformed document as a string
		/// </summary>
		/// <returns></returns>
		public string Transform()
		{
			string result = null;
			applyTransform(x => result = x.ToString());
			return result;
		}

		/// <summary>
		/// Applies the transform
		/// </summary>
		/// <param name="result"></param>
		void applyTransform(Action<XDocument> result)
		{
			var input = XDocument.Load(this.TargetFile, LoadOptions.PreserveWhitespace);
			foreach (var element in input.Descendants())
			{
				if (this.TargetElements.Contains(element.Name.LocalName))
				{
					string content = element.Value;
					if (content.Contains('\n'))
					{
						string normalized = XmlUtility.NormalizeIndentation(content);

						if (normalized != content)
							element.Value = normalized;
					}
				}
			}

			result(input);
		}
	}
}

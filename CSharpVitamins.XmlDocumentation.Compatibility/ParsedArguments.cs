using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVitamins.XmlDocumentation.Compatibility
{
	/// <summary>
	/// Parses a array of arguments into switches and named params
	/// 
	/// switches:
	///		`/SWITCH` turns a switch on
	///		`-SWITCH` turns a switch off
	///	
	/// both forms of switch make it "specified"
	/// 
	/// named parameters:
	/// 	set a named param with either prefex e.g.
	///		`-PARAM:VALUE` 
	///		`/PARAM:VALUE`
	/// </summary>
	class ParsedArguments
	{
		readonly string[] args;
		IList<string> parameters = new List<string>(); // unnamed params
		NamedParams named = new NamedParams();
		ParsedSwitches switches = new ParsedSwitches();

		public ParsedArguments(params string[] args)
		{
			this.args = args;
			this.parse();
		}

		/// <summary>
		/// The original arguments
		/// </summary>
		public string[] Raw { get { return args; } }

		/// <summary>
		/// The number of arguments parsed
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Any stand-alone arguments (non-switched, non-named arguments) - in the order they were specified
		/// </summary>
		public IList<string> Params { get { return parameters; } }

		/// <summary>
		/// The dictionary of named params
		/// </summary>
		public NamedParams Named { get { return named; } }

		/// <summary>
		/// A dictionary of switches
		/// </summary>
		public ParsedSwitches Switches { get { return switches; } }

		/// <summary>
		/// If the switch was specified via the command line
		/// </summary>
		/// <param name="switch"></param>
		/// <returns></returns>
		public bool HasSwitch(string @switch)
		{
			return switches.GetOrDefault(@switch);
		}

		/// <summary>
		/// If the named param is specified 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasNamedParam(string name)
		{
			return named.ContainsKey(name);
		}

		/// <summary>
		/// Gets the named param value, or null if not found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetNamedParam(string name)
		{
			return named.GetOrDefault(name);
		}

		/// <summary>
		/// Gets a named switch - or a new one. Check "IsSpecified" 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Switch GetSwitch(string name)
		{
			return switches.GetOrDefault(name) ?? new Switch
			{
				Name = name,
				On = false,
				IsSpecified = false
			};
		}

		/// <summary>
		/// Parses the argument array into named params and switches
		/// </summary>
		void parse()
		{
			foreach (string arg in args)
			{
				++this.Count;

				var prefix = arg[0];
				if (prefix != '/' && prefix != '-')
					parameters.Add(arg);

				else
				{
					var sep = arg.IndexOf(':');
					if (sep != -1)
					{
						var name = arg.Substring(1, sep - 1);
						var data = arg.Substring(sep + 1);
						named.Add(name, data);
					}
					else
					{
						var swt = new Switch
						{
							IsSpecified = true,
							Name = arg.Substring(1),
							On = prefix == '/' // '-' should turn it off,
						};
						switches.Add(swt.Name, swt);
					}
				}
			}
		}
	}

	class ParsedSwitches : Dictionary<string, Switch>
	{
		public ParsedSwitches()
			: base(StringComparer.OrdinalIgnoreCase)
		{ }
	}

	class NamedParams : Dictionary<string, string>
	{
		public NamedParams()
			: base(StringComparer.OrdinalIgnoreCase)
		{ }
	}

	[DebuggerDisplay("Switch[{Name}, {On}]")]
	class Switch
	{
		public Switch()
		{
			On = true;
			IsSpecified = false;
		}

		/// <summary>
		/// The name of the switch
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// If the switch is on (the default when specified)
		/// </summary>
		public bool On { get; set; }

		/// <summary>
		/// Antonym to "On" 
		/// </summary>
		public bool Off { get { return !On; } set { On = !value; } }

		/// <summary>
		/// 
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// If the switch was specified via the command line
		/// </summary>
		public bool IsSpecified { get; set; }

		public static implicit operator bool(Switch @switch)
		{
			return @switch.IsSpecified;
		}
	}
}

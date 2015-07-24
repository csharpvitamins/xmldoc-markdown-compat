using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVitamins.XmlDocumentation.Compatibility
{
	static class DictionaryExtensions
	{
		public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
			where TValue : new()
		{
			return dictionary.GetOrCreate(key, () => new TValue());
		}

		public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> create)
		{
			TValue value;
			if (!dictionary.TryGetValue(key, out value))
				dictionary[key] = value = create();
			return value;
		}

		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue value;
			dictionary.TryGetValue(key, out value);
			return value;
		}
	}
}

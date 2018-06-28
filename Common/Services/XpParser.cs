using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrderCloudMessageSender.Common
{
	// the only types we need to care about
	// https://en.wikipedia.org/wiki/JSON#Data_types.2C_syntax_and_example
	public enum JsonType { Number = 0, String = 1, Boolean = 2 }

	public class XpRow
	{
		public string Key { get; set; }
		public int? Index { get; set; }
		public object Value { get; set; }
	}

	public class XpParser
	{
		/// <summary>
		/// prep an xp for optimal db storage.
		/// (why not dictionary? because we want to maintain sort order and allow duplicate keys)
		/// </summary>
		public static IList<XpRow> ToRows(string json)
		{
			if (string.IsNullOrWhiteSpace(json) || json == "null")
				return new XpRow[0];

			return InnerToRows(null, null, JToken.Parse(json)).ToList();
		}

		private static IEnumerable<XpRow> InnerToRows(string key, int? index, JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					foreach (var prop in ((JObject)token).Properties())
					{
						var subkey = string.IsNullOrEmpty(key) ? prop.Name : key + "." + prop.Name;
						foreach (var row in InnerToRows(subkey, index, prop.Value))
							yield return row;
					}
					break;
				case JTokenType.Array:
					var arr = (JArray)token;
					yield return new XpRow
					{
						Key = key,
						Index = index,
						Value = token.Value<Array>()
					};
					//for (var i = 0; i < arr.Count; i++)
					//{
					//	foreach (var row in InnerToRows(key, i, arr[i]))
					//		yield return row;
					//}
					break;
				default:
					yield return new XpRow { Key = key, Index = index, Value = token.Value<string>() };
					break;
			}
		}

		private static JsonType GetJsonType(JTokenType jtt)
		{
			switch (jtt)
			{
				case JTokenType.Boolean:
					return JsonType.Boolean;
				case JTokenType.Float:
				case JTokenType.Integer:
					return JsonType.Number;
				default:
					return JsonType.String;
			}
		}

	}
}

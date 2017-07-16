using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrderCloudMessageSender
{
	// the only types we need to care about
	// https://en.wikipedia.org/wiki/JSON#Data_types.2C_syntax_and_example
	public enum JsonType { Number = 0, String = 1, Boolean = 2 }

	public class XpRow
	{
		public string Key { get; set; }
		public int? Index { get; set; }
		public JsonType JsonType { get; set; }
		public string Value { get; set; }
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

		/// <summary>
		/// build an xp object from representation stored in db
		/// </summary>
		public static object FromRows(IEnumerable<XpRow> rows)
		{
			if (rows == null || !rows.Any())
				return null;

			var obj = new Dictionary<string, object>();

			foreach (var row in rows.OrderBy(r => r.Key).ThenBy(r => r.Index ?? 0))
			{
				var keyParts = new Queue<string>(row.Key.Split('.'));
				var key = keyParts.Dequeue();
				var parent = obj;
				while (keyParts.Any())
				{
					// traverse .'s and build deep objects
					if (!parent.ContainsKey(key))
						parent.Add(key, new Dictionary<string, object>());
					parent = (Dictionary<string, object>)parent[key];
					key = keyParts.Dequeue();
				}

				var val = GetObjectValue(row);
				if (row.Index.HasValue)
				{
					// array
					if (!parent.ContainsKey(key))
						parent[key] = new List<object>();
					((IList<object>)parent[key]).Add(val);
				}
				else
				{
					// primitive val
					parent.Add(key, val);
				}
			}
			return obj;
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
					for (var i = 0; i < arr.Count; i++)
					{
						foreach (var row in InnerToRows(key, i, arr[i]))
							yield return row;
					}
					break;
				default:
					yield return new XpRow { Key = key, Index = index, JsonType = GetJsonType(token.Type), Value = token.Value<string>() };
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

		private static object GetObjectValue(XpRow row)
		{
			if (row.Value == null)
				return null;

			switch (row.JsonType)
			{
				case JsonType.Number:
					return decimal.Parse(row.Value);
				case JsonType.Boolean:
					return bool.Parse(row.Value);
				default: return row.Value;
			}
		}
	}
}

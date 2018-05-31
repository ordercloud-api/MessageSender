using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using OrderCloud.AzureStorage;


namespace OrderCloudMessageSender.Common
{
	public interface IConfigReader
	{
		Task<MessageSenderConfig> GetMessageConfigAsync(string configid);
	}
	public class AzureTableConfigReaderService : IConfigReader
	{
		private readonly TableService _table;

		public AzureTableConfigReaderService(TableService table)
		{
			_table = table;
		}
		private class Config : TableEntity
		{
			public string MandrillKey { get; set; }
			public string MandrillSubaccount { get; set; }
			public string OcHashKey { get; set; }
		}
		public async Task<MessageSenderConfig> GetMessageConfigAsync(string configid)
		{
			try
			{
				var tableConifg = await _table.GetAsync<Config>("SenderConfig", configid, "SenderConfig");

				return new MessageSenderConfig
				{
					MandrillKey = tableConifg.MandrillKey,
					MandrillSubaccount = tableConifg.MandrillSubaccount,
					OcHashKey = tableConifg.OcHashKey

				};
			}
			catch(Exception e)
			{
				throw e;
			}
		}
	}
}

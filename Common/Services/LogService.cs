using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using OrderCloud.AzureStorage;

namespace OrderCloudMessageSender.Common
{
	public interface IMessageLog
	{
		Task LogAsync(string configId, string serviceName, string logData, string filename);
	}

	public class AzureTableLogService : IMessageLog
	{
		private readonly TableService _table;

		private class MessageLog : TableEntity
		{
			public string LogData { get; set; }
			public string FileName { get; set; }
		}
		public AzureTableLogService(TableService table)
		{
			_table = table;
		}

		public async Task LogAsync(string configId, string serviceName, string logData, string filename)
		{
			await _table.InsertOrReplaceAsync(new MessageLog
			{
				LogData = logData,
				PartitionKey = serviceName + configId,
				FileName = filename,
				RowKey = Guid.NewGuid().ToString()
			});
		}
	}
}

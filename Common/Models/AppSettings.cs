using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public interface IAppSettings
	{
		string StorageConnection { get; }
		string BUILD_NUMBER { get; }
		string BUILD_COMMIT_ID { get; }

	}
	public class AppSettings : IAppSettings
	{
		public string StorageConnection {get; set; }
		public string BUILD_NUMBER { get; set; }
		public string BUILD_COMMIT_ID { get; set; }
	}
}

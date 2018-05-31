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
	}
	public class AppSettings : IAppSettings
	{
		public string StorageConnection {get; set; }
	}
}

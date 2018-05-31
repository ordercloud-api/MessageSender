using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class MessageSenderConfig
	{
		public string ConfigId { get; set; }
		public string MandrillKey { get; set; }
		public string MandrillSubaccount { get; set; }
		public string OcHashKey { get; set; }
	}
}

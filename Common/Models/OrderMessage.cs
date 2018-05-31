using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class OrderMessage : MessageNotification
	{
		public OrderEventBody EventBody { get; set; }
	}
}

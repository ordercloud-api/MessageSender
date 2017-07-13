using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class OrderMessage : MessageNotification
	{
		public OrderEventBody EventBody { get; set; }
	}
}

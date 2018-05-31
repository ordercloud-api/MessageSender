using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class ShipmentMessage : MessageNotification
	{
		public ShipmentEventBody EventBody { get; set; }
	}
}

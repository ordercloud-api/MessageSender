using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class ShipmentMessage : MessageNotification
	{
		public ShipmentEventBody EventBody { get; set; }
	}
}

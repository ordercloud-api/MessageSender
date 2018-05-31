using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class ShipmentItem : LineItem
	{
		public string OrderID { get; set; }
		public int QuantityShipped { get; set; }
	}
}

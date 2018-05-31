using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class LineItem
	{
		public string ID { get; set; }
		public decimal Cost { get; set; }
		public int Quantity { get; set; }
		public string ProductID { get; set; }
		public decimal LineTotal { get; set; }
		public Address ShippingAddress { get; set; }
	}
}

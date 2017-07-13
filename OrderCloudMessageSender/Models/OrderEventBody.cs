using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class OrderEventBody
	{
		public List<LineItem> LineItems { get; set; }
		public List<Product> Products { get; set; }
		public Order Order { get; set; }
		public List<OrderApproval> Approvals { get; set; }
	}
}

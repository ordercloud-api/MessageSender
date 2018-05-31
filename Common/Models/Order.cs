using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OrderCloudMessageSender.Common
{
	public class Order
	{
		public User FromUser { get; set; }
		public decimal? Total { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? TaxCost { get; set; }
		public decimal? ShippingCost { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public string ID { get; set; }
		public int LineItemCount { get; set; }
		public dynamic xp { get; set; }
	}
}

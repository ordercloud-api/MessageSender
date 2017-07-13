using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class Product
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int QuantityMultiplier { get; set; }
		public decimal? ShipWeight { get; set; }
		public decimal? ShipHeight { get; set; }
		public decimal? ShipWidth { get; set; }
		public decimal? ShipLength { get; set; }
		public bool Active { get; set; }
		public int SpecCount { get; internal set; }
		public dynamic xp { get; set; }
		public int VariantCount { get; internal set; }
		public string ShipFromAddressID { get; set; }
		public Inventory Inventory { get; set; }
		public string AutoForwardSupplierID { get; set; }
	}
}

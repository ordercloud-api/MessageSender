using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class Inventory
	{
		public bool Enabled { get; set; }
		public int? NotificationPoint { get; set; }
		public bool VariantLevelTracking { get; set; }
		public bool OrderCanExceed { get; set; }
		public int? QuantityAvailable { get; set; }
		public DateTimeOffset? LastUpdated { get; internal set; }
	}
}

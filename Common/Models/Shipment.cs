﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class Shipment
	{
		public string ID { get; set; }
		public string TrackingNumber { get; set; }
		public string Shipper { get; set; }
		public DateTimeOffset DateShipped { get; set; }
		public Address ToAddress { get; set; }
		public dynamic xp { get; set; }
	}
}

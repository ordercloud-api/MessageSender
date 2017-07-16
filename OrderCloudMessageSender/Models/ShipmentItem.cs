﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class ShipmentItem : LineItem
	{
		public string OrderID { get; set; }
		public int QuantityShipped { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class SetPasswordMessage : MessageNotification
	{
		public SetPasswordEventBody EventBody { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class NewUserInvitationMessage : MessageNotification
	{
		public NewUserInvitaionEventBody EventBody { get; set; }
	}
}

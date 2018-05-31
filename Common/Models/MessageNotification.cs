using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public enum MessageType { OrderDeclined, OrderSubmitted, ShipmentCreated, ForgottenPassword, OrderSubmittedForYourApproval, OrderSubmittedForApproval, OrderApproved, OrderSubmittedForYourApprovalHasBeenApproved, OrderSubmittedForYourApprovalHasBeenDeclined, NewUserInvitation }

	public class MessageNotification
	{
		public string BuyerID { get; set; }
		public string UserToken { get; set; }
		public User Recipient { get; set; }
		public MessageType MessageType { get; set; }
		public dynamic ConfigData { get; set; }
	}
}

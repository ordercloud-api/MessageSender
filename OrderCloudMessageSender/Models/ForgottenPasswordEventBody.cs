using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class ForgottenPasswordEventBody : NewUserInvitaionEventBody
	{
		public string PasswordRenewalUrl { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class SetPasswordEventBody
	{
		public string Username { get; set; }
		public string PasswordRenewalAccessToken { get; set; }
		public string PasswordRenewalVerificationCode { get; set; }
		public string PasswordRenewalUrl { get; set; }
	}
}

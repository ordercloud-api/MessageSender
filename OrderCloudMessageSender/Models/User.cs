using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class User
	{
		public string ID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTimeOffset? TermsAccepted { get; set; }
		public bool Active { get; set; }
		public dynamic xp { get; set; }
		public IReadOnlyList<string> AvailableRoles { get; internal set; }
		public User()
		{
			xp = new ExpandoObject();
		}
	}
}

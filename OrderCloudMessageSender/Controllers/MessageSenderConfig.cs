using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OrderCloudMessageSender.Controllers
{
	public class MessageSenderConfig
    {
		[HttpPost, Route("MessageSenderConfig")]
		public object CreateMessageSenderConfig()
		{

			return new { };
		
		}
    }
}

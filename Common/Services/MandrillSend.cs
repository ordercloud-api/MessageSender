using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using OrderCloud.AzureStorage;

namespace OrderCloudMessageSender.Common
{
	public interface IMandrillSend
	{
		Task<string> SendAsync(string configid, MessageNotification notification, List<GlobalMergeVar> mergeVars);
	}

	public class MandrillSend : IMandrillSend
	{
		private readonly IMessageLog _log;
		private readonly IConfigReader _configReader;

		public MandrillSend(IMessageLog log, IConfigReader configReader )
		{
			_log = log;
			_configReader = configReader;
		}

		public async Task<string> SendAsync(string configid, MessageNotification notification, List<GlobalMergeVar> mergeVars)
		{
			var config = await _configReader.GetMessageConfigAsync(configid);

			var mandrill = new MandrillMessage
			{
				key = config.MandrillKey,
				template_name = notification.MessageType.ToString(),
				template_content = new List<TemplateContent>() { new TemplateContent { name = "main", content = "messageConfig.MainContent" } },

				message = new Message
				{
					from_email = notification.ConfigData.FromEmail,
					to = new List<To>() {
						new To(){ email = notification.Recipient.Email, sendtype = "to"}
					},
					auto_html = "true",
					inline_css = "true",
					subaccount = config.MandrillSubaccount,
					global_merge_vars = mergeVars
				}
			};
			var response = await "https://mandrillapp.com/api/1.0/messages/send-template.json".AllowAnyHttpStatus().PostJsonAsync(mandrill);
			var body = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				await _log.LogAsync(configid, "mandrill", "Success", body);
			else
				await _log.LogAsync(configid, "mandrill", "Fail", body);

			return body;
		}
	}
}

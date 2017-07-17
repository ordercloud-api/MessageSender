using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCloudMessageSender.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OrderCloudMessageSender.Controllers
{
    [Produces("application/json")]
    [Route("SendGrid")]
    public class SendGridController : Controller
    {
	    private async Task<string> SendSendGrid(MessageNotification notification, SendGridMessage msg)
	    {
			var client = new SendGridClient(notification.ConfigData.SendGridKey.ToString());
			msg.SetFrom(new EmailAddress(notification.ConfigData.FromEmail.ToString()));
			msg.AddTo(new EmailAddress(notification.Recipient.Email.ToString()));
			msg.SetTemplateId(notification.ConfigData[$"{notification.MessageType.ToString()}TemplateID"].ToString());
		    try
		    {
			    var response = await client.SendEmailAsync(msg);
			    return await response.Body.ReadAsStringAsync();
			}
		    catch (Exception e)
		    {
			    return e.ToString();
		    }
	    }

	    [HttpPost, Route("ordersubmitted")]
	    public async Task<string> SendOrderSubmittedMessage([FromBody] Models.OrderMessage notification)
	    {
		    var msg = BuildOrderMessage(notification.EventBody);
			return await SendSendGrid(notification, msg);
	    }
		private SendGridMessage BuildOrderMessage(OrderEventBody eventBody)
		{
		    var msg = new SendGridMessage();
			msg.AddSubstitution("-firstname-", eventBody.Order.FromUser.FirstName);
			msg.AddSubstitution("-lastname-", eventBody.Order.FromUser.LastName);
			msg.AddSubstitution("-orderid-", eventBody.Order.ID);
			msg.AddSubstitution("-datesubmitted-", eventBody.Order.DateSubmitted.ToString());
			msg.AddSubstitution("-subtotal-", eventBody.Order.Subtotal.ToString());
			msg.AddSubstitution("-tax-", eventBody.Order.TaxCost.ToString());
			msg.AddSubstitution("-shipping-", eventBody.Order.ShippingCost.ToString());
			msg.AddSubstitution("-total-", eventBody.Order.Total.ToString());
			msg.AddSubstitution("-lineitemcount-", eventBody.Order.LineItemCount.ToString());
			return msg;
		}
		
		[HttpPost, Route("newuserinvitation")]
	    public async Task<string> SendNewUserInvitation([FromBody] Models.SetPasswordMessage notification)
	    {
		    var msg = new SendGridMessage();
		    msg.AddSubstitution("-username-", notification.EventBody.Username);
		    msg.AddSubstitution("-passwordtoken-", notification.EventBody.PasswordRenewalAccessToken);
		    msg.AddSubstitution("-passwordverificationcode-", notification.EventBody.PasswordRenewalVerificationCode);
		    return await SendSendGrid(notification, msg);
		}
	}
}
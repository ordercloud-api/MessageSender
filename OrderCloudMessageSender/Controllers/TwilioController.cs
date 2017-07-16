using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using OrderCloudMessageSender.Models;

namespace OrderCloudMessageSender.Controllers
{
	[Produces("application/json")]
	[Route("Twilio")]
	public class TwilioController : Controller
	{
		private async Task<MessageResource> SendTwilio(string messageText, MessageNotification notification)
		{
			TwilioClient.Init((string)notification.ConfigData.AccountSid, (string)notification.ConfigData.AuthToken);

			try
			{
				return await MessageResource.CreateAsync(
					to: new PhoneNumber($"+1{(string)notification.Recipient.Phone}"),
					from: new PhoneNumber((string)notification.ConfigData.FromNumber),
					body: messageText);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		[HttpPost, Route("ordersubmitted")]
		public async Task<MessageResource> SendOrderSubmittedMessage([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"Thank you, {notification.Recipient.FirstName} {notification.Recipient.LastName}. Your order with {notification.EventBody.Order.LineItemCount} item(s) has been received", notification);
		}
		[HttpPost, Route("newuserinvitation")]
		public async Task<MessageResource> SendNewUserInvitation([FromBody] Models.SetPasswordMessage notification)
		{
			return await SendTwilio($"verification code: {notification.EventBody.PasswordRenewalVerificationCode}", notification);
		}
		[HttpPost, Route("forgottenpassword")]
		public async Task<MessageResource> SendForgottenPassword([FromBody] Models.SetPasswordMessage notification)
		{
			return await SendTwilio($"password reset code: {notification.EventBody.PasswordRenewalVerificationCode}", notification);
		}
		[HttpPost, Route("ordersubmittedforapproval")]
		public async Task<MessageResource> SendOrderSubmittedForApproval([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"Your order {notification.EventBody.Order.ID} is awaiting approval", notification);
		}
		[HttpPost, Route("orderapproved")]
		public async Task<MessageResource> SendOrderApproved([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"Your order {notification.EventBody.Order.ID} has been approved", notification);
		}
		[HttpPost, Route("orderdeclined")]
		public async Task<MessageResource> SendOrderDeclined([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"Your order {notification.EventBody.Order.ID} has been declined by {notification.EventBody.Approvals[0]?.Approver?.FirstName} {notification.EventBody.Approvals[0]?.Approver?.LastName}. Comments: {notification.EventBody.Approvals[0]?.Comments}", notification);
		}
		[HttpPost, Route("shipmentcreated")]
		public async Task<MessageResource> SendShipmentCreated([FromBody] Models.ShipmentMessage notification)
		{
			return await SendTwilio($"Your order has shipped. Tracking number {notification.EventBody.Shipment.TrackingNumber}",notification);
		}
		[HttpPost, Route("ordersubmittedforyourapproval")]
		public async Task<MessageResource> SendOrderSubmittedForYourApproval([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"You have an order from {notification.EventBody.Order.FromUser.FirstName} {notification.EventBody.Order.FromUser.LastName} to approve. OrderID: {notification.EventBody.Order.ID}", notification);
		}
		[HttpPost, Route("ordersubmittedforyourapprovalhasbeendeclined")]
		public async Task<MessageResource> OrderSubmittedForYourApprovalHasBeenDeclined([FromBody] Models.OrderMessage notification)
		{
			return await SendTwilio($"Order {notification.EventBody.Order.ID} was awaiting your aproval and has been declined.", notification);
		}
	}
}
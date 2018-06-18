using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCloudMessageSender.Common;
using Flurl.Http;

namespace OrderCloudMessageSender.Controllers
{
    [Produces("application/json")]
    [Route("mandrill")]
    public class MadrillController : Controller
    {
	    private readonly IMandrillSend _mandrillSend;

	    public MadrillController(IMandrillSend mandrillSend)
	    {
		    _mandrillSend = mandrillSend;
	    }
		[HttpPost, Route("shipmentcreated")]
	    public async Task<string> SendShipmentCreatedMessage([FromBody] ShipmentMessage notification, [FromQuery] string configid)
	    {
		    try
		    {
			    var mergeVars = BuildShipmentVars(notification);
			    return await _mandrillSend.SendAsync(configid, notification, mergeVars);
		    }
		    catch (Exception e)
		    {
			    return e.ToString();
			    throw;
		    }
	    }

		[HttpPost, 
			Route("ordersubmitted"),
			Route("ordersubmittedforyourapproval"),
			Route("orderdeclined"),
			Route("ordersubmittedforapproval"),
			Route("OrderApproved"),
			Route("ordersubmittedforyourapprovalhasbeendeclined")]
	    public async Task<string> SendOrderSubmittedMessage([FromBody] OrderMessage notification, [FromQuery] string configid)
	    {
		    try
		    {
			    var mergeVars = BuildOrderMergeVars(notification.EventBody);
				return await _mandrillSend.SendAsync(configid, notification, mergeVars);
			}
		    catch (Exception e)
		    {
			    return e.ToString();
			    throw;
		    }
	    }

		[HttpPost, Route("forgottenpassword"), Route("newuserinvitation")]
	    public async Task<string> SendSetPassword([FromBody] SetPasswordMessage notification, [FromQuery] string configid)
	    {

		    var mergeVars = new List<GlobalMergeVar>() {
					new GlobalMergeVar {name = "username", content = notification.EventBody.Username},
					new GlobalMergeVar {name = "passwordtoken", content = notification.EventBody.PasswordRenewalAccessToken},
					new GlobalMergeVar {name = "passwordverificationcode", content = notification.EventBody.PasswordRenewalVerificationCode},
					new GlobalMergeVar {name = "passwordrenewalurl", content = notification.EventBody.PasswordRenewalUrl}
				};
				return await _mandrillSend.SendAsync(configid, notification, mergeVars);
			
		}
		private List<GlobalMergeVar> BuildShipmentVars(ShipmentMessage notification)
	    {
		    var mergeVars = BuildOrderMergeVars(notification.EventBody);
		    mergeVars.Add(new GlobalMergeVar { name = "ShipmentID", content = notification.EventBody.Shipment.ID });
			mergeVars.Add(new GlobalMergeVar { name = "ShipmentTrackingNumber", content = notification.EventBody.Shipment.TrackingNumber });
			mergeVars.Add(new GlobalMergeVar { name = "Shipper", content = notification.EventBody.Shipment.Shipper });
			mergeVars.Add(new GlobalMergeVar { name = "dateShipped", content = notification.EventBody.Shipment.DateShipped });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressID", content = notification.EventBody.Shipment.ToAddress?.ID });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressCompany", content = notification.EventBody.Shipment.ToAddress?.CompanyName });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressFirstName", content = notification.EventBody.Shipment.ToAddress?.FirstName });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressLastName", content = notification.EventBody.Shipment.ToAddress?.LastName });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressStreet1", content = notification.EventBody.Shipment.ToAddress?.Street1 });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressStreet2", content = notification.EventBody.Shipment.ToAddress?.Street2 });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressCity", content = notification.EventBody.Shipment.ToAddress?.City });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressState", content = notification.EventBody.Shipment.ToAddress?.State });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressPostalCode", content = notification.EventBody.Shipment.ToAddress?.Zip });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressCountry", content = notification.EventBody.Shipment.ToAddress?.Country });
			mergeVars.Add(new GlobalMergeVar { name = "toAddressName", content = notification.EventBody.Shipment.ToAddress?.AddressName });
			mergeVars.Add(new GlobalMergeVar
			{
				name = "shipmentItems",
				content = notification.EventBody.ShipmentItems
					.Where(si => si.OrderID == notification.EventBody.Order.ID)
					.Select(si =>
					{
						var msi = new MandrillShipmentItem
						{
							Cost = si.LineTotal,
							QuantityShipped = si.QuantityShipped,
							ProductDesc = notification.EventBody.Products.FirstOrDefault(p => p.ID == si.ProductID)?.Description,
							ProductID = si.ProductID,
							ProductName = notification.EventBody.Products.FirstOrDefault(p => p.ID == si.ProductID)?.Name
						};
						return msi;
					}).ToArray()
			});
		    if (notification.EventBody.Shipment.xp != null)
            {
                IList<XpRow> xpRows = XpParser.ToRows(notification.EventBody.Shipment.xp.ToString());
                mergeVars.AddRange(xpRows.Select(
                xpRow => xpRow.Index == null ?
                  new GlobalMergeVar { name = "shipmentxp_" + xpRow.Key.Replace(".", "_"), content = xpRow.Value } :
                  new GlobalMergeVar { name = $"shipmentxp_{xpRow.Key.Replace(".", "_")}_{xpRow.Index}", content = xpRow.Value }));
            }
		    return mergeVars;
		}
	    private List<GlobalMergeVar> BuildOrderMergeVars(OrderEventBody eventBody)
	    {
		    var mergeVars = new List<GlobalMergeVar>() {
			    new GlobalMergeVar {name = "firstname", content = eventBody.Order.FromUser.FirstName},
			    new GlobalMergeVar {name = "lastname", content = eventBody.Order.FromUser.LastName},
			    new GlobalMergeVar {name = "orderid", content = eventBody.Order.ID},
			    new GlobalMergeVar {name = "datesubmitted", content = eventBody.Order.DateSubmitted},
			    new GlobalMergeVar {name = "subtotal", content = eventBody.Order.Subtotal},
			    new GlobalMergeVar {name = "tax", content = eventBody.Order.TaxCost},
			    new GlobalMergeVar {name = "shipping", content = eventBody.Order.ShippingCost},
			    new GlobalMergeVar {name = "total", content = eventBody.Order.Total},
			    new GlobalMergeVar {name = "lineitemcount", content = eventBody.Order.LineItemCount},
			    new GlobalMergeVar {
				    name = "products",
				    content = eventBody.LineItems.Select(li =>
				    {
					    var mli = new MandrillLineItem
					    {
						    Cost = li.LineTotal,
						    Quantity = li.Quantity,
						    ProductDesc = eventBody.Products.FirstOrDefault(p => p.ID == li.ProductID)?.Description,
						    ProductID = li.ProductID,
						    ProductName = eventBody.Products.FirstOrDefault(p => p.ID == li.ProductID)?.Name,
						    ShipToName = li.ShippingAddress?.FirstName + " " + li.ShippingAddress?.LastName,
						    ShipToStreet1 = li.ShippingAddress?.Street1,
						    ShipToStreet2 = li.ShippingAddress?.Street2,
						    ShipToCity = li.ShippingAddress?.City,
						    ShipToState = li.ShippingAddress?.State,
						    ShipToPostalCode = li.ShippingAddress?.Zip,
						    ShipToCountry = li.ShippingAddress?.Country
					    };

					    return mli; }).ToArray()
			    },
			    new GlobalMergeVar {
				    name = "approvals",
				    content = eventBody.Approvals?.Select(a =>
				    {
					    var ma = new MandrillApproval
					    {
						    ApprovingGroupID = a.ApprovingGroupID,
						    Status = a.Status,
						    DateCreated = a.DateCreated,
						    DateCompleted = a.DateCompleted,
						    ApproverID = a.Approver?.ID,
						    ApproverUsername = a.Approver?.Username,
						    ApproverEmail = a.Approver?.Email,
						    ApproverFirstName = a.Approver?.FirstName,
						    ApproverLastName = a.Approver?.LastName,
						    ApproverPhone = a.Approver?.Phone,
						    Comments = a.Comments
					    };
					    return ma;
				    }).ToArray()
			    }
		    };
		    if (eventBody.Order.xp != null)
		    {
			    IList<XpRow> xpRows = XpParser.ToRows(eventBody.Order.xp.ToString());
				mergeVars.AddRange(xpRows.Select(
				 xpRow => xpRow.Index == null ?
				  new GlobalMergeVar { name = "orderxp_" + xpRow.Key.Replace(".", "_"), content = xpRow.Value } :
				  new GlobalMergeVar { name = $"orderxp_{xpRow.Key.Replace(".", "_")}_{xpRow.Index}", content = xpRow.Value }));
			}
            return mergeVars;
	    }
	}
}
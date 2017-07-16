using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public class TemplateContent
	{
		public string name { get; set; }
		public string content { get; set; }
	}

	public class To
	{
		public string email { get; set; }
		public string name { get; set; }
		public string sendtype { get; set; } //to,cc,bcc
	}

	public class Headers
	{
		public string reply_to { get; set; }
	}

	public class GlobalMergeVar
	{
		public string name { get; set; }
		public object content { get; set; }
	}
	public class MandrillLineItem
	{
		public string ProductID { get; set; }
		public string ProductDesc { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Cost { get; set; }
		public string ShipToName { get; set; }
		public string ShipToStreet1 { get; set; }
		public string ShipToStreet2 { get; set; }
		public string ShipToCity { get; set; }
		public string ShipToState { get; set; }
		public string ShipToPostalCode { get; set; }
		public string ShipToCountry { get; set; }
	}

	public class MandrillShipmentItem : MandrillLineItem
	{
		public string OrderID { get; set; }
		public int QuantityShipped { get; set; }
	}
	public class MandrillApproval
	{
		public string ApprovingGroupID { get; set; }
		public ApprovalStatus Status { get; set; }
		public DateTimeOffset? DateCreated { get; set; }
		public DateTimeOffset? DateCompleted { get; set; }
		public string ApproverID { get; set; }
		public string ApproverUsername { get; set; }
		public string ApproverEmail { get; set; }
		public string ApproverFirstName { get; set; }
		public string ApproverLastName { get; set; }
		public string ApproverPhone { get; set; }
		public string Comments { get; set; }

	}
	public class Message
	{
		public object subject { get; set; }
		public string from_email { get; set; }
		public string from_name { get; set; }
		public List<To> to { get; set; }
		public Headers headers { get; set; }
		public string auto_html { get; set; }
		public string inline_css { get; set; }
		public string subaccount { get; set; }
		public List<GlobalMergeVar> global_merge_vars { get; set; }
	}

	public class MandrillMessage
	{
		public object key { get; set; }
		public object template_name { get; set; }
		public List<TemplateContent> template_content { get; set; }
		public Message message { get; set; }
	}
}

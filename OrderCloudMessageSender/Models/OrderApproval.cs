using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Models
{
	public enum ApprovalStatus { Pending, Approved, Declined }
	public class OrderApproval
	{
		public string ApprovalRuleID { get; set; }
		public string ApprovingGroupID { get; set; }
		public ApprovalStatus Status { get; set; }
		public DateTimeOffset? DateCreated { get; set; }
		public DateTimeOffset? DateCompleted { get; set; }
		public User Approver { get; set; }
		public string Comments { get; set; }
	}
	//
	//"ApprovalRuleID":"over-200-sg1",
	//"ApprovingGroupID":"approverssg1",
	//"Status":"Pending",
	//"DateCreated":"2017-07-13T14:55:35.677+00:00",
	//"DateCompleted":null,
	//"Approver":null,
	//"Comments":null
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloudMessageSender.Common
{
	public class ListPage<T>
	{
		public ListPageMeta Meta { get; set; }
		public IList<T> Items { get; set; }
	}

	public class ListPageMeta
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public int TotalPages { get; set; }
		public int[] ItemRange { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossLink.Tests.TestData
{
	public class GroupResponse : IRestLinked
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public IEnumerable<UserResponse> Users { get; set; }
		public Dictionary<string, string> Links { get; set; }

		public GroupResponse()
		{
			Users = Enumerable.Empty<UserResponse>();
			Links = new Dictionary<string, string>();
		}
	}
}

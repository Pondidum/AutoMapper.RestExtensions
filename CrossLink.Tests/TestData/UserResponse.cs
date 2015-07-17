using System;
using System.Collections.Generic;

namespace CrossLink.Tests.TestData
{
	public class UserResponse : IRestLinked
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public DateTime Dob { get; set; }
		public Dictionary<string, string> Links { get; set; }

		public UserResponse()
		{
			Links = new Dictionary<string, string>();
		}
	}
}

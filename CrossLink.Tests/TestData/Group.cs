using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossLink.Tests.TestData
{
	public class Group
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public IEnumerable<User> Users { get; set; }

		public Group()
		{
			Users = Enumerable.Empty<User>();
		}
	}
}

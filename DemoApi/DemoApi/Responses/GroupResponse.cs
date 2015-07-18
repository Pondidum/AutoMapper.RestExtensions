using System.Collections.Generic;
using System.Linq;
using AutoMapper.Rest;

namespace DemoApi.Responses
{
	public class GroupResponse : IRestLinked
	{
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

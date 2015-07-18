using System.Collections.Generic;
using AutoMapper.Rest;

namespace DemoApi.Responses
{
	public class UserResponse : IRestLinked
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public Dictionary<string, string> Links { get; set; }

		public UserResponse()
		{
			Links = new Dictionary<string, string>();
		}
	}
}

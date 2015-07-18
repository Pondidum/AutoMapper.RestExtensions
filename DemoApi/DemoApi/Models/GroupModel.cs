using System.Collections.Generic;
using System.Linq;

namespace DemoApi.Models
{
	public class GroupModel
	{
		public string Name { get; set; }
		public IEnumerable<UserModel> Users { get; set; }

		public GroupModel()
		{
			Users = Enumerable.Empty<UserModel>();
		}
	}
}
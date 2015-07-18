using System.Web.Http;
using AutoMapper;
using DemoApi.Models;
using DemoApi.Responses;

namespace DemoApi.Controllers
{
	public class GroupsController : ApiController
	{
		public GroupModel GetGroup(string name)
		{
			return new GroupModel
			{
				Name = "testing",
				Users = new[]
				{
					new UserModel {ID = 0, Name = "First"},
					new UserModel {ID = 1, Name = "Second"}
				}
			};
		}
	}
}

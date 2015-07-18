using System.Web.Http;
using AutoMapper;
using AutoMapper.Rest;
using DemoApi.Models;
using DemoApi.Responses;

namespace DemoApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			Mapper
				.CreateMap<UserModel, UserResponse>()
				.Link("self", user => "/users/" + user.ID);

			Mapper
				.CreateMap<GroupModel, GroupResponse>()
				.Link("self", group => "/groups/" + group.Name)
				.Link("users", group => "/groups/" + group.Name + "/users");

			//force json
			config.Formatters.Add(new BrowserJsonFormatter());
			config.Filters.Add(new RestfulMapperFilter(Mapper.GetAllTypeMaps()));

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{name}",
				defaults: new { name = RouteParameter.Optional }
			);
		}
	}
}

# AutoMapper.RestExtensions

A single file you can add to your project to make populating restful responses in apis easier.

## Usage

Assuming our API has these models:
```c#
namespace Models
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

	public class UserModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
	}
}
```
And these are the responses to send from controllers:

```c#
namespace ApiResponses
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
```

In your startup, add the following:

```c#
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

		//...
	}
}
```

Now in your controller you can do this:

```c#
public class GroupsController : ApiController
{
	public GroupResponse GetGroup(string name)
	{
		var group = new GroupModel
		{
			Name = "testing",
			Users = new[]
			{
				new UserModel {ID = 0, Name = "First"},
				new UserModel {ID = 1, Name = "Second"}
			}
		};

		return Mapper.Map<GroupResponse>(group);
	}
}
```

And the json resposnse you would get would be:

```json
{
  "Name": "testing",
  "Users": [
    {
      "ID": 0,
      "Name": "First",
      "Links": {
        "self": "/users/0"
      }
    },
    {
      "ID": 1,
      "Name": "Second",
      "Links": {
        "self": "/users/1"
      }
    }
  ],
  "Links": {
    "self": "/groups/testing",
    "users": "/groups/testing/users"
  }
}
```

## Improvements

### Use an ActionFilter rather than calls to AutoMapper

Change your controller method to this:

```c#
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
```

Change the Register method to add the ActionFilter:

```c#
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

		config.Filters.Add(new RestfulMapperFilter(Mapper.GetAllTypeMaps()));
		//...
	}
}
```

And create the `RestfulMapperFilter` class:

```c#
public class RestfulMapperFilter : ActionFilterAttribute
{
	private readonly Dictionary<Type, Type> _map;

	public RestfulMapperFilter(IEnumerable<TypeMap> allTypes)
	{
		_map = allTypes.ToDictionary(
			m => m.SourceType,
			m => m.DestinationType);
	}

	public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
	{
		var response = actionExecutedContext.Response;

		object source;
		if (response.TryGetContentValue(out source))
		{
			Type destinationType;
			if (_map.TryGetValue(source.GetType(), out destinationType))
			{
				var destination = Mapper.Map(source, source.GetType(), destinationType);

				response.Content = new ObjectContent(
					destinationType,
					destination,
					((ObjectContent)(response.Content)).Formatter);
			}
		}

		base.OnActionExecuted(actionExecutedContext);
	}
}
```

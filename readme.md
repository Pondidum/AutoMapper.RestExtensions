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
			Users = Enumerable.Empty<User>();
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
		public Dictionary<string, string> Links { get; set;}

		public GroupResponse()
		{
			Users = Enumerable.Empty<UserResponse>();
		}
	}

	public class UserResponse : IRestLinked
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public Dictionary<string, string> Links { get; set;}
	}
}
```

In your startup, add the following:

```c#
public class Startup
{
	public void Configuration(IAppBuilder app)
	{
		AutoMapper
			.CreateMap<User, UserResponse>()
			.Link("self", user => "/users/" + user.ID);

		AutoMapper
			.CreateMap<Group, GroupResponse>()
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
	public GroupResponse GetGroup(string groupName)
	{
		var group = new GroupModel
		{
			Name = "testing",
			Users = new[]
			{
				new UserModel { ID = 0, Name = "First" },
				new UserModel { ID = 1, Name = "Second" }
			}
		}

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
using System;
using System.Collections.Generic;
using System.Linq;
using CrossLink.Tests.TestData;
using Shouldly;
using Xunit;

namespace CrossLink.Tests
{
	public class Acceptance
	{
		public Acceptance()
		{
			Linker.ClearAllMappings();
		}

		[Fact]
		public void When_the_objects_are_not_registered()
		{
			var input = new User();
			Should.Throw<NotImplementedException>(() => Linker.Apply<User, UserResponse>(input));
		}

		[Fact]
		public void When_there_are_no_conventions()
		{
			Linker.Setup<User, UserResponse>();

			var input = new User
			{
				ID = Guid.NewGuid(),
				Name = "Testing",
				Dob = DateTime.Now
			};

			var response = Linker.Apply<User, UserResponse>(input);

			response.Links.ShouldBeEmpty();
			response.ID.ShouldBe(input.ID);
			response.Name.ShouldBe(input.Name);
			response.Dob.ShouldBe(input.Dob);
		}

		[Fact]
		public void When_a_convention_is_setup()
		{
			Linker.Setup<User, UserResponse>()
				.Link("self", person => "people/byID/" + person.ID);

			var input = new User { ID = Guid.NewGuid() };
			var response = Linker.Apply<User, UserResponse>(input);

			response.Links.Single().Key.ShouldBe("self");
			response.Links.Single().Value.ShouldBe("people/byID/" + input.ID);
		}
	}
}

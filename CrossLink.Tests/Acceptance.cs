using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace CrossLink.Tests
{
	public class Acceptance
	{
		[Fact]
		public void When_the_objects_are_not_registered()
		{
			var input = new PersonDto();
			Should.Throw<NotImplementedException>(() => Linker.Apply<PersonDto, PersonResponse>(input));
		}

		[Fact]
		public void When_there_are_no_conventions()
		{
			Linker.Setup<PersonDto, PersonResponse>();

			var input = new PersonDto
			{
				ID = Guid.NewGuid(),
				Name = "Testing",
				Dob = DateTime.Now
			};

			var response = Linker.Apply<PersonDto, PersonResponse>(input);

			response.Links.ShouldBeEmpty();
			response.ID.ShouldBe(input.ID);
			response.Name.ShouldBe(input.Name);
			response.Dob.ShouldBe(input.Dob);
		}
	}

	public class PersonDto
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public DateTime Dob { get; set; }
	}

	public class PersonResponse : IRestLinked
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public DateTime Dob { get; set; }

		public Dictionary<string, string> Links { get; set; }
	}
}

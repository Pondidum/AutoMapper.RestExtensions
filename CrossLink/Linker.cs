using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossLink
{
	public class Linker
	{
		private static readonly List<Mapping> Mappings;

		static Linker()
		{
			Mappings = new List<Mapping>();
		}

		public static MappingConfiguration<TInput> Setup<TInput, TOutput>()
		{
			var mapping = new Mapping(typeof(TInput), typeof(TOutput));
			Mappings.Add(mapping);

			return new MappingConfiguration<TInput>(mapping);
		}

		public static TOutput Apply<TInput, TOutput>(TInput instance)
			where TOutput : IRestLinked
		{
			var input = typeof(TInput);
			var output = typeof(TOutput);
			
			var mapping = Mappings.FirstOrDefault(m => m.Input == input && m.Output == output );

			if (mapping == null)
			{
				throw new NotImplementedException(string.Format(
					"You must setup this mapping first (try calling Linker.Setup<{0}, {1}>()",
					input.Name,
					output.Name));
			}

			return mapping.Apply<TOutput>(instance);
		}
	}
}

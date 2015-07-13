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
			var mapping = Mappings.FirstOrDefault(m => m.Input == typeof(TInput) && m.Output == typeof(TOutput));

			if (mapping == null)
			{
				throw new NotSupportedException();
			}

			return mapping.Apply<TOutput>(instance);
		}
	}
}

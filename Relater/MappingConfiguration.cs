using System;

namespace Relater
{
	public class MappingConfiguration<TInput>
	{
		private readonly Mapping _mapping;

		public MappingConfiguration(Mapping mapping)
		{
			_mapping = mapping;
		}

		public MappingConfiguration<TInput> Link(string key, Func<TInput, string> map)
		{
			_mapping.Add(key, input => map((TInput)input));
			return this;
		}
	}
}

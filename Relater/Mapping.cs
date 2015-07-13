using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Relater
{
	public class Mapping
	{
		public Type Input { get; private set; }
		public Type Output { get; private set; }

		private readonly Dictionary<string, Func<object, string>> _maps;

		public Mapping(Type input, Type output)
		{
			Input = input;
			Output = output;

			_maps = new Dictionary<string, Func<object, string>>();

			Mapper.CreateMap(input, output);
		}

		public void Add(string key, Func<object, string> map)
		{
			_maps.Add(key, map);
		}

		public TOutput Apply<TOutput>(object input)
			where TOutput : IRestLinked
		{
			var result = Mapper.Map<TOutput>(input);

			result.Links = _maps.ToDictionary(m => m.Key, m => m.Value(input));

			return result;
		}
	}
}

using System;
using System.Collections.Generic;

namespace AutoMapper.Rest
{
	public static class Extensions
	{
		public static IMappingExpression<TSource, TDestination> Link<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping, string key, Func<TSource, string> createLink)
			where TDestination : IRestLinked
		{
			return mapping.AfterMap((u, r) => r.Links[key] = createLink(u));
		}
	}

	public interface IRestLinked
	{
		Dictionary<string, string> Links { get; set; }
	}
}

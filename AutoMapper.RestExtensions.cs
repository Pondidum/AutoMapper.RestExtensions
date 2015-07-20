using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;

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
}

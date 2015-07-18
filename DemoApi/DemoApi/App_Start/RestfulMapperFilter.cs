using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;
using AutoMapper;

namespace DemoApi
{
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
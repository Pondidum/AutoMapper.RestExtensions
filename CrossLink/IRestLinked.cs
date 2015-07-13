using System.Collections.Generic;

namespace CrossLink
{
	public interface IRestLinked
	{
		Dictionary<string, string> Links { get; set; }
	}
}

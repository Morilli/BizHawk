using System.Collections.Generic;
using System.Linq;

namespace BizHawk.Client.Common.RamSearchEngine
{
	internal static class Extensions
	{
		public static IEnumerable<IMiniWatch> ToBytes(this IEnumerable<long> addresses, SearchEngineSettings settings)
			=> addresses.Select(a => new MiniByteWatch(settings.Domain, a));

		public static IEnumerable<IMiniWatch> ToWords(this IEnumerable<long> addresses, SearchEngineSettings settings)
			=> addresses.Select(a => new MiniWordWatch(settings.Domain, a, settings.BigEndian));

		public static IEnumerable<IMiniWatch> ToDWords(this IEnumerable<long> addresses, SearchEngineSettings settings)
			=> addresses.Select(a => new MiniDWordWatch(settings.Domain, a, settings.BigEndian));
	}
}

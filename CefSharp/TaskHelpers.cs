using System;
using System.Threading.Tasks;

namespace CefSharp
{
	public static class TaskHelpers
	{
		public static Task Run(Action action)
		{
			return 
#if NET40
				TaskEx
#else
				Task
#endif
					.Run(action);
		}

		public static Task<TResult> FromResult<TResult>(TResult result)
		{
			return
#if NET40
				TaskEx
#else
				Task
#endif
					.FromResult<TResult>(result);
		}

		public static Task Delay(int millisecondsDelay)
		{
			return
#if NET40
				TaskEx
#else
				Task
#endif
					.Delay(millisecondsDelay);
		}
	}
}

using System.Reflection;

namespace CefSharp.Internals
{
	public class RootMemberWrapper
	{
		public static PropertyInfo PropertyInfo = typeof (RootMemberWrapper).GetProperty("Object", BindingFlags.Instance | BindingFlags.Public);

		public object Object { get; set; }
	}
}

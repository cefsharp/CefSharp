using System;
using System.Collections.Generic;
using System.Reflection;
using CefSharp.Internals;

namespace CefSharp
{
	public static class JavascriptKnownTypesRegistra
	{
		private static readonly HashSet<Type> KnownTypes = new HashSet<Type>();

		static JavascriptKnownTypesRegistra()
		{
			Register(typeof(object[]));
			Register(typeof(Dictionary<string, object>));
			Register(typeof(JavascriptObject));
			Register(typeof(JavascriptProperty));
			Register(typeof(JavascriptMethod));
		}

		public static void Register(Type type)
		{
			KnownTypes.Add(type);
		}

		public static void UnRegister(Type type)
		{
			KnownTypes.Remove(type);
		}

		public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
		{
			return KnownTypes;
		}
	}
}

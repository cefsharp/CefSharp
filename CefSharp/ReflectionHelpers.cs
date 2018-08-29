using System;
using System.Reflection;

namespace CefSharp
{
	public static class ReflectionHelpers
	{
		public static bool IsGenericType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

#if NET40
			return type.IsGenericType;
#else
			return type.GetTypeInfo().IsGenericType;
#endif
		}

		public static Type GetBaseType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

#if NET40
			return type.BaseType;
#else
			return type.GetTypeInfo().BaseType;
#endif
		}
	}
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CefSharp.Wpf.Example.Controls
{
	public static class VisualTreeExtensions
	{
		public static List<T> FindChildren<T>(this DependencyObject parent) where T : DependencyObject
		{
			if (parent == null) return null;

			var children = new List<T>();

			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);

			for (var i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				var childType = child as T;
				if (childType == null)
				{
					children.AddRange(FindChildren<T>(child));
				}
				else
				{
					children.Add((T)child);
				}
			}

			return children;
		}
	}
}
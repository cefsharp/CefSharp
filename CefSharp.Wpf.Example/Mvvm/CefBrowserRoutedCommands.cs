using System.Windows.Input;

namespace CefSharp.Wpf.Example.Mvvm
{
	public static class CefBrowserRoutedCommands
	{
		public static RoutedUICommand FocusAddress = new RoutedUICommand("FocusAddress", "FocusAddress", typeof(CefBrowserRoutedCommands), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F6) }));
	}
}

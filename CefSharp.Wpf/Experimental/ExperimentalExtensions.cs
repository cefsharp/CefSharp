using CefSharp.Wpf.Internals;

namespace CefSharp.Wpf.Experimental
{
    public static class ExperimentalExtensions
    {
        public static void UsePopupMouseTransform(this ChromiumWebBrowser chromiumWebBrowser)
        {
            chromiumWebBrowser.MousePositionTransform = new MousePositionTransform();
        }
    }
}

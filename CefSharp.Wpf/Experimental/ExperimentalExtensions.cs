using CefSharp.Wpf.Internals;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// Experimental Extensions
    /// </summary>
    public static class ExperimentalExtensions
    {
        /// <summary>
        /// Html dropdown goes off screen when near bottom of page by default
        /// Calling this method to use the <see cref="MousePositionTransform"/> implementation
        /// to reopsition Popups and mouse.
        ///
        /// Issue https://github.com/cefsharp/CefSharp/issues/2820
        /// </summary>
        /// <param name="chromiumWebBrowser">browser</param>
        public static void UsePopupMouseTransform(this ChromiumWebBrowser chromiumWebBrowser)
        {
            chromiumWebBrowser.MousePositionTransform = new MousePositionTransform();
        }

        /// <summary>
        /// Use a custom <see cref="IMousePositionTransform"/> implemntation
        /// </summary>
        /// <param name="chromiumWebBrowser">browser</param>
        /// <param name="mousePositionTransform">custom implementation of <see cref="IMousePositionTransform"/>
        /// or defaults to <see cref="NoOpMousePositionTransform"/> if null.
        /// </param>
        public static void UsePopupMouseTransform(this ChromiumWebBrowser chromiumWebBrowser, IMousePositionTransform mousePositionTransform)
        {
            chromiumWebBrowser.MousePositionTransform = mousePositionTransform ?? new NoOpMousePositionTransform();
        }
    }
}

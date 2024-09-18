namespace CefSharp
{
    /// <summary>
    /// CEF supports both a Chrome runtime (based on the Chrome UI layer) and an
    /// Alloy runtime (based on the Chromium content layer). The Chrome runtime
    /// provides the full Chrome UI and browser functionality whereas the Alloy
    /// runtime provides less default browser functionality but adds additional
    /// client callbacks and support for windowless (off-screen) rendering. For
    /// additional comparative details on runtime types see
    /// https://bitbucket.org/chromiumembedded/cef/wiki/Architecture.md#markdown-header-cef3
    ///
    /// Each runtime is composed of a bootstrap component and a style component.
    /// The style component is individually
    /// configured for each window/browser at creation time and, in combination with
    /// the Chrome bootstrap, different styles can be mixed during runtime.
    ///
    /// Windowless rendering will always use Alloy style. Windowed rendering with a
    /// default window or client-provided parent window can configure the style via
    /// CefWindowInfo.runtime_style. Windowed rendering with the Views framework can
    /// configure the style via CefWindowDelegate::GetWindowRuntimeStyle and
    /// CefBrowserViewDelegate::GetBrowserRuntimeStyle. Alloy style Windows with the
    /// Views framework can host only Alloy style BrowserViews but Chrome style
    /// Windows can host both style BrowserViews. Additionally, a Chrome style
    /// Window can host at most one Chrome style BrowserView but potentially
    /// multiple Alloy style BrowserViews. See CefWindowInfo.runtime_style
    /// documentation for any additional platform-specific limitations.
    /// </summary>
    public enum CefRuntimeStyle
    {
        /// <summary>
        /// Use the default runtime style. See above documentation
        /// for exceptions.
        /// </summary>
        Default,

        /// <summary>
        /// Use the Chrome runtime style.
        /// </summary>
        Chrome,

        /// <summary>
        /// Use the Alloy runtime style.
        /// runtime.
        /// </summary>
        Alloy,
    }
}

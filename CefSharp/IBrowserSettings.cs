// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Interface representing browser initialization settings. 
    /// </summary>
    public interface IBrowserSettings
    {
        /// <summary>
        /// StandardFontFamily
        /// </summary>
        string StandardFontFamily { get; set; }

        /// <summary>
        /// FixedFontFamily
        /// </summary>
        string FixedFontFamily { get; set; }

        /// <summary>
        /// SerifFontFamily
        /// </summary>
        string SerifFontFamily { get; set; }
        
        /// <summary>
        /// SansSerifFontFamily
        /// </summary>
        string SansSerifFontFamily { get; set; }

        /// <summary>
        /// CursiveFontFamily
        /// </summary>
        string CursiveFontFamily { get; set; }

        /// <summary>
        /// FantasyFontFamily
        /// </summary>
        string FantasyFontFamily { get; set; }

        /// <summary>
        /// DefaultFontSize
        /// </summary>
        int DefaultFontSize { get; set; }

        /// <summary>
        /// DefaultFixedFontSize
        /// </summary>
        int DefaultFixedFontSize { get; set; }

        /// <summary>
        /// MinimumFontSize
        /// </summary>
        int MinimumFontSize { get; set; }

        /// <summary>
        /// MinimumLogicalFontSize
        /// </summary>
        int MinimumLogicalFontSize { get; set; }

        /// <summary>
        /// Default encoding for Web content. If empty "ISO-8859-1" will be used. Also
        /// configurable using the "default-encoding" command-line switch.
        /// </summary>
        string DefaultEncoding { get; set; }

        /// <summary>
        /// Controls the loading of fonts from remote sources. Also configurable using
        /// the "disable-remote-fonts" command-line switch.
        /// </summary>
        CefState RemoteFonts { get; set; }

        /// <summary>
        /// Controls whether JavaScript can be executed.
        /// (Disable javascript)
        /// </summary>
        CefState Javascript { get; set; }

        /// <summary>
        /// Controls whether JavaScript can be used to close windows that were not
        /// opened via JavaScript. JavaScript can still be used to close windows that
        /// were opened via JavaScript. Also configurable using the
        /// "disable-javascript-close-windows" command-line switch.
        /// </summary>
        CefState JavascriptCloseWindows { get; set; }

        /// <summary>
        /// Controls whether JavaScript can access the clipboard. Also configurable
        /// using the "disable-javascript-access-clipboard" command-line switch.
        /// </summary>
        CefState JavascriptAccessClipboard { get; set; }

        /// <summary>
        /// Controls whether DOM pasting is supported in the editor via
        /// execCommand("paste"). The |javascript_access_clipboard| setting must also
        /// be enabled. Also configurable using the "disable-javascript-dom-paste"
        /// command-line switch.
        /// </summary>
        CefState JavascriptDomPaste { get; set; }

        /// <summary>
        /// Controls whether any plugins will be loaded. Also configurable using the
        /// "disable-plugins" command-line switch.
        /// </summary>
        CefState Plugins { get; set; }

        /// <summary>
        /// Controls whether file URLs will have access to all URLs. Also configurable
        /// using the "allow-universal-access-from-files" command-line switch.
        /// </summary>
        CefState UniversalAccessFromFileUrls { get; set; }

        /// <summary>
        /// Controls whether file URLs will have access to other file URLs. Also
        /// configurable using the "allow-access-from-files" command-line switch.
        /// </summary>
        CefState FileAccessFromFileUrls { get; set; }

        /// <summary>
        /// Controls whether web security restrictions (same-origin policy) will be
        /// enforced. Disabling this setting is not recommend as it will allow risky
        /// security behavior such as cross-site scripting (XSS). Also configurable
        /// using the "disable-web-security" command-line switch.
        /// </summary>
        CefState WebSecurity { get; set; }

        /// <summary>
        /// Controls whether image URLs will be loaded from the network. A cached image
        /// will still be rendered if requested. Also configurable using the
        /// "disable-image-loading" command-line switch.
        /// </summary>
        CefState ImageLoading { get; set; }

        /// <summary>
        /// Controls whether standalone images will be shrunk to fit the page. Also
        /// configurable using the "image-shrink-standalone-to-fit" command-line
        /// switch.
        /// </summary>
        CefState ImageShrinkStandaloneToFit { get; set; }

        /// <summary>
        /// Controls whether text areas can be resized. Also configurable using the
        /// "disable-text-area-resize" command-line switch.
        /// </summary>
        CefState TextAreaResize { get; set; }

        /// <summary>
        /// Controls whether the tab key can advance focus to links. Also configurable
        /// using the "disable-tab-to-links" command-line switch.
        /// </summary>
        CefState TabToLinks { get; set; }

        /// <summary>
        /// Controls whether local storage can be used. Also configurable using the
        /// "disable-local-storage" command-line switch.
        /// </summary>
        CefState LocalStorage { get; set; }

        /// <summary>
        /// Controls whether databases can be used. Also configurable using the
        /// "disable-databases" command-line switch.
        /// </summary>
        CefState Databases { get; set; }

        /// <summary>
        /// Controls whether the application cache can be used. Also configurable using
        /// the "disable-application-cache" command-line switch.
        /// </summary>
        CefState ApplicationCache { get; set; }

        /// <summary>
        /// Controls whether WebGL can be used. Note that WebGL requires hardware
        /// support and may not work on all systems even when enabled. Also
        /// configurable using the "disable-webgl" command-line switch.
        /// </summary>
        CefState WebGl { get; set; }
        
        /// <summary>
        /// Opaque background color used for the browser before a document is loaded
        /// and when no document color is specified. By default the background color
        /// will be the same as CefSettings.BackgroundColor. Only the RGB compontents
        /// of the specified value will be used. The alpha component must greater than
        /// 0 to enable use of the background color but will be otherwise ignored.
        /// </summary>
        uint BackgroundColor { get; set; }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that
        /// will be used in the "Accept-Language" HTTP header. May be overridden on a
        /// per-browser basis using the CefBrowserSettings.AcceptLanguageList value.
        /// If both values are empty then "en-US,en" will be used. Can be overridden
        /// for individual RequestContext instances via the
        /// RequestContextSettings.AcceptLanguageList value.
        /// </summary>
        string AcceptLanguageList { get; set; }

        /// <summary>
        /// The maximum rate in frames per second (fps) that CefRenderHandler::OnPaint
        /// will be called for a windowless browser. The actual fps may be lower if
        /// the browser cannot generate frames at the requested rate. The minimum
        /// value is 1 and the maximum value is 60 (default 30). This value can also be
        /// changed dynamically via IBrowserHost.SetWindowlessFrameRate.
        /// </summary>
        int WindowlessFrameRate { get; set; }
    }
}

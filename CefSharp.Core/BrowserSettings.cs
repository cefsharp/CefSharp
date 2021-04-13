// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <inheritdoc/>
    public class BrowserSettings : IBrowserSettings
    {
        private CefSharp.Core.BrowserSettings settings;

        /// <inheritdoc/>
        public BrowserSettings(bool autoDispose = false)
        {
            settings = new CefSharp.Core.BrowserSettings(autoDispose);
        }

        /// <inheritdoc/>
        public string StandardFontFamily
        {
            get { return settings.StandardFontFamily; }
            set { settings.StandardFontFamily = value; }
        }

        /// <inheritdoc/>
        public string FixedFontFamily
        {
            get { return settings.FixedFontFamily; }
            set { settings.FixedFontFamily = value; }
        }

        /// <inheritdoc/>
        public string SerifFontFamily
        {
            get { return settings.SerifFontFamily; }
            set { settings.SerifFontFamily = value; }
        }

        /// <inheritdoc/>
        public string SansSerifFontFamily
        {
            get { return settings.SansSerifFontFamily; }
            set { settings.SansSerifFontFamily = value; }
        }

        /// <inheritdoc/>
        public string CursiveFontFamily
        {
            get { return settings.CursiveFontFamily; }
            set { settings.CursiveFontFamily = value; }
        }

        /// <inheritdoc/>
        public string FantasyFontFamily
        {
            get { return settings.FantasyFontFamily; }
            set { settings.FantasyFontFamily = value; }
        }

        /// <inheritdoc/>
        public int DefaultFontSize
        {
            get { return settings.DefaultFontSize; }
            set { settings.DefaultFontSize = value; }
        }

        /// <inheritdoc/>
        public int DefaultFixedFontSize
        {
            get { return settings.DefaultFixedFontSize; }
            set { settings.DefaultFixedFontSize = value; }
        }

        /// <inheritdoc/>
        public int MinimumFontSize
        {
            get { return settings.MinimumFontSize; }
            set { settings.MinimumFontSize = value; }
        }

        /// <inheritdoc/>
        public int MinimumLogicalFontSize
        {
            get { return settings.MinimumLogicalFontSize; }
            set { settings.MinimumLogicalFontSize = value; }
        }

        /// <inheritdoc/>
        public string DefaultEncoding
        {
            get { return settings.DefaultEncoding; }
            set { settings.DefaultEncoding = value; }
        }

        /// <inheritdoc/>
        public CefState RemoteFonts
        {
            get { return settings.RemoteFonts; }
            set { settings.RemoteFonts = value; }
        }

        /// <inheritdoc/>
        public CefState Javascript
        {
            get { return settings.Javascript; }
            set { settings.Javascript = value; }
        }

        /// <inheritdoc/>
        public CefState JavascriptCloseWindows
        {
            get { return settings.JavascriptCloseWindows; }
            set { settings.JavascriptCloseWindows = value; }
        }

        /// <inheritdoc/>
        public CefState JavascriptAccessClipboard
        {
            get { return settings.JavascriptAccessClipboard; }
            set { settings.JavascriptAccessClipboard = value; }
        }

        /// <inheritdoc/>
        public CefState JavascriptDomPaste
        {
            get { return settings.JavascriptDomPaste; }
            set { settings.JavascriptDomPaste = value; }
        }

        /// <inheritdoc/>
        public CefState Plugins
        {
            get { return settings.Plugins; }
            set { settings.Plugins = value; }
        }

        /// <inheritdoc/>
        public CefState UniversalAccessFromFileUrls
        {
            get { return settings.UniversalAccessFromFileUrls; }
            set { settings.UniversalAccessFromFileUrls = value; }
        }

        /// <inheritdoc/>
        public CefState FileAccessFromFileUrls
        {
            get { return settings.FileAccessFromFileUrls; }
            set { settings.FileAccessFromFileUrls = value; }
        }

        /// <inheritdoc/>
        public CefState ImageLoading
        {
            get { return settings.ImageLoading; }
            set { settings.ImageLoading = value; }
        }

        /// <inheritdoc/>
        public CefState ImageShrinkStandaloneToFit
        {
            get { return settings.ImageShrinkStandaloneToFit; }
            set { settings.ImageShrinkStandaloneToFit = value; }
        }

        /// <inheritdoc/>
        public CefState TextAreaResize
        {
            get { return settings.TextAreaResize; }
            set { settings.TextAreaResize = value; }
        }

        /// <inheritdoc/>
        public CefState TabToLinks
        {
            get { return settings.TabToLinks; }
            set { settings.TabToLinks = value; }
        }

        /// <inheritdoc/>
        public CefState LocalStorage
        {
            get { return settings.LocalStorage; }
            set { settings.LocalStorage = value; }
        }

        /// <inheritdoc/>
        public CefState Databases
        {
            get { return settings.Databases; }
            set { settings.Databases = value; }
        }

        /// <inheritdoc/>
        public CefState ApplicationCache
        {
            get { return settings.ApplicationCache; }
            set { settings.ApplicationCache = value; }
        }

        /// <inheritdoc/>
        public CefState WebGl
        {
            get { return settings.WebGl; }
            set { settings.WebGl = value; }
        }

        /// <inheritdoc/>
        public uint BackgroundColor
        {
            get { return settings.BackgroundColor; }
            set { settings.BackgroundColor = value; }
        }

        /// <inheritdoc/>
        public string AcceptLanguageList
        {
            get { return settings.AcceptLanguageList; }
            set { settings.AcceptLanguageList = value; }
        }

        /// <inheritdoc/>
        public int WindowlessFrameRate
        {
            get { return settings.WindowlessFrameRate; }
            set { settings.WindowlessFrameRate = value; }
        }

        /// <inheritdoc/>
        public bool IsDisposed
        {
            get { return settings.IsDisposed; }
        }

        /// <inheritdoc/>
        public bool AutoDispose
        {
            get { return settings.AutoDispose; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            settings.Dispose();
        }

        /// <summary>
        /// Used internally to get the underlying <see cref="IBrowserSettings"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IBrowserSettings UnWrap()
        {
            return settings;
        }

        /// <summary>
        /// Create a new instance of <see cref="IBrowserSettings"/>
        /// </summary>
        /// <param name="autoDispose">set to false if you plan to reuse the instance, otherwise true</param>
        /// <returns>BrowserSettings</returns>
        public static IBrowserSettings Create(bool autoDispose = false)
        {
            return new CefSharp.Core.BrowserSettings(autoDispose);
        }
    }
}

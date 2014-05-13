// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Command which navigates to the previous page in the browser history. Will automatically be enabled/disabled depending
        /// on the browser state.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Command which navigates to the next page in the browser history. Will automatically be enabled/disabled depending on
        /// the browser state.
        /// </summary>
        ICommand ForwardCommand { get; }

        /// <summary>
        /// Command which reloads the content of the current page. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        ICommand ReloadCommand { get; }

        /// <summary>
        /// Command which prints the current browser contents.
        /// </summary>
        ICommand PrintCommand { get; }

        /// <summary>
        /// Command which increases the zoom level
        /// </summary>
        ICommand ZoomInCommand { get; }

        /// <summary>
        /// Command which decreases the zoom level
        /// </summary>
        ICommand ZoomOutCommand { get; }

        /// <summary>
        /// Command which resets the zoom level to default
        /// </summary>
        ICommand ZoomResetCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        ICommand ViewSourceCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        void ViewSource();

        /// <summary>
        /// Attempts to give focus to the WebBrowser control.
        /// </summary>
        /// <returns><c>true</c> if keyboard focus and logical focus were set to this element; <c>false</c> if only logical focus
        /// was set to this element, or if the call to this method did not force the focus to change.</returns>
        bool Focus();

        /// <summary>
        /// Reloads the current WebView
        /// </summary>
        void Reload();

        /// <summary>
        /// Reloads the current WebView, optionally ignoring the cache
        /// (which means the whole page including all .css, .js etc. resources will be re-fetched)
        /// </summary>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring borwser cache; <c>false</c> A reload is
        /// performed using browser cache</param>
        void Reload(bool ignoreCache);

        /// <summary>
        /// The zoom level at which the browser control is currently displaying. Can be set to 0 to clear the zoom level (resets to
        /// default zoom level)
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        double ZoomLevel { get; set; }
    }
}

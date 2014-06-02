// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Navigates to the previous page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Navigates to the next page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        ICommand ForwardCommand { get; }

        /// <summary>
        /// Reloads the content of the current page. Will automatically be enabled/disabled depending on the browser state.
        /// </summary>
        ICommand ReloadCommand { get; }

        /// <summary>
        /// Prints the current browser contents.
        /// </summary>
        ICommand PrintCommand { get; }

        /// <summary>
        /// Increases the zoom level.
        /// </summary>
        ICommand ZoomInCommand { get; }

        /// <summary>
        /// Decreases the zoom level.
        /// </summary>
        ICommand ZoomOutCommand { get; }

        /// <summary>
        /// Resets the zoom level to the default. (100%)
        /// </summary>
        ICommand ZoomResetCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        ICommand ViewSourceCommand { get; }

        /// <summary>
        /// Command which cleans up the Resources used by the ChromiumWebBrowser 
        /// </summary>
        ICommand CleanupCommand { get; }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        ICommand StopCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        void ViewSource();

        /// <summary>
        /// Attempts to give focus to the IWpfWebBrowser control.
        /// </summary>
        /// <returns><c>true</c> if keyboard focus and logical focus were set to this element; <c>false</c> if only logical focus
        /// was set to this element, or if the call to this method did not force the focus to change.</returns>
        bool Focus();

        /// <summary>
        /// Reloads the page being displayed.
        /// </summary>
        void Reload();

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js
        /// etc. resources will be re-fetched).
        /// </summary>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring borwser cache; <c>false</c> A reload is
        /// performed using browser cache</param>
        void Reload(bool ignoreCache);

        /// <summary>
        /// The zoom level at which the browser control is currently displaying. Can be set to 0 to clear the zoom level (resets to
        /// default zoom level).
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        double ZoomLevel { get; set; }
    }
}

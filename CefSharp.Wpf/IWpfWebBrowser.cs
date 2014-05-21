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
        /// Command which increases the zoom level.
        /// </summary>
        ICommand ZoomInCommand { get; }

        /// <summary>
        /// Command which decreases the zoom level.
        /// </summary>
        ICommand ZoomOutCommand { get; }

        /// <summary>
        /// Command which resets the zoom level to default.
        /// </summary>
        ICommand ZoomResetCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        ICommand ViewSourceCommand { get; }
        /// <summary>
        /// command which cleans up the Resources used by the WebView 
        /// </summary>
        ICommand CleanupCommand { get; }

        /// <summary>
        /// Command which stops loading the current page.
        /// </summary>
        ICommand StopCommand { get; }
    }
}

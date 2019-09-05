// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Input;
using System.Windows.Threading;
using CefSharp.Enums;
using CefSharp.Internals;

namespace CefSharp.Wpf
{
    /// <summary>
    /// WPF specific implementation, has reference to some of the commands
    /// and properties the <see cref="ChromiumWebBrowser" /> exposes.
    /// </summary>
    /// <seealso cref="CefSharp.IWebBrowser" />
    public interface IWpfWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Navigates to the previous page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        /// <value>The back command.</value>
        ICommand BackCommand { get; }

        /// <summary>
        /// Navigates to the next page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        /// <value>The forward command.</value>
        ICommand ForwardCommand { get; }

        /// <summary>
        /// Reloads the content of the current page. Will automatically be enabled/disabled depending on the browser state.
        /// </summary>
        /// <value>The reload command.</value>
        ICommand ReloadCommand { get; }

        /// <summary>
        /// Prints the current browser contents.
        /// </summary>
        /// <value>The print command.</value>
        ICommand PrintCommand { get; }

        /// <summary>
        /// Increases the zoom level.
        /// </summary>
        /// <value>The zoom in command.</value>
        ICommand ZoomInCommand { get; }

        /// <summary>
        /// Decreases the zoom level.
        /// </summary>
        /// <value>The zoom out command.</value>
        ICommand ZoomOutCommand { get; }

        /// <summary>
        /// Resets the zoom level to the default. (100%)
        /// </summary>
        /// <value>The zoom reset command.</value>
        ICommand ZoomResetCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        /// <value>The view source command.</value>
        ICommand ViewSourceCommand { get; }

        /// <summary>
        /// Command which cleans up the Resources used by the ChromiumWebBrowser
        /// </summary>
        /// <value>The cleanup command.</value>
        ICommand CleanupCommand { get; }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        /// <value>The stop command.</value>
        ICommand StopCommand { get; }

        /// <summary>
        /// Cut selected text to the clipboard.
        /// </summary>
        /// <value>The cut command.</value>
        ICommand CutCommand { get; }

        /// <summary>
        /// Copy selected text to the clipboard.
        /// </summary>
        /// <value>The copy command.</value>
        ICommand CopyCommand { get; }

        /// <summary>
        /// Paste text from the clipboard.
        /// </summary>
        /// <value>The paste command.</value>
        ICommand PasteCommand { get; }

        /// <summary>
        /// Select all text.
        /// </summary>
        /// <value>The select all command.</value>
        ICommand SelectAllCommand { get; }

        /// <summary>
        /// Undo last action.
        /// </summary>
        /// <value>The undo command.</value>
        ICommand UndoCommand { get; }

        /// <summary>
        /// Redo last action.
        /// </summary>
        /// <value>The redo command.</value>
        ICommand RedoCommand { get; }

        /// <summary>
        /// Gets the <see cref="Dispatcher" /> associated with this instance.
        /// </summary>
        /// <value>The dispatcher.</value>
        Dispatcher Dispatcher { get; }

        /// <summary>
        /// The zoom level at which the browser control is currently displaying.
        /// Can be set to 0 to clear the zoom level (resets to default zoom level).
        /// </summary>
        /// <value>The zoom level.</value>
        double ZoomLevel { get; set; }

        /// <summary>
        /// The increment at which the <see cref="ZoomLevel" /> property will be incremented/decremented.
        /// </summary>
        /// <value>The zoom level increment.</value>
        double ZoomLevelIncrement { get; set; }

        /// <summary>
        /// The title of the web page being currently displayed.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks>This property is implemented as a Dependency Property and fully supports data binding.</remarks>
        string Title { get; }

        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnPaint"/> is called. You can access the underlying buffer, though it's
        /// preferable to either override <see cref="ChromiumWebBrowser.OnPaint"/> or implement your own <see cref="IRenderHandler"/> as there is no outwardly
        /// accessible locking (locking is done within the default <see cref="IRenderHandler"/> implementations).
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<PaintEventArgs> Paint;

        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnVirtualKeyboardRequested(IBrowser, TextInputMode)"/> is called. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<VirtualKeyboardRequestedEventArgs> VirtualKeyboardRequested;
    }
}

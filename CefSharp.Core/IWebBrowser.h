// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefErrorCode.h"
#include "ConsoleMessageEventArgs.h"
#include "LoadCompletedEventArgs.h"

using namespace System;
using namespace System::ComponentModel;

namespace CefSharp
{
    interface class IWebBrowser;
    interface class IRequestHandler;

    /// <summary>
    /// A delegate type used to listen to LoadError messages.
    /// </summary>
    /// <param name="failedUrl">The URL that failed to load.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorText">The error text.</param>
    public delegate void LoadErrorEventHandler(String^ failedUrl, CefErrorCode errorCode, String^ errorText);

    public interface class IWebBrowser : IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// </summary>
        event ConsoleMessageEventHandler^ ConsoleMessage;

        /// <summary>
        /// Event handler that will get called whenever page loading is complete.
        /// </summary>        
        event LoadCompletedEventHandler^ LoadCompleted;

        /// <summary>
        /// Event handler that will get called whenever a load error occurs.
        /// </summary>        
        event LoadErrorEventHandler^ LoadError;
        
        void Load(String^ url);
        
        /// <summary>
        /// Loads custom HTML content into the web browser.
        /// </summary>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        void LoadHtml(String^ html, String^ url);

        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        void RegisterJsObject(String^ name, Object^ objectToBind);

        /// <summary>
        /// Execute some JavaScript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="script">The JavaScript code that should be executed.</param>
        void ExecuteScriptAsync(String^ script);

        /// <summary>
        /// Execute some JavaScript code in the context of this WebBrowser, and return the result of the evaluation.
        /// </summary>
        /// <param name="script">The JavaScript code that should be executed.</param>
        Object^ EvaluateScript(String^ script);

        /// <summary>
        /// Execute some JavaScript code in the context of this WebBrowser, and return the result of the evaluation.
        /// </summary>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">The timeout after which the JavaScript code execution should be aborted.</param>
        Object^ EvaluateScript(String^ script, Nullable<TimeSpan> timeout);

        property IRequestHandler^ RequestHandler;
        property bool IsBrowserInitialized { bool get(); }
        property bool IsLoading { bool get(); }
        property bool CanGoBack { bool get(); }
        property bool CanGoForward { bool get(); }
        property String^ Address;
        property String^ Title;
        property String^ TooltipText;
    };
}

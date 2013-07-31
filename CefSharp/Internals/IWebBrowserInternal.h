// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IWebBrowser.h"

namespace CefSharp
{
    interface class ILifeSpanHandler;
    interface class ILoadHandler;
    interface class IRequestHandler;
    interface class IMenuHandler;
    interface class IKeyboardHandler;
    interface class IJsDialogHandler;

    private interface class IWebBrowserInternal : IWebBrowser
    {
        event ConsoleMessageEventHandler^ ConsoleMessage;

        property bool IsBrowserInitialized { bool get(); }
        property bool IsLoading { bool get(); }

        property String^ TooltipText;

        property ILifeSpanHandler^ LifeSpanHandler;
        property ILoadHandler^ LoadHandler;
        property IRequestHandler^ RequestHandler;
        property IMenuHandler^ MenuHandler;
        property IKeyboardHandler^ KeyboardHandler;
        property IJsDialogHandler^ JsDialogHandler;

        void OnInitialized();

        void SetAddress(String^ address);
        void SetTitle(String^ title);
        void LoadHtml(String^ html);
        void Stop();
        void Reload();
        void Reload(bool ignoreCache);
        void ClearHistory();
        void ShowDevTools();
        void CloseDevTools();

        void Undo();
        void Redo();
        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void SelectAll();
        void Print();

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void OnFrameLoadStart(String^ url);
        void OnFrameLoadEnd(String^ url);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(String^ message, String^ source, int line);

        void RegisterJsObject(String^ name, Object^ objectToBind);
        IDictionary<String^, Object^>^ GetBoundObjects();
    };
}
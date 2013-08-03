// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IWebBrowser.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    interface class ILifeSpanHandler;
    interface class IRequestHandler;
    interface class IMenuHandler;
    interface class IKeyboardHandler;
    interface class IJsDialogHandler;

    // TODO: Move to the CefSharp.WinForms project once we have recreated it with C#.
    private interface class IWinFormsWebBrowser : IWebBrowser
    {
        property IMenuHandler^ MenuHandler;
    };

    private interface class IWebBrowserInternal : IWebBrowser
    {
        property bool IsBrowserInitialized { bool get(); }

        property ILifeSpanHandler^ LifeSpanHandler;
        property IRequestHandler^ RequestHandler;
        property IKeyboardHandler^ KeyboardHandler;
        property IJsDialogHandler^ JsDialogHandler;

        void OnInitialized();

        void SetAddress(String^ address);
        void SetIsLoading(bool);
        void SetNavState(bool canGoBack, bool canGoForward);
        void SetTitle(String^ title);
        void SetTooltipText(String^ tooltipText);
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

        void OnFrameLoadStart(String^ url);
        void OnFrameLoadEnd(String^ url);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(String^ message, String^ source, int line);
        void OnLoadError(String^ url, CefErrorCode errorCode, String^ errorText);

        void RegisterJsObject(String^ name, Object^ objectToBind);
        IDictionary<String^, Object^>^ GetBoundObjects();
    };
}
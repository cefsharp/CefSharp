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
        void SetNavState(bool canGoBack, bool canGoForward, bool canReload);
        void SetTitle(String^ title);
        void SetTooltipText(String^ tooltipText);
        void ShowDevTools();
        void CloseDevTools();

        void OnFrameLoadStart(String^ url);
        void OnFrameLoadEnd(String^ url);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(String^ message, String^ source, int line);
        void OnLoadError(String^ url, CefErrorCode errorCode, String^ errorText);

        IDictionary<String^, Object^>^ GetBoundObjects();
    };
}
// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "Async/JavascriptAsyncRootObjectWrapper.h"
#include "Async/JavascriptAsyncMethodCallback.h"

using namespace CefSharp::Internals;
using namespace CefSharp::Internals::Async;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    // "Master class" for wrapping everything that the Cef Subprocess needs 
    // for ONE CefBrowser.
    public ref class CefBrowserWrapper
    {
    private:
        MCefRefPtr<CefBrowser> _cefBrowser;
        Dictionary<int64, JavascriptAsyncMethodCallback^>^ _methodCallbacks;
        int64 _lastCallback;
        JavascriptCallbackRegistry^ _callbackRegistry;
        JavascriptRootObjectWrapper^ _javascriptRootObjectWrapper;
        JavascriptAsyncRootObjectWrapper^ _javascriptAsyncRootObjectWrapper;

    internal:
        property JavascriptCallbackRegistry^ CallbackRegistry
        {
            CefSharp::Internals::JavascriptCallbackRegistry^ get();
        }

		int64 SaveMethodCallback(JavascriptAsyncMethodCallback^ callback);

    public:
        CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
        {
            _cefBrowser = cefBrowser;
            BrowserId = cefBrowser->GetIdentifier();
            IsPopup = cefBrowser->IsPopup();
            _callbackRegistry = gcnew JavascriptCallbackRegistry(BrowserId);
            _methodCallbacks = gcnew Dictionary<int64, JavascriptAsyncMethodCallback^>();
        }
        
        !CefBrowserWrapper()
        {
            _cefBrowser = nullptr;
        }

        ~CefBrowserWrapper()
        {
            this->!CefBrowserWrapper();

			_methodCallbacks = nullptr;

			if (_callbackRegistry != nullptr)
			{
				delete _callbackRegistry;
				_callbackRegistry = nullptr;
			}

            if (JavascriptRootObjectWrapper != nullptr)
            {
                delete JavascriptRootObjectWrapper;

                JavascriptRootObjectWrapper = nullptr;
            }

			if (JavascriptAsyncRootObjectWrapper != nullptr)
			{
				delete JavascriptAsyncRootObjectWrapper;

				JavascriptAsyncRootObjectWrapper = nullptr;
			}
        }

        property int BrowserId;
        property bool IsPopup;

        // This allows us to create the WCF proxies back to our parent process.
        property ChannelFactory<IBrowserProcess^>^ ChannelFactory;

        // The serialized registered object data waiting to be used (only contains methods and bound async).
        property JavascriptRootObject^ JavascriptAsyncRootObject;

        // The serialized registered object data waiting to be used.
        property JavascriptRootObject^ JavascriptRootObject;

        property JavascriptRootObjectWrapper^ JavascriptRootObjectWrapper;

		property JavascriptAsyncRootObjectWrapper^ JavascriptAsyncRootObjectWrapper;

        // The WCF proxy to the parent process.
        property IBrowserProcess^ BrowserProcess;

        bool TryGetAndRemoveMethodCallback(int64 id, JavascriptAsyncMethodCallback^% callback);
    };
}

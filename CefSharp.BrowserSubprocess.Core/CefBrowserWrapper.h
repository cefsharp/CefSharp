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
#include "Async/JavascriptAsyncMethodCallback.h"
#include "ConcurrentObjectRegistry.h"

using namespace CefSharp::Internals::Async;
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
        // The entire set of possible JavaScript functions to
        // call directly into.
        JavascriptCallbackRegistry^ _callbackRegistry;
        ConcurrentObjectRegistry<JavascriptAsyncMethodCallback^>^ _methodCallbacks;
    
    internal:
        property ConcurrentDictionary<int64, JavascriptRootObjectWrapper^>^ JavascriptRootObjectWrappers;
        
        property JavascriptCallbackRegistry^ CallbackRegistry
        {
            CefSharp::Internals::JavascriptCallbackRegistry^ get()
            {
                return _callbackRegistry;
            }
        }

        property ConcurrentObjectRegistry<JavascriptAsyncMethodCallback^>^ MethodCallbackRegistry
        {
            CefSharp::Internals::ConcurrentObjectRegistry<JavascriptAsyncMethodCallback^>^ get()
            {
                return _methodCallbacks;
            }
        }

    public:
        CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
        {
            _methodCallbacks = gcnew ConcurrentObjectRegistry<JavascriptAsyncMethodCallback^>();
            JavascriptRootObjectWrappers = gcnew ConcurrentDictionary<int64, JavascriptRootObjectWrapper^>();
            _cefBrowser = cefBrowser;
            BrowserId = cefBrowser->GetIdentifier();
            _callbackRegistry = gcnew JavascriptCallbackRegistry(BrowserId);
            IsPopup = cefBrowser->IsPopup();
        }
        
        !CefBrowserWrapper()
        {
            _cefBrowser = nullptr;
        }

        ~CefBrowserWrapper()
        {
            this->!CefBrowserWrapper();
            if (JavascriptRootObjectWrappers != nullptr)
            {
                for each(auto entry in JavascriptRootObjectWrappers)
                {
                    delete entry.Value;
                }
                delete JavascriptRootObjectWrappers;

                JavascriptRootObjectWrappers = nullptr;
            }
            if (_callbackRegistry != nullptr)
            {
                delete _callbackRegistry;
                _callbackRegistry = nullptr;
            }

            if (_methodCallbacks != nullptr)
            {
                delete _methodCallbacks;
                _methodCallbacks = nullptr;
            }
        }

        property int BrowserId;
        property bool IsPopup;

        // This allows us to create the WCF proxies back to our parent process.
        property ChannelFactory<IBrowserProcess^>^ ChannelFactory;

        // The WCF proxy to the parent process.
        property IBrowserProcess^ BrowserProcess;
    };
}

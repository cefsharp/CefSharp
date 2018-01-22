// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "ManagedCefBrowserAdapter.h"
#include "Internals/Messaging/Messages.h"
#include "Internals/CefFrameWrapper.h"
#include "Internals/CefSharpBrowserWrapper.h"

using namespace CefSharp::Internals::Messaging;

bool ManagedCefBrowserAdapter::IsDisposed::get()
{
    return _isDisposed;
}

void ManagedCefBrowserAdapter::CreateOffscreenBrowser(IntPtr windowHandle, BrowserSettings^ browserSettings, RequestContext^ requestContext, String^ address)
{
    //Create the required BitmapInfo classes before the offscreen browser is initialized  
    auto renderClientAdapter = dynamic_cast<RenderClientAdapter*>(_clientAdapter.get());  
    renderClientAdapter->CreateBitmapInfo();

    auto hwnd = static_cast<HWND>(windowHandle.ToPointer());

    CefWindowInfo window;
    window.SetAsWindowless(hwnd);
    CefString addressNative = StringUtils::ToNative(address);

    if (!CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, static_cast<CefRefPtr<CefRequestContext>>(requestContext)))
    {
        throw gcnew InvalidOperationException("Failed to create offscreen browser. Call Cef.Initialize() first.");
    }
}

void ManagedCefBrowserAdapter::OnAfterBrowserCreated(IBrowser^ browser)
{
    if (!_isDisposed)
    {
        _browserWrapper = browser;
        _javascriptCallbackFactory->BrowserAdapter = gcnew WeakReference(this);

        //Browser has been initialized, it's now too late to register a sync JSB object if Wcf wasn't enabled
        _javaScriptObjectRepository->IsBrowserInitialized = true;

        if (CefSharpSettings::WcfEnabled)
        {
            _browserProcessServiceHost = gcnew BrowserProcessServiceHost(_javaScriptObjectRepository, Process::GetCurrentProcess()->Id, browser->Identifier, _javascriptCallbackFactory);
            //NOTE: Attempt to solve timing issue where browser is opened and rapidly disposed. In some cases a call to Open throws
            // an exception about the process already being closed. Two relevant issues are #862 and #804.
            if (_browserProcessServiceHost->State == CommunicationState::Created)
            {
                try
                {
                    _browserProcessServiceHost->Open();
                }
                catch (Exception^)
                {
                    //Ignore exception as it's likely cause when the browser is closing
                }
            }
        }
    
        if (_webBrowserInternal != nullptr)
        {
            _webBrowserInternal->OnAfterBrowserCreated(browser);
        }
    }
}

void ManagedCefBrowserAdapter::CreateBrowser(BrowserSettings^ browserSettings, RequestContext^ requestContext, IntPtr sourceHandle, String^ address)
{
    HWND hwnd = static_cast<HWND>(sourceHandle.ToPointer());
    RECT rect;
    GetClientRect(hwnd, &rect);
    CefWindowInfo window;
    window.SetAsChild(hwnd, rect);
    CefString addressNative = StringUtils::ToNative(address);

    CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, static_cast<CefRefPtr<CefRequestContext>>(requestContext));
}

void ManagedCefBrowserAdapter::Resize(int width, int height)
{
    HWND browserHwnd = _clientAdapter->GetBrowserHwnd();
    if (browserHwnd)
    {
        if (width == 0 && height == 0)
        {
            // For windowed browsers when the frame window is minimized set the
            // browser window size to 0x0 to reduce resource usage.
            SetWindowPos(browserHwnd, NULL, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOACTIVATE);
        }
        else
        {
            SetWindowPos(browserHwnd, NULL, 0, 0, width, height, SWP_NOZORDER);
        }
    }
}

IBrowser^ ManagedCefBrowserAdapter::GetBrowser(int browserId)
{
    return _clientAdapter->GetBrowserWrapper(browserId);
}

IJavascriptCallbackFactory^ ManagedCefBrowserAdapter::JavascriptCallbackFactory::get()
{
    return _javascriptCallbackFactory;
}

JavascriptObjectRepository^ ManagedCefBrowserAdapter::JavascriptObjectRepository::get()
{
    return _javaScriptObjectRepository;
}

MethodRunnerQueue^ ManagedCefBrowserAdapter::MethodRunnerQueue::get()
{
    return _methodRunnerQueue;
}

void ManagedCefBrowserAdapter::MethodInvocationComplete(Object^ sender, MethodInvocationCompleteArgs^ e)
{
    auto result = e->Result;
    if (result->CallbackId.HasValue)
    {
        _clientAdapter->MethodInvocationComplete(result);
    }
}

MCefRefPtr<ClientAdapter> ManagedCefBrowserAdapter::GetClientAdapter()
{
    return _clientAdapter;
}

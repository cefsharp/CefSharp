// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_request_context.h"
#include "RequestContextSettings.h"
#include "SchemeHandlerFactoryWrapper.h"
#include "RequestContextHandler.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Internals\CookieManager.h"
#include "Internals\CefWrapper.h"
#include "Internals\CefResolveCallbackAdapter.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    /// <summary>
    /// A request context provides request handling for a set of related browser objects.
    /// A request context is specified when creating a new browser object via the CefBrowserHost
    /// static factory methods. Browser objects with different request contexts will never be
    /// hosted in the same render process. Browser objects with the same request context may or
    /// may not be hosted in the same render process depending on the process model.
    /// Browser objects created indirectly via the JavaScript window.open function or targeted
    /// links will share the same render process and the same request context as the source browser.
    /// When running in single-process mode there is only a single render process (the main process)
    /// and so all browsers created in single-process mode will share the same request context.
    /// This will be the first request context passed into a CefBrowserHost static factory method
    /// and all other request context objects will be ignored. 
    /// </summary>
    public ref class RequestContext : public IRequestContext, public CefWrapper
    {
    private:
        MCefRefPtr<CefRequestContext> _requestContext;
        RequestContextSettings^ _settings;

    internal:
        RequestContext(CefRefPtr<CefRequestContext>& context)
        {
            _requestContext = context;
            _settings = nullptr;
        }

    public:
        RequestContext()
        {
            CefRequestContextSettings settings;
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        RequestContext(RequestContextSettings^ settings) : _settings(settings)
        {
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        RequestContext(IPluginHandler^ pluginHandler)
        {
            CefRequestContextSettings settings;
            _requestContext = CefRequestContext::CreateContext(settings, new RequestContextHandler(pluginHandler));
        }

        RequestContext(RequestContextSettings^ settings, IPluginHandler^ pluginHandler) : _settings(settings)
        {
            _requestContext = CefRequestContext::CreateContext(settings, new RequestContextHandler(pluginHandler));
        }

        !RequestContext()
        {
            _requestContext = NULL;
        }

        ~RequestContext()
        {
            this->!RequestContext();

            delete _settings;

            _disposed = true;
        }

        virtual bool IsSame(IRequestContext^ context)
        {
            ThrowIfDisposed();

            auto requestContext = (RequestContext^)context;

            return _requestContext->IsSame(requestContext);
        }

        virtual bool IsSharingWith(IRequestContext^ context)
        {
            ThrowIfDisposed();

            auto requestContext = (RequestContext^)context;

            return _requestContext->IsSharingWith(requestContext);
        }

        virtual ICookieManager^ GetDefaultCookieManager(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            auto cookieManager = _requestContext->GetDefaultCookieManager(wrapper);
            if (cookieManager.get())
            {
                return gcnew CookieManager(cookieManager);
            }
            return nullptr;
        }

        virtual property bool IsGlobal
        {
            bool get()
            {
                ThrowIfDisposed();
                return _requestContext->IsGlobal();
            }
        }

        virtual bool RegisterSchemeHandlerFactory(String^ schemeName, String^ domainName, ISchemeHandlerFactory^ factory)
        {
            ThrowIfDisposed();

            auto wrapper = new SchemeHandlerFactoryWrapper(factory);
            return _requestContext->RegisterSchemeHandlerFactory(StringUtils::ToNative(schemeName), StringUtils::ToNative(domainName), wrapper);
        }

        virtual bool ClearSchemeHandlerFactories()
        {
            ThrowIfDisposed();

            return _requestContext->ClearSchemeHandlerFactories();
        }

        ///
        // Returns the cache path for this object. If empty an "incognito mode"
        // in-memory cache is being used.
        ///
        /*--cef()--*/
        virtual property String^ CachePath
        {
            String^ get()
            {
                ThrowIfDisposed();

                return StringUtils::ToClr(_requestContext->GetCachePath());
            }
        }

        ///
        // Tells all renderer processes associated with this context to throw away
        // their plugin list cache. If |reload_pages| is true they will also reload
        // all pages with plugins. CefRequestContextHandler::OnBeforePluginLoad may
        // be called to rebuild the plugin list cache.
        ///
        /*--cef()--*/
        virtual void PurgePluginListCache(bool reloadPages)
        {
            ThrowIfDisposed();

            _requestContext->PurgePluginListCache(reloadPages);
        }

        ///
        // Returns true if a preference with the specified |name| exists. This method
        // must be called on the browser process UI thread.
        ///
        /*--cef()--*/
        virtual bool HasPreference(String^ name)
        {
            ThrowIfDisposed();

            return _requestContext->HasPreference(StringUtils::ToNative(name));
        }

        ///
        // Returns the value for the preference with the specified |name|. Returns
        // NULL if the preference does not exist. The returned object contains a copy
        // of the underlying preference value and modifications to the returned object
        // will not modify the underlying preference value. This method must be called
        // on the browser process UI thread.
        ///
        /*--cef()--*/
        virtual Object^ GetPreference(String^ name)
        {
            ThrowIfDisposed();

            return TypeConversion::FromNative(_requestContext->GetPreference(StringUtils::ToNative(name)));
        }

        ///
        // Returns all preferences as a dictionary. If |include_defaults| is true then
        // preferences currently at their default value will be included. The returned
        // object contains a copy of the underlying preference values and
        // modifications to the returned object will not modify the underlying
        // preference values. This method must be called on the browser process UI
        // thread.
        ///
        /*--cef()--*/
        virtual IDictionary<String^, Object^>^ GetAllPreferences(bool includeDefaults)
        {
            ThrowIfDisposed();

            auto preferences = _requestContext->GetAllPreferences(includeDefaults);

            return TypeConversion::FromNative(preferences);
        }

        ///
        // Returns true if the preference with the specified |name| can be modified
        // using SetPreference. As one example preferences set via the command-line
        // usually cannot be modified. This method must be called on the browser
        // process UI thread.
        ///
        /*--cef()--*/
        virtual bool CanSetPreference(String^ name)
        {
            ThrowIfDisposed();

            return _requestContext->CanSetPreference(StringUtils::ToNative(name));
        }

        ///
        // Set the |value| associated with preference |name|. Returns true if the
        // value is set successfully and false otherwise. If |value| is NULL the
        // preference will be restored to its default value. If setting the preference
        // fails then |error| will be populated with a detailed description of the
        // problem. This method must be called on the browser process UI thread.
        // Preferences set via the command-line usually cannot be modified.
        ///
        /*--cef(optional_param=value)--*/
        virtual bool SetPreference(String^ name, Object^ value, [Out] String^ %error)
        {
            ThrowIfDisposed();

            CefString cefError;

            auto success = _requestContext->SetPreference(StringUtils::ToNative(name), TypeConversion::ToNative(value), cefError);

            error = StringUtils::ToClr(cefError);

            return success;
        }

        ///
        // Clears all certificate exceptions that were added as part of handling
        // CefRequestHandler::OnCertificateError(). If you call this it is
        // recommended that you also call CloseAllConnections() or you risk not
        // being prompted again for server certificates if you reconnect quickly.
        // If |callback| is non-NULL it will be executed on the UI thread after
        // completion.
        ///
        /*--cef(optional_param=callback)--*/
        virtual void ClearCertificateExceptions(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            _requestContext->ClearCertificateExceptions(wrapper);
        }

        ///
        // Clears all active and idle connections that Chromium currently has.
        // This is only recommended if you have released all other CEF objects but
        // don't yet want to call CefShutdown(). If |callback| is non-NULL it will be
        // executed on the UI thread after completion.
        ///
        /*--cef(optional_param=callback)--*/
        virtual void CloseAllConnections(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            _requestContext->CloseAllConnections(wrapper);
        }

        ///
        // Attempts to resolve |origin| to a list of associated IP addresses.
        // |callback| will be executed on the UI thread after completion.
        ///
        /*--cef()--*/
        virtual Task<ResolveCallbackResult>^ ResolveHostAsync(Uri^ origin)
        {
            ThrowIfDisposed();

            auto callback = gcnew TaskResolveCallbackHandler();

            CefRefPtr<CefResolveCallback> callbackWrapper = new CefResolveCallbackAdapter(callback);

            _requestContext->ResolveHost(StringUtils::ToNative(origin->AbsoluteUri), callbackWrapper);

            return callback->Task;
        }

        ///
        // Attempts to resolve |origin| to a list of associated IP addresses using
        // cached data. |resolved_ips| will be populated with the list of resolved IP
        // addresses or empty if no cached data is available. Returns ERR_NONE on
        // success. This method must be called on the browser process IO thread.
        ///
        /*--cef(default_retval=ERR_FAILED)--*/
        virtual CefErrorCode ResolveHostCached(Uri^ origin, [Out] IList<String^>^ %resolvedIpAddresses)
        {
            ThrowIfDisposed();

            std::vector<CefString> addresses;

            auto errorCode = _requestContext->ResolveHostCached(StringUtils::ToNative(origin->AbsoluteUri), addresses);

            resolvedIpAddresses = StringUtils::ToClr(addresses);

            return (CefErrorCode)errorCode;
        }

        operator CefRefPtr<CefRequestContext>()
        {
            if(this == nullptr)
            {
                return NULL;
            }
            return _requestContext.get();
        }
    };
}
// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_REQUESTCONTEXT_H_
#define CEFSHARP_CORE_REQUESTCONTEXT_H_

#pragma once

#include "Stdafx.h"
#include "include\cef_request_context.h"

#include "RequestContextSettings.h"
#include "Internals\CefRequestContextHandlerAdapter.h"
#include "Internals\CefWrapper.h"

using namespace System::Runtime::InteropServices;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    namespace Core
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
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class RequestContext : public IRequestContext, public CefWrapper
        {
        private:
            MCefRefPtr<CefRequestContext> _requestContext;
            RequestContextSettings^ _settings;

        internal:
            RequestContext(const CefRefPtr<CefRequestContext>& context)
            {
                _requestContext = context.get();
                _settings = nullptr;
            }

            operator CefRefPtr<CefRequestContext>()
            {
                if (this == nullptr)
                {
                    return nullptr;
                }
                return _requestContext.get();
            }

        public:
            RequestContext()
            {
                CefRequestContextSettings settings;
                _requestContext = CefRequestContext::CreateContext(settings, nullptr).get();
            }

            RequestContext(RequestContextSettings^ settings) : _settings(settings)
            {
                PathCheck::AssertAbsolute(settings->CachePath, "RequestContextSettings.CachePath");

                _requestContext = CefRequestContext::CreateContext(settings, nullptr).get();
            }

            RequestContext(IRequestContextHandler^ requestContextHandler)
            {
                CefRequestContextSettings settings;
                _requestContext = CefRequestContext::CreateContext(settings, new CefRequestContextHandlerAdapter(requestContextHandler)).get();
            }

            RequestContext(RequestContextSettings^ settings, IRequestContextHandler^ requestContextHandler) : _settings(settings)
            {
                PathCheck::AssertAbsolute(settings->CachePath, "RequestContextSettings.CachePath");

                _requestContext = CefRequestContext::CreateContext(settings, new CefRequestContextHandlerAdapter(requestContextHandler)).get();
            }

            ///Creates a new context object that shares storage with | other | and uses an optional | handler | .
            RequestContext(IRequestContext^ otherRequestContext)
            {
                _requestContext = CefRequestContext::CreateContext((RequestContext^)otherRequestContext->UnWrap(), nullptr).get();
            }

            RequestContext(IRequestContext^ otherRequestContext, IRequestContextHandler^ requestContextHandler)
            {
                _requestContext = CefRequestContext::CreateContext((RequestContext^)otherRequestContext->UnWrap(), new CefRequestContextHandlerAdapter(requestContextHandler)).get();
            }

            !RequestContext()
            {
                _requestContext = nullptr;
            }

            ~RequestContext()
            {
                this->!RequestContext();

                delete _settings;

                _disposed = true;
            }

            /// <summary>
            /// Creates a new context object that shares storage with other and uses an
            /// optional handler.
            /// </summary>
            /// <param name="other">shares storage with this RequestContext</param>
            /// <param name="requestContextHandler">optional requestContext handler</param>
            /// <returns>Returns a new RequestContext</returns>
            static IRequestContext^ CreateContext(IRequestContext^ other, IRequestContextHandler^ requestContextHandler)
            {
                auto otherRequestContext = static_cast<RequestContext^>(other->UnWrap());
                CefRefPtr<CefRequestContextHandler> handler = requestContextHandler == nullptr ? nullptr : new CefRequestContextHandlerAdapter(requestContextHandler);

                auto newContext = CefRequestContext::CreateContext(otherRequestContext, handler);
                return gcnew RequestContext(newContext);
            }

            /// <summary>
            /// Returns true if this object is pointing to the same context object.
            /// </summary>
            /// <param name="context">context to compare</param>
            /// <returns>Returns true if the same</returns>
            virtual bool IsSame(IRequestContext^ context);

            /// <summary>
            /// Returns true if this object is sharing the same storage as the specified context.
            /// </summary>
            /// <param name="context">context to compare</param>
            /// <returns>Returns true if same storage</returns>
            virtual bool IsSharingWith(IRequestContext^ context);

            /// <summary>
            /// Returns the default cookie manager for this object. This will be the global
            /// cookie manager if this object is the global request context. 
            /// </summary>
            /// <param name="callback">If callback is non-NULL it will be executed asnychronously on the CEF IO thread
            /// after the manager's storage has been initialized.</param>
            /// <returns>Returns the default cookie manager for this object</returns>
            virtual ICookieManager^ GetCookieManager(ICompletionCallback^ callback);

            /// <summary>
            /// Returns true if this object is the global context. The global context is
            /// used by default when creating a browser or URL request with a NULL context
            /// argument.
            /// </summary>
            virtual property bool IsGlobal
            {
                bool get()
                {
                    ThrowIfDisposed();
                    return _requestContext->IsGlobal();
                }
            }

            /// <summary>
            /// Register a scheme handler factory for the specified schemeName and optional domainName.
            /// An empty domainName value for a standard scheme will cause the factory to match all domain
            /// names. The domainName value will be ignored for non-standard schemes. If schemeName is
            /// a built-in scheme and no handler is returned by factory then the built-in scheme handler
            /// factory will be called. If schemeName is a custom scheme then you must also implement the
            /// IApp.OnRegisterCustomSchemes() method in all processes. This function may be called multiple
            /// times to change or remove the factory that matches the specified schemeName and optional
            /// domainName.
            /// </summary>
            /// <param name="schemeName">Scheme Name</param>
            /// <param name="domainName">Optional domain name</param>
            /// <param name="factory">Scheme handler factory</param>
            /// <returns>Returns false if an error occurs.</returns>
            virtual bool RegisterSchemeHandlerFactory(String^ schemeName, String^ domainName, ISchemeHandlerFactory^ factory);

            /// <summary>
            /// Clear all registered scheme handler factories. 
            /// </summary>
            /// <returns>Returns false on error.</returns>
            virtual bool ClearSchemeHandlerFactories();

            /// <summary>
            /// Returns the cache path for this object. If empty an "incognito mode"
            /// in-memory cache is being used.
            /// </summary>
            virtual property String^ CachePath
            {
                String^ get()
                {
                    ThrowIfDisposed();

                    return StringUtils::ToClr(_requestContext->GetCachePath());
                }
            }

            /// <summary>
            /// Returns true if a preference with the specified name exists. This method
            /// must be called on the CEF UI thread.
            /// </summary>
            /// <param name="name">name of preference</param>
            /// <returns>bool if the preference exists</returns>
            /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
            /// <see cref="IBrowserProcessHandler::OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
            /// executed on the CEF UI thread, so can be called directly.
            /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
            /// application thread will be the CEF UI thread.</remarks>
            virtual bool HasPreference(String^ name);

            /// <summary>
            /// Returns the value for the preference with the specified name. Returns
            /// NULL if the preference does not exist. The returned object contains a copy
            /// of the underlying preference value and modifications to the returned object
            /// will not modify the underlying preference value. This method must be called
            /// on the CEF UI thread.
            /// </summary>
            /// <param name="name">preference name</param>
            /// <returns>Returns the value for the preference with the specified name</returns>
            /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
            /// <see cref="IBrowserProcessHandler::OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
            /// executed on the CEF UI thread, so can be called directly.
            /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
            /// application thread will be the CEF UI thread.</remarks>
            virtual Object^ GetPreference(String^ name);

            /// <summary>
            /// Returns all preferences as a dictionary. The returned
            /// object contains a copy of the underlying preference values and
            /// modifications to the returned object will not modify the underlying
            /// preference values. This method must be called on the browser process UI
            /// thread.
            /// </summary>
            /// <param name="includeDefaults">If true then
            /// preferences currently at their default value will be included.</param>
            /// <returns>Preferences (dictionary can have sub dictionaries)</returns>
            virtual IDictionary<String^, Object^>^ GetAllPreferences(bool includeDefaults);

            /// <summary>
            /// Returns true if the preference with the specified name can be modified
            /// using SetPreference. As one example preferences set via the command-line
            /// usually cannot be modified. This method must be called on the CEF UI thread.
            /// </summary>
            /// <param name="name">preference key</param>
            /// <returns>Returns true if the preference with the specified name can be modified
            /// using SetPreference</returns>
            /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
            /// <see cref="IBrowserProcessHandler::OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
            /// executed on the CEF UI thread, so can be called directly.
            /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
            /// application thread will be the CEF UI thread.</remarks>
            virtual bool CanSetPreference(String^ name);

            /// <summary>
            /// Set the value associated with preference name. If value is null the
            /// preference will be restored to its default value. If setting the preference
            /// fails then error will be populated with a detailed description of the
            /// problem. This method must be called on the CEF UI thread.
            /// Preferences set via the command-line usually cannot be modified.
            /// </summary>
            /// <param name="name">preference key</param>
            /// <param name="value">preference value</param>
            /// <param name="error">out error</param>
            /// <returns>Returns true if the value is set successfully and false otherwise.</returns>
            /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
            /// <see cref="IBrowserProcessHandler::OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
            /// executed on the CEF UI thread, so can be called directly.
            /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
            /// application thread will be the CEF UI thread.</remarks>
            virtual bool SetPreference(String^ name, Object^ value, [Out] String^ %error);

            /// <summary>
            /// Clears all certificate exceptions that were added as part of handling
            /// <see cref="IRequestHandler::OnCertificateError"/>. If you call this it is
            /// recommended that you also call <see cref="IRequestContext::CloseAllConnections"/> or you risk not
            /// being prompted again for server certificates if you reconnect quickly.
            /// </summary>
            /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
            /// completion. This param is optional</param>
            virtual void ClearCertificateExceptions(ICompletionCallback^ callback);

            /// <summary>
            /// Clears all HTTP authentication credentials that were added as part of handling
            /// <see cref="IRequestHandler::GetAuthCredentials"/>.
            /// </summary>
            /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
            /// completion. This param is optional</param>
            virtual void ClearHttpAuthCredentials(ICompletionCallback^ callback);

            /// <summary>
            /// Clears all active and idle connections that Chromium currently has.
            /// This is only recommended if you have released all other CEF objects but
            /// don't yet want to call Cef.Shutdown().
            /// </summary>
            /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
            /// completion. This param is optional</param>
            virtual void CloseAllConnections(ICompletionCallback^ callback);

            /// <summary>
            /// Attempts to resolve origin to a list of associated IP addresses.
            /// </summary>
            /// <param name="origin">host name to resolve</param>
            /// <returns>A task that represents the Resoolve Host operation. The value of the TResult parameter contains ResolveCallbackResult.</returns>
            virtual Task<ResolveCallbackResult>^ ResolveHostAsync(Uri^ origin);

            /// <summary>
            /// Returns the current value for <paramref name="contentType"/> that applies for the
            /// specified URLs. If both URLs are empty the default value will be returned.
            /// Returns null if no value is configured.
            /// Must be called on the browser
            /// process UI thread.
            /// </summary>
            /// <param name="requestingUrl">Requesting url</param>
            /// <param name="topLevelUrl">Top level url</param>
            /// <param name="contentType">Content type</param>
            /// <returns>Returns the current value for <paramref name="contentType"/> that applies for the
            /// specified URLs.</returns>
            virtual Object^ GetWebsiteSetting(String^ requestingUrl, String^ topLevelUrl, Enums::ContentSettingTypes contentType);

            /// <summary>
            /// Sets the current value for <paramref name="contentType"/> for the specified URLs in the
            /// default scope. If both URLs are empty, and the context is not incognito,
            /// the default value will be set. Pass null for <paramref name="value"/> to remove the
            /// default value for this content type.
            ///
            /// WARNING: Incorrect usage of this method may cause instability or security
            /// issues in Chromium. Make sure that you first understand the potential
            /// impact of any changes to <paramref name="contentType"/> by reviewing the related source
            /// code in Chromium. For example, if you plan to modify
            /// <see cref="ContentSettingTypes.Popups"/>, first review and understand the usage of
            /// ContentSettingsType::POPUPS in Chromium:
            /// https://source.chromium.org/search?q=ContentSettingsType::POPUPS
            /// </summary>
            /// <param name="requestingUrl">Requesting url</param>
            /// <param name="topLevelUrl">Top level url</param>
            /// <param name="contentType">Content type</param>
            /// <param name="value">value </param>
            virtual void SetWebsiteSetting(String^ requestingUrl, String^ topLevelUrl, Enums::ContentSettingTypes contentType, Object^ value);

            /// <summary>
            /// Returns the current value for <paramref name="contentType"/> that applies for the
            /// specified URLs. If both URLs are empty the default value will be returned.
            /// Returns <see cref="ContentSettingValues.Default"/> if no value is configured. Must
            /// be called on the browser process UI thread.
            /// </summary>
            /// <param name="requestingUrl">Requesting url</param>
            /// <param name="topLevelUrl">Top level url</param>
            /// <param name="contentType">Content type</param>
            /// <returns>Returns the current value for <paramref name="contentType"/> that applies for the
            /// specified URLs.</returns>
            virtual Enums::ContentSettingValues GetContentSetting(String^ requestingUrl, String^ topLevelUrl, Enums::ContentSettingTypes contentType);

            /// <summary>
            /// Sets the current value for <paramref name="contentType"/> for the specified URLs in the
            /// default scope. If both URLs are empty, and the context is not incognito,
            /// the default value will be set. Pass <see cref="ContentSettingValues.Default"/> for
            /// <paramref name="value"/> to use the default value for this content type.
            ///
            /// WARNING: Incorrect usage of this method may cause instability or security
            /// issues in Chromium. Make sure that you first understand the potential
            /// impact of any changes to |content_type| by reviewing the related source
            /// code in Chromium. For example, if you plan to modify
            /// <see cref="ContentSettingTypes.Popups"/>, first review and understand the usage of
            /// ContentSettingsType::POPUPS in Chromium:
            /// https://source.chromium.org/search?q=ContentSettingsType::POPUPS
            /// </summary>
            /// <param name="requestingUrl">Requesting url</param>
            /// <param name="topLevelUrl">Top level url</param>
            /// <param name="contentType">Content type</param>
            /// <param name="value">value</param>
            virtual void SetContentSetting(String^ requestingUrl, String^ topLevelUrl, Enums::ContentSettingTypes contentType, Enums::ContentSettingValues value);

            /// <summary>
            /// Gets the inner most instance
            /// </summary>
            /// <returns>current instance</returns>
            virtual IRequestContext^ UnWrap();
        };
    }
}
#endif  // CEFSHARP_CORE_REQUESTCONTEXT_H_

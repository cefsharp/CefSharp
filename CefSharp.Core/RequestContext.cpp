// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "RequestContext.h"

#include "include\cef_parser.h"

//For the x86 version we define and undefine CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_
//as the /clr compliation option attempts to be helpful and convers all the __fastcall versions
//to __stdcall which already exist, so we just use the standard calling convention and ignore
//the optimised ones. The original error is
//warning C4561: '__fastcall' incompatible with the '/clr' option: converting to '__stdcall'
//(compiling source file RequestContext.cpp) 
#define CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_
#include "include\base\cef_bind.h"
#undef CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_

#include "include\wrapper\cef_closure_task.h"

#include "CookieManager.h"
#include "SchemeHandlerFactoryWrapper.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Internals\CefExtensionWrapper.h"
#include "Internals\CefExtensionHandlerAdapter.h"
#include "Internals\CefResolveCallbackAdapter.h"
#include "Internals\TypeConversion.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    /// <summary>
        /// Returns true if this object is pointing to the same context object.
        /// </summary>
        /// <param name="context">context to compare</param>
        /// <returns>Returns true if the same</returns>
    bool RequestContext::IsSame(IRequestContext^ context)
    {
        ThrowIfDisposed();

        auto requestContext = (RequestContext^)context;

        return _requestContext->IsSame(requestContext);
    }

    /// <summary>
    /// Returns true if this object is sharing the same storage as the specified context.
    /// </summary>
    /// <param name="context">context to compare</param>
    /// <returns>Returns true if same storage</returns>
    bool RequestContext::IsSharingWith(IRequestContext^ context)
    {
        ThrowIfDisposed();

        auto requestContext = (RequestContext^)context;

        return _requestContext->IsSharingWith(requestContext);
    }

    /// <summary>
    /// Returns the default cookie manager for this object. This will be the global
    /// cookie manager if this object is the global request context. Otherwise,
    /// this will be the default cookie manager used when this request context does
    /// not receive a value via IRequestContextHandler.GetCookieManager(). 
    /// </summary>
    /// <param name="callback">If callback is non-NULL it will be executed asnychronously on the CEF IO thread
    /// after the manager's storage has been initialized.</param>
    /// <returns>Returns the default cookie manager for this object</returns>
    ICookieManager^ RequestContext::GetDefaultCookieManager(ICompletionCallback^ callback)
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

    /// <summary>
    /// Register a scheme handler factory for the specified schemeName and optional domainName.
    /// An empty domainName value for a standard scheme will cause the factory to match all domain
    /// names. The domainName value will be ignored for non-standard schemes. If schemeName is
    /// a built-in scheme and no handler is returned by factory then the built-in scheme handler
    /// factory will be called. If schemeName is a custom scheme then you must also implement the
    /// CefApp::OnRegisterCustomSchemes() method in all processes. This function may be called multiple
    /// times to change or remove the factory that matches the specified schemeName and optional
    /// domainName.
    /// </summary>
    /// <param name="schemeName">Scheme Name</param>
    /// <param name="domainName">Optional domain name</param>
    /// <param name="factory">Scheme handler factory</param>
    /// <returns>Returns false if an error occurs.</returns>
    bool RequestContext::RegisterSchemeHandlerFactory(String^ schemeName, String^ domainName, ISchemeHandlerFactory^ factory)
    {
        ThrowIfDisposed();

        auto wrapper = new SchemeHandlerFactoryWrapper(factory);
        return _requestContext->RegisterSchemeHandlerFactory(StringUtils::ToNative(schemeName), StringUtils::ToNative(domainName), wrapper);
    }

    /// <summary>
    /// Clear all registered scheme handler factories. 
    /// </summary>
    /// <returns>Returns false on error.</returns>
    bool RequestContext::ClearSchemeHandlerFactories()
    {
        ThrowIfDisposed();

        return _requestContext->ClearSchemeHandlerFactories();
    }

    /// <summary>
    /// Tells all renderer processes associated with this context to throw away
    /// their plugin list cache. If reloadPages is true they will also reload
    /// all pages with plugins. RequestContextHandler.OnBeforePluginLoad may
    /// be called to rebuild the plugin list cache.
    /// </summary>
    /// <param name="reloadPages">reload any pages with pluginst</param>
    void RequestContext::PurgePluginListCache(bool reloadPages)
    {
        ThrowIfDisposed();

        _requestContext->PurgePluginListCache(reloadPages);
    }

    /// <summary>
    /// Returns true if a preference with the specified name exists. This method
    /// must be called on the CEF UI thread.
    /// </summary>
    /// <param name="name">name of preference</param>
    /// <returns>bool if the preference exists</returns>
    /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
    /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
    /// executed on the CEF UI thread, so can be called directly.
    /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
    /// application thread will be the CEF UI thread.</remarks>
    bool RequestContext::HasPreference(String^ name)
    {
        ThrowIfDisposed();

        return _requestContext->HasPreference(StringUtils::ToNative(name));
    }

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
    /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
    /// executed on the CEF UI thread, so can be called directly.
    /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
    /// application thread will be the CEF UI thread.</remarks>
    Object^ RequestContext::GetPreference(String^ name)
    {
        ThrowIfDisposed();

        return TypeConversion::FromNative(_requestContext->GetPreference(StringUtils::ToNative(name)));
    }

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
    IDictionary<String^, Object^>^ RequestContext::GetAllPreferences(bool includeDefaults)
    {
        ThrowIfDisposed();

        auto preferences = _requestContext->GetAllPreferences(includeDefaults);

        return TypeConversion::FromNative(preferences);
    }

    /// <summary>
    /// Returns true if the preference with the specified name can be modified
    /// using SetPreference. As one example preferences set via the command-line
    /// usually cannot be modified. This method must be called on the CEF UI thread.
    /// </summary>
    /// <param name="name">preference key</param>
    /// <returns>Returns true if the preference with the specified name can be modified
    /// using SetPreference</returns>
    /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
    /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
    /// executed on the CEF UI thread, so can be called directly.
    /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
    /// application thread will be the CEF UI thread.</remarks>
    bool RequestContext::CanSetPreference(String^ name)
    {
        ThrowIfDisposed();

        return _requestContext->CanSetPreference(StringUtils::ToNative(name));
    }

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
    /// /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
    /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
    /// executed on the CEF UI thread, so can be called directly.
    /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
    /// application thread will be the CEF UI thread.</remarks>
    bool RequestContext::SetPreference(String^ name, Object^ value, [Out] String^ %error)
    {
        ThrowIfDisposed();

        CefString cefError;

        auto success = _requestContext->SetPreference(StringUtils::ToNative(name), TypeConversion::ToNative(value), cefError);

        error = StringUtils::ToClr(cefError);

        return success;
    }

    /// <summary>
    /// Clears all certificate exceptions that were added as part of handling
    /// <see cref="IRequestHandler.OnCertificateError"/>. If you call this it is
    /// recommended that you also call <see cref="IRequestContext.CloseAllConnections"/> or you risk not
    /// being prompted again for server certificates if you reconnect quickly.
    /// </summary>
    /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
    /// completion. This param is optional</param>
    void RequestContext::ClearCertificateExceptions(ICompletionCallback^ callback)
    {
        ThrowIfDisposed();

        CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

        _requestContext->ClearCertificateExceptions(wrapper);
    }

    /// <summary>
    /// Clears all active and idle connections that Chromium currently has.
    /// This is only recommended if you have released all other CEF objects but
    /// don't yet want to call Cef.Shutdown().
    /// </summary>
    /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
    /// completion. This param is optional</param>
    void RequestContext::CloseAllConnections(ICompletionCallback^ callback)
    {
        ThrowIfDisposed();

        CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

        _requestContext->CloseAllConnections(wrapper);
    }

    /// <summary>
    /// Attempts to resolve origin to a list of associated IP addresses.
    /// </summary>
    /// <param name="origin">host name to resolve</param>
    /// <returns>A task that represents the Resoolve Host operation. The value of the TResult parameter contains ResolveCallbackResult.</returns>
    Task<ResolveCallbackResult>^ RequestContext::ResolveHostAsync(Uri^ origin)
    {
        ThrowIfDisposed();

        auto callback = gcnew TaskResolveCallback();

        CefRefPtr<CefResolveCallback> callbackWrapper = new CefResolveCallbackAdapter(callback);

        _requestContext->ResolveHost(StringUtils::ToNative(origin->AbsoluteUri), callbackWrapper);

        return callback->Task;
    }

    /// <summary>
    /// Attempts to resolve origin to a list of associated IP addresses using
    /// cached data. This method must be called on the CEF IO thread. Use
    /// Cef.IOThreadTaskFactory to execute on that thread.
    /// </summary>
    /// <param name="origin">host name to resolve</param>
    /// <param name="resolvedIpAddresses">list of resolved IP
    /// addresses or empty list if no cached data is available.</param>
    /// <returns> Returns <see cref="CefErrorCode.None"/> on success</returns>
    CefErrorCode RequestContext::ResolveHostCached(Uri^ origin, [Out] IList<String^>^ %resolvedIpAddresses)
    {
        ThrowIfDisposed();

        std::vector<CefString> addresses;

        auto errorCode = _requestContext->ResolveHostCached(StringUtils::ToNative(origin->AbsoluteUri), addresses);

        resolvedIpAddresses = StringUtils::ToClr(addresses);

        return (CefErrorCode)errorCode;
    }

    /// <summary>
    /// Returns true if this context was used to load the extension identified by extensionId. Other contexts sharing the same storage will also have access to the extension (see HasExtension).
    /// This method must be called on the CEF UI thread.
    /// </summary>
    /// <returns>Returns true if this context was used to load the extension identified by extensionId</returns>
    bool RequestContext::DidLoadExtension(String^ extensionId)
    {
        ThrowIfDisposed();

        return _requestContext->DidLoadExtension(StringUtils::ToNative(extensionId));
    }

    /// <summary>
    /// Returns the extension matching extensionId or null if no matching extension is accessible in this context (see HasExtension).
    /// This method must be called on the CEF UI thread.
    /// </summary>
    /// <param name="extensionId">extension Id</param>
    /// <returns>Returns the extension matching extensionId or null if no matching extension is accessible in this context</returns>
    IExtension^ RequestContext::GetExtension(String^ extensionId)
    {
        ThrowIfDisposed();

        auto extension = _requestContext->GetExtension(StringUtils::ToNative(extensionId));

        if (extension.get())
        {
            return gcnew CefExtensionWrapper(extension);
        }

        return nullptr;
    }

    /// <summary>
    /// Retrieve the list of all extensions that this context has access to (see HasExtension).
    /// <see cref="extensionIds"/> will be populated with the list of extension ID values.
    /// This method must be called on the CEF UI thread.
    /// </summary>
    /// <param name="extensionIds">output a list of extensions Ids</param>
    /// <returns>returns true on success otherwise false</returns>
    bool RequestContext::GetExtensions([Out] IList<String^>^ %extensionIds)
    {
        ThrowIfDisposed();

        std::vector<CefString> extensions;

        auto success = _requestContext->GetExtensions(extensions);

        extensionIds = StringUtils::ToClr(extensions);

        return success;
    }

    /// <summary>
    /// Returns true if this context has access to the extension identified by extensionId.
    /// This may not be the context that was used to load the extension (see DidLoadExtension).
    /// This method must be called on the CEF UI thread.
    /// </summary>
    /// <param name="extensionId">extension id</param>
    /// <returns>Returns true if this context has access to the extension identified by extensionId</returns>
    bool RequestContext::HasExtension(String^ extensionId)
    {
        ThrowIfDisposed();

        return _requestContext->HasExtension(StringUtils::ToNative(extensionId));
    }

    /// <summary>
    /// Load an extension. If extension resources will be read from disk using the default load implementation then rootDirectoy
    /// should be the absolute path to the extension resources directory and manifestJson should be null.
    /// If extension resources will be provided by the client (e.g. via IRequestHandler and/or IExtensionHandler) then rootDirectory
    /// should be a path component unique to the extension (if not absolute this will be internally prefixed with the PK_DIR_RESOURCES path)
    /// and manifestJson should contain the contents that would otherwise be read from the "manifest.json" file on disk.
    /// The loaded extension will be accessible in all contexts sharing the same storage (HasExtension returns true).
    /// However, only the context on which this method was called is considered the loader (DidLoadExtension returns true) and only the
    /// loader will receive IRequestContextHandler callbacks for the extension. <see cref="IExtensionHandler.OnExtensionLoaded"/> will be
    /// called on load success or <see cref="IExtensionHandler.OnExtensionLoadFailed"/> will be called on load failure.
    /// If the extension specifies a background script via the "background" manifest key then <see cref="IExtensionHandler.OnBeforeBackgroundBrowser"/>
    /// will be called to create the background browser. See that method for additional information about background scripts.
    /// For visible extension views the client application should evaluate the manifest to determine the correct extension URL to load and then pass
    /// that URL to the IBrowserHost.CreateBrowser* function after the extension has loaded. For example, the client can look for the "browser_action"
    /// manifest key as documented at https://developer.chrome.com/extensions/browserAction. Extension URLs take the form "chrome-extension:///".
    /// Browsers that host extensions differ from normal browsers as follows: - Can access chrome.* JavaScript APIs if allowed by the manifest.
    /// Visit chrome://extensions-support for the list of extension APIs currently supported by CEF. - Main frame navigation to non-extension
    /// content is blocked.
    /// - Pinch-zooming is disabled.
    /// - <see cref="IBrowserHost.GetExtension"/> returns the hosted extension.
    /// - CefBrowserHost::IsBackgroundHost returns true for background hosts. See https://developer.chrome.com/extensions for extension implementation and usage documentation.
    /// </summary>
    /// <param name="rootDirectory">If extension resources will be read from disk using the default load implementation then rootDirectoy
    /// should be the absolute path to the extension resources directory and manifestJson should be null</param>
    /// <param name="manifestJson">If extension resources will be provided by the client then rootDirectory should be a path component unique to the extension
    /// and manifestJson should contain the contents that would otherwise be read from the manifest.json file on disk</param>
    /// <param name="handler">handle events related to browser extensions</param>
    void RequestContext::LoadExtension(String^ rootDirectory, String^ manifestJson, IExtensionHandler^ handler)
    {
        ThrowIfDisposed();

        CefRefPtr<CefDictionaryValue> manifest;

        if (!String::IsNullOrEmpty(manifestJson))
        {
            cef_json_parser_error_t errorCode;
            CefString errorMessage;
            auto value = CefParseJSONAndReturnError(StringUtils::ToNative(manifestJson),
                cef_json_parser_options_t::JSON_PARSER_ALLOW_TRAILING_COMMAS,
                errorCode,
                errorMessage);

            if (errorCode == cef_json_parser_error_t::JSON_NO_ERROR)
            {
                manifest = value->GetDictionary();
            }
            else
            {
                throw gcnew Exception("Unable to parse JSON ErrorCode:" + Convert::ToString((int)errorCode) + "; ErrorMessage:" + StringUtils::ToClr(errorMessage));
            }
        }

        CefRefPtr<CefExtensionHandler> extensionHandler = handler == nullptr ? NULL : new CefExtensionHandlerAdapter(handler);

        if (CefCurrentlyOn(CefThreadId::TID_UI))
        {
            _requestContext->LoadExtension(StringUtils::ToNative(rootDirectory), manifest, extensionHandler);
        }
        else
        {
            CefPostTask(TID_UI, base::Bind(&CefRequestContext::LoadExtension, _requestContext.get(), StringUtils::ToNative(rootDirectory), manifest, extensionHandler));
        }
    }
}

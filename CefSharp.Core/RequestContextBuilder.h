// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "RequestContextSettings.h"

using namespace CefSharp::Handler;

namespace CefSharp
{
    /// <summary>
    /// Fluent style builder for creating IRequestContext instances.
    /// </summary>
    public ref class RequestContextBuilder
    {
    private:
        RequestContextSettings^ _settings;
        IRequestContext^ _otherContext;
        RequestContextHandler^ _handler;

        void ThrowExceptionIfContextAlreadySet()
        {
            if (_otherContext != nullptr)
            {
                throw gcnew Exception("A call to WithSharedSettings has already been made, it is no possible to provide custom settings.");
            }
        }

        void ThrowExceptionIfCustomSettingSpecified()
        {
            if (_settings != nullptr)
            {
                throw gcnew Exception("A call to WithCachePath/PersistUserPreferences has already been made, it's not possible to share settings with another RequestContext.");
            }
        }

    public:
        /// <summary>
        /// Create the actual RequestContext instance
        /// </summary>
        /// <returns>Returns a new RequestContext instance.</returns>
        IRequestContext^ Create();

        /// <summary>
        /// Action is called in IRequestContextHandler.OnRequestContextInitialized
        /// </summary>
        /// <param name="action">called when the context has been initialized.</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ OnInitialize(Action<IRequestContext^>^ action);

        /// <summary>
        /// Sets the Cache Path
        /// </summary>
        /// <param name="cachePath">
        /// The location where cache data for this request context will be stored on
        /// disk. If this value is non-empty then it must be an absolute path that is
        /// either equal to or a child directory of CefSettings.RootCachePath.
        /// If the value is empty then browsers will be created in "incognito mode"
        /// where in-memory caches are used for storage and no data is persisted to disk.
        /// HTML5 databases such as localStorage will only persist across sessions if a
        /// cache path is specified. To share the global browser cache and related
        /// configuration set this value to match the CefSettings.CachePath value.
        /// </param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithCachePath(String^ cachePath);

        /// <summary>
        /// Invoke this method tp persist user preferences as a JSON file in the cache path directory.
        /// Can be set globally using the CefSettings.PersistUserPreferences value.
        /// This value will be ignored if CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ PersistUserPreferences();

        /// <summary>
        /// Set the value associated with preference name when the RequestContext
        /// is initialzied. If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Preferences set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="name">preference key</param>
        /// <param name="value">preference value</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithPreference(String^ name, Object^ value);

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithProxyServer(String^ host);

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port (optional)</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithProxyServer(String^ host, Nullable<int> port);

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="scheme">proxy scheme</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port (optional)</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithProxyServer(String^ scheme, String^ host, Nullable<int> port);

        /// <summary>
        /// Shares storage with other RequestContext
        /// </summary>
        /// <param name="other">shares storage with this RequestContext</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        RequestContextBuilder^ WithSharedSettings(IRequestContext^ other);
    };
}


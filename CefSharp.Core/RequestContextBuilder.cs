// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using CefSharp.Handler;
using System;

namespace CefSharp
{
    /// <summary>
    /// Fluent style builder for creating IRequestContext instances.
    /// </summary>
    public class RequestContextBuilder
    {
        private RequestContextSettings _settings;
        private IRequestContext _otherContext;
        private RequestContextHandler _handler;

        void ThrowExceptionIfContextAlreadySet()
        {
            if (_otherContext != null)
            {
                throw new Exception("A call to WithSharedSettings has already been made, it is no possible to provide custom settings.");
            }
        }

        void ThrowExceptionIfCustomSettingSpecified()
        {
            if (_settings != null)
            {
                throw new Exception("A call to WithCachePath/PersistUserPreferences has already been made, it's not possible to share settings with another RequestContext.");
            }
        }
        /// <summary>
        /// Create the actual RequestContext instance
        /// </summary>
        /// <returns>Returns a new RequestContext instance.</returns>
        public IRequestContext Create()
        {
            if (_otherContext != null)
            {
                return new CefSharp.Core.RequestContext(_otherContext, _handler);
            }

            if (_settings != null)
            {
                return new CefSharp.Core.RequestContext(_settings.settings, _handler);
            }

            return new CefSharp.Core.RequestContext(_handler);
        }

        /// <summary>
        /// Action is called in IRequestContextHandler.OnRequestContextInitialized
        /// </summary>
        /// <param name="action">called when the context has been initialized.</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder OnInitialize(Action<IRequestContext> action)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.OnInitialize(action);

            return this;
        }

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
        public RequestContextBuilder WithCachePath(string cachePath)
        {
            ThrowExceptionIfContextAlreadySet();

            if (_settings == null)
            {
                _settings = new RequestContextSettings();
            }

            _settings.CachePath = cachePath;

            return this;
        }

        /// <summary>
        /// Invoke this method tp persist user preferences as a JSON file in the cache path directory.
        /// Can be set globally using the CefSettings.PersistUserPreferences value.
        /// This value will be ignored if CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder PersistUserPreferences()
        {
            ThrowExceptionIfContextAlreadySet();

            if (_settings == null)
            {
                _settings = new RequestContextSettings();
            }

            _settings.PersistUserPreferences = true;

            return this;
        }

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
        public RequestContextBuilder WithPreference(string name, object value)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetPreferenceOnContextInitialized(name, value);

            return this;
        }

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithProxyServer(string host)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(host, null);

            return this;
        }

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
        public RequestContextBuilder WithProxyServer(string host, int? port)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(host, port);

            return this;
        }

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
        public RequestContextBuilder WithProxyServer(string scheme, string host, int? port)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(scheme, host, port);

            return this;
        }

        /// <summary>
        /// Shares storage with other RequestContext
        /// </summary>
        /// <param name="other">shares storage with this RequestContext</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithSharedSettings(IRequestContext other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ThrowExceptionIfCustomSettingSpecified();

            _otherContext = other;

            return this;
        }
    }
}

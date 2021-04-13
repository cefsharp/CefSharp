// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Handler
{
    /// <summary>
    /// Implement this interface to provide handler implementations. The handler
    /// instance will not be released until all objects related to the context have
    /// been destroyed. Implement this interface to cancel loading of specific plugins
    /// </summary>
    public class RequestContextHandler : IRequestContextHandler
    {
        private readonly IList<KeyValuePair<string, object>> preferences = new List<KeyValuePair<string, object>>();
        private bool requestContextInitialized = false;
        private Action<IRequestContext> onContextInitialziedAction;

        /// <summary>
        /// The <see cref="Action{IRequestContext}"/> is executed when the RequestContext has been initialized, after the
        /// preferences/proxy preferences have been set, before OnRequestContextInitialized.
        /// </summary>
        /// <param name="onContextInitialziedAction">action to perform on context initialize</param>
        /// <returns>A <see cref="RequestContextHandler"/> instance allowing you to chain multiple AddPreference calls together </returns>
        /// <remarks>Only a single action reference is maintained, multiple calls will result in the
        /// previous action reference being overriden.</remarks>
        public RequestContextHandler OnInitialize(Action<IRequestContext> onContextInitialziedAction)
        {
            this.onContextInitialziedAction = onContextInitialziedAction;

            return this;
        }

        /// <summary>
        /// Sets the preferences when the <see cref="IRequestContextHandler.OnRequestContextInitialized(IRequestContext)"/>
        /// method is called. If <paramref name="value"/> is null the  preference will be restored
        /// to its default value. Preferences set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="name">preference name</param>
        /// <param name="value">preference value</param>
        /// <returns>A <see cref="RequestContextHandler"/> instance allowing you to chain multiple AddPreference calls together </returns>
        public RequestContextHandler SetPreferenceOnContextInitialized(string name, object value)
        {
            if (requestContextInitialized)
            {
                throw new System.Exception("RequestContext has already been initialized, ");
            }

            preferences.Add(new KeyValuePair<string, object>(name, value));

            return this;
        }

        /// <summary>
        /// Sets the proxy preferences when the <see cref="IRequestContextHandler.OnRequestContextInitialized(IRequestContext)"/>
        /// method is called. Proxy set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port</param>
        /// <returns>A <see cref="RequestContextHandler"/> instance allowing you to chain multiple AddPreference calls together </returns>
        public RequestContextHandler SetProxyOnContextInitialized(string host, int? port = null)
        {
            return SetProxyOnContextInitialized(null, host, port);
        }

        /// <summary>
        /// Sets the proxy preferences when the <see cref="IRequestContextHandler.OnRequestContextInitialized(IRequestContext)"/>
        /// method is called. Proxy set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="scheme">is the protocol of the proxy server, and is one of: 'http', 'socks', 'socks4', 'socks5'. Also note that 'socks' is equivalent to 'socks5'.</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port</param>
        /// <returns>A <see cref="RequestContextHandler"/> instance allowing you to chain multiple AddPreference calls together </returns>
        public RequestContextHandler SetProxyOnContextInitialized(string scheme, string host, int? port)
        {
            if (requestContextInitialized)
            {
                throw new System.Exception("RequestContext has already been initialized, ");
            }

            var value = RequestContextExtensions.GetProxyDictionary(scheme, host, port);

            preferences.Add(new KeyValuePair<string, object>("proxy", value));

            return this;
        }

        IResourceRequestHandler IRequestContextHandler.GetResourceRequestHandler(IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return GetResourceRequestHandler(browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource request is initiated.
        /// This method will not be called if the client associated with <paramref name="browser"/> returns a non-NULL value
        /// from <see cref="IRequestHandler.GetResourceRequestHandler"/> for the same request (identified by <see cref="IRequest.Identifier"/>).
        /// </summary>
        /// <param name="browser">represent the source browser of the request, and may be null for requests originating from service workers.</param>
        /// <param name="frame">represent the source frame of the request, and may be null for requests originating from service workers.</param>
        /// <param name="request">represents the request contents and cannot be modified in this callback</param>
        /// <param name="isNavigation">will be true if the resource request is a navigation</param>
        /// <param name="isDownload">will be true if the resource request is a download</param>
        /// <param name="requestInitiator">is the origin (scheme + domain) of the page that initiated the request</param>
        /// <param name="disableDefaultHandling">Set to true to disable default handling of the request, in which case it will need to be handled via <see cref="IResourceRequestHandler.GetResourceHandler(IWebBrowser, IBrowser, IFrame, IRequest)"/> or it will be canceled</param>
        /// <returns>To allow the resource load to proceed with default handling return null. To specify a handler for the resource return a <see cref="IResourceRequestHandler"/> object.</returns>
        protected virtual IResourceRequestHandler GetResourceRequestHandler(IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
        }

        bool IRequestContextHandler.OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy)
        {
            return OnBeforePluginLoad(mimeType, url, isMainFrame, topOriginUrl, pluginInfo, ref pluginPolicy);
        }

        /// <summary>
        /// Called on the CEF IO thread before a plugin instance is loaded.
        /// The default plugin policy can be set at runtime using the `--plugin-policy=[allow|detect|block]` command-line flag.
        /// </summary>
        /// <param name="mimeType">is the mime type of the plugin that will be loaded</param>
        /// <param name="url">is the content URL that the plugin will load and may be empty</param>
        /// <param name="isMainFrame">will be true if the plugin is being loaded in the main (top-level) frame</param>
        /// <param name="topOriginUrl">is the URL for the top-level frame that contains the plugin</param>
        /// <param name="pluginInfo">includes additional information about the plugin that will be loaded</param>
        /// <param name="pluginPolicy">Modify and return true to change the policy.</param>
        /// <returns>Return false to use the recommended policy. Modify and return true to change the policy.</returns>
        protected virtual bool OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy)
        {
            return false;
        }

        void IRequestContextHandler.OnRequestContextInitialized(IRequestContext requestContext)
        {
            requestContextInitialized = true;
            string errorMessage;

            foreach (var pref in preferences)
            {
                if (!requestContext.SetPreference(pref.Key, pref.Value, out errorMessage))
                {
                    //TODO: Do something if there's an error
                }
            }

            onContextInitialziedAction?.Invoke(requestContext);

            OnRequestContextInitialized(requestContext);
        }

        /// <summary>
        /// Called immediately after the request context has been initialized.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread.
        /// </summary>
        /// <param name="requestContext">the request context</param>
        protected virtual void OnRequestContextInitialized(IRequestContext requestContext)
        {

        }
    }
}

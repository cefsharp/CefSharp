// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Enums;

namespace CefSharp
{
    /// <inheritdoc/>
    public class RequestContext : IRequestContext
    {
        private CefSharp.Core.RequestContext requestContext;

        /// <inheritdoc/>
        public RequestContext()
        {
            requestContext = new CefSharp.Core.RequestContext();
        }

        /// <inheritdoc/>
        public RequestContext(IRequestContext otherRequestContext)
        {
            if (otherRequestContext == null)
            {
                throw new ArgumentNullException(nameof(otherRequestContext));
            }
            requestContext = new CefSharp.Core.RequestContext(otherRequestContext);
        }

        /// <inheritdoc/>
        public RequestContext(IRequestContext otherRequestContext, IRequestContextHandler requestContextHandler)
        {
            if (otherRequestContext == null)
            {
                throw new ArgumentNullException(nameof(otherRequestContext));
            }

            if (requestContextHandler == null)
            {
                throw new ArgumentNullException(nameof(requestContextHandler));
            }

            requestContext = new CefSharp.Core.RequestContext(otherRequestContext, requestContextHandler);
        }

        /// <inheritdoc/>
        public RequestContext(IRequestContextHandler requestContextHandler)
        {
            if (requestContextHandler == null)
            {
                throw new ArgumentNullException(nameof(requestContextHandler));
            }
            requestContext = new CefSharp.Core.RequestContext(requestContextHandler);
        }

        /// <inheritdoc/>
        public RequestContext(RequestContextSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            requestContext = new CefSharp.Core.RequestContext(settings.settings);
        }

        /// <inheritdoc/>
        public RequestContext(RequestContextSettings settings, IRequestContextHandler requestContextHandler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (requestContextHandler == null)
            {
                throw new ArgumentNullException(nameof(requestContextHandler));
            }

            requestContext = new CefSharp.Core.RequestContext(settings.settings, requestContextHandler);
        }

        /// <summary>
        /// Creates a new RequestContextBuilder which can be used to fluently set
        /// preferences
        /// </summary>
        /// <returns>Returns a new RequestContextBuilder</returns>
        public static RequestContextBuilder Configure()
        {
            var builder = new RequestContextBuilder();

            return builder;
        }

        /// <inheritdoc/>
        public bool IsGlobal
        {
            get { return requestContext.IsGlobal; }
        }

        /// <inheritdoc/>
        public string CachePath
        {
            get { return requestContext.CachePath; }
        }

        /// <inheritdoc/>
        public bool IsDisposed
        {
            get { return requestContext.IsDisposed; }
        }

        /// <inheritdoc/>
        public bool IsSame(IRequestContext context)
        {
            return requestContext.IsSame(context);
        }

        /// <inheritdoc/>
        public bool IsSharingWith(IRequestContext context)
        {
            return requestContext.IsSharingWith(context);
        }

        /// <inheritdoc/>
        public ICookieManager GetCookieManager(ICompletionCallback callback)
        {
            return requestContext.GetCookieManager(callback);
        }

        /// <inheritdoc/>
        public bool RegisterSchemeHandlerFactory(string schemeName, string domainName, ISchemeHandlerFactory factory)
        {
            return requestContext.RegisterSchemeHandlerFactory(schemeName, domainName, factory);
        }

        /// <inheritdoc/>
        public bool ClearSchemeHandlerFactories()
        {
            return requestContext.ClearSchemeHandlerFactories();
        }

        /// <inheritdoc/>
        public bool HasPreference(string name)
        {
            return requestContext.HasPreference(name);
        }

        /// <inheritdoc/>
        public object GetPreference(string name)
        {
            return requestContext.GetPreference(name);
        }

        /// <inheritdoc/>
        public IDictionary<string, object> GetAllPreferences(bool includeDefaults)
        {
            return requestContext.GetAllPreferences(includeDefaults);
        }

        /// <inheritdoc/>
        public bool CanSetPreference(string name)
        {
            return requestContext.CanSetPreference(name);
        }

        /// <inheritdoc/>
        public bool SetPreference(string name, object value, out string error)
        {
            return requestContext.SetPreference(name, value, out error);
        }

        /// <inheritdoc/>
        public void ClearCertificateExceptions(ICompletionCallback callback)
        {
            requestContext.ClearCertificateExceptions(callback);
        }

        /// <inheritdoc/>
        public void ClearHttpAuthCredentials(ICompletionCallback callback = null)
        {
            requestContext.ClearHttpAuthCredentials(callback);
        }

        /// <inheritdoc/>
        public void CloseAllConnections(ICompletionCallback callback)
        {
            requestContext.CloseAllConnections(callback);
        }

        /// <inheritdoc/>
        public Task<ResolveCallbackResult> ResolveHostAsync(Uri origin)
        {
            return requestContext.ResolveHostAsync(origin);
        }

        public object GetWebsiteSetting(string requestingUrl, string topLevelUrl, ContentSettingTypes contentType)
        {
            return requestContext.GetWebsiteSetting(requestingUrl, topLevelUrl, contentType);
        }

        public void SetWebsiteSetting(string requestingUrl, string topLevelUrl, ContentSettingTypes contentType, object value)
        {
            requestContext.SetWebsiteSetting(requestingUrl, topLevelUrl, contentType, value);
        }

        public ContentSettingValues GetContentSetting(string requestingUrl, string topLevelUrl, ContentSettingTypes contentType)
        {
            return requestContext.GetContentSetting(requestingUrl, topLevelUrl, contentType);
        }

        public void SetContentSetting(string requestingUrl, string topLevelUrl, ContentSettingTypes contentType, ContentSettingValues value)
        {
            requestContext.SetContentSetting(requestingUrl, topLevelUrl, contentType, value);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            requestContext.Dispose();
        }

        /// <summary>
        /// Used internally to get the underlying <see cref="IRequestContext"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IRequestContext UnWrap()
        {
            return requestContext;
        }
    }
}

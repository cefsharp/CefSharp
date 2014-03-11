﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CefSharp.Internals;
using System.IO;

namespace CefSharp
{
    public abstract class CefManagedBase : ObjectBase
    {
        public bool IsInitialized { get; private set; }
        public IDictionary<string, object> BoundObjects { get; private set; }

        public TaskFactory IOTaskFactory { get; protected set; }

        /// <summary>Gets a value that indicates the version of CefSharp currently being used.</summary>
        /// <value>The CefSharp version.</value>
        public static Version CefSharpVersion { get { return typeof(CefManagedBase).Assembly.GetName().Version; } }
        
        public CefManagedBase()
        {
            BoundObjects = new Dictionary<string, object>();
        }

        protected override void DoDispose(bool isDisposing)
        {
            AppDomain.CurrentDomain.ProcessExit -= ParentProcessExitHandler;
            IsInitialized = false;

            base.DoDispose(isDisposing);
        }
                        
        /// <summary>Initializes CefSharp with user-provided settings.</summary>
        ///<param name="cefSettings">CefSharp configuration settings.</param>
        /// <return>true if successful; otherwise, false.</return>
        public bool Initialize(CefSettingsBase cefSettings)
        {
            bool success = false;

            // TODO: is it really sensible to completely skip initialization if we get called multiple times, but with
            // (potentially) different settings...? :)
            if (!IsInitialized)
            {
                IsInitialized = success = DoInitialize(cefSettings);

                if (success)
                {
                    AppDomain.CurrentDomain.ProcessExit += ParentProcessExitHandler;
                }
            }

            return success;
        }

        protected abstract bool DoInitialize(CefSettingsBase cefSettings);

        /// <summary>Binds a C# class to a JavaScript object.</summary>
        /// <param name="name">The name for the new object in the JavaScript engine (e.g. 'foo' for an object accessible as 'foo' or 'window.foo').</param>
        /// <param name="objectToBind">The .NET object to bind.</param>
        /// <return>Always returns true.</return>
        public bool RegisterJsObject(string name, object objectToBind)
        {
            BoundObjects[name] = objectToBind;
            return true;
        }

        private void ParentProcessExitHandler(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>Visits all cookies using the provided Cookie Visitor. The returned cookies are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
        public abstract bool VisitAllCookies(ICookieVisitor visitor);

        /// <summary>Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
        /// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
        /// are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
        public abstract bool VisitUrlCookies(string url, bool includeHttpOnly, ICookieVisitor visitor);

        /// <summary>Sets a CefSharp Cookie. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="name">The cookie name</param>
        /// <param name="value">The cookie value</param>
        /// <param name="domain">The cookie domain</param>
        /// <param name="path">The cookie path</param>
        /// <param name="secure">A flag that determines whether the cookie will be marked as "secure" (i.e. its scope is limited to secure channels, typically HTTPS).</param>
        /// <param name="httponly">A flag that determines whether the cookie will be marked as HTTP Only (i.e. the cookie is inaccessible to client-side scripts).</param>
        /// <param name="has_expires">A flag that determines whether the cookie has an expiration date. Must be set to true when the "expires" parameter is provided.</param>
        /// <param name="expires">The DateTime when the cookie will be treated as expired. Will only be taken into account if the "has_expires" is set to true.</param>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
        public bool SetCookie(string url, string name, string value, string domain, string path, bool secure, bool httponly, bool has_expires, DateTime expires)
        {
            using (var task = IOTaskFactory.StartNew( () => DoSetCookie( url, name, value, domain, path, secure, httponly, has_expires, expires ) ) )
            {
                return task.Result;
            }
        }

        protected abstract bool DoSetCookie(string url, string name, string value, string domain, string path, bool secure, bool httponly, bool has_expires, DateTime expires );

        /// <summary>Sets a cookie using mostly default parameters. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="name">The cookie name.</param>
        /// <param name="value">The cookie value.</param>
        /// <param name="domain">The cookie domain.</param>
        /// <param name="expires">The DateTime when the cookie will be treated as expired.</param>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
        public bool SetCookie(string url, string domain, string name, string value, DateTime expires)
        {
            return SetCookie(url, name, value, domain, "/", false, false, true, expires);
        }

        /// <summary>Deletes all cookies that matches all the provided parameters. If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.</summary>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
        /// <return>false if a non-empty invalid URL is specified, or if cookies cannot be accessed; otherwise, true.</return>
        public bool DeleteCookies(string url, string name)
        {
            using (var task = IOTaskFactory.StartNew( () => DoDeleteCookies(url, name) ) )
            {
                return task.Result;
            }
        }


        protected abstract bool DoDeleteCookies(string url, string name);

        /// <summary> Sets the directory path that will be used for storing cookie data. If <paramref name="path"/> is empty data will be stored in 
        /// memory only. Otherwise, data will be stored at the specified path. To persist session cookies (cookies without an expiry 
        /// date or validity interval) set <paramref name="persist_session_cookies"/> to true. Session cookies are generally intended to be transient and 
        /// most Web browsers do not persist them.</summary>
        /// <param name="path">The file path to write cookies to.</param>
        /// <param name="persistSessionCookies">A flag that determines whether session cookies will be persisted or not.</param>
        /// <return> false if a non-empty invalid URL is specified, or if the CookieManager is not available; otherwise, true.</return>
        public bool SetCookiePath(string path, bool persistSessionCookies)
        {
            return DoSetCookiePath(path, persistSessionCookies);
        }
        
        protected abstract bool DoSetCookiePath(string path, bool persistSessionCookies);        
    }
}

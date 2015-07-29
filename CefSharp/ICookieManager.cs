// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Used for managing cookies. The methods may be called on any thread unless otherwise indicated.
    /// </summary>
    public interface ICookieManager
    {
        /// <summary>Deletes all cookies that matches all the provided parameters. If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.</summary>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
        /// <return>false if a non-empty invalid URL is specified, or if cookies cannot be accessed; otherwise, true.</return>
        bool DeleteCookies(string url, string name);

        /// <summary>Sets a cookie using mostly default parameters. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="cookie">The cookie</param>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
        bool SetCookie(string url, Cookie cookie);

        /// <summary> Sets the directory path that will be used for storing cookie data. If <paramref name="path"/> is empty data will be stored in 
        /// memory only. Otherwise, data will be stored at the specified path. To persist session cookies (cookies without an expiry 
        /// date or validity interval) set <paramref name="persistSessionCookies"/> to true. Session cookies are generally intended to be transient and 
        /// most Web browsers do not persist them.</summary>
        /// <param name="path">The file path to write cookies to.</param>
        /// <param name="persistSessionCookies">A flag that determines whether session cookies will be persisted or not.</param>
        /// <return> false if a non-empty invalid URL is specified; otherwise, true.</return>
        bool SetStoragePath(string path, bool persistSessionCookies);

        /// <summary>
        /// Set the schemes supported by this manager. By default only "http" and "https" schemes are supported. Must be called before any cookies are accessed.
        /// </summary>
        /// <param name="schemes">The list of supported schemes.</param>
        void SetSupportedSchemes(params string[] schemes);

        /// <summary>Visits all cookies using the provided Cookie Visitor. The returned cookies are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if cookies cannot be accessed; otherwise, true.</return>
        bool VisitAllCookies(ICookieVisitor visitor);

        /// <summary>Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
        /// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
        /// are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if cookies cannot be accessed; otherwise, true.</return>
        bool VisitUrlCookies(string url, bool includeHttpOnly, ICookieVisitor visitor);

        /// <summary> Flush the backing store (if any) to disk and execute the specified |handler| on the IO thread when done. Returns </summary>
        /// <param name="handler">A user-provided ICompletionHandler implementation.</param>
        /// <return>Returns false if cookies cannot be accessed.</return>
        bool FlushStore(ICompletionHandler handler);
    }
}

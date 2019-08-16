// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Used for managing cookies. The methods may be called on any thread unless otherwise indicated.
    /// </summary>
    public interface ICookieManager : IDisposable
    {
        /// <summary>
        /// Delete all cookies that match the specified parameters.
        /// If both <paramref name="url"/> and <paramref name="name"/> values are specified all host and domain cookies matching both will be deleted.
        /// If only <paramref name="url"/> is specified all host cookies (but not domain cookies) irrespective of path will be deleted.
        /// If <paramref name="url"/> is empty all cookies for all hosts and domains will be deleted.
        /// Cookies can alternately be deleted using the Visit*Cookies() methods.
        /// </summary>
        /// <param name="url">The cookie URL.</param>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="callback">If non-NULL it will be executed asnychronously on the CEF UI thread after the cookies have been deleted.</param>
        /// <returns>Returns false if a non-empty invalid URL is specified or if cookies cannot be accessed; otherwise, true.</returns>
        bool DeleteCookies(string url = null, string name = null, IDeleteCookiesCallback callback = null);

        /// <summary>
        /// Sets a cookie given a valid URL and explicit user-provided cookie attributes. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and fail without setting the cookie if such characters are found.
        /// This method will be executed on the CEF UI thread in an async fashion, to be notified upon completion implement <see cref="ISetCookieCallback"/>
        /// and pass in as <paramref name="callback"/>
        /// </summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="cookie">The cookie</param>
        /// <param name="callback">If non-NULL it will be executed asnychronously on the CEF UI thread after the cookie has been set.</param>
        /// <returns>Returns false if an invalid URL is specified or if cookies cannot be accessed.</returns>
        bool SetCookie(string url, Cookie cookie, ISetCookieCallback callback = null);

        /// <summary>
        /// Set the schemes supported by this manager. Calling this method with an empty <paramref name="schemes"/> value and <paramref name="includeDefaults"/>
        /// set to false will disable all loading and saving of cookies for this manager. Must be called before any cookies are accessed.
        /// </summary>
        /// <param name="schemes">The list of supported schemes.</param>
        /// <param name="includeDefaults">If true the default schemes ("http", "https", "ws" and "wss") will also be supported. Calling this method with an empty schemes value and includeDefaults
        /// set to false will disable all loading and saving of cookies for this manager</param>
        /// <param name="callback">If non-NULL it will be executed asnychronously on the CEF UI thread after the change has been applied.</param>
        void SetSupportedSchemes(string[] schemes, bool includeDefaults, ICompletionCallback callback = null);

        /// <summary>
        /// Visit all cookies on the UI thread. The returned cookies are ordered by longest path, then by earliest creation date.
        /// </summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <returns>Returns false if cookies cannot be accessed; otherwise, true.</returns>
        bool VisitAllCookies(ICookieVisitor visitor);

        /// <summary>
        /// Visit a subset of cookies on the CEF UI thread.
        /// The results are filtered by the given url scheme, host, domain and path.
        /// The returned cookies are ordered by longest path, then by earliest creation date. 
        /// </summary>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">If true HTTP-only cookies will also be included in the results.</param>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <returns>Returns false if cookies cannot be accessed; otherwise, true.</returns>
        bool VisitUrlCookies(string url, bool includeHttpOnly, ICookieVisitor visitor);

        /// <summary>
        /// Flush the backing store (if any) to disk
        /// This method will be executed on the CEF UI thread in an async fashion, to be notified upon completion implement <see cref="ICompletionCallback"/>
        /// and pass in as <paramref name="callback"/>
        /// </summary>
        /// <param name="callback">If non-NULL it will be executed asnychronously on the CEF UI thread after the flush is complete.</param>
        /// <returns>Returns false if cookies cannot be accessed.</returns>
        bool FlushStore(ICompletionCallback callback);

        /// <summary>
        /// Returns true if disposed
        /// </summary>
        bool IsDisposed { get; }
    }
}

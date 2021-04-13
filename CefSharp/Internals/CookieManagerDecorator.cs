// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// CookieManager with additional checks to ensure the store is initialized.
    /// Throws an exception when attempting to access the store before it's ready.
    /// </summary>
    public class CookieManagerDecorator : ICookieManager
    {
        private const string NotInitialziedExceptionMsg = "CookieManager store is not initialized.";
        private ICookieManager manager;
        private volatile bool managerReady;

        internal CookieManagerDecorator(ICookieManager manager, TaskCompletionCallback callback)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("manager");
            }

            this.manager = manager;

            if (callback.Task.Status == TaskStatus.RanToCompletion)
            {
                managerReady = callback.Task.Result;
            }
            else
            {
                callback.Task.ContinueWith(x =>
                {
                    managerReady = x.Result;
                });
            }
        }

        bool ICookieManager.IsDisposed
        {
            get { return manager.IsDisposed; }
        }

        bool ICookieManager.DeleteCookies(string url, string name, IDeleteCookiesCallback callback)
        {
            if (managerReady)
            {
                return manager.DeleteCookies(url, name, callback);
            }

            throw new InvalidOperationException(NotInitialziedExceptionMsg);
        }

        void IDisposable.Dispose()
        {
            manager.Dispose();
        }

        bool ICookieManager.FlushStore(ICompletionCallback callback)
        {
            if (managerReady)
            {
                return manager.FlushStore(callback);
            }

            throw new InvalidOperationException(NotInitialziedExceptionMsg);
        }

        bool ICookieManager.SetCookie(string url, Cookie cookie, ISetCookieCallback callback)
        {
            if (managerReady)
            {
                return manager.SetCookie(url, cookie, callback);
            }

            throw new InvalidOperationException(NotInitialziedExceptionMsg);
        }

        bool ICookieManager.VisitAllCookies(ICookieVisitor visitor)
        {
            if (managerReady)
            {
                return manager.VisitAllCookies(visitor);
            }

            throw new InvalidOperationException(NotInitialziedExceptionMsg);
        }

        bool ICookieManager.VisitUrlCookies(string url, bool includeHttpOnly, ICookieVisitor visitor)
        {
            if (managerReady)
            {
                return manager.VisitUrlCookies(url, includeHttpOnly, visitor);
            }

            throw new InvalidOperationException(NotInitialziedExceptionMsg);
        }
    }
}

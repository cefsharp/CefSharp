// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// Based on: http://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
    
    /// <summary>
    /// Provides base class functionality for implementing the Dispose Pattern.
    /// </summary>
    public abstract class DisposableResource : IDisposable
    {
        ~DisposableResource()
        {
            DoDispose(false);
            IsDisposed = true;
        }

        public void Dispose()
        {
            DoDispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeMember<T>(ref T field)
            where T : class, IDisposable
        {
            var oldvalue = field;
            field = null;

            if (oldvalue != null)
            {
                oldvalue.Dispose();
            }
        }

        protected virtual void DoDispose(bool isDisposing)
        {
        }

        /// <summary>
        /// If this Object is disposed the Method will throw a <see cref="ObjectDisposedException"/>.
        /// Use this method to guard methods that are not allowed to be called after dispose was called
        /// </summary>
        /// <exception cref="System.ObjectDisposedException"></exception>
        protected virtual void DisposeGuard()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed. 
        /// This Value Should be used for lifetime management. 
        /// i.e. EventHandlers may not be registered if this object is already disposed
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }
    }
}

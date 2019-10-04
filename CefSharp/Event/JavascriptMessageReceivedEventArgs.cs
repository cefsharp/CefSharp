// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.ModelBinding;

namespace CefSharp
{
    /// <summary>
    /// Calling CefSharp.PostMessage in Javascript triggers the JavascriptMessageReceived
    /// This event args contains the frame, browser and message corrisponding to that call
    /// </summary>
    public class JavascriptMessageReceivedEventArgs : EventArgs
    {
        private static readonly IBinder Binder = new DefaultBinder();

        public IFrame Frame { get; private set; }
        public IBrowser Browser { get; private set; }
        public object Message { get; private set; }

        public JavascriptMessageReceivedEventArgs(IBrowser browser, IFrame frame, object message)
        {
            Browser = browser;
            Frame = frame;
            Message = message;
        }

        /// <summary>
        /// Converts the <see cref="Message"/> to a specific type using the
        /// <see cref="DefaultBinder"/> that CefSharp provides
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Type</returns>
        public T ConvertMessageTo<T>()
        {
            if (Message == null)
            {
                return default(T);
            }
            return (T)Binder.Bind(Message, typeof(T));
        }
    }
}

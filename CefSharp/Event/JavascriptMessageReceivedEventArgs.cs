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
        private static IBinder Binder = new DefaultBinder();

        /// <summary>
        /// The frame that called CefSharp.PostMessage in Javascript
        /// </summary>
        public IFrame Frame { get; private set; }

        /// <summary>
        /// The browser that hosts the <see cref="IFrame"/>
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// Message can be a primative type or a simple object that represents a copy
        /// of the data sent from the browser
        /// </summary>
        public object Message { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="browser">The browser that hosts the <see cref="IFrame"/></param>
        /// <param name="frame">The frame that called CefSharp.PostMessage in Javascript.</param>
        /// <param name="message">Message can be a primative type or a simple object that represents a copy of the data sent from the
        /// browser.</param>
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

        /// <summary>
        /// Provide a custom instance of <see cref="IBinder"/>
        /// that will be used when <see cref="ConvertMessageTo{T}"/>
        /// is called. You may wish to provide a custom instance in cases where you
        /// wish to override the name conversion.
        /// e.g. You wish to convert names from camelCase
        /// </summary>
        /// <param name="binder">binder instance</param>
        /// <code>
        /// JavascriptMessageReceivedEventArgs.SetBinder(new DefaultBinder(new CamelCaseJavascriptNameConverter()));
        /// </code>
        public static void SetBinder(IBinder binder)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            Binder = binder;
        }
    }
}

// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
using System;

namespace CefSharp.Example
{
    /// <summary>
    /// A class that is used to demonstrate how asynchronous javascript events can be returned to the .Net runtime environment.
    /// </summary>
    /// <seealso cref="ScriptedMethods"/>
    /// <seealso cref="resources/ScriptedMethodsTest.html"/>
    public class ScriptedMethodsBoundObject
    {
        /// <summary>
        /// Raised when a Javascript event arrives.
        /// </summary>
        public event Action<string,object> EventArrived;

        /// <summary>
        /// This method will be exposed to the Javascript environment. It is
        /// invoked in the Javascript environment when some event of interest
        /// happens.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventData">Data provided by the invoker pertaining to the event.</param>
        /// <remarks>
        /// By default RaiseEvent will be translated to raiseEvent as a javascript function.
        /// This is configurable when calling RegisterJsObject by setting camelCaseJavascriptNames;
        /// </remarks>
        public void RaiseEvent(string eventName, object eventData = null)
        {
            if (EventArrived != null)
            {
                EventArrived(eventName, eventData);
            }
        }
    }
}

// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Callback
{
    /// <summary>
    /// Callback interface for <see cref="IBrowserHost.AddDevToolsMessageObserver"/>.
    /// The methods of this class will be called on the CEF UI thread.
    /// </summary>
    public interface IDevToolsMessageObserver : IDisposable
    {
        /// <summary>
        /// Method that will be called on receipt of a DevTools protocol message.
        /// Method result dictionaries include an "id" (int) value that identifies the
        /// orginating method call sent from IBrowserHost.SendDevToolsMessage, and
        /// optionally either a "result" (dictionary) or "error" (dictionary) value.
        /// The "error" dictionary will contain "code" (int) and "message" (string)
        /// values. Event dictionaries include a "method" (string) value and optionally
        /// a "params" (dictionary) value. See the DevTools protocol documentation at
        /// https://chromedevtools.github.io/devtools-protocol/ for details of
        /// supported method calls and the expected "result" or "params" dictionary
        /// contents. JSON dictionaries can be parsed using the CefParseJSON function
        /// if desired, however be aware of performance considerations when parsing
        /// large messages (some of which may exceed 1MB in size).
        /// </summary>
        /// <param name="browser">is the originating browser instance</param>
        /// <param name="message">is a UTF8-encoded JSON dictionary representing either a method result or an event. 
        /// is only valid for the scope of this callback and should be copied if necessary
        /// </param>
        /// <returns>Return true if the message was handled or false if the message
        /// should be further processed and passed to the OnDevToolsMethodResult or
        /// OnDevToolsEvent methods as appropriate.</returns>
        bool OnDevToolsMessage(IBrowser browser, Stream message);

        /// <summary>
        /// Method that will be called after attempted execution of a DevTools protocol
        /// </summary>
        /// <param name="browser">is the originating browser instance</param>
        /// <param name="messageId">is the id value that identifies the originating method call message</param>
        /// <param name="success">If the method succeeded <paramref name="success"/> will be true and <paramref name="result"/> will be the
        /// UTF8-encoded JSON "result" dictionary value (which may be empty).
        /// If the method failed <paramref name="success"/> will be false and <paramref name="result"/> will be the UTF8-encoded
        /// JSON "error" dictionary value.
        /// </param>
        /// <param name="result">The stream is only valid for the scope of this
        /// callback and should be copied if necessary. See the OnDevToolsMessage
        /// documentation for additional details on  contents</param>
        void OnDevToolsMethodResult(IBrowser browser, int messageId, bool success, Stream result);

        /// <summary>
        /// Method that will be called on receipt of a DevTools protocol event.
        /// </summary>
        /// <param name="browser">is the originating browser instance</param>
        /// <param name="method">is the method value</param>
        /// <param name="parameters">is the UTF8-encoded JSON "params" dictionary value (which
        /// may be empty). This stream is only valid for the scope of this callback and
        /// should be copied if necessary. See the OnDevToolsMessage documentation for
        /// additional details on contents.
        /// </param>
        void OnDevToolsEvent(IBrowser browser, string method, Stream parameters);

        /// <summary>
        /// Method that will be called when the DevTools agent has attached.
        /// This will generally occur in response to the first message sent while the agent is detached.
        /// </summary>
        /// <param name="browser">is the originating browser instance</param>
        void OnDevToolsAgentAttached(IBrowser browser);

        /// <summary>
        /// Method that will be called when the DevTools agent has detached.
        /// Any method results that were pending before the agent became detached will not be delivered, and any active
        /// event subscriptions will be canceled.
        /// </summary>
        /// <param name="browser">is the originating browser instance</param>
        void OnDevToolsAgentDetached(IBrowser browser);
    }
}

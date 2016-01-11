// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CefSharp.Example
{
    /// <summary>
    /// Methods whose functionaity is mostly implemented by evaluating or
    /// executing scripts in the browser.
    /// </summary>
    public static class ScriptedMethods
    {
        /// <summary>
        /// Determine if the active element in a frame accepts text input.
        /// </summary>
        /// <param name="frame">Test the active element in this frame.</param>
        /// <returns>True if the active element accepts text input.</returns>
        public static async Task<bool> ActiveElementAcceptsTextInput(this IFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentException("An IFrame instance is required.", "frame");
            }

            // Scripts should be minified for production builds. The script
            // could also be read from a file...
            const string script =
                  @"(function ()
                    {
                        var isText = false;
                        var activeElement = document.activeElement;
                        if (activeElement) {
                            if (activeElement.tagName.toLowerCase() === 'textarea') {
                                isText = true;
                            } else {
                                if (activeElement.tagName.toLowerCase() === 'input') {
                                    if (activeElement.hasAttribute('type')) {
                                        var inputType = activeElement.getAttribute('type').toLowerCase();
                                        if (inputType === 'text' || inputType === 'email' || inputType === 'password' || inputType === 'tel' || inputType === 'number' || inputType === 'range' || inputType === 'search' || inputType === 'url') {
                                            isText = true;
                                        }
                                    }
                                }
                            }
                        }
                        return isText;
                    })();";

            var response = await frame.EvaluateScriptAsync(script);
            if (!response.Success)
            {
                throw new Exception(response.Message);
            }

            return (bool)response.Result;
        }

        /// <summary>
        /// Determine if the frame contains an element with the specified id.
        /// </summary>
        /// <param name="frame">Test the elements in this frame.</param>
        /// <param name="id">The id to find.</param>
        /// <returns>True if an element with the specified id exists in the frame.</returns>
        public static async Task<bool> ElementWithIdExists(this IFrame frame, string id)
        {
            if (frame == null)
            {
                throw new ArgumentException("An IFrame instance is required.", "frame");
            }

            var script =
                @"(function () {
                    var n = document.getElementById('##ID##');
                    return n !== null && typeof n !== 'undefined';
                })();";

            // For simple inline scripts you could use String.Format() but
            // beware of braces in the javascript code. If reading from a file
            // it's probably safer to include tokens that can be replaced via
            // regex.
            script = Regex.Replace(script, "##ID##", id);

            var response = await frame.EvaluateScriptAsync(script);
            if (!response.Success)
            {
                throw new Exception(response.Message);
            }

            return (bool)response.Result;
        }

        /// <summary>
        /// Set an event listener on the element with the provided id. When the
        /// event listener callback is invoked an attempt will be made to pass
        /// event information to a .Net class bound to the browser. See
        /// ScriptedMethodsBoundObject.
        /// </summary>
        /// <param name="frame">The element is in this frame.</param>
        /// <param name="id">The id of an element that exists in the frame.</param>
        /// <param name="eventName">Subscribe to this event. For example 'click'.</param>
        public static void ListenForEvent(this IFrame frame, string id, string eventName)
        {
            if (frame == null)
            {
                throw new ArgumentException("An IFrame instance is required.", "frame");
            }

            // Adds a click event listener to a DOM element with the provided
            // ID. When the element is clicked the ScriptedMethodsBoundObject's
            // RaiseEvent function is invoked. This is one way to get
            // asynchronous events from the web page. Typically though the web
            // page would be aware of window.boundEvent.RaiseEvent and would
            // simply raise it as needed.
            //
            // Scripts should be minified for production builds. The script
            // could also be read from a file...
            var script =
                @"(function () {
                    var counter = 0;
                    var elem = document.getElementById('##ID##');
                    elem.removeAttribute('disabled');
                    elem.addEventListener('##EVENT##', function(e){
                        if (!window.boundEvent){
                            console.log('window.boundEvent does not exist.');
                            return;
                        }
                        counter++;
                        //NOTE RaiseEvent was converted to raiseEvent in JS (this is configurable when registering the object)
                        window.boundEvent.raiseEvent('##EVENT##', {count: counter, id: e.target.id, tagName: e.target.tagName});
                    });
                    console.log(`Added ##EVENT## listener to ${elem.id}.`);
                })();";

            // For simple inline scripts you could use String.Format() but
            // beware of braces in the javascript code. If reading from a file
            // it's probably safer to include tokens that can be replaced via
            // regex.
            script = Regex.Replace(script, "##ID##", id);
            script = Regex.Replace(script, "##EVENT##", eventName);

            frame.ExecuteJavaScriptAsync(script);
        }
    }
}

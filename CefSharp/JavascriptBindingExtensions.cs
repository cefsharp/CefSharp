// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CefSharp.Event;

namespace CefSharp
{
    /// <summary>
    /// Javascript binding extension methods
    /// </summary>
    public static class JavascriptBindingExtensions
    {
        /// <summary>
        /// Make sure an object is bound in javascript. Executes against the main frame
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="names">object names</param>
        /// <returns>List of objects that were bound</returns>
        public static Task<IList<string>> EnsureObjectBoundAsync(this IWebBrowser browser, params string[] names)
        {
            var objBoundTasks = new TaskCompletionSource<IList<string>>();

            EventHandler<JavascriptBindingMultipleCompleteEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Remove handler
                browser.JavascriptObjectRepository.ObjectsBoundInJavascript -= handler;

                var allObjectsBound = names.ToList().SequenceEqual(args.ObjectNames);
                if (allObjectsBound)
                {
                    objBoundTasks.SetResult(args.ObjectNames);
                }
                else
                {
                    objBoundTasks.SetException(new Exception("Not all objects were bound successfully, bound objects were " + string.Join(",", args.ObjectNames)));
                }
            };

            browser.JavascriptObjectRepository.ObjectsBoundInJavascript += handler;

            var bindCommand = "(function() { CefSharp.BindObjectAsync({ NotifyIfAlreadyBound: true, IgnoreCache: false }, '" + string.Join("', '", names) + "'); })();";

            browser.ExecuteScriptAsync(bindCommand);

            return objBoundTasks.Task;
        }
    }
}

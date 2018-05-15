// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CefSharp.Event;

namespace CefSharp
{
    public static class JavascriptBindingExtensions
    {
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

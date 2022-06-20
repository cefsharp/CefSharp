// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// BrowserProcessHandler implementation that takes a <see cref="TaskCompletionSource{TResult}"/>
    /// and resolves when <see cref="OnContextInitialized"/> is called.
    /// </summary>
    public class InitializeAsyncBrowserProcessHandler : CefSharp.Handler.BrowserProcessHandler
    {
        private TaskCompletionSource<bool> taskCompletionSource;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tcs">task complection source</param>
        public InitializeAsyncBrowserProcessHandler(TaskCompletionSource<bool> tcs)
        {
            taskCompletionSource = tcs;
        }

        /// <inheritdoc/>
        protected override void OnContextInitialized()
        {
            base.OnContextInitialized();

            taskCompletionSource.TrySetResult(true);
        }
    }
}

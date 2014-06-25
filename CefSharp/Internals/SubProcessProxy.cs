// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.ServiceModel;

namespace CefSharp.Internals
{
    public class SubProcessProxy : ISubProcessProxy
    {
        public ISubProcessCallback Callback { get; private set; }

        public void Initialize()
        {
            CefSubprocess.Instance.ServiceHost.Service = this;
            Callback = OperationContext.Current.GetCallbackChannel<ISubProcessCallback>();
        }

        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var result = CefSubprocess.Instance.Browser.EvaluateScript(frameId, script, timeout);
            return result;
        }

        public void Terminate()
        {
            CefSubprocess.Instance.ServiceHost.Service = null;
            CefSubprocess.Instance.Dispose();
        }
    }
}

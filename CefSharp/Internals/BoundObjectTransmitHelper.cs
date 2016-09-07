// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public sealed class BoundObjectTransmitHelper
    {
        //dummy IRequestCallback implementation that stores callback values
        private class DummyCallback : IRequestCallback
        {
            private bool isDisposed;

            public bool Allowed { get; private set; }

            public bool Cancelled { get; private set; }

            public void Dispose()
            {
                isDisposed = true;
            }

            public void Continue(bool allow)
            {
                Allowed = allow;
            }

            public void Cancel()
            {
                Cancelled = true;
            }

            public bool IsDisposed
            {
                get { return isDisposed; }
            }
        }

        private DummyCallback passableCallback;
        private CefReturnValue returnValue;
        private IRequestCallback originalCallback;

        //bound objects has arrived to the subprocess
        public bool Completed { get; private set; }

        public IRequestCallback PassableCallback
        {
            get
            {
                if (passableCallback == null)
                {
                    passableCallback = new DummyCallback();
                }
                return passableCallback;
            }
        }

        //mark it complete without further action (for example when no bound objects)
        public void CompleteSync()
        {
            Completed = true;
        }

        //complete after ack received from subprocess for bound object message
        public void CompleteAsync()
        {
            if (Completed)
                return;

            switch (returnValue)
            {
                case CefReturnValue.Cancel:
                    originalCallback.Cancel();
                    break;
                case CefReturnValue.ContinueAsync:
                    ContinueAsyncBasedOnPassableCallback();
                    break;
                case CefReturnValue.Continue:
                    originalCallback.Continue(true);
                    break;
            }

            ResetCallbacks();
            Completed = true;
        }

        public void Reset()
        {
            ResetCallbacks();
            Completed = false;
        }

        public CefReturnValue DoYield(IRequestCallback originalCallback, CefReturnValue returnValue)
        {
            this.originalCallback = originalCallback;
            this.returnValue = returnValue;
            return CefReturnValue.ContinueAsync;
        }

        private void ResetCallbacks()
        {
            if (originalCallback != null)
            {
                originalCallback.Dispose();
            }
            originalCallback = null;
            passableCallback = null;
        }

        private void ContinueAsyncBasedOnPassableCallback()
        {
            if (passableCallback != null)
            {
                if (passableCallback.Cancelled)
                {
                    originalCallback.Cancel();
                }
                else
                {
                    originalCallback.Continue(passableCallback.Allowed);
                }
            }
            else
            {
                originalCallback.Continue(true);
            }
        }
    }
}
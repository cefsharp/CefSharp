// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms.Example.Handlers
{
    internal class GeolocationHandler : IGeolocationHandler
    {
        bool IGeolocationHandler.OnRequestGeolocationPermission(IWebBrowser browserControl, IBrowser browser, string requestingUrl, int requestId, IGeolocationCallback callback)
        {
            //The callback has been disposed, so we are unable to continue
            if(callback.IsDisposed)
            {
                return false;
            }

            var control = (Control)browserControl;

            control.InvokeOnUiThreadIfRequired(delegate()
            {
                //Callback wraps a managed resource, so we'll wrap in a using statement so it's always disposed of.
                using (callback)
                {
                    var result = MessageBox.Show(string.Format("{0} wants to use your computer's location.  Allow?  ** You must set your Google API key in CefExample.Init() for this to work. **", requestingUrl), "Geolocation", MessageBoxButtons.YesNo);

                    callback.Continue(result == DialogResult.Yes);
                }
            });    

            //To cancel the request immediately we'd return false here, as we're returning true
            // the callback will be used to allow/deny the permission request.
            return true;
        }

        void IGeolocationHandler.OnCancelGeolocationPermission(IWebBrowser browserControl, IBrowser browser, int requestId)
        {
        }
    }
}

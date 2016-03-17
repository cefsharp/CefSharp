// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

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

            using (callback)
            {
                var result = MessageBox.Show(String.Format("{0} wants to use your computer's location.  Allow?  ** You must set your Google API key in CefExample.Init() for this to work. **", requestingUrl), "Geolocation", MessageBoxButtons.YesNo);

                callback.Continue(result == DialogResult.Yes);
                callback.Dispose();

                return result == DialogResult.Yes;
            }
        }

        void IGeolocationHandler.OnCancelGeolocationPermission(IWebBrowser browserControl, IBrowser browser, int requestId)
        {
        }
    }
}

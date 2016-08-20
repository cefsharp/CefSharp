// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    internal class GeolocationHandler : IGeolocationHandler
    {
        bool IGeolocationHandler.OnRequestGeolocationPermission(IWebBrowser browserControl, IBrowser browser, string requestingUrl, int requestId, IGeolocationCallback callback)
        {
            //You can execute the callback inline
            //callback.Continue(true);
            //return true;

            //You can execute the callback in an `async` fashion
            //Open a message box on the `UI` thread and ask for user input.
            //You can open a form, or do whatever you like, just make sure you either
            //execute the callback or call `Dispose` as it's an `unmanaged` wrapper.
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            chromiumWebBrowser.Dispatcher.BeginInvoke((Action)(() =>
            {
                //Callback wraps an unmanaged resource, so we'll make sure it's Disposed (calling Continue will also Dipose of the callback, it's safe to dispose multiple times).
                using (callback)
                {
                    var result = MessageBox.Show(String.Format("{0} wants to use your computer's location.  Allow?  ** You must set your Google API key in CefExample.Init() for this to work. **", requestingUrl), "Geolocation", MessageBoxButton.YesNo);

                    //Execute the callback, to allow/deny the request.
                    callback.Continue(result == MessageBoxResult.Yes);
                }
            }));

            //Yes we'd like to handle this request ourselves.
            return true;
        }

        void IGeolocationHandler.OnCancelGeolocationPermission(IWebBrowser browserControl, IBrowser browser, int requestId)
        {
        }
    }
}

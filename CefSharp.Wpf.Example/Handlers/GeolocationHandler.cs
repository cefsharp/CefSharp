// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    internal class GeolocationHandler : IGeolocationHandler
    {
        public bool OnRequestGeolocationPermission(IWebBrowser browser, string requestingUrl, int requestId)
        {
            var result = MessageBox.Show(String.Format("{0} wants to use your computer's location.  Allow?  ** You must set your Google API key in CefExample.Init() for this to work. **", requestingUrl), "Geolocation", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        public void OnCancelGeolocationPermission(IWebBrowser browser, string requestingUrl, int requestId)
        {
        }
    }
}

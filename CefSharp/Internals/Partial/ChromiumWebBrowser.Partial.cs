// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


#if OFFSCREEN
namespace CefSharp.OffScreen
#elif WPF
namespace CefSharp.Wpf
#elif WINFORMS
namespace CefSharp.WinForms
#endif
{
    //ChromiumWebBrowser Partial class implementation shared between the 
    //WPF, Winforms and Offscreen
    public partial class ChromiumWebBrowser
    {
        private void SetHandlersToNullExceptLifeSpan()
        {
            AudioHandler = null;
            DialogHandler = null;
            FindHandler = null;
            RequestHandler = null;
            DisplayHandler = null;
            LoadHandler = null;
            KeyboardHandler = null;
            JsDialogHandler = null;
            DragHandler = null;
            DownloadHandler = null;
            MenuHandler = null;
            FocusHandler = null;
            ResourceRequestHandlerFactory = null;
            RenderProcessMessageHandler = null;
        }
    }
}

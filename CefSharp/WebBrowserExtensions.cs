// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public static class WebBrowserExtensions
    {
        public static void Undo(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if(frame != null)
            {
                frame.Undo();
            }
        }

        public static void Redo(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.Redo();
            }
        }

        public static void Cut(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.Cut();
            }
        }

        public static void Copy(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.Copy();
            }
        }

        public static void Paste(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.Paste();
            }
        }

        public static void Delete(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.Delete();
            }
        }

        public static void SelectAll(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            if (frame != null)
            {
                frame.SelectAll();
            }
        }
    }
}

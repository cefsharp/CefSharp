// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Wpf.Example.Handlers;
using System.Windows.Automation.Peers;

namespace CefSharp.Wpf.Example.Controls
{
    public class ChromiumWebBrowserWithAccessibilitySupport : ChromiumWebBrowser
    {
        protected override void OnIsBrowserInitializedChanged(bool oldValue, bool newValue)
        {
            if (IsBrowserInitialized)
            {
                this.GetBrowserHost().SetAccessibilityState(CefState.Enabled);
            }

            base.OnIsBrowserInitializedChanged(oldValue, newValue);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            if (AccessibilityHandler is AccessibilityHandler accessibilityHandler)
            {
                return accessibilityHandler.AutomationPeer;
            }

            return base.OnCreateAutomationPeer();
        }
    }
}

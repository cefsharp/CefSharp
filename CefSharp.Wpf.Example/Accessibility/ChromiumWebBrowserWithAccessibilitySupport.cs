// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Automation.Peers;

namespace CefSharp.Wpf.Example.Accessibility
{
    public class ChromiumWebBrowserWithAccessibilitySupport : ChromiumWebBrowser
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            if (AccessibilityHandler is AccessibilityHandler accessibilityHandler)
            {
                var automationPeer = accessibilityHandler.GetAutomationPeer(this);
                return automationPeer;
            }

            return base.OnCreateAutomationPeer();
        }
    }
}

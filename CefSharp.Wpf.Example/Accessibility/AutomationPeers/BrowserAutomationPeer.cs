// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;

namespace CefSharp.Wpf.Example.Accessibility.AutomationPeers
{
    public class BrowserAutomationPeer : FrameworkElementAutomationPeer
    {
        private readonly FrameworkElement owner;
        private readonly AccessibilityHandler accessibilityHandler;

        public BrowserAutomationPeer(
            FrameworkElement owner, 
            AccessibilityHandler accessibilityHandler) : base(owner)
        {

            this.owner = owner;
            this.accessibilityHandler = accessibilityHandler;

            this.accessibilityHandler.AccessibilityTreeAdded += AccessibilityHandlerAccessibilityTreeAdded;
        }

        private void AccessibilityHandlerAccessibilityTreeAdded(object sender, EventArgs e)
        {
            RaiseAutomationEvent(AutomationEvents.StructureChanged);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var roots = accessibilityHandler.GetAccessibilityTrees();

            var result = roots
                .Select(x => new AccessibilityTreeAutomationPeer(owner, x))
                .Cast<AutomationPeer>()
                .ToList();

            return result;
        }

        protected override string GetNameCore()
        {
            return "Chromium Web Browser";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Group;
        }
    }
}

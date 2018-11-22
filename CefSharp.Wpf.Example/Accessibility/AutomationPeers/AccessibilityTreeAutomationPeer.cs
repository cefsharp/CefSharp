// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using CefSharp.Wpf.Example.Accessibility.Tree;

namespace CefSharp.Wpf.Example.Accessibility.AutomationPeers
{
    public class AccessibilityTreeAutomationPeer : FrameworkElementAutomationPeer
    {
        private readonly FrameworkElement owner;
        private readonly AccessibilityTree accessibilityTree;

        public AccessibilityTreeAutomationPeer(
            FrameworkElement owner, 
            AccessibilityTree accessibilityTree) : base(owner)
        {
            this.owner = owner;
            this.accessibilityTree = accessibilityTree;
            this.accessibilityTree.RootNodeChanged += AccessibilityTreeRootNodeChanged;
        }

        private void AccessibilityTreeRootNodeChanged(object sender, System.EventArgs e)
        {
            RaiseAutomationEvent(AutomationEvents.StructureChanged);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var rootNode = accessibilityTree.GetRootNode();

            if (rootNode == null)
            {
                return new List<AutomationPeer>(0);
            }

            return new List<AutomationPeer>(1)
            {
                new AccessibilityNodeAutomationPeer(owner, rootNode)
            };
        }
    }
}

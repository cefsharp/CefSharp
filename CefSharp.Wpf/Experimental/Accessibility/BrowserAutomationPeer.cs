// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;

namespace CefSharp.Wpf.Experimental.Accessibility
{
    public class BrowserAutomationPeer : FrameworkElementAutomationPeer
    {
        private readonly Dictionary<string, AccessibilityTree> accessibilityTrees;

        public BrowserAutomationPeer(FrameworkElement owner) : base(owner)
        {
            this.accessibilityTrees = new Dictionary<string, AccessibilityTree>();
        }

        public virtual void OnAccessibilityTreeChange(string treeId, IDictionary<string, IValue> accessibilityUpdateDictionary)
        {
            AccessibilityTree accessibilityTree;
            if (accessibilityTrees.ContainsKey(treeId))
            {
                accessibilityTree = accessibilityTrees[treeId];
            }
            else
            {
                accessibilityTree = new AccessibilityTree((FrameworkElement)Owner, treeId);
                accessibilityTrees.Add(treeId, accessibilityTree);

                RaiseAutomationEvent(AutomationEvents.StructureChanged);
            }

            accessibilityTree.Update(accessibilityUpdateDictionary);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            return accessibilityTrees.Values.Cast<AutomationPeer>().ToList();
        }

        protected override string GetNameCore()
        {
            return "Web Browser";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Group;
        }
    }
}

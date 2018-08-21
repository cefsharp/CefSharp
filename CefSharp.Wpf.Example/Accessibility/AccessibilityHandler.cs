// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using CefSharp.Wpf.Example.Accessibility.AutomationPeers;
using CefSharp.Wpf.Example.Accessibility.Tree;
using ValueType = CefSharp.Enums.ValueType;

namespace CefSharp.Wpf.Example.Accessibility
{
    public class AccessibilityHandler : IAccessibilityHandler
    {
        private readonly Dictionary<int, AccessibilityTree> accessibilityTrees;

        public AccessibilityHandler()
        {
            accessibilityTrees = new Dictionary<int, AccessibilityTree>();
        }

        public void OnAccessibilityLocationChange(IValue value)
        {
        }

        public void OnAccessibilityTreeChange(IValue value)
        {
            if (value.Type != ValueType.List) return;

            foreach (var listValue in value.GetList())
            {
                if (listValue.Type == ValueType.Dictionary)
                {
                    var eventDictionary = listValue.GetDictionary();

                    var treeId = eventDictionary["ax_tree_id"].GetInt();

                    AccessibilityTree accessibilityTree;
                    if (accessibilityTrees.ContainsKey(treeId))
                    {
                        accessibilityTree = accessibilityTrees[treeId];
                    }
                    else
                    {
                        accessibilityTree = new AccessibilityTree(treeId);
                        accessibilityTrees.Add(treeId, accessibilityTree);
                        OnAccessibilityTreeAdded();
                    }

                    accessibilityTree.Update(eventDictionary);
                }
            }
        }

        internal AutomationPeer GetAutomationPeer(FrameworkElement chromiumWebBrowser)
        {
            return new BrowserAutomationPeer(chromiumWebBrowser, this);
        }

        internal List<AccessibilityTree> GetAccessibilityTrees()
        {
            return accessibilityTrees.Values.ToList();
        }

        internal event EventHandler AccessibilityTreeAdded;

        protected virtual void OnAccessibilityTreeAdded()
        {
            AccessibilityTreeAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}

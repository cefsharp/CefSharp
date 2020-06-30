// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows;
using CefSharp.Wpf.Example.Accessibility;

namespace CefSharp.Wpf.Example.Handlers
{
    public class AccessibilityHandler : IAccessibilityHandler
    {
        public BrowserAutomationPeer AutomationPeer { get; }

        private readonly FrameworkElement owner;

        public AccessibilityHandler(FrameworkElement owner)
        {
            this.AutomationPeer = new BrowserAutomationPeer(owner);

            this.owner = owner;
        }

        public void OnAccessibilityLocationChange(IValue value)
        {
        }

        public void OnAccessibilityTreeChange(IValue value)
        {
            if (value.Type != Enums.ValueType.Dictionary)
                return;

            IDictionary<string, IValue> accessibilityUpdateDictionary = value.GetDictionary();
            string treeId = accessibilityUpdateDictionary["ax_tree_id"].GetString();

            owner.Dispatcher.Invoke(new Action(() =>
                AutomationPeer.OnAccessibilityTreeChange(treeId, accessibilityUpdateDictionary)
            ));
        }
    }
}

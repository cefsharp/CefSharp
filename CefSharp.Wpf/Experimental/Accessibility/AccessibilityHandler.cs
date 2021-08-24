// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;

namespace CefSharp.Wpf.Experimental.Accessibility
{
    /// <summary>
    /// Provides a limited read-only Accessibility Handler implementation.
    /// To enable accessibility support use the --force-renderer-accessibility to enable
    /// for all browsers or call <see cref="IBrowserHost.SetAccessibilityState(CefState)"/>
    /// on a per browser basis to enable. By default accessibility is disabled by default.
    /// Having accessibility enabled can impact performance until accessibility is disabled.
    /// </summary>
    public class AccessibilityHandler : IAccessibilityHandler
    {
        public BrowserAutomationPeer AutomationPeer { get; protected set; }

        protected readonly FrameworkElement owner;

        public AccessibilityHandler(FrameworkElement owner)
        {
            this.AutomationPeer = new BrowserAutomationPeer(owner);

            this.owner = owner;
        }

        void IAccessibilityHandler.OnAccessibilityLocationChange(IValue value)
        {
            OnAccessibilityLocationChange(value);
        }

        /// <summary>
        /// Called after renderer process sends accessibility location changes to the browser process.
        /// </summary>
        /// <param name="value">Updated location info.</param>
        protected virtual void OnAccessibilityLocationChange(IValue value)
        {

        }

        void IAccessibilityHandler.OnAccessibilityTreeChange(IValue value)
        {
            OnAccessibilityTreeChange(value);
        }

        /// <summary>
        /// Called after renderer process sends accessibility tree changes to the browser process.
        /// </summary>
        /// <param name="value">Updated tree info.</param>
        protected virtual void OnAccessibilityTreeChange(IValue value)
        {
            if (value.Type != Enums.ValueType.Dictionary)
            {
                return;
            }

            var accessibilityUpdateDictionary = value.GetDictionary();
            if (accessibilityUpdateDictionary == null || !accessibilityUpdateDictionary.ContainsKey("ax_tree_id"))
            {
                return;
            }

            string treeId = accessibilityUpdateDictionary["ax_tree_id"].GetString();

            owner.Dispatcher.Invoke(new Action(() =>
                AutomationPeer.OnAccessibilityTreeChange(treeId, accessibilityUpdateDictionary)
            ));
        }
    }
}

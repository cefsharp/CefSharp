// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using CefSharp.Wpf.Example.Accessibility.Tree;

namespace CefSharp.Wpf.Example.Accessibility.AutomationPeers
{
    public class AccessibilityNodeAutomationPeer : AutomationPeer
    {
        // Mapping implemented according to the following link
        // https://docs.microsoft.com/en-us/windows/desktop/winauto/uiauto-ariaspecification
        // 'Custom' control type is used for unsupported control types
        private static readonly Dictionary<string, AutomationControlType> ControlTypeMapping = new Dictionary<string, AutomationControlType>
        {
            {"alert", AutomationControlType.Text},
            {"application", AutomationControlType.Pane},
            {"buttonDropDown", AutomationControlType.ComboBox}, // No mapping for this one in the reference
            {"popUpButton", AutomationControlType.Button}, // No mapping for this one in the reference
            {"checkBox", AutomationControlType.CheckBox},
            {"comboBox", AutomationControlType.ComboBox},
            {"dialog", AutomationControlType.Pane},
            {"genericContainer", AutomationControlType.Group},
            {"group", AutomationControlType.Group},
            {"image", AutomationControlType.Image},
            {"link", AutomationControlType.Hyperlink},
            {"locationBar", AutomationControlType.Group},
            {"menuBar", AutomationControlType.MenuBar},
            {"menuItem", AutomationControlType.MenuItem},
            {"menuListPopup", AutomationControlType.Menu},
            {"tree", AutomationControlType.Tree},
            {"treeItem", AutomationControlType.TreeItem},
            {"tab", AutomationControlType.TabItem},
            {"tabList", AutomationControlType.Tab},
            {"pane", AutomationControlType.Pane},
            {"progressIndicator", AutomationControlType.ProgressBar},
            {"button", AutomationControlType.Button},
            {"radioButton", AutomationControlType.RadioButton},
            {"scrollBar", AutomationControlType.ScrollBar},
            {"splitter", AutomationControlType.Separator},
            {"slider", AutomationControlType.Slider},
            {"staticText", AutomationControlType.Text},
            {"textField", AutomationControlType.Edit},
            {"titleBar", AutomationControlType.Pane}, // No mapping for this one in the reference
            {"toolbar", AutomationControlType.ToolBar},
            {"webView", AutomationControlType.Group},
            {"window", AutomationControlType.Window},
            {"client", AutomationControlType.Calendar}
        };

        private readonly FrameworkElement owner;
        private readonly AccessibilityNode accessibilityNode;

        public AccessibilityNodeAutomationPeer(FrameworkElement owner, AccessibilityNode accessibilityNode)
        {
            this.owner = owner;
            this.accessibilityNode = accessibilityNode;

            accessibilityNode.ChildenChanged += AccessibilityNodeOnChildenChanged;
            accessibilityNode.FocusChanged += AccessibilityNodeOnFocusChanged;
        }

        private void AccessibilityNodeOnChildenChanged(object sender, EventArgs e)
        {
            RaiseAutomationEvent(AutomationEvents.StructureChanged);
        }

        private void AccessibilityNodeOnFocusChanged(object sender, System.EventArgs e)
        {
            RaiseAutomationEvent(AutomationEvents.AutomationFocusChanged);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var accessibilityNodes = accessibilityNode.GetChildren();

            return accessibilityNodes
                .Select(x => new AccessibilityNodeAutomationPeer(owner, x))
                .Cast<AutomationPeer>()
                .ToList();
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            return null;
        }

        protected override Rect GetBoundingRectangleCore()
        {
            var location = accessibilityNode.GetLocation();

            var topLeft = owner.PointToScreen(location.TopLeft);
            var bottomRight = owner.PointToScreen(location.BottomRight);

            var result = new Rect(topLeft, bottomRight);

            return result;
        }

        protected override bool IsOffscreenCore()
        {
            return false;
        }

        protected override AutomationOrientation GetOrientationCore()
        {
            return AutomationOrientation.None;
        }

        protected override string GetItemTypeCore()
        {
            return accessibilityNode.Role;
        }

        protected override string GetClassNameCore()
        {
            return string.Empty;
        }

        protected override string GetItemStatusCore()
        {
            return string.Empty;
        }

        protected override bool IsRequiredForFormCore()
        {
            return false;
        }

        protected override bool IsKeyboardFocusableCore()
        {
            return true;
        }

        protected override bool HasKeyboardFocusCore()
        {
            return accessibilityNode.HasFocus();
        }

        protected override bool IsEnabledCore()
        {
            return true;
        }

        protected override bool IsPasswordCore()
        {
            return false;
        }

        protected override string GetAutomationIdCore()
        {
            return accessibilityNode.Id.ToString();
        }

        protected override string GetNameCore()
        {
            return accessibilityNode.Name;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            var role = accessibilityNode.Role;

            if (ControlTypeMapping.ContainsKey(role))
            {
                return ControlTypeMapping[role];
            }

            return AutomationControlType.Custom;
        }

        protected override bool IsContentElementCore()
        {
            return true;
        }

        protected override bool IsControlElementCore()
        {
            return true;
        }

        protected override AutomationPeer GetLabeledByCore()
        {
            return null;
        }

        protected override string GetHelpTextCore()
        {
            return string.Empty;
        }

        protected override string GetAcceleratorKeyCore()
        {
            return string.Empty;
        }

        protected override string GetAccessKeyCore()
        {
            return string.Empty;
        }

        protected override Point GetClickablePointCore()
        {
            var axLocation = accessibilityNode.GetLocation();
            return new Point(axLocation.X, axLocation.Y);
        }

        protected override void SetFocusCore()
        {
        }
    }
}

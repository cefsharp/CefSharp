// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using Point = System.Windows.Point;
using Rect = System.Windows.Rect;

namespace CefSharp.Wpf.Experimental.Accessibility
{
    public class AccessibilityNode : FrameworkElementAutomationPeer
    {
        public int Id { get; private set; } = -1;
        public string Role { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }

        private readonly AccessibilityTree accessibilityTree;

        private CefSharp.Structs.Rect location;
        private CefSharp.Structs.Point scroll;
        private int offsetContainerId = -1;
        private HashSet<int> childIds;

        // Mapping implemented according to the following link
        // https://docs.microsoft.com/en-us/windows/desktop/winauto/uiauto-ariaspecification
        // 'Custom' control type is used for unsupported control types
        private static readonly Dictionary<string, AutomationControlType> ControlTypeMapping = new Dictionary<string, AutomationControlType>
        {
            {"alert", AutomationControlType.Text},
            {"application", AutomationControlType.Pane},
            {"button", AutomationControlType.Button},
            {"buttonDropDown", AutomationControlType.ComboBox}, // No mapping for this one in the reference
            {"client", AutomationControlType.Calendar},
            {"popUpButton", AutomationControlType.Button}, // No mapping for this one in the reference
            {"checkBox", AutomationControlType.CheckBox},
            {"comboBox", AutomationControlType.ComboBox},
            {"dialog", AutomationControlType.Pane},
            {"genericContainer", AutomationControlType.Group},
            {"group", AutomationControlType.Group},
            {"image", AutomationControlType.Image},
            {"inlineTextBox", AutomationControlType.Text},
            {"link", AutomationControlType.Hyperlink},
            {"locationBar", AutomationControlType.Group},
            {"menuBar", AutomationControlType.MenuBar},
            {"menuItem", AutomationControlType.MenuItem},
            {"menuListPopup", AutomationControlType.Menu},
            {"pane", AutomationControlType.Pane},
            {"progressIndicator", AutomationControlType.ProgressBar},
            {"radioButton", AutomationControlType.RadioButton},
            {"rootWebArea", AutomationControlType.Group },
            {"scrollBar", AutomationControlType.ScrollBar},
            {"slider", AutomationControlType.Slider},
            {"splitter", AutomationControlType.Separator},
            {"staticText", AutomationControlType.Text},
            {"tab", AutomationControlType.TabItem},
            {"tabList", AutomationControlType.Tab},
            {"textField", AutomationControlType.Edit},
            {"titleBar", AutomationControlType.Pane}, // No mapping for this one in the reference
            {"toolbar", AutomationControlType.ToolBar},
            {"tree", AutomationControlType.Tree},
            {"treeItem", AutomationControlType.TreeItem},
            {"webView", AutomationControlType.Group},
            {"window", AutomationControlType.Window},
        };

        public AccessibilityNode(FrameworkElement owner, IDictionary<string, IValue> node, AccessibilityTree accessibilityTree) : base(owner)
        {
            this.accessibilityTree = accessibilityTree;

            this.childIds = new HashSet<int>();

            Update(node);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            return GetChildAccessibilityNodes().Cast<AutomationPeer>().ToList();
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            return null;
        }

        protected override Rect GetBoundingRectangleCore()
        {
            CefSharp.Structs.Rect location = GetLocation();
            Point point = CefPointToScreen(location.X, location.Y);
            Size size = new Size(location.Width, location.Height);
            return new Rect(point, size);
        }

        protected override bool IsOffscreenCore()
        {
            return false; // TODO
        }

        protected override AutomationOrientation GetOrientationCore()
        {
            return AutomationOrientation.None;
        }

        protected override string GetItemTypeCore()
        {
            return Role;
        }

        protected override string GetClassNameCore()
        {
            return $"CefSharpAccessibilityNode:{Role}";
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
            return HasFocus();
        }

        protected override bool IsEnabledCore()
        {
            return true;
        }

        protected override bool IsPasswordCore()
        {
            return false; // TODO
        }

        protected override string GetAutomationIdCore()
        {
            return Id.ToString();
        }

        protected override string GetNameCore()
        {
            return Name;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            if (ControlTypeMapping.ContainsKey(Role))
            {
                return ControlTypeMapping[Role];
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
            return Description;
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
            Rect rect = GetBoundingRectangleCore();
            return new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
        }

        protected override void SetFocusCore()
        {
            // TODO?
        }

        public void Update(IDictionary<string, IValue> node)
        {
            if (node == null || !node.ContainsKey("id"))
            {
                return;
            }

            Id = node["id"].GetInt();

            if (node.ContainsKey("role"))
            {
                Role = node["role"].GetString();
            }

            if (node.ContainsKey("child_ids"))
            {
                IEnumerable<int> newChildIdsList = node["child_ids"].GetList().Select(x => x.GetInt());
                if (!childIds.SetEquals(newChildIdsList))
                {
                    childIds = new HashSet<int>(newChildIdsList);
                    OnChildrenChanged();
                }
            }

            if (node.ContainsKey("location"))
            {
                IDictionary<string, IValue> locationDictionary = node["location"].GetDictionary();
                if (locationDictionary != null)
                {
                    int x = (int)(locationDictionary["x"].GetDouble());
                    int y = (int)(locationDictionary["y"].GetDouble());
                    int width = (int)(locationDictionary["width"].GetDouble());
                    int height = (int)(locationDictionary["height"].GetDouble());
                    location = new CefSharp.Structs.Rect(x, y, width, height);
                }
            }

            if (node.ContainsKey("offset_container_id"))
            {
                offsetContainerId = node["offset_container_id"].GetInt();
            }

            if (node.ContainsKey("attributes"))
            {
                IDictionary<string, IValue> attributes = node["attributes"].GetDictionary();
                if (attributes != null)
                {
                    if (attributes.ContainsKey("name"))
                    {
                        Name = attributes["name"].GetString();
                    }
                    if (attributes.ContainsKey("value"))
                    {
                        Value = attributes["value"].GetString();
                    }
                    if (attributes.ContainsKey("description"))
                    {
                        Description = attributes["description"].GetString();
                    }
                    if (attributes.ContainsKey("scrollX") && attributes.ContainsKey("scrollY"))
                    {
                        int scrollX = attributes["scrollX"].GetInt();
                        int scrollY = attributes["scrollY"].GetInt();
                        scroll = new CefSharp.Structs.Point(scrollX, scrollY);
                    }
                }
            }
        }

        protected virtual CefSharp.Structs.Rect GetLocation()
        {
            var offsetNode = accessibilityTree.GetNode(offsetContainerId);
            if (offsetNode == null)
            {
                return location;
            }

            // Add offset from parent location
            int x = location.X;
            int y = location.Y;
            CefSharp.Structs.Rect offsetNodeRect = offsetNode.GetLocation();
            x += offsetNodeRect.X - offsetNode.scroll.X;
            y += offsetNodeRect.Y - offsetNode.scroll.Y;

            return new CefSharp.Structs.Rect(x, y, location.Width, location.Height);
        }

        public virtual List<AccessibilityNode> GetChildAccessibilityNodes()
        {
            return childIds.Select(x => accessibilityTree.GetNode(x)).Where(y => y != null).ToList();
        }

        protected virtual bool HasFocus()
        {
            return Id == accessibilityTree.FocusedNodeId;
        }

        protected virtual Point CefPointToScreen(int x, int y)
        {
            var browser = (ChromiumWebBrowser)Owner;

            if (browser.DpiScaleFactor > 1)
            {
                //
                int pixelX = (int)(x / (browser.DpiScaleFactor / 96));
                int pixelY = (int)(y / (browser.DpiScaleFactor / 96));

                return Owner.PointToScreen(new Point(pixelX, pixelY));
            }

            return Owner.PointToScreen(new Point(x, y));
        }

        protected virtual void OnChildrenChanged()
        {
            RaiseAutomationEvent(AutomationEvents.StructureChanged);
        }

        public virtual void OnFocusChanged()
        {
            RaiseAutomationEvent(AutomationEvents.AutomationFocusChanged);
        }
    }
}

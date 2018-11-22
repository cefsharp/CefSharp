// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CefSharp.Wpf.Example.Accessibility.Tree
{
    public class AccessibilityNode
    {
        private readonly AccessibilityTree accessibilityTree;

        private Rect location;
        private int offsetContainerId = -1;
        private HashSet<int> childIds;
        private IDictionary<string, IValue> attributes;

        public AccessibilityNode(
            IDictionary<string, IValue> node, 
            AccessibilityTree accessibilityTree)
        {
            this.accessibilityTree = accessibilityTree;

            childIds = new HashSet<int>();

            Update(node);
        }

        public int Id { get; private set; } = -1;

        public string Role { get; private set; }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public string Description { get; private set; }

        public void Update(IDictionary<string, IValue> node)
        {
            if (node == null || !node.ContainsKey("id")) return;

            Id = node["id"].GetInt();

            if (node.ContainsKey("role"))
            {
                Role = node["role"].GetString();
            }

            if (node.ContainsKey("child_ids"))
            {
                var newChildIds = node["child_ids"].GetList().Select(x => x.GetInt());
                var children = new HashSet<int>(newChildIds);
                
                if (!childIds.SetEquals(children))
                {
                    childIds = children;
                    OnChildenChanged();
                }
            }

            if (node.ContainsKey("location"))
            {
                var locationDictionary = node["location"].GetDictionary();
                if (locationDictionary != null)
                {
                    location = new Rect(
                        locationDictionary["x"].GetDouble(), 
                        locationDictionary["y"].GetDouble(),
                        locationDictionary["width"].GetDouble(),
                        locationDictionary["height"].GetDouble());
                }
            }

            if (node.ContainsKey("offset_container_id"))
            {
                offsetContainerId = node["offset_container_id"].GetInt();
            }

            if (node.ContainsKey("attributes"))
            {
                attributes = node["attributes"].GetDictionary();

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
                }
            }
        }

        public Rect GetLocation()
        {
            var result = location;
            var offsetNode = accessibilityTree.GetNode(offsetContainerId);
            // Add offset from parent Lcoation
            if (offsetNode != null)
            {
                var offset = offsetNode.GetLocation();
                result.X += offset.X;
                result.Y += offset.Y;
            }
            return result;
        }

        public List<AccessibilityNode> GetChildren()
        {
            return childIds.Select(x => accessibilityTree.GetNode(x)).ToList();
        }

        public bool HasFocus()
        {
            return Id == accessibilityTree.FocusedNodeId;
        }

        public event EventHandler ChildenChanged;
        protected virtual void OnChildenChanged()
        {
            ChildenChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler FocusChanged;
        public virtual void OnFocusChanged()
        {
            FocusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
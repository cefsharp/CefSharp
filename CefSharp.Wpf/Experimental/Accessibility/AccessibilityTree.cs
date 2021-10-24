// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;

namespace CefSharp.Wpf.Experimental.Accessibility
{
    public class AccessibilityTree : FrameworkElementAutomationPeer
    {
        public string Id { get; }
        public int RootNodeId { get; private set; } = -1;
        public int FocusedNodeId { get; private set; } = -1;

        private readonly Dictionary<int, AccessibilityNode> accessibilityNodeMap;

        public AccessibilityTree(FrameworkElement owner, string Id) : base(owner)
        {
            this.Id = Id;

            this.accessibilityNodeMap = new Dictionary<int, AccessibilityNode>();
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var rootNode = GetRootNode();
            if (rootNode == null)
            {
                return new List<AutomationPeer>(0);
            }

            return new List<AutomationPeer>(1)
            {
                rootNode
            };
        }

        protected virtual AccessibilityNode GetRootNode()
        {
            return GetNode(RootNodeId);
        }

        protected virtual AccessibilityNode GetFocusedNode()
        {
            return GetNode(FocusedNodeId);
        }

        public virtual void Update(IDictionary<string, IValue> accessibilityUpdateDictionary)
        {
            if (accessibilityUpdateDictionary == null || !accessibilityUpdateDictionary.ContainsKey("events") || !accessibilityUpdateDictionary.ContainsKey("updates"))
            {
                return;
            }

            var events = accessibilityUpdateDictionary["events"].GetList();
            var updates = accessibilityUpdateDictionary["updates"].GetList();
            if (events == null || updates == null)
            {
                return;
            }

            var eventDictionaries = events.Select(x => x.GetDictionary()).Where(y => y.ContainsKey("event_type")).ToList();
            var updateDictionaries = updates.Select(x => x.GetDictionary()).ToList();

            foreach (var eventDictionary in eventDictionaries)
            {
                string eventType = eventDictionary["event_type"].GetString();
                if (eventType == "layoutComplete")
                {
                    UpdateLayout(eventDictionary, updateDictionaries);
                }
                else if (eventType == "focus")
                {
                    UpdateFocus(eventDictionary, updateDictionaries);
                }
            }
        }

        protected virtual void UpdateLayout(IDictionary<string, IValue> eventDictionary, List<IDictionary<string, IValue>> updateDictionaries)
        {
            foreach (var updateDictionary in updateDictionaries)
            {
                bool rootNodeChanged = false;

                // If a node is to be cleared
                if (updateDictionary.ContainsKey("node_id_to_clear"))
                {
                    // Reset root node if that is to be cleared
                    int nodeIdToClear = updateDictionary["node_id_to_clear"].GetInt();
                    if (nodeIdToClear == RootNodeId)
                    {
                        RootNodeId = -1;
                    }

                    AccessibilityNode node = GetNode(nodeIdToClear);
                    RemoveNode(node);
                }

                if (updateDictionary.ContainsKey("root_id"))
                {
                    int newRootNodeId = updateDictionary["root_id"].GetInt();
                    if (newRootNodeId != RootNodeId)
                    {
                        RootNodeId = newRootNodeId;
                        rootNodeChanged = true;
                    }
                }

                UpdateNodes(updateDictionary);

                if (rootNodeChanged)
                {
                    RaiseAutomationEvent(AutomationEvents.StructureChanged);
                }
            }
        }

        protected virtual void UpdateFocus(IDictionary<string, IValue> eventDictionary, List<IDictionary<string, IValue>> updateDictionaries)
        {
            bool focusedNodeChanged = false;

            if (eventDictionary.ContainsKey("id"))
            {
                int newFocusedNodeId = eventDictionary["id"].GetInt();
                if (newFocusedNodeId != FocusedNodeId)
                {
                    FocusedNodeId = newFocusedNodeId;
                    focusedNodeChanged = true;
                }
            }

            if (focusedNodeChanged)
            {
                var focusedNode = GetFocusedNode();
                if (focusedNode == null)
                {
                    foreach (var updateDictionary in updateDictionaries)
                    {
                        UpdateNodes(updateDictionary);
                    }
                }

                focusedNode = GetFocusedNode();
                focusedNode?.OnFocusChanged();
            }
        }

        protected virtual void RemoveNode(AccessibilityNode node)
        {
            if (node == null)
            {
                return;
            }

            foreach (var child in node.GetChildAccessibilityNodes())
            {
                RemoveNode(child);
            }

            accessibilityNodeMap.Remove(node.Id);
        }

        protected virtual void UpdateNodes(IDictionary<string, IValue> update)
        {
            if (update == null || !update.ContainsKey("nodes"))
            {
                return;
            }

            var nodes = update["nodes"].GetList();
            foreach (var node in nodes)
            {
                var nodeDictionary = node.GetDictionary();
                if (nodeDictionary == null)
                {
                    continue;
                }

                int nodeId = nodeDictionary["id"].GetInt();
                var accessibilityNode = GetNode(nodeId);

                // Create if it is a new one
                if (accessibilityNode != null)
                {
                    accessibilityNode.Update(nodeDictionary);
                }
                else
                {
                    accessibilityNode = new AccessibilityNode((FrameworkElement)Owner, nodeDictionary, this);
                    accessibilityNodeMap[nodeId] = accessibilityNode;
                }
            }
        }

        internal AccessibilityNode GetNode(int nodeId)
        {
            if (accessibilityNodeMap.ContainsKey(nodeId))
            {
                return accessibilityNodeMap[nodeId];
            }

            return null;
        }
    }
}

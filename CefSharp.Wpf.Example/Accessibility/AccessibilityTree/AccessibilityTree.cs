// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.Wpf.Example.Accessibility.Tree
{
    public class AccessibilityTree
    {
        private readonly Dictionary<int, AccessibilityNode> accessibilityNodeMap;

        public AccessibilityTree(int id)
        {
            accessibilityNodeMap = new Dictionary<int, AccessibilityNode>();

            Id = id;
            RootNodeId = -1;
            FocusedNodeId = -1;
        }

        public int Id { get; }

        public int RootNodeId { get; private set; }

        public int FocusedNodeId { get; private set; }

        public void Update(IDictionary<string, IValue> eventDictionary)
        {
            if (eventDictionary == null) return;
            if (!eventDictionary.ContainsKey("event_type")) return;

            var eventType = eventDictionary["event_type"].GetString();

            if (eventType == "layoutComplete")
            {
                UpdateLayout(eventDictionary);
            }

            if (eventType == "focus")
            {
                UpdateFocus(eventDictionary);
            }
        }

        private void UpdateLayout(IDictionary<string, IValue> eventDictionary)
        {
            if (!eventDictionary.ContainsKey("update")) return;

            var update = eventDictionary["update"].GetDictionary();
            if (update == null) return;

            var rootNodeChanged = false;

            // If a node is to be cleared
            if (update.ContainsKey("node_id_to_clear"))
            {
                var nodeIdToClear = update["node_id_to_clear"].GetInt();

                // reset root node if that is to be cleared
                if (nodeIdToClear == RootNodeId)
                {
                    RootNodeId = -1;
                }

                var node = GetNode(nodeIdToClear);
                RemoveNode(node);
            }

            if (update.ContainsKey("root_id"))
            {
                var newRootNodeId = update["root_id"].GetInt();

                if (RootNodeId != newRootNodeId)
                {
                    RootNodeId = newRootNodeId;
                    rootNodeChanged = true;
                }
            }

            UpdateNodes(update);

            if (rootNodeChanged)
            {
                OnRootNodeChanged();
            }
        }

        private void RemoveNode(AccessibilityNode node)
        {
            if (node == null) return;

            foreach (var child in node.GetChildren())
            {
                RemoveNode(child);
            }

            accessibilityNodeMap.Remove(node.Id);
        }

        private void UpdateFocus(IDictionary<string, IValue> eventDictionary)
        {
            var focusedNodeChanged = false;

            if (eventDictionary.ContainsKey("id"))
            {
                var newFocusedNodeId = eventDictionary["id"].GetInt();
                
                if (FocusedNodeId != newFocusedNodeId)
                {
                    FocusedNodeId = newFocusedNodeId;
                    focusedNodeChanged = true;
                }
            }

            if (eventDictionary.ContainsKey("update"))
            {
                var update = eventDictionary["update"].GetDictionary();
                UpdateNodes(update);
            }
            
            if (focusedNodeChanged)
            {
                var focusedNode = GetFocusedNode();
                focusedNode?.OnFocusChanged();
            }
        }

        private void UpdateNodes(IDictionary<string, IValue> update)
        {
            if (update == null) return;
            if (!update.ContainsKey("nodes")) return;

            var nodes = update["nodes"].GetList();

            foreach (var node in nodes)
            {
                var nodeDictionary = node.GetDictionary();
                if (nodeDictionary != null)
                {
                    var nodeId = nodeDictionary["id"].GetInt();
                    var accessibilityNode = GetNode(nodeId);
                    
                    // Create if it is a new one
                    if (accessibilityNode != null)
                    {
                        accessibilityNode.Update(nodeDictionary);
                    }
                    else
                    {
                        accessibilityNode = new AccessibilityNode(nodeDictionary, this);
                        accessibilityNodeMap[nodeId] = accessibilityNode;
                    }
                }
            }
        }

        public AccessibilityNode GetRootNode()
        {
            return GetNode(RootNodeId);
        }

        public AccessibilityNode GetFocusedNode()
        {
            return GetNode(FocusedNodeId);
        }

        internal AccessibilityNode GetNode(int nodeId)
        {
            if (accessibilityNodeMap.ContainsKey(nodeId))
            {
                return accessibilityNodeMap[nodeId];
            }

            return null;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            PrintNode(GetRootNode(), 0, result);

            return result.ToString();
        }

        private static void PrintNode(AccessibilityNode node, int level, StringBuilder result)
        {
            if (node == null)
            {
                return;
            }

            result.Append(new string('\t', level));
            result.AppendLine($"{node.Role}[{node.Id}] {node.Name} {node.Value} {(node.HasFocus() ? "<--" : "")}");

            foreach (var child in node.GetChildren())
            {
                PrintNode(child, level + 1, result);
            }
        }

        public event EventHandler RootNodeChanged;

        protected virtual void OnRootNodeChanged()
        {
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

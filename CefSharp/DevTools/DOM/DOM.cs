// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// This domain exposes DOM read/write operations. Each DOM Node is represented with its mirror object
    public partial class DOM
    {
        public DOM(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Collects class names for the node with given id and all of it's child nodes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CollectClassNamesFromSubtree(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.CollectClassNamesFromSubtree", dict);
            return result;
        }

        /// <summary>
        /// Creates a deep copy of the specified node and places it into the target container before the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CopyTo(int nodeId, int targetNodeId, int insertBeforeNodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"targetNodeId", targetNodeId}, {"insertBeforeNodeId", insertBeforeNodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.CopyTo", dict);
            return result;
        }

        /// <summary>
        /// Describes node given its id, does not require domain to be enabled. Does not start tracking any
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DescribeNode(int nodeId, int backendNodeId, string objectId, int depth, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, {"depth", depth}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.DescribeNode", dict);
            return result;
        }

        /// <summary>
        /// Scrolls the specified rect of the given node into view if not already visible.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ScrollIntoViewIfNeeded(int nodeId, int backendNodeId, string objectId, Rect rect)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, {"rect", rect}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.ScrollIntoViewIfNeeded", dict);
            return result;
        }

        /// <summary>
        /// Disables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.Disable", dict);
            return result;
        }

        /// <summary>
        /// Discards search results from the session with the given id. `getSearchResults` should no longer
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DiscardSearchResults(string searchId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"searchId", searchId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.DiscardSearchResults", dict);
            return result;
        }

        /// <summary>
        /// Enables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.Enable", dict);
            return result;
        }

        /// <summary>
        /// Focuses the given element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Focus(int nodeId, int backendNodeId, string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.Focus", dict);
            return result;
        }

        /// <summary>
        /// Returns attributes for the specified node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetAttributes(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetAttributes", dict);
            return result;
        }

        /// <summary>
        /// Returns boxes for the given node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetBoxModel(int nodeId, int backendNodeId, string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetBoxModel", dict);
            return result;
        }

        /// <summary>
        /// Returns quads that describe node position on the page. This method
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetContentQuads(int nodeId, int backendNodeId, string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetContentQuads", dict);
            return result;
        }

        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetDocument(int depth, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"depth", depth}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetDocument", dict);
            return result;
        }

        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetFlattenedDocument(int depth, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"depth", depth}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetFlattenedDocument", dict);
            return result;
        }

        /// <summary>
        /// Finds nodes with a given computed style in a subtree.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetNodesForSubtreeByStyle(int nodeId, System.Collections.Generic.IList<CSSComputedStyleProperty> computedStyles, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"computedStyles", computedStyles}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetNodesForSubtreeByStyle", dict);
            return result;
        }

        /// <summary>
        /// Returns node id at given location. Depending on whether DOM domain is enabled, nodeId is
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetNodeForLocation(int x, int y, bool includeUserAgentShadowDOM, bool ignorePointerEventsNone)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"x", x}, {"y", y}, {"includeUserAgentShadowDOM", includeUserAgentShadowDOM}, {"ignorePointerEventsNone", ignorePointerEventsNone}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetNodeForLocation", dict);
            return result;
        }

        /// <summary>
        /// Returns node's HTML markup.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetOuterHTML(int nodeId, int backendNodeId, string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetOuterHTML", dict);
            return result;
        }

        /// <summary>
        /// Returns the id of the nearest ancestor that is a relayout boundary.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetRelayoutBoundary(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetRelayoutBoundary", dict);
            return result;
        }

        /// <summary>
        /// Returns search results from given `fromIndex` to given `toIndex` from the search with the given
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetSearchResults(string searchId, int fromIndex, int toIndex)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"searchId", searchId}, {"fromIndex", fromIndex}, {"toIndex", toIndex}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetSearchResults", dict);
            return result;
        }

        /// <summary>
        /// Hides any highlight.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HideHighlight()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.HideHighlight", dict);
            return result;
        }

        /// <summary>
        /// Highlights DOM node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HighlightNode()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.HighlightNode", dict);
            return result;
        }

        /// <summary>
        /// Highlights given rectangle.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HighlightRect()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.HighlightRect", dict);
            return result;
        }

        /// <summary>
        /// Marks last undoable state.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> MarkUndoableState()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.MarkUndoableState", dict);
            return result;
        }

        /// <summary>
        /// Moves node into the new container, places it before the given anchor.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> MoveTo(int nodeId, int targetNodeId, int insertBeforeNodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"targetNodeId", targetNodeId}, {"insertBeforeNodeId", insertBeforeNodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.MoveTo", dict);
            return result;
        }

        /// <summary>
        /// Searches for a given string in the DOM tree. Use `getSearchResults` to access search results or
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PerformSearch(string query, bool includeUserAgentShadowDOM)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"query", query}, {"includeUserAgentShadowDOM", includeUserAgentShadowDOM}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.PerformSearch", dict);
            return result;
        }

        /// <summary>
        /// Requests that the node is sent to the caller given its path. // FIXME, use XPath
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PushNodeByPathToFrontend(string path)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"path", path}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.PushNodeByPathToFrontend", dict);
            return result;
        }

        /// <summary>
        /// Requests that a batch of nodes is sent to the caller given their backend node ids.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PushNodesByBackendIdsToFrontend(int backendNodeIds)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"backendNodeIds", backendNodeIds}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.PushNodesByBackendIdsToFrontend", dict);
            return result;
        }

        /// <summary>
        /// Executes `querySelector` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> QuerySelector(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"selector", selector}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.QuerySelector", dict);
            return result;
        }

        /// <summary>
        /// Executes `querySelectorAll` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> QuerySelectorAll(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"selector", selector}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.QuerySelectorAll", dict);
            return result;
        }

        /// <summary>
        /// Re-does the last undone action.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Redo()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.Redo", dict);
            return result;
        }

        /// <summary>
        /// Removes attribute with given name from an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveAttribute(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"name", name}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.RemoveAttribute", dict);
            return result;
        }

        /// <summary>
        /// Removes node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveNode(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.RemoveNode", dict);
            return result;
        }

        /// <summary>
        /// Requests that children of the node with given id are returned to the caller in form of
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RequestChildNodes(int nodeId, int depth, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"depth", depth}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.RequestChildNodes", dict);
            return result;
        }

        /// <summary>
        /// Requests that the node is sent to the caller given the JavaScript node object reference. All
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RequestNode(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.RequestNode", dict);
            return result;
        }

        /// <summary>
        /// Resolves the JavaScript node object for a given NodeId or BackendNodeId.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResolveNode(int nodeId, int backendNodeId, string objectGroup, int executionContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectGroup", objectGroup}, {"executionContextId", executionContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.ResolveNode", dict);
            return result;
        }

        /// <summary>
        /// Sets attribute for an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAttributeValue(int nodeId, string name, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"name", name}, {"value", value}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetAttributeValue", dict);
            return result;
        }

        /// <summary>
        /// Sets attributes on element with given id. This method is useful when user edits some existing
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAttributesAsText(int nodeId, string text, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"text", text}, {"name", name}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetAttributesAsText", dict);
            return result;
        }

        /// <summary>
        /// Sets files for the given file input element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetFileInputFiles(string files, int nodeId, int backendNodeId, string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"files", files}, {"nodeId", nodeId}, {"backendNodeId", backendNodeId}, {"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetFileInputFiles", dict);
            return result;
        }

        /// <summary>
        /// Sets if stack traces should be captured for Nodes. See `Node.getNodeStackTraces`. Default is disabled.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetNodeStackTracesEnabled(bool enable)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enable", enable}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetNodeStackTracesEnabled", dict);
            return result;
        }

        /// <summary>
        /// Gets stack traces associated with a Node. As of now, only provides stack trace for Node creation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetNodeStackTraces(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetNodeStackTraces", dict);
            return result;
        }

        /// <summary>
        /// Returns file information for the given
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetFileInfo(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetFileInfo", dict);
            return result;
        }

        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetInspectedNode(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetInspectedNode", dict);
            return result;
        }

        /// <summary>
        /// Sets node name for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetNodeName(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"name", name}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetNodeName", dict);
            return result;
        }

        /// <summary>
        /// Sets node value for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetNodeValue(int nodeId, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"value", value}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetNodeValue", dict);
            return result;
        }

        /// <summary>
        /// Sets node HTML markup, returns new node id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetOuterHTML(int nodeId, string outerHTML)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"outerHTML", outerHTML}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.SetOuterHTML", dict);
            return result;
        }

        /// <summary>
        /// Undoes the last performed action.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Undo()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.Undo", dict);
            return result;
        }

        /// <summary>
        /// Returns iframe node that owns iframe with the given domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetFrameOwner(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.GetFrameOwner", dict);
            return result;
        }
    }
}
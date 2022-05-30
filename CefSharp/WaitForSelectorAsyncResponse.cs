// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// WaitForSelectorAsyncResponse
    /// </summary>
    public class WaitForSelectorAsyncResponse
    {
        /// <summary>
        /// Element Id
        /// </summary>
        public string ElementId { get; private set; }
        /// <summary>
        /// Tag Name
        /// </summary>
        public string TagName { get; private set; }
        /// <summary>
        /// True if the javascript was executed successfully
        /// </summary>
        public bool Success { get; private set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// True if the element was added to the DOM otherwise false if it was removed
        /// </summary>
        public bool ElementAdded { get; private set; }

        public WaitForSelectorAsyncResponse(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public WaitForSelectorAsyncResponse(string elementId, string tagName,  bool elementAdded)
        {
            Success = true;
            ErrorMessage = string.Empty;

            ElementId = elementId;
            TagName = tagName;
            ElementAdded = elementAdded;
        }
    }
}

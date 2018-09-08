// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CefSharp
{
    /// <summary>
    /// Represents the tag name and attribute data belonging to a node in the
    /// browser's DOM.
    /// </summary>
    public interface IDomNode : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Get the value of an attribute.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute value to get.
        /// </param>
        /// <returns>
        /// The attribute value if the name exists in the DomNode's attributes.
        /// Null if the name does not exist.
        /// </returns>
        string this[string attributeName] { get; }

        /// <summary>
        /// The name of the HTML element.
        /// </summary>
        string TagName { get; }

        /// <summary>
        /// Get a read only list of the attribute names.
        /// </summary>
        ReadOnlyCollection<string> AttributeNames { get; }

        /// <summary>
        /// Determine if the DomNode has the requested attribute.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute value.
        /// </param>
        /// <returns>
        /// True if the attribute exists in the DomNode, false if it does not.
        /// </returns>
        bool HasAttribute(string attributeName);
    }
}

// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// Represents a node in the browser's DOM.
    /// </summary>
    public class DomNode : IDomNode
    {
        private readonly IDictionary<string, string> _attributes;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="attributes">The attributes.</param>
        public DomNode(string tagName, IDictionary<string, string> attributes)
        {
            TagName = tagName;
            _attributes = attributes;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_attributes != null)
            {
                foreach (var pair in _attributes)
                {
                    sb.AppendFormat("{0}{1}:'{2}'", 0 < sb.Length ? ", " : String.Empty, pair.Key, pair.Value);
                }
            }

            if (!String.IsNullOrWhiteSpace(TagName))
            {
                sb.Insert(0, String.Format("{0} ", TagName));
            }

            if (sb.Length < 1)
            {
                return base.ToString();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the value of an attribute.
        /// </summary>
        /// <param name="name">The name of the attribute value to get.</param>
        /// <returns>
        /// The attribute value if the name exists in the DomNode's attributes. Null if the name does not exist.
        /// </returns>
        public string this[string name]
        {
            get
            {
                if (_attributes == null || _attributes.Count < 1 || !_attributes.ContainsKey(name))
                {
                    return null;
                }

                return _attributes[name];
            }
        }

        /// <summary>
        /// The name of the HTML element.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string TagName { get; private set; }

        /// <summary>
        /// Get a read only list of the attribute names.
        /// </summary>
        /// <value>
        /// A list of names of the attributes.
        /// </value>
        public ReadOnlyCollection<string> AttributeNames
        {
            get
            {
                if (_attributes == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return Array.AsReadOnly<string>(_attributes.Keys.ToArray());
            }
        }

        /// <summary>
        /// Determine if the DomNode has the requested attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute value.</param>
        /// <returns>
        /// True if the attribute exists in the DomNode, false if it does not.
        /// </returns>
        public bool HasAttribute(string attributeName)
        {
            if (_attributes == null)
            {
                return false;
            }

            return _attributes.ContainsKey(attributeName);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            if (_attributes == null)
            {
                return new Dictionary<string, string>().GetEnumerator();
            }

            return _attributes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

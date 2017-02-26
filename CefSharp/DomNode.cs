// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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

        public DomNode (string tagName, IDictionary<string, string> attributes)
        {
            TagName = tagName;
            _attributes = attributes;
        }

        public override string ToString ()
        {
            var sb = new StringBuilder ();
            if (_attributes != null)
            {
                foreach (var pair in _attributes)
                {
                    sb.AppendFormat ("{0}{1}:'{2}'", 0 < sb.Length ? ", " : String.Empty, pair.Key, pair.Value);
                }
            }

            if (!String.IsNullOrWhiteSpace (TagName))
            {
                sb.Insert (0, String.Format ("{0} ", TagName));
            }

            if (sb.Length < 1)
            {
                return base.ToString ();
            }

            return sb.ToString ();
        }

        public string this[string name]
        {
            get
            {
                if (_attributes == null || _attributes.Count < 1 || !_attributes.ContainsKey (name))
                {
                    return null;
                }

                return _attributes[name];
            }
        }

        public string TagName { get; private set; }

        public ReadOnlyCollection<string> AttributeNames
        {
            get
            {
                if (_attributes == null)
                {
                    return new ReadOnlyCollection<string> (new List<string> ());
                }

                return Array.AsReadOnly<string> (_attributes.Keys.ToArray ());
            }
        }

        public bool HasAttribute (string attributeName)
        {
            if (_attributes == null)
            {
                return false;
            }

            return _attributes.ContainsKey (attributeName);
        }

        public IEnumerator<KeyValuePair<string, string>>GetEnumerator()
        {
            if (_attributes == null)
            {
                return new Dictionary<string, string>().GetEnumerator();
            }

            return _attributes.GetEnumerator ();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator ();
        }
    }
}

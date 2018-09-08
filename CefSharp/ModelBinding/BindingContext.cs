// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Model binding context object
    /// </summary>
    public class BindingContext
    {
        /// <summary>
        /// Binding destination type
        /// </summary>
        public Type DestinationType { get; set; }

        /// <summary>
        /// The generic type of a collection is only used when DestinationType is a enumerable.
        /// </summary>
        public Type GenericType { get; set; }

        /// <summary>
        /// The current model object (or null for body deserialization)
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// DestinationType properties that are not black listed
        /// </summary>
        public IEnumerable<BindingMemberInfo> ValidModelBindingMembers { get; set; }

        /// <summary>
        /// The incoming data fields
        /// </summary>
        public object Object { get; set; }
    }
}
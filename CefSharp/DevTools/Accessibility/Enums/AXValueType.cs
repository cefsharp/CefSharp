// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// Enum of possible property types.
    /// </summary>
    public enum AXValueType
    {
        /// <summary>
        /// boolean
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("boolean"))]
        Boolean,
        /// <summary>
        /// tristate
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("tristate"))]
        Tristate,
        /// <summary>
        /// booleanOrUndefined
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("booleanOrUndefined"))]
        BooleanOrUndefined,
        /// <summary>
        /// idref
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("idref"))]
        Idref,
        /// <summary>
        /// idrefList
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("idrefList"))]
        IdrefList,
        /// <summary>
        /// integer
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("integer"))]
        Integer,
        /// <summary>
        /// node
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("node"))]
        Node,
        /// <summary>
        /// nodeList
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("nodeList"))]
        NodeList,
        /// <summary>
        /// number
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("number"))]
        Number,
        /// <summary>
        /// string
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("string"))]
        String,
        /// <summary>
        /// computedString
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("computedString"))]
        ComputedString,
        /// <summary>
        /// token
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("token"))]
        Token,
        /// <summary>
        /// tokenList
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("tokenList"))]
        TokenList,
        /// <summary>
        /// domRelation
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("domRelation"))]
        DomRelation,
        /// <summary>
        /// role
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("role"))]
        Role,
        /// <summary>
        /// internalRole
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("internalRole"))]
        InternalRole,
        /// <summary>
        /// valueUndefined
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("valueUndefined"))]
        ValueUndefined
    }
}
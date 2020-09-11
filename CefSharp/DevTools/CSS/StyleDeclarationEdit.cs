// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// A descriptor of operation to mutate style declaration text.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StyleDeclarationEdit : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The css style sheet identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (true))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// The range of the style text in the enclosing stylesheet.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("range"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.SourceRange Range
        {
            get;
            set;
        }

        /// <summary>
        /// New style text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public string Text
        {
            get;
            set;
        }
    }
}
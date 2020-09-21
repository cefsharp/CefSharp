// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Browser window bounds information
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Bounds : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The offset from the left edge of the screen to the window in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("left"), IsRequired = (false))]
        public int? Left
        {
            get;
            set;
        }

        /// <summary>
        /// The offset from the top edge of the screen to the window in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("top"), IsRequired = (false))]
        public int? Top
        {
            get;
            set;
        }

        /// <summary>
        /// The window width in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (false))]
        public int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// The window height in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (false))]
        public int? Height
        {
            get;
            set;
        }

        public CefSharp.DevTools.Browser.WindowState? WindowState
        {
            get
            {
                return (CefSharp.DevTools.Browser.WindowState? )(StringToEnum(typeof(CefSharp.DevTools.Browser.WindowState? ), windowState));
            }

            set
            {
                windowState = (EnumToString(value));
            }
        }

        /// <summary>
        /// The window state. Default to normal.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("windowState"), IsRequired = (false))]
        internal string windowState
        {
            get;
            set;
        }
    }
}
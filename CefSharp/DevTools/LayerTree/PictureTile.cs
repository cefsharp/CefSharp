// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// Serialized fragment of layer picture along with its offset within the layer.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PictureTile : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Offset from owning layer left boundary
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("x"), IsRequired = (true))]
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Offset from owning layer top boundary
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("y"), IsRequired = (true))]
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// Base64-encoded snapshot data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("picture"), IsRequired = (true))]
        public byte[] Picture
        {
            get;
            set;
        }
    }
}
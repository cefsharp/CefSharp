﻿// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptCallback
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int BrowserId { get; set; }

        [DataMember]
        public long FrameId { get; set; }
    }
}

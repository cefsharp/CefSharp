// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Example
{
    public class ResponseFilter : IResponseFilter
    {
        bool IResponseFilter.InitFilter()
        {
            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, long dataInSize, out long dataInRead, Stream dataOut, long dataOutSize, out long dataOutWritten)
        {
            dataInRead = dataInSize;
            dataOutWritten = Math.Min(dataInRead, dataOutSize);

            dataIn.CopyTo(dataOut);

            return FilterStatus.Done;
        }
    }
}

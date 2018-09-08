// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to filter resource response content.
    /// The methods of this class will be called on the CEF IO thread. 
    /// </summary>
    public interface IResponseFilter : IDisposable
    {
        /// <summary>
        /// Initialize the response filter. Will only be called a single time.
        /// The filter will not be installed if this method returns false.
        /// </summary>
        /// <returns>The filter will not be installed if this method returns false.</returns>
        bool InitFilter();

        /// <summary>
        /// Called to filter a chunk of data.
        /// This method will be called repeatedly until there is no more data to filter (resource response is complete),
        /// dataInRead matches dataIn.Length (all available pre-filter bytes have been read), and the method
        /// returns FilterStatus.Done or FilterStatus.Error.
        /// </summary>
        /// <param name="dataIn">is a Stream wrapping the underlying input buffer containing pre-filter data. Can be null.</param>
        /// <param name="dataInRead">Set to the number of bytes that were read from dataIn</param>
        /// <param name="dataOut">is a Stream wrapping the underlying output buffer that can accept filtered output data.
        /// Check dataOut.Length for maximum buffer size</param>
        /// <param name="dataOutWritten">Set to the number of bytes that were written into dataOut</param>
        /// <returns>If some or all of the pre-filter data was read successfully but more data is needed in order
        /// to continue filtering (filtered output is pending) return FilterStatus.NeedMoreData. If some or all of the pre-filter
        /// data was read successfully and all available filtered output has been written return FilterStatus.Done. If an error
        /// occurs during filtering return FilterStatus.Error. </returns>
        /// <remarks>Do not keep a reference to the buffers(Streams) passed to this method.</remarks>
        FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten);
    }
}

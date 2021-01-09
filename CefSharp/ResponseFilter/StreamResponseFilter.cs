// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.ResponseFilter
{
    /// <summary>
    /// StreamResponseFilter - copies all data from IResponseFilter.Filter
    /// to the provided Stream. The <see cref="Stream"/> must be writable, no data will be copied otherwise.
    /// The StreamResponseFilter will release it's reference (set to null) to the <see cref="Stream"/> when it's Disposed.
    /// </summary>
    public class StreamResponseFilter : IResponseFilter
    {
        private Stream responseStream;

        /// <summary>
        /// StreamResponseFilter constructor
        /// </summary>
        /// <param name="stream">a writable stream</param>
        public StreamResponseFilter(Stream stream)
        {
            responseStream = stream;
        }

        bool IResponseFilter.InitFilter()
        {
            return responseStream != null && responseStream.CanWrite;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;

                return FilterStatus.Done;
            }

            //Calculate how much data we can read, in some instances dataIn.Length is
            //greater than dataOut.Length
            dataInRead = Math.Min(dataIn.Length, dataOut.Length);
            dataOutWritten = dataInRead;

            var readBytes = new byte[dataInRead];
            dataIn.Read(readBytes, 0, readBytes.Length);
            dataOut.Write(readBytes, 0, readBytes.Length);

            //Write buffer to the memory stream
            responseStream.Write(readBytes, 0, readBytes.Length);

            //If we read less than the total amount available then we need
            //return FilterStatus.NeedMoreData so we can then write the rest
            if (dataInRead < dataIn.Length)
            {
                return FilterStatus.NeedMoreData;
            }

            return FilterStatus.Done;
        }

        void IDisposable.Dispose()
        {
            responseStream = null;
        }
    }
}

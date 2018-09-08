// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CefSharp.Example.Filters
{
    /// <summary>
    /// Experimental filter that buffers all the data before any sort of processing
    /// This has not been production tested, no performance analysis has been done.
    /// The idea here being that we can buffer all the data, then perform some sort of
    /// post processing (replacing/adding/removing text).
    /// </summary>
    public class ExperimentalStreamResponseFilter : IResponseFilter
    {
        private List<byte> dataOutBuffer = new List<byte>();

        bool IResponseFilter.InitFilter()
        {
            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;

                var maxWrite = Math.Min(dataOutBuffer.Count, dataOut.Length);

                //Write the maximum portion that fits in dataOut.
                if (maxWrite > 0)
                {
                    dataOut.Write(dataOutBuffer.ToArray(), 0, (int)maxWrite);
                    dataOutWritten += maxWrite;
                }

                //If dataOutBuffer is bigger than dataOut then we'll write the
                // data on the second pass
                if (maxWrite < dataOutBuffer.Count)
                {
                    // Need to write more bytes than will fit in the output buffer. 
                    // Remove the bytes that were written already
                    dataOutBuffer.RemoveRange(0, (int)(maxWrite - 1));

                    return FilterStatus.NeedMoreData;
                }

                //All data was written, so we clear the buffer and return FilterStatus.Done
                dataOutBuffer.Clear();

                return FilterStatus.Done;
            }

            //We're going to read all of dataIn
            dataInRead = dataIn.Length;

            var dataInBuffer = new byte[(int)dataIn.Length];
            dataIn.Read(dataInBuffer, 0, dataInBuffer.Length);

            //Add all the bytes to the dataOutBuffer
            dataOutBuffer.AddRange(dataInBuffer);

            dataOutWritten = 0;

            //The assumption here is dataIn is smaller than dataOut then this will be the last of the
            //data to read and we can start processing, this is not a production tested assumption
            if (dataIn.Length < dataOut.Length)
            {
                var bytes = ReplaceTextInBufferedData(dataOutBuffer.ToArray());

                //Clear the buffer and add the processed data, it's possible
                //that with our processing the data has become to large to fit in dataOut
                //So we'll have to write the data in multiple passes in that case
                dataOutBuffer.Clear();
                dataOutBuffer.AddRange(bytes);

                var maxWrite = Math.Min(dataOutBuffer.Count, dataOut.Length);

                //Write the maximum portion that fits in dataOut.
                if (maxWrite > 0)
                {
                    dataOut.Write(dataOutBuffer.ToArray(), 0, (int)maxWrite);
                    dataOutWritten += maxWrite;
                }

                //If dataOutBuffer is bigger than dataOut then we'll write the
                // data on the second pass
                if (maxWrite < dataOutBuffer.Count)
                {
                    // Need to write more bytes than will fit in the output buffer. 
                    // Remove the bytes that were written already
                    dataOutBuffer.RemoveRange(0, (int)(maxWrite - 1));

                    return FilterStatus.NeedMoreData;
                }

                //All data was written, so we clear the buffer and return FilterStatus.Done
                dataOutBuffer.Clear();

                return FilterStatus.Done;
            }
            else
            {
                //We haven't got all of our dataIn yet, so we keep buffering so that when it's finished
                //we can process the buffer, replace some words etc and then write it all out.
                return FilterStatus.NeedMoreData;
            }
        }

        void IDisposable.Dispose()
        {

        }

        /// <summary>
        /// If your copy and pasting this example, then this is the part your most likely to change
        /// (unless your fixing a bug)
        /// </summary>
        /// <param name="bytes">buffered data</param>
        /// <returns>post processed data, text added/replaced/etc</returns>
        private byte[] ReplaceTextInBufferedData(byte[] bytes)
        {
            //NOTE: This is a quick and dirty example of manipulating the data as a string
            //By all means contribute a better working example, this is good enough for my demo purpose at this point in time.
            var dataString = Encoding.UTF8.GetString(bytes);

            dataString = dataString.Replace("CefSharp", "CEF#");

            return Encoding.UTF8.GetBytes(dataString);
        }
    }
}

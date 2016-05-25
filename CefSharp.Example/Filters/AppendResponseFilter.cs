// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.Example.Filters
{
    public class AppendResponseFilter : IResponseFilter
    {
        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// Overflow from the output buffer.
        /// </summary>
        private List<byte> overflow = new List<byte>();

        public AppendResponseFilter(string stringToAppend)
        {
            //Add the encoded string into the overflow.
            overflow.AddRange(encoding.GetBytes(stringToAppend));
        }

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

                return FilterStatus.Done;
            }

            //We'll read all the data
            dataInRead = dataIn.Length;
            dataOutWritten = Math.Min(dataInRead, dataOut.Length);

            if(dataIn.Length > 0)
            { 
                //Copy all the existing data first
                dataIn.CopyTo(dataOut);
            }

            // If we have overflow data then write it.
            if (overflow.Count > 0)
            {
                // Number of bytes remaining in the output buffer.
                var remainingSpace = dataOut.Length - dataOutWritten;
                // Maximum number of bytes we can write into the output buffer.
                var maxWrite = Math.Min(overflow.Count, remainingSpace);

                // Write the maximum portion that fits in the output buffer.
                if (maxWrite > 0)
                {
                    dataOut.Write(overflow.ToArray(), 0, (int)maxWrite);
                    dataOutWritten += maxWrite;
                }

                if(maxWrite == 0 && overflow.Count > 0)
                {
                    //We haven't yet got space to append our data
                    return FilterStatus.NeedMoreData;
                }

                if (maxWrite < overflow.Count)
                {
                    // Need to write more bytes than will fit in the output buffer. 
                    // Remove the bytes that were written already
                    overflow.RemoveRange(0, (int)(maxWrite - 1));
                }
                else
                {
                    overflow.Clear();
                }
            } 

            if(overflow.Count > 0)
            {
                return FilterStatus.NeedMoreData; 
            }

            return FilterStatus.Done;
        }

        public void Dispose()
        {
            
        }
    }
}

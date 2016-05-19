// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.Example.Filters
{
    public class FindReplaceResponseFilter : IResponseFilter
    {
        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// String to find
        /// </summary>
        private string findString;

        /// <summary>
        /// String used for replacement
        /// </summary>
        private string replacementString;

        /// <summary>
        /// The portion of the find string that is currently matching.
        /// </summary>
        private int findMatchOffset;

        /// <summary>
        /// Overflow from the output buffer.
        /// </summary>
        private List<byte> overflow = new List<byte>();

        /// <summary>
        /// Number of times the the string was found/replaced.
        /// </summary>
        private long replaceCount;

        public FindReplaceResponseFilter(string find, string replacement)
        {
            findString = find;
            replacementString = replacement;
        }

        bool IResponseFilter.InitFilter()
        {
            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            // All data will be read.
            dataInRead = dataIn == null ? 0 : dataIn.Length;
            dataOutWritten = 0;

            // Write overflow then reset
            if (overflow.Count > 0)
            {
                // Write the overflow from last time.
                WriteOverflow(dataOut, ref dataOutWritten);
            }

            // Evaluate each character in the input buffer. Track how many characters in
            // a row match findString. If findString is completely matched then write
            // replacement. Otherwise, write the input characters as-is.
            for (var i = 0; i < dataInRead; ++i)
            {
                var readByte = (byte)dataIn.ReadByte();
                var charForComparison = Convert.ToChar(readByte);

                if (charForComparison == findString[findMatchOffset])
                {
                    //We have a match, increment the counter
                    findMatchOffset++;

                    // If all characters match the string specified
                    if (findMatchOffset == findString.Length)
                    {
                        // Complete match of the find string. Write the replace string.
                        var replaceString = ++replaceCount + ". " + replacementString;
                        WriteString(replaceString, replaceString.Length, dataOut, ref dataOutWritten);

                        // Start over looking for a match.
                        findMatchOffset = 0;
                    }
                    continue;
                }

                // Character did not match the find string.
                if (findMatchOffset > 0)
                {
                    // Write the portion of the find string that has matched so far.
                    WriteString(findString, findMatchOffset, dataOut, ref dataOutWritten);

                    // Start over looking for a match.
                    findMatchOffset = 0;
                }

                // Write the current character.
                WriteSingleByte(readByte, dataOut, ref dataOutWritten);
            }

            if(overflow.Count > 0)
            {
                //If we end up with overflow data then we'll need to return NeedMoreData
                // On the next pass the data will be written, then the next batch will be processed.
                return FilterStatus.NeedMoreData;
            }

            // If a match is currently in-progress we need more data. Otherwise, we're
            // done.
            return findMatchOffset > 0 ? FilterStatus.NeedMoreData : FilterStatus.Done;
        }

        private void WriteOverflow(Stream dataOut, ref long dataOutWritten)
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

        private void WriteString(string str, int stringSize, Stream dataOut, ref long dataOutWritten)
        {
            // Number of bytes remaining in the output buffer.
            var remainingSpace = dataOut.Length - dataOutWritten;
            // Maximum number of bytes we can write into the output buffer.
            var maxWrite = Math.Min(stringSize, remainingSpace);

            // Write the maximum portion that fits in the output buffer.
            if (maxWrite > 0)
            {
                var bytes = encoding.GetBytes(str);
                dataOut.Write(bytes, 0, (int)maxWrite);
                dataOutWritten += maxWrite;
            }

            if (maxWrite < stringSize)
            {
                // Need to write more bytes than will fit in the output buffer. Store the
                // remainder in the overflow buffer.
                overflow.AddRange(encoding.GetBytes(str.Substring((int)maxWrite, (int)(stringSize - maxWrite))));
            }
        }

        private void WriteSingleByte(byte data, Stream dataOut, ref long dataOutWritten)
        {
            // Number of bytes remaining in the output buffer.
            var remainingSpace = dataOut.Length - dataOutWritten;

            // Write the byte to the buffer or add it to the overflow
            if (remainingSpace > 0)
            {
                dataOut.WriteByte(data);
                dataOutWritten += 1;
            }
            else
            {
                // Need to write more bytes than will fit in the output buffer. Store the
                // remainder in the overflow buffer.
                overflow.Add(data);
            }
        }

        public void Dispose()
        {
            
        }
    }
}

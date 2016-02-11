// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Example.Filters
{
    public class FindReplaceResponseFilter : IResponseFilter
    {
        private const string kFindString = "REPLACE_THIS_STRING";
        private const string kReplaceString = "This is the replaced string!";

        // The portion of the find string that is currently matching.
        private long find_match_offset_;

        // The likely amount of overflow.
        private long replace_overflow_size_;

        // Overflow from the output buffer.
        private string overflow_;

        // Number of times the the string was found/replaced.
        private long replace_count_;

        bool IResponseFilter.InitFilter()
        {
            long find_size = kFindString.Length - 1;
            long replace_size = kReplaceString.Length - 1;

            // Determine a reasonable amount of space for find/replace overflow. For
            // example, the amount of space required if the search string is
            // found/replaced 10 times (plus space for the count).
            if (replace_size > find_size)
                replace_overflow_size_ = (replace_size - find_size + 3) * 10;

            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, long dataInSize, out long dataInRead, Stream dataOut, long dataOutSize, out long dataOutWritten)
        {
            //DCHECK((data_in_size == 0U && !data_in) || (data_in_size > 0U && data_in));
            //DCHECK_EQ(data_in_read, 0U);
            //DCHECK(data_out);
            //DCHECK_GT(data_out_size, 0U);
            //DCHECK_EQ(data_out_written, 0U);

            // All data will be read.
            dataInRead = dataInSize;
            dataOutWritten = 0;

            int find_size = kFindString.Length - 1;

            //const char* data_in_ptr = static_cast<char*>(data_in);
            //char* data_out_ptr = static_cast<char*>(data_out);

            // Reset the overflow.
            string old_overflow = "";
            if (!string.IsNullOrEmpty(overflow_))
            {
                old_overflow = overflow_;
                overflow_ = "";
            }
            
            if (!string.IsNullOrEmpty(old_overflow))
            {
                // Write the overflow from last time.
                Write(old_overflow, old_overflow.Length, dataOut, dataOutSize, ref dataOutWritten);
            }

            dataIn.Position = 0;

            // Evaluate each character in the input buffer. Track how many characters in
            // a row match kFindString. If kFindString is completely matched then write
            // kReplaceString. Otherwise, write the input characters as-is.
            for (var i = 0; i < dataInSize; ++i)
            {
                var charForComparison = Convert.ToChar(dataIn.ReadByte());

                if (charForComparison == kFindString[(int)find_match_offset_])
                {
                    // Matched the next character in the find string.
                    if (++find_match_offset_ == find_size)
                    {
                        // Complete match of the find string. Write the replace string.
                        var replace_str = ++replace_count_ + ". " + kReplaceString;
                        Write(replace_str, replace_str.Length, dataOut, dataOutSize, ref dataOutWritten);

                        // Start over looking for a match.
                        find_match_offset_ = 0;
                    }
                    continue;
                }

                // Character did not match the find string.
                if (find_match_offset_ > 0)
                {
                    // Write the portion of the find string that has matched so far.
                    Write(kFindString, find_match_offset_, dataOut, dataOutSize, ref dataOutWritten);

                    // Start over looking for a match.
                    find_match_offset_ = 0;
                }

                // Write the current character.
                Write(Convert.ToString(charForComparison), 1, dataOut, dataOutSize, ref dataOutWritten);
            }

            // If a match is currently in-progress we need more data. Otherwise, we're
            // done.
            return find_match_offset_ > 0 ? FilterStatus.NeedMoreData : FilterStatus.Done;
        }

        private void Write(string str, long str_size, Stream data_out_ptr, long data_out_size, ref long data_out_written)
        {
            // Number of bytes remaining in the output buffer.
            var remaining_space = data_out_size - data_out_written;
            // Maximum number of bytes we can write into the output buffer.
            var max_write = Math.Min(str_size, remaining_space);

            // Write the maximum portion that fits in the output buffer.
            if (max_write == 1)
            {
                // Small optimization for single character writes.
                //*data_out_ptr = str[0];
                //data_out_ptr += 1;
                //data_out_written += 1;
            }
            else if (max_write > 1)
            {
                //data_out_ptr.Write()
                //memcpy(data_out_ptr, str, max_write);
                //data_out_ptr += max_write;
                //data_out_written += max_write;
            }

            if (max_write < str_size)
            {
                // Need to write more bytes than will fit in the output buffer. Store the
                // remainder in the overflow buffer.
                overflow_ += str.Substring((int)max_write, (int)(str_size - max_write));
            }
        }
    }
}

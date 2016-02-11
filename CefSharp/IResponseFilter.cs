// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    public interface IResponseFilter
    {
        /// <summary>
        /// Initialize the response filter. Will only be called a single time.
        /// The filter will not be installed if this method returns false.
        /// </summary>
        /// <returns>The filter will not be installed if this method returns false.</returns>
        bool InitFilter();

        /// <summary>
        /// Called to filter a chunk of data. |data_in| is the input buffer containing
        /// |data_in_size| bytes of pre-filter data (|data_in| will be NULL if
        /// |data_in_size| is zero). |data_out| is the output buffer that can accept up
        /// to |data_out_size| bytes of filtered output data. Set |data_in_read| to the
        /// number of bytes that were read from |data_in|. Set |data_out_written| to
        /// the number of bytes that were written into |data_out|. If some or all of
        /// the pre-filter data was read successfully but more data is needed in order
        /// to continue filtering (filtered output is pending) return
        /// RESPONSE_FILTER_NEED_MORE_DATA. If some or all of the pre-filter data was
        /// read successfully and all available filtered output has been written return
        /// RESPONSE_FILTER_DONE. If an error occurs during filtering return
        /// RESPONSE_FILTER_ERROR. This method will be called repeatedly until there is
        /// no more data to filter (resource response is complete), |data_in_read|
        /// matches |data_in_size| (all available pre-filter bytes have been read), and
        /// the method returns RESPONSE_FILTER_DONE or RESPONSE_FILTER_ERROR. Do not
        /// keep a reference to the buffers passed to this method.
        /// optional_param=data_in,default_retval=RESPONSE_FILTER_ERROR
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="dataInRead"></param>
        /// <param name="dataOut"></param>
        /// <param name="dataOutWritten"></param>
        /// <returns></returns>
        FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten);
    }
}

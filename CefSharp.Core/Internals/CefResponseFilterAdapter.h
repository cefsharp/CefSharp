// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_response_filter.h"

using namespace System::IO;

namespace CefSharp
{
    namespace Internals
    {
        private class CefResponseFilterAdapter : public CefResponseFilter
        {
        private:
            gcroot<IResponseFilter^> _filter;

        public:
            CefResponseFilterAdapter(IResponseFilter^ filter) : 
                _filter(filter)
            {
            
            }

            ~CefResponseFilterAdapter()
            {
                delete _filter;
                _filter = nullptr;
            }

            virtual bool InitFilter() OVERRIDE
            {
                return _filter->InitFilter();
            }

            // Called to filter a chunk of data. |data_in| is the input buffer containing
            // |data_in_size| bytes of pre-filter data (|data_in| will be NULL if
            // |data_in_size| is zero). |data_out| is the output buffer that can accept up
            // to |data_out_size| bytes of filtered output data. Set |data_in_read| to the
            // number of bytes that were read from |data_in|. Set |data_out_written| to
            // the number of bytes that were written into |data_out|. If some or all of
            // the pre-filter data was read successfully but more data is needed in order
            // to continue filtering (filtered output is pending) return
            // RESPONSE_FILTER_NEED_MORE_DATA. If some or all of the pre-filter data was
            // read successfully and all available filtered output has been written return
            // RESPONSE_FILTER_DONE. If an error occurs during filtering return
            // RESPONSE_FILTER_ERROR. This method will be called repeatedly until there is
            // no more data to filter (resource response is complete), |data_in_read|
            // matches |data_in_size| (all available pre-filter bytes have been read), and
            // the method returns RESPONSE_FILTER_DONE or RESPONSE_FILTER_ERROR. Do not
            // keep a reference to the buffers passed to this method.
            /*--cef(optional_param=data_in,default_retval=RESPONSE_FILTER_ERROR)--*/
            virtual FilterStatus Filter(void* dataIn, size_t dataInSize, size_t& dataInRead, void* dataOut, size_t dataOutSize, size_t& dataOutWritten) OVERRIDE
            {
                Int64 dataInReadPtr = 0;
                Int64 dataOutWrittenPtr = 0;
                CefSharp::FilterStatus status;

                UnmanagedMemoryStream writeStream((Byte*)dataOut, (Int64)dataOutSize, (Int64)dataOutSize, FileAccess::Write);

                if (dataInSize > 0)
                {
                    UnmanagedMemoryStream readStream((Byte*)dataIn, (Int64)dataInSize, (Int64)dataInSize, FileAccess::Read);
                
                    status = _filter->Filter(%readStream, dataInReadPtr, %writeStream, dataOutWrittenPtr);
                }
                else
                {
                    status = _filter->Filter(nullptr, dataInReadPtr, %writeStream, dataOutWrittenPtr);
                }

                dataInRead = dataInReadPtr;
                dataOutWritten = dataOutWrittenPtr;

                return (FilterStatus)status;
            }

            IMPLEMENT_REFCOUNTING(CefResponseFilterAdapter);
        };
    }
}
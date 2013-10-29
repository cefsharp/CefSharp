#pragma once

#include "Stdafx.h"
#include "SchemeHandlerWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;

namespace CefSharp
{
    class SchemeHandlerWrapper;

    public ref class SchemeHandlerResponse
    {
    internal:
        CefRefPtr<SchemeHandlerWrapper>* _schemeHandlerWrapper;
        void OnRequestCompleted();
        
    public:
        /// <summary>
        /// A Stream with the response data. If the request didn't return any response, leave this property as null.
        /// </summary>
        property Stream^ ResponseStream;

        property String^ MimeType;
        property IDictionary<String^, String^>^ ResponseHeaders;

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        property int StatusCode;

        /// <summary>
        /// The length of the response contents. Defaults to -1, which means unknown length
        /// and causes CefSharp to read the response stream in pieces. Thus, setting a length
        /// is optional but allows for more optimal response reading.
        /// </summary>
        property int ContentLength;

        /// <summary>
        /// URL to redirect to (leave empty to not redirect).
        /// </summary>
        property String^ RedirectUrl;

        /// <summary>
        /// Set to true to close the response stream once it has been read. The default value
        /// is false in order to preserve the old CefSharp behavior of not closing the stream.
        /// </summary>
        property bool CloseStream;

        SchemeHandlerResponse(SchemeHandlerWrapper* schemeHandlerWrapper)
        {
            ContentLength = -1;
            _schemeHandlerWrapper = new CefRefPtr<SchemeHandlerWrapper>(schemeHandlerWrapper);
        }

        void ReleaseSchemeHandlerWrapper()
        {
            delete _schemeHandlerWrapper;
        }
    };
};

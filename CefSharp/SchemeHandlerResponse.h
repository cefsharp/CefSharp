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

        SchemeHandlerResponse(SchemeHandlerWrapper* schemeHandlerWrapper)
        {
            _schemeHandlerWrapper = new CefRefPtr<SchemeHandlerWrapper>(schemeHandlerWrapper);
        }

        void ReleaseSchemeHandlerWrapper()
        {
            delete _schemeHandlerWrapper;
        }
    };
};

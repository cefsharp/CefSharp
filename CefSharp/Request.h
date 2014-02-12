#pragma once

#include "Stdafx.h"
#include "IRequest.h"
#include "SchemeHandlerResponse.h"
#include "PostData.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    class SchemeHandlerWrapper;

    ref class CefRequestWrapper : public IRequest
    {
        MCefRefPtr<CefRequest> _wrappedRequest;

    internal:
        CefRequestWrapper(CefRefPtr<CefRequest> cefRequest) : _wrappedRequest(cefRequest) {}

    public:
        virtual property String^ Url { String^ get(); void set(String^ url); }
        virtual property String^ Method { String^ get(); }
        virtual property String^ Body { String^ get(); }
        virtual IDictionary<String^, String^>^ GetHeaders();
        virtual void SetHeaders(IDictionary<String^, String^>^ headers);
        virtual CefPostDataWrapper^ GetPostData();
    };
}

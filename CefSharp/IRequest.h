#pragma once

#include "PostData.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public interface class IRequest
    {
        property String^ Url { String^ get(); void set(String^ url); }
        property String^ Method { String^ get(); }
        property String^ Body { String^ get(); }
        IDictionary<String^, String^>^ GetHeaders();
        void SetHeaders(IDictionary<String^, String^>^ headers);
        CefPostDataWrapper^ GetPostData();
    };
}

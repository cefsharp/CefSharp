#include "stdafx.h"

#include "Request.h"

namespace CefSharp
{

    String^ CefRequestWrapper::Url::get() 
    {
        return convertToString(_wrappedRequest->GetURL());
    }

    void CefRequestWrapper::Url::set(String^ url)
    {
        if(url == nullptr) 
        {
            throw gcnew System::ArgumentException("cannot be null", "url");
        }
        
        CefString str = convertFromString(url);
        _wrappedRequest->SetURL(str);
    }

}
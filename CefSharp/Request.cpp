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

    String^ CefRequestWrapper::Method::get() 
    {  
        return convertToString(_wrappedRequest->GetMethod());
    }

    IDictionary<String^, String^>^ CefRequestWrapper::GetHeaders()
    {
        CefRequest::HeaderMap hm;
        _wrappedRequest->GetHeaderMap(hm);

        IDictionary<String^, String^>^ headers = gcnew Dictionary<String^, String^>();

        for (CefRequest::HeaderMap::iterator it = hm.begin(); it != hm.end(); ++it)
        {
            String^ name = convertToString(it->first);
            String^ value = convertToString(it->second);
            headers->Add(name, value);
        }

        return headers;
    }
    
    
    void CefRequestWrapper::SetHeaders(IDictionary<String^, String^>^ headers)
    {
        CefRequest::HeaderMap hm;

        for each(KeyValuePair<String^, String^>^ pair in headers) 
        {
            CefString name = convertFromString(pair->Key);
            CefString value = convertFromString(pair->Value);
            hm[name] = value;
        }

        _wrappedRequest->SetHeaderMap(hm);
    }


}
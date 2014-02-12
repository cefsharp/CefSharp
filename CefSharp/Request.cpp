#include "Stdafx.h"

#include "Request.h"

namespace CefSharp
{
    String^ CefRequestWrapper::Url::get()
    {
        return toClr(_wrappedRequest->GetURL());
    }

    void CefRequestWrapper::Url::set(String^ url)
    {
        if (url == nullptr)
        {
            throw gcnew System::ArgumentException("cannot be null", "url");
        }

        CefString str = toNative(url);
        _wrappedRequest->SetURL(str);
    }

    String^ CefRequestWrapper::Method::get()
    {
        return toClr(_wrappedRequest->GetMethod());
    }

    String^ CefRequestWrapper::Body::get()
    {
        CefPostData::ElementVector ev;

        CefRefPtr<CefPostData> data = _wrappedRequest->GetPostData();

        if (data.get() != nullptr) 
        {
            data.get()->GetElements(ev);

            for (CefPostData::ElementVector::iterator it = ev.begin(); it != ev.end(); ++it)
            {
                CefPostDataElement *el = it->get();

                if (el->GetType() == PDE_TYPE_BYTES) 
                {
                    size_t count = el->GetBytesCount();
                    char* bytes = new char[count];

                    el->GetBytes(count, bytes);

                    return gcnew String(bytes, 0, count);
                }
                else if (el->GetType() == PDE_TYPE_FILE)
                {
                    return toClr(el->GetFile());
                }
            }
        }

        return nullptr;
    }

    IDictionary<String^, String^>^ CefRequestWrapper::GetHeaders()
    {
        CefRequest::HeaderMap hm;
        _wrappedRequest->GetHeaderMap(hm);

        IDictionary<String^, String^>^ headers = gcnew Dictionary<String^, String^>();

        for (CefRequest::HeaderMap::iterator it = hm.begin(); it != hm.end(); ++it)
        {
            String^ name = toClr(it->first);
            String^ value = toClr(it->second);
            headers[name] = value;
        }

        return headers;
    }

    void CefRequestWrapper::SetHeaders(IDictionary<String^, String^>^ headers)
    {
        CefRequest::HeaderMap hm;

        for each(KeyValuePair<String^, String^>^ pair in headers)
        {
            CefString name = toNative(pair->Key);
            CefString value = toNative(pair->Value);
            hm.insert(std::make_pair(name, value));
        }

        _wrappedRequest->SetHeaderMap(hm);
    }

    CefPostDataWrapper^ CefRequestWrapper::GetPostData()
    {
        return gcnew CefPostDataWrapper(_wrappedRequest->GetPostData());
    }
}

#include "Stdafx.h"
#include "RequestResponse.h"

namespace CefSharp
{
    void RequestResponse::Cancel()
    {
        _action = ResponseAction::Cancel;
    }

    void RequestResponse::Redirect(String^ url)
    {
        _redirectUrl = url;
        _action = ResponseAction::Redirect;
    }

    void RequestResponse::RespondWith(Stream^ stream, String^ mimeType)
    {
        RespondWith(stream, mimeType, "OK", 200, nullptr);
    }

    void RequestResponse::RespondWith(Stream^ stream, String^ mimeType, String^ statusText, int statusCode, IDictionary<String^, String^>^ responseHeaders)
    {
        if (String::IsNullOrEmpty(mimeType))
        {
            throw gcnew ArgumentException("must provide a mime type", "mimeType");
        }

        if (stream == nullptr)
        {
            throw gcnew ArgumentNullException("stream");
        }

        _responseStream = stream;
        _mimeType = mimeType;
        _statusText = statusText;
        _statusCode = statusCode;
        _responseHeaders = responseHeaders;

        _action = ResponseAction::Respond;
    }
}
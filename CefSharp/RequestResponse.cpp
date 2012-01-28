#include "stdafx.h"
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
        if(String::IsNullOrEmpty(mimeType))
        {
            throw gcnew ArgumentException("must provide a mime type", "mimeType");
        }

        if(stream == nullptr)
        {
            throw gcnew ArgumentNullException("stream");
        }

        _responseStream = stream;
        _mimeType = mimeType;
        _action = ResponseAction::Respond;
    }

}
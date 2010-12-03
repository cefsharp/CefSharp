// CefSharp.h

#include "stdafx.h"
#pragma once

#include "BrowserControl.h"
#include "Request.h"
#include "ReturnValue.h"

using namespace System;
using namespace System::IO;

namespace CefSharp 
{
    public interface class IBeforeResourceLoad
    {
    public:
        
        /* Event called before a resource is loaded.  
        
        To allow the resource to load normally return <c>ReturnValue.Continue</c>. 
        
        To redirect the resource to a new url populate the <c>redirectUrl</c> value 
        and return <c>ReturnValue.Continue</c>.
        
        To specify data for the resource return a Stream object in <c>resourceStream</c>,
        set <c>mimeType</c> to the resource stream's mime type, and return <c>ReturnValue.Continue</c>.

        To cancel loading of the resource return <c>ReturnValue.Handled</c>.  
        
        Any modifications to request will be observed.  If the URL in <c>request</c> is changed and
         <c>redirectUrl</c> is also set, the URL in <c>request</c> will be used. */
        ReturnValue HandleBeforeResourceLoad(BrowserControl^ browserControl, IRequest^ request, 
            String^% redirectUrl, Stream^% resourceStream, String^% mimeType, int loadFlags);
    };
}

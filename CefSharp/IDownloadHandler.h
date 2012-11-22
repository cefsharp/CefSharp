#include "stdafx.h"
#pragma once

#include "IWebBrowser.h"
//#include "RequestResponse.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    public interface class IDownloadHandler
    {
    public:
		bool HandleDownload(IWebBrowser^ browserControl, String^ mimeType, int64 contentLength, String^ fileName);	
		void HandleComplete();
		bool HandleReceivedData(array<Byte>^ data);
    };
}
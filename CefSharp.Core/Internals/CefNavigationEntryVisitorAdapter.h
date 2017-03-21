// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_browser.h"

using namespace System::Security::Cryptography::X509Certificates;

namespace CefSharp
{
    namespace Internals
    {
        private class CefNavigationEntryVisitorAdapter : public CefNavigationEntryVisitor
        {
        private:
            gcroot<INavigationEntryVisitor^> _handler;

        public:
            CefNavigationEntryVisitorAdapter(INavigationEntryVisitor^ handler)
            {
                _handler = handler;
            }

            ~CefNavigationEntryVisitorAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            bool Visit(CefRefPtr<CefNavigationEntry> entry,
                bool current,
                int index,
                int total) OVERRIDE
            {
                NavigationEntry^ navEntry;
                SslStatus^ sslStatus;

                if (entry->IsValid())
                {
                    auto time = entry->GetCompletionTime();
                    DateTime completionTime = CefSharp::Internals::CefTimeUtils::ConvertCefTimeToDateTime(time.GetDoubleT());
                    auto ssl = entry->GetSSLStatus();
                    X509Certificate2^ sslCertificate;

                    if (ssl.get())
                    {
                        auto certificate = ssl->GetX509Certificate();
                        if (certificate.get())
                        {
                            auto derEncodedCertificate = certificate->GetDEREncoded();
                            auto byteCount = derEncodedCertificate->GetSize();
                            if (byteCount > 0)
                            {
                                auto bytes = gcnew cli::array<Byte>(byteCount);
                                pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                                derEncodedCertificate->GetData(static_cast<void*>(src), byteCount, 0);

                                sslCertificate = gcnew X509Certificate2(bytes);
                            }
                        }
                        sslStatus = gcnew SslStatus(ssl->IsSecureConnection(), (CertStatus)ssl->GetCertStatus(), (SslVersion)ssl->GetSSLVersion(), (SslContentStatus)ssl->GetContentStatus(), sslCertificate);
                    }

                    navEntry = gcnew NavigationEntry(current, completionTime, StringUtils::ToClr(entry->GetDisplayURL()), entry->GetHttpStatusCode(), StringUtils::ToClr(entry->GetOriginalURL()), StringUtils::ToClr(entry->GetTitle()), (TransitionType)entry->GetTransitionType(), StringUtils::ToClr(entry->GetURL()), entry->HasPostData(), true, sslStatus);
                }
                else
                {
                    //Invalid nav entry
                    navEntry = gcnew NavigationEntry(current, DateTime::MinValue, nullptr, -1, nullptr, nullptr, (TransitionType)-1, nullptr, false, false, sslStatus);
                }

                return _handler->Visit(navEntry, current, index, total);
            }

            IMPLEMENT_REFCOUNTING(CefNavigationEntryVisitorAdapter);
        };
    }
}


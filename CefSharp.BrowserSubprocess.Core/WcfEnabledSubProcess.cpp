// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"
#include "WcfEnabledSubProcess.h"

using namespace System::ServiceModel;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        void WcfEnabledSubProcess::OnBrowserCreated(CefBrowserWrapper^ browser)
        {
            if (!_parentBrowserId.HasValue)
            {
                _parentBrowserId = browser->BrowserId;
            }

            if (!_parentBrowserId.HasValue)
            {
                return;
            }

            //TODO: This can likely be simplified as both values are likely equal
            auto browserId = browser->IsPopup ? _parentBrowserId.Value : browser->BrowserId;

            auto serviceName = RenderprocessClientFactory::GetServiceName(_parentProcessId, browserId);

            auto binding = BrowserProcessServiceHost::CreateBinding();

            auto channelFactory = gcnew ChannelFactory<IBrowserProcess^>(
                binding,
                gcnew EndpointAddress(serviceName)
                );

            channelFactory->Open();

            auto browserProcess = channelFactory->CreateChannel();
            auto clientChannel = ((IClientChannel^)browserProcess);

            try
            {
                clientChannel->Open();

                browser->ChannelFactory = channelFactory;
                browser->BrowserProcess = browserProcess;
            }
            catch (Exception^)
            {
            }
        }

        void WcfEnabledSubProcess::OnBrowserDestroyed(CefBrowserWrapper^ browser)
        {
            auto channelFactory = browser->ChannelFactory;

            try
            {
                if (channelFactory->State == CommunicationState::Opened)
                {
                    channelFactory->Close();
                }
            }
            catch (Exception^)
            {
                
                channelFactory->Abort();
            }

            auto clientChannel = ((IClientChannel^)browser->BrowserProcess);

            try
            {
                if (clientChannel->State == CommunicationState::Opened)
                {
                    clientChannel->Close();
                }
            }
            catch (Exception^)
            {
                clientChannel->Abort();
            }

            browser->ChannelFactory = nullptr;
            browser->BrowserProcess = nullptr;
        }
    }
}
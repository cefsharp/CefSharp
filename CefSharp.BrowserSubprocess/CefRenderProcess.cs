using System;
using System.Linq;
using System.Threading.Tasks;
// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System.Collections.Generic;
using System.ServiceModel;
using TaskExtensions = CefSharp.Internals.TaskExtensions;

namespace CefSharp.BrowserSubprocess
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults=true)]
    public class CefRenderProcess : CefSubProcess
    {
        private int? parentBrowserId;
        private List<CefBrowserWrapper> browsers = new List<CefBrowserWrapper>();

        public CefRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
            
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            foreach(var browser in browsers)
            {
                browser.Dispose();
            }

            browsers = null;

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserWrapper browser)
        {
            browsers.Add(browser);

            if (parentBrowserId == null)
            {
                parentBrowserId = browser.BrowserId;
            }

            if (ParentProcessId == null || parentBrowserId == null)
            {
                return;
            }

            var browserId = browser.IsPopup ? parentBrowserId.Value : browser.BrowserId;

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, browserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            var channelFactory = new ChannelFactory<IBrowserProcess>(
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var browserProcess = channelFactory.CreateChannel();
            var clientChannel = ((IClientChannel)browserProcess);

            try
            {
                clientChannel.Open();
                if (!browser.IsPopup)
                {
                    browserProcess.Connect();
                }

                browser.ChannelFactory = channelFactory;
                browser.BrowserProcess = browserProcess;
            }
            catch(Exception)
            {
            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            browsers.Remove(browser);

            var channelFactory = browser.ChannelFactory;

            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }

            var clientChannel = ((IClientChannel)browser.BrowserProcess);

            if (clientChannel.State == CommunicationState.Opened)
            {
                clientChannel.Close();
            }

            browser.ChannelFactory = null;
            browser.BrowserProcess = null;
            browser.JavascriptRootObject = null;
        }
    }
}

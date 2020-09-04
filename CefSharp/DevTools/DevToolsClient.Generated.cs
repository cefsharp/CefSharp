// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools
{
    /// <summary>
    /// Generated DevToolsClient methods
    /// </summary>
    public partial class DevToolsClient
    {
        private CefSharp.DevTools.Browser.Browser _Browser;
        public CefSharp.DevTools.Browser.Browser Browser
        {
            get
            {
                if ((_Browser) == (null))
                {
                    _Browser = (new CefSharp.DevTools.Browser.Browser(this));
                }

                return _Browser;
            }
        }

        private CefSharp.DevTools.DOM.DOM _DOM;
        public CefSharp.DevTools.DOM.DOM DOM
        {
            get
            {
                if ((_DOM) == (null))
                {
                    _DOM = (new CefSharp.DevTools.DOM.DOM(this));
                }

                return _DOM;
            }
        }

        private CefSharp.DevTools.DOMDebugger.DOMDebugger _DOMDebugger;
        public CefSharp.DevTools.DOMDebugger.DOMDebugger DOMDebugger
        {
            get
            {
                if ((_DOMDebugger) == (null))
                {
                    _DOMDebugger = (new CefSharp.DevTools.DOMDebugger.DOMDebugger(this));
                }

                return _DOMDebugger;
            }
        }

        private CefSharp.DevTools.Emulation.Emulation _Emulation;
        public CefSharp.DevTools.Emulation.Emulation Emulation
        {
            get
            {
                if ((_Emulation) == (null))
                {
                    _Emulation = (new CefSharp.DevTools.Emulation.Emulation(this));
                }

                return _Emulation;
            }
        }

        private CefSharp.DevTools.IO.IO _IO;
        public CefSharp.DevTools.IO.IO IO
        {
            get
            {
                if ((_IO) == (null))
                {
                    _IO = (new CefSharp.DevTools.IO.IO(this));
                }

                return _IO;
            }
        }

        private CefSharp.DevTools.Input.Input _Input;
        public CefSharp.DevTools.Input.Input Input
        {
            get
            {
                if ((_Input) == (null))
                {
                    _Input = (new CefSharp.DevTools.Input.Input(this));
                }

                return _Input;
            }
        }

        private CefSharp.DevTools.Log.Log _Log;
        public CefSharp.DevTools.Log.Log Log
        {
            get
            {
                if ((_Log) == (null))
                {
                    _Log = (new CefSharp.DevTools.Log.Log(this));
                }

                return _Log;
            }
        }

        private CefSharp.DevTools.Network.Network _Network;
        public CefSharp.DevTools.Network.Network Network
        {
            get
            {
                if ((_Network) == (null))
                {
                    _Network = (new CefSharp.DevTools.Network.Network(this));
                }

                return _Network;
            }
        }

        private CefSharp.DevTools.Page.Page _Page;
        public CefSharp.DevTools.Page.Page Page
        {
            get
            {
                if ((_Page) == (null))
                {
                    _Page = (new CefSharp.DevTools.Page.Page(this));
                }

                return _Page;
            }
        }

        private CefSharp.DevTools.Performance.Performance _Performance;
        public CefSharp.DevTools.Performance.Performance Performance
        {
            get
            {
                if ((_Performance) == (null))
                {
                    _Performance = (new CefSharp.DevTools.Performance.Performance(this));
                }

                return _Performance;
            }
        }

        private CefSharp.DevTools.Security.Security _Security;
        public CefSharp.DevTools.Security.Security Security
        {
            get
            {
                if ((_Security) == (null))
                {
                    _Security = (new CefSharp.DevTools.Security.Security(this));
                }

                return _Security;
            }
        }

        private CefSharp.DevTools.Target.Target _Target;
        public CefSharp.DevTools.Target.Target Target
        {
            get
            {
                if ((_Target) == (null))
                {
                    _Target = (new CefSharp.DevTools.Target.Target(this));
                }

                return _Target;
            }
        }
    }
}
// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CefSharp
{
    public class ProxyConfig
    {
        private const uint InternetOptionProxy = 38;

        public static InternetProxyInfo GetProxyInformation()
        {
            int bufferLength = 0;
            InternetQueryOption(IntPtr.Zero, InternetOptionProxy, IntPtr.Zero, ref bufferLength);
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(bufferLength);


                if (InternetQueryOption(IntPtr.Zero, InternetOptionProxy, buffer, ref bufferLength))
                {
                    var ipi = (InternetProxyInfo)Marshal.PtrToStructure(buffer, typeof(InternetProxyInfo));
                    return ipi;
                }
                else
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer);
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetQueryOption(IntPtr hInternet, uint dwOption, IntPtr lpBuffer, ref int lpdwBufferLength);
    }

    public enum InternetOpenType
    {
        Preconfig = 0,
        Direct = 1,
        Proxy = 3,
        PreconfigWithNoAutoProxy = 4
    }

    public struct InternetProxyInfo
    {
        public InternetOpenType AccessType;
        public string ProxyAddress;
        public string ProxyBypass;
    }
}

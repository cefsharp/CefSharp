﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Internals
{
    public class BitmapInfo
    {
        public object BitmapLock;
        public IntPtr BackBufferHandle;

        public bool IsPopup { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public IntPtr FileMappingHandle { get; set; }

        // Cannot be InteropBitmap since we really don't want CefSharp to be dependent on WPF libraries.
        public object InteropBitmap;

        public BitmapInfo()
        {
            BitmapLock = new object();
        }
    }
}

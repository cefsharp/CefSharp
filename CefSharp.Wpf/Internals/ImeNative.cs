// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace CefSharp.Wpf.Internals
{
    public static class ImeNative
    {
        internal const uint GCS_RESULTSTR = 0x0800;
        internal const uint GCS_COMPSTR = 0x0008;
        internal const uint GCS_COMPATTR = 0x0010;
        internal const uint GCS_CURSORPOS = 0x0080;
        internal const uint GCS_COMPCLAUSE = 0x0020;

        internal const uint CPS_CANCEL = 0x0004;
        internal const uint CS_NOMOVECARET = 0x4000;
        internal const uint NI_COMPOSITIONSTR = 0x0015;

        internal const uint ATTR_TARGET_CONVERTED = 0x01;
        internal const uint ATTR_TARGET_NOTCONVERTED = 0x03;

        internal const uint ISC_SHOWUICOMPOSITIONWINDOW = 0x80000000;

        internal const uint CFS_DEFAULT = 0x0000;
        internal const uint CFS_RECT = 0x0001;
        internal const uint CFS_POINT = 0x0002;
        internal const uint CFS_FORCE_POSITION = 0x0020;
        internal const uint CFS_CANDIDATEPOS = 0x0040;
        internal const uint CFS_EXCLUDE = 0x0080;

        internal const uint LANG_JAPANESE = 0x11;
        internal const uint LANG_CHINESE = 0x04;
        internal const uint LANG_KOREAN = 0x12;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public int dwStyle;
            public POINT ptCurrentPos;
            public RECT rcArea;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CANDIDATEFORM
        {
            public int dwIndex;
            public int dwStyle;
            public POINT ptCurrentPos;
            public RECT rcArea;
        }

        [DllImport("imm32.dll")]
        internal static extern IntPtr ImmCreateContext();

        [DllImport("imm32.dll")]
        internal static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll")]
        internal static extern bool ImmDestroyContext(IntPtr hIMC);

        [DllImport("imm32.dll")]
        internal static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        internal static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("Imm32.dll")]
        internal static extern bool ImmNotifyIME(IntPtr hIMC, uint action, uint index, uint value);

        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint ImmGetCompositionString(IntPtr hIMC, uint dwIndex, byte[] lpBuf, uint dwBufLen);

        [DllImport("imm32.dll")]
        internal static extern int ImmSetCompositionWindow(IntPtr hIMC, ref COMPOSITIONFORM lpCompForm);

        [DllImport("imm32.dll")]
        public static extern int ImmSetCandidateWindow(IntPtr hIMC, [MarshalAs(UnmanagedType.Struct)] ref CANDIDATEFORM lpCandidateForm);

        [DllImport("user32.dll")]
        internal static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);

        [DllImport("user32.dll")]
        internal static extern bool DestroyCaret();

        [DllImport("user32.dll")]
        internal static extern bool SetCaretPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetFocus(IntPtr hWnd);
    }
}

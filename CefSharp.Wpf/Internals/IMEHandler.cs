// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Text;
using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// IMEHandler provides implementattion when message WM_IME_COMPOSITION is received.
    /// </summary>
    public class IMEHandler
    {
        // Black SkColor value for underline.
        internal const uint ColorUNDERLINE = 0xFF000000;
        // White SkColor value for background.
        internal const uint ColorBKCOLOR = 0xFFFFFFFF;    

        IntPtr hWnd;

        public IMEHandler(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        internal bool GetResult(uint lParam, out string text)
        {
            IntPtr hIMC = NativeIME.ImmGetContext(hWnd);

            var ret = GetString(hIMC, lParam, NativeIME.GCS_RESULTSTR, out text);

            NativeIME.ImmReleaseContext(hWnd, hIMC);

            return ret;
        }

        internal bool GetComposition(uint lParam, List<CompositionUnderline> underlines, ref int compositionStart, out string text)
        {
            IntPtr hIMC = NativeIME.ImmGetContext(hWnd);

            bool ret = GetString(hIMC, lParam, NativeIME.GCS_COMPSTR, out text);
            if (ret)
            {
                GetCompositionInfo(lParam, text, underlines, ref compositionStart);
            }

            NativeIME.ImmReleaseContext(hWnd, hIMC);

            return ret;
        }

        private static bool GetString(IntPtr hIMC, uint lParam, uint type, out string text)
        {
            text = string.Empty;

            if (!IsParam(lParam, type))
            {
                return false;
            }

            var strLen = NativeIME.ImmGetCompositionString(hIMC, type, null, 0);
            if (strLen <= 0)
            {
                return false;
            }

            // buffer contains char (2 bytes)
            byte[] buffer = new byte[strLen];
            NativeIME.ImmGetCompositionString(hIMC, type, buffer, strLen);
            text = Encoding.Unicode.GetString(buffer);

            return true;
        }

        private void GetCompositionInfo(uint lParam, string text, List<CompositionUnderline> underlines, ref int compositionStart)
        {
            IntPtr hIMC = NativeIME.ImmGetContext(hWnd);

            underlines.Clear();

            int targetStart = text.Length;
            int targetEnd = text.Length;
            if (IsParam(lParam, NativeIME.GCS_COMPATTR))
            {
                GetCompositionSelectionRange(hIMC, ref targetStart, ref targetEnd);
            }

            // Retrieve the selection range information. If CS_NOMOVECARET is specified
            // it means the cursor should not be moved and we therefore place the caret at
            // the beginning of the composition string. Otherwise we should honour the
            // GCS_CURSORPOS value if it's available.
            if (!IsParam(lParam, NativeIME.CS_NOMOVECARET) && IsParam(lParam, NativeIME.GCS_CURSORPOS))
            {
                // IMM32 does not support non-zero-width selection in a composition. So
                // always use the caret position as selection range.
                int cursor = (int)NativeIME.ImmGetCompositionString(hIMC, NativeIME.GCS_CURSORPOS, null, 0);
                compositionStart = cursor;
            }
            else
            {
                compositionStart = 0;
            }

            if (IsParam(lParam, NativeIME.GCS_COMPCLAUSE))
            {
                GetCompositionUnderlines(hIMC, targetStart, targetEnd, underlines);
            }

            if (underlines.Count == 0)
            {
                Range range = new Range();

                bool thick = false;

                if (targetStart > 0)
                {
                    range = new Range(0, targetStart);
                }

                if (targetEnd > targetStart)
                {
                    range = new Range(targetStart, targetEnd);
                    thick = true;
                }

                if (targetEnd < text.Length)
                {
                    range = new Range(targetEnd, text.Length);
                }

                underlines.Add(new CompositionUnderline(range, ColorUNDERLINE, ColorBKCOLOR, thick));
            }

            NativeIME.ImmReleaseContext(hWnd, hIMC);
        }

        private static void GetCompositionUnderlines(IntPtr hIMC, int targetStart, int targetEnd, List<CompositionUnderline> underlines)
        {
            var clauseSize = NativeIME.ImmGetCompositionString(hIMC, NativeIME.GCS_COMPCLAUSE, null, 0);
            if (clauseSize <= 0)
            {
                return;
            }

            int clauseLength = (int)clauseSize / sizeof(Int32);

            // buffer contains 32 bytes (4 bytes) array
            var clauseData = new byte[(int)clauseSize];
            NativeIME.ImmGetCompositionString(hIMC, NativeIME.GCS_COMPCLAUSE, clauseData, clauseSize);

            var clauseLength_1 = clauseLength - 1;
            for (int i = 0; i < clauseLength_1; i++)
            {
                int from = BitConverter.ToInt32(clauseData, i * sizeof(Int32));
                int to = BitConverter.ToInt32(clauseData, (i + 1) * sizeof(Int32));

                var range = new Range(from, to);
                bool thick = (range.From >= targetStart && range.To <= targetEnd);

                underlines.Add(new CompositionUnderline(range, ColorUNDERLINE, ColorBKCOLOR, thick));
            }
        }

        private static void GetCompositionSelectionRange(IntPtr hIMC, ref int targetStart, ref int targetEnd)
        {
            var attributeSize = NativeIME.ImmGetCompositionString(hIMC, NativeIME.GCS_COMPATTR, null, 0);
            if (attributeSize <= 0)
            {
                return;
            }

            int start = 0;
            int end = 0;

            // Buffer contains 8bit array
            var attributeData = new byte[attributeSize];
            NativeIME.ImmGetCompositionString(hIMC, NativeIME.GCS_COMPATTR, attributeData, attributeSize);

            for (start = 0; start < attributeSize; ++start)
            {
                if (IsSelectionAttribute(attributeData[start]))
                {
                    break;
                }
            }

            for (end = start; end < attributeSize; ++end)
            {
                if (!IsSelectionAttribute(attributeData[end]))
                {
                    break;
                }
            }

            targetStart = start;
            targetEnd = end;
        }

        private static bool IsSelectionAttribute(byte attribute)
        {
            return (attribute == NativeIME.ATTR_TARGET_CONVERTED || attribute == NativeIME.ATTR_TARGET_NOTCONVERTED);
        }

        private static bool IsParam(uint lParam, uint type)
        {
            return (lParam & type) == type;
        }
    }
}

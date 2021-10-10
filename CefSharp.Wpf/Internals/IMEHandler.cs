// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Text;
using CefSharp.Structs;
using Range = CefSharp.Structs.Range;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// ImeHandler provides implementation when message WM_IME_COMPOSITION is received.
    /// </summary>
    public static class ImeHandler
    {
        // Black SkColor value for underline.
        public static uint ColorUNDERLINE = 0xFF000000;
        // Transparent SkColor value for background.
        public static uint ColorBKCOLOR = 0x00000000;

        public static bool GetResult(IntPtr hwnd, uint lParam, out string text)
        {
            var hIMC = ImeNative.ImmGetContext(hwnd);

            var ret = GetString(hIMC, lParam, ImeNative.GCS_RESULTSTR, out text);

            ImeNative.ImmReleaseContext(hwnd, hIMC);

            return ret;
        }

        public static bool GetComposition(IntPtr hwnd, uint lParam, List<CompositionUnderline> underlines, ref int compositionStart, out string text)
        {
            var hIMC = ImeNative.ImmGetContext(hwnd);

            bool ret = GetString(hIMC, lParam, ImeNative.GCS_COMPSTR, out text);
            if (ret)
            {
                GetCompositionInfo(hwnd, lParam, text, underlines, ref compositionStart);
            }

            ImeNative.ImmReleaseContext(hwnd, hIMC);

            return ret;
        }

        private static bool GetString(IntPtr hIMC, uint lParam, uint type, out string text)
        {
            text = string.Empty;

            if (!IsParam(lParam, type))
            {
                return false;
            }

            var strLen = ImeNative.ImmGetCompositionString(hIMC, type, null, 0);
            if (strLen <= 0)
            {
                return false;
            }

            // buffer contains char (2 bytes)
            byte[] buffer = new byte[strLen];
            ImeNative.ImmGetCompositionString(hIMC, type, buffer, strLen);
            text = Encoding.Unicode.GetString(buffer);

            return true;
        }

        private static void GetCompositionInfo(IntPtr hwnd, uint lParam, string text, List<CompositionUnderline> underlines, ref int compositionStart)
        {
            var hIMC = ImeNative.ImmGetContext(hwnd);

            underlines.Clear();

            byte[] attributes = null;
            int targetStart = text.Length;
            int targetEnd = text.Length;
            if (IsParam(lParam, ImeNative.GCS_COMPATTR))
            {
                attributes = GetCompositionSelectionRange(hIMC, ref targetStart, ref targetEnd);
            }

            // Retrieve the selection range information. If CS_NOMOVECARET is specified
            // it means the cursor should not be moved and we therefore place the caret at
            // the beginning of the composition string. Otherwise we should honour the
            // GCS_CURSORPOS value if it's available.
            if (!IsParam(lParam, ImeNative.CS_NOMOVECARET) && IsParam(lParam, ImeNative.GCS_CURSORPOS))
            {
                // IMM32 does not support non-zero-width selection in a composition. So
                // always use the caret position as selection range.
                int cursor = (int)ImeNative.ImmGetCompositionString(hIMC, ImeNative.GCS_CURSORPOS, null, 0);
                compositionStart = cursor;
            }
            else
            {
                compositionStart = 0;
            }

            if (attributes != null &&
                // character before
                ((compositionStart > 0 && (compositionStart - 1) < attributes.Length && attributes[compositionStart - 1] == ImeNative.ATTR_INPUT)
                ||
                // character after
                (compositionStart >= 0 && compositionStart < attributes.Length && attributes[compositionStart] == ImeNative.ATTR_INPUT)))
            {
                // as MS does with their ime implementation we should only use the GCS_CURSORPOS if the character
                // before or after is new input.
                // https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/windows/Documents/ImmComposition.cs,1079
            }
            else
            {
                compositionStart = text.Length;
            }

            if (IsParam(lParam, ImeNative.GCS_COMPCLAUSE))
            {
                GetCompositionUnderlines(hIMC, targetStart, targetEnd, underlines);
            }

            if (underlines.Count == 0)
            {
                var range = new Range();

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

            ImeNative.ImmReleaseContext(hwnd, hIMC);
        }

        private static void GetCompositionUnderlines(IntPtr hIMC, int targetStart, int targetEnd, List<CompositionUnderline> underlines)
        {
            var clauseSize = ImeNative.ImmGetCompositionString(hIMC, ImeNative.GCS_COMPCLAUSE, null, 0);
            if (clauseSize <= 0)
            {
                return;
            }

            int clauseLength = (int)clauseSize / sizeof(Int32);

            // buffer contains 32 bytes (4 bytes) array
            var clauseData = new byte[(int)clauseSize];
            ImeNative.ImmGetCompositionString(hIMC, ImeNative.GCS_COMPCLAUSE, clauseData, clauseSize);

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

        private static byte[] GetCompositionSelectionRange(IntPtr hIMC, ref int targetStart, ref int targetEnd)
        {
            var attributeSize = ImeNative.ImmGetCompositionString(hIMC, ImeNative.GCS_COMPATTR, null, 0);
            if (attributeSize <= 0)
            {
                return null;
            }

            int start = 0;
            int end = 0;

            // Buffer contains 8bit array
            var attributeData = new byte[attributeSize];
            ImeNative.ImmGetCompositionString(hIMC, ImeNative.GCS_COMPATTR, attributeData, attributeSize);

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
            return attributeData;
        }

        private static bool IsSelectionAttribute(byte attribute)
        {
            return (attribute == ImeNative.ATTR_TARGET_CONVERTED || attribute == ImeNative.ATTR_TARGET_NOTCONVERTED);
        }

        private static bool IsParam(uint lParam, uint type)
        {
            return (lParam & type) == type;
        }
    }
}

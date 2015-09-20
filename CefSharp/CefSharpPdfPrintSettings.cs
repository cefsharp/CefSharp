// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public sealed class CefSharpPdfPrintSettings
    {
        public string HeaderFooterTitle { get; set; }

        public string HeaderFooterUrl { get; set; }

        public int PageWidth { get; set; }

        public int PageHeight { get; set; }

        public double MarginLeft { get; set; }

        public double MarginTop { get; set; }

        public double MarginRight { get; set; }

        public double MarginBottom { get; set; }

        public CefPdfPrintMarginType MarginType { get; set; }

        public bool HeaderFooterEnabled { get; set; }

        public bool SelectionOnly { get; set; }

        public bool Landscape { get; set; }

        public bool BackgroundsEnabled { get; set; }
    }
}

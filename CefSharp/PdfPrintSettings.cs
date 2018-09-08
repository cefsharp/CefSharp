// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Print to Pdf Settings
    /// </summary>
    public sealed class PdfPrintSettings
    {
        /// <summary>
        /// Page title to display in the header. Only used if <see cref="HeaderFooterEnabled"/>
        /// is set to true.
        /// </summary>
        public string HeaderFooterTitle { get; set; }

        /// <summary>
        /// URL to display in the footer. Only used if <see cref="HeaderFooterEnabled"/> is set
        /// to true.
        /// </summary>
        public string HeaderFooterUrl { get; set; }

        /// <summary>
        /// Output page size in microns. If either of these values is less than or
        /// equal to zero then the default paper size (A4) will be used.
        /// </summary>
        public int PageWidth { get; set; }

        /// <summary>
        /// Output page size in microns. If either of these values is less than or
        /// equal to zero then the default paper size (A4) will be used.
        /// </summary>
        public int PageHeight { get; set; }

        /// <summary>
        /// Margin in millimeters. Only used if MarginType is set to Custom.
        /// </summary>
        public double MarginLeft { get; set; }

        /// <summary>
        /// Margin in millimeters. Only used if MarginType is set to Custom.
        /// </summary>
        public double MarginTop { get; set; }

        /// <summary>
        /// Margin in millimeters. Only used if MarginType is set to Custom.
        /// </summary>
        public double MarginRight { get; set; }

        /// <summary>
        /// Margin in millimeters. Only used if MarginType is set to Custom.
        /// </summary>
        public double MarginBottom { get; set; }

        /// <summary>
        /// Margin type.
        /// </summary>
        public CefPdfPrintMarginType MarginType { get; set; }

        /// <summary>
        /// Scale the PDF by the specified amount, defaults to 100%.
        /// </summary>
        public int ScaleFactor { get; set; }

        /// <summary>
        /// Set to true to print headers and footers or false to not print
        /// headers and footers.
        /// </summary>
        public bool HeaderFooterEnabled { get; set; }

        /// <summary>
        /// Set to true to print the selection only or false to print all.
        /// </summary>
        public bool SelectionOnly { get; set; }

        /// <summary>
        /// Set to true for landscape mode or false for portrait mode.
        /// </summary>
        public bool Landscape { get; set; }

        /// <summary>
        /// Set to true to print background graphics or false to not print
        /// background graphics.
        /// </summary>
        public bool BackgroundsEnabled { get; set; }
    }
}

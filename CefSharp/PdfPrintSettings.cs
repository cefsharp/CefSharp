// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Print to Pdf Settings
    /// </summary>
    public sealed class PdfPrintSettings
    {
        /// <summary>
        /// Set to true for landscape mode or false for portrait mode.
        /// </summary>
        public bool Landscape { get; set; }

        /// <summary>
        /// Set to true to print background graphics or false to not print
        /// background graphics.
        /// </summary>
        public bool PrintBackground { get; set; }

        /// <summary>
        /// The percentage to scale the PDF by before printing (e.g. .5 is 50%).
        /// If this value is less than or equal to zero the default value of 1.0
        /// will be used.
        /// </summary>
        public double Scale { get; set; } = 1.0;

        /// <summary>
        /// Output paper size in inches. If either of these values is less than or
        /// equal to zero then the default paper size (letter, 8.5 x 11 inches) will
        /// be used.
        /// </summary>
        public double PaperWidth { get; set; }

        /// <summary>
        /// Output paper size in inches. If either of these values is less than or
        /// equal to zero then the default paper size (letter, 8.5 x 11 inches) will
        /// be used.
        /// </summary>
        public double PaperHeight { get; set; }

        /// <summary>
        /// Set to true to prefer page size as defined by css. Defaults to false
        /// in which case the content will be scaled to fit the paper size.
        /// </summary>
        public bool PreferCssPageSize { get; set; }

        /// <summary>
        /// Margin type.
        /// </summary>
        public CefPdfPrintMarginType MarginType { get; set; }

        /// <summary>
        /// Margins in inches. Only used if <see cref="MarginType"/> is set to
        /// <see cref="CefPdfPrintMarginType.Custom"/>.
        /// </summary>
        public double MarginLeft { get; set; }

        /// <summary>
        /// Margins in inches. Only used if <see cref="MarginType"/> is set to
        /// <see cref="CefPdfPrintMarginType.Custom"/>.
        /// </summary>
        public double MarginTop { get; set; }

        /// <summary>
        /// Margins in inches. Only used if <see cref="MarginType"/> is set to
        /// <see cref="CefPdfPrintMarginType.Custom"/>.
        /// </summary>
        public double MarginRight { get; set; }

        /// <summary>
        /// Margins in inches. Only used if <see cref="MarginType"/> is set to
        /// <see cref="CefPdfPrintMarginType.Custom"/>.
        /// </summary>
        public double MarginBottom { get; set; }

        /// <summary>
        /// Paper ranges to print, one based, e.g. '1-5, 8, 11-13'. Pages are printed
        /// in the document order, not in the order specified, and no more than once.
        /// Defaults to empty string, which implies the entire document is printed.
        /// The page numbers are quietly capped to actual page count of the document,
        /// and ranges beyond the end of the document are ignored. If this results in
        /// no pages to print, an error is reported. It is an error to specify a range
        /// with start greater than end.
        /// </summary>
        public string PageRanges { get; set; }

        /// <summary>
        /// Set to true  to display the header and/or footer. Modify
        /// |header_template| and/or |footer_template| to customize the display.
        /// </summary>
        public bool DisplayHeaderFooter { get; set; }

        /// <summary>
        /// HTML template for the print header. Only displayed if
        /// <see cref="DisplayHeaderFooter"/> is true. Should be valid HTML markup with
        /// the following classes used to inject printing values into them:
        ///
        /// - date: formatted print date
        /// - title: document title
        /// - url: document location
        /// - pageNumber: current page number
        /// - totalPages: total pages in the document
        ///
        /// For example, "&lt;span class=title&gt;&lt;/span&gt;" would generate a span containing
        /// the title.
        /// </summary>
        public string HeaderTemplate { get; set; }

        /// <summary>
        /// HTML template for the print footer. Only displayed if
        /// <see cref="DisplayHeaderFooter"/> is true. Should be valid HTML markup with
        /// the following classes used to inject printing values into them:
        ///
        /// - date: formatted print date
        /// - title: document title
        /// - url: document location
        /// - pageNumber: current page number
        /// - totalPages: total pages in the document
        ///
        /// For example, "&lt;span class=title&gt;&lt;/span&gt;" would generate a span containing
        /// the title.
        /// </summary>
        public string FooterTemplate { get; set; }

        // Obsolete properties

        /// <summary>
        /// Set to true to print background graphics or false to not print
        /// background graphics.
        /// </summary>
        [Obsolete("Use PrintBackground instead")]
        public bool BackgroundsEnabled
        {
            get { return PrintBackground; }
            set { PrintBackground = value; }
        }

        /// <summary>
        /// Set to true to print headers and footers or false to not print
        /// headers and footers.
        /// </summary>
        [Obsolete("Use DisplayHeaderFooter instead")]
        public bool HeaderFooterEnabled
        {
            get { return DisplayHeaderFooter; }
            set { DisplayHeaderFooter = value; }
        }
    }
}

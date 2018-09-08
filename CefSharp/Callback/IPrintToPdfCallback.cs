// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface for <see cref="IBrowserHost.PrintToPdf"/>. The methods of this interface
    /// will be called on the CEF UI thread.
    /// </summary>
    public interface IPrintToPdfCallback : IDisposable
    {
        /// <summary>
        /// Method that will be executed when the PDF printing has completed.
        /// </summary>
        /// <param name="path">The output path.</param>
        /// <param name="ok">Will be true if the printing completed
        /// successfully or false otherwise.</param>
        void OnPdfPrintFinished(string path, bool ok);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

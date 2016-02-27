﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Callback interface for <see cref="IBrowserHost.PrintToPDF"/>. The methods of this interface
    /// will be called on the browser process UI thread.
    /// </summary>
    public interface IPrintToPdfCallback
    {
        /// <summary>
        /// Method that will be executed when the PDF printing has completed.
        /// </summary>
        /// <param name="path">The output path.</param>
        /// <param name="ok">Will be true if the printing completed
        /// successfully or false otherwise.</param>
        void OnPdfPrintFinished(string path, bool ok);
    }
}

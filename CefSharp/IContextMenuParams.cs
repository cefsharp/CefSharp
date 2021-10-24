// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Wrapper for the CefContextMenuParams
    /// </summary>
    public interface IContextMenuParams : IDisposable
    {
        /// <summary>
        /// Returns the Y coordinate of the mouse where the context menu was invoked.
        /// Coords are relative to the associated RenderView's origin.
        /// </summary>
        int YCoord { get; }

        /// <summary>
        /// Returns the X coordinate of the mouse where the context menu was invoked.
        /// Coords are relative to the associated RenderView's origin.
        /// </summary>
        int XCoord { get; }

        /// <summary>
        /// Returns flags representing the type of node that the context menu was invoked on.
        /// </summary>
        ContextMenuType TypeFlags { get; }

        /// <summary>
        /// Returns the URL of the link, if any, that encloses the node that the
        /// context menu was invoked on.
        /// </summary>
        string LinkUrl { get; }

        /// <summary>
        /// Returns the link URL, if any, to be used ONLY for "copy link address". We
        /// don't validate this field in the frontend process.
        /// </summary>
        string UnfilteredLinkUrl { get; }

        /// <summary>
        /// Returns the source URL, if any, for the element that the context menu was
        /// invoked on. Example of elements with source URLs are img, audio, and video.
        /// </summary>
        string SourceUrl { get; }

        /// <summary>
        /// Returns true if the context menu was invoked on an image which has
        /// non-empty contents.
        /// </summary>
        bool HasImageContents { get; }

        /// <summary>
        /// Returns the URL of the top level page that the context menu was invoked on.
        /// </summary>
        string PageUrl { get; }

        /// <summary>
        /// Returns the URL of the subframe that the context menu was invoked on.
        /// </summary>
        string FrameUrl { get; }

        /// <summary>
        /// Returns the character encoding of the subframe that the context menu was
        /// invoked on.
        /// </summary>
        string FrameCharset { get; }

        /// <summary>
        /// Returns the type of context node that the context menu was invoked on.
        /// </summary>
        ContextMenuMediaType MediaType { get; }

        /// <summary>
        /// Returns flags representing the actions supported by the media element, if
        /// any, that the context menu was invoked on.
        /// </summary>
        ContextMenuMediaState MediaStateFlags { get; }

        /// <summary>
        /// Returns the text of the selection, if any, that the context menu was
        /// invoked on.
        /// </summary>
        string SelectionText { get; }

        /// <summary>
        /// Returns the text of the misspelled word, if any, that the context menu was
        /// invoked on.
        /// </summary>
        string MisspelledWord { get; }

        /// <summary>
        /// Returns a list of strings from the spell check service for the misspelled word if there is one.
        /// </summary>
        List<string> DictionarySuggestions { get; }

        /// <summary>
        /// Returns true if the context menu was invoked on an editable node.
        /// </summary>
        bool IsEditable { get; }

        /// <summary>
        /// Returns true if the context menu was invoked on an editable node where
        /// spell-check is enabled.
        /// </summary>
        bool IsSpellCheckEnabled { get; }

        /// <summary>
        /// Returns flags representing the actions supported by the editable node, if
        /// any, that the context menu was invoked on.
        /// </summary>
        /// <returns>Returns ContextMenuEditState as flags</returns>
        ContextMenuEditState EditStateFlags { get; }

        /// <summary>
        /// Returns true if the context menu contains items specified by the renderer
        /// process (for example, plugin placeholder or pepper plugin menu items).
        /// </summary>
        bool IsCustomMenu { get; }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

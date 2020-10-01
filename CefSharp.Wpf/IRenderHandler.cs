// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Controls;
using CefSharp.Structs;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Implement this interface to handle Offscreen Rendering (OSR).
    /// NOTE: Currently only OnPaint is implemented, at some point expand the API to include all
    /// of CefRenderHandler methods http://magpcss.org/ceforum/apidocs3/projects/(default)/CefRenderHandler.html
    /// </summary>
    public interface IRenderHandler : IDisposable
    {
        /// <summary>
        /// Called when an element has been rendered to the shared texture handle.
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to true
        /// </summary>
        /// <param name="isPopup">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="sharedHandle">is the handle for a D3D11 Texture2D that can be accessed via ID3D11Device using the OpenSharedResource method.</param>
        void OnAcceleratedPaint(bool isPopup, Rect dirtyRect, IntPtr sharedHandle);

        /// <summary>
        /// Called when an element should be painted. (Invoked from CefRenderHandler.OnPaint)
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to false.
        /// </summary>
        /// <param name="isPopup">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="image">image used as parent for rendered bitmap</param>
        void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image);
    }
}

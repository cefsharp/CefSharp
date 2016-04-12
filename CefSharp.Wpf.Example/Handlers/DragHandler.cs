// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Wpf.Example.Handlers
{
    public class DragHandler : IDragHandler, IDisposable
    {
        public event Action<IList<System.Windows.Rect>> RegionsChanged;

        bool IDragHandler.OnDragEnter(IWebBrowser browserControl, IBrowser browser, IDragData dragData, DragOperationsMask mask)
        {
            return false;
        }

        void IDragHandler.OnDraggableRegionsChanged(IWebBrowser browserControl, IBrowser browser, IList<DraggableRegion> regions)
        {
            if(browser.IsPopup == false)
            {
                //NOTE: I haven't tested with dynamically adding removing regions so this may need some tweaking

                var draggableRegions = new List<System.Windows.Rect>();

                foreach(var region in regions)
                { 
                    if(region.Draggable)
                    {
                        draggableRegions.Add(new System.Windows.Rect(region.X, region.Y, region.Width, region.Height));
                    }
                }

                var handler = RegionsChanged;

                if(handler != null)
                {
                    handler(draggableRegions);
                }
            } 
        }

        public void Dispose()
        {
            RegionsChanged = null;
        }
    }
}

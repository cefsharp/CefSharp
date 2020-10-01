// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Drawing;
using CefSharp.Enums;

namespace CefSharp.Wpf.Example.Handlers
{
    public class DragHandler : IDragHandler, IDisposable
    {
        public event Action<Region> RegionsChanged;

        bool IDragHandler.OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
        {
            return false;
        }

        void IDragHandler.OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
        {
            //By default popup browers are native windows in WPF so we cannot handle their drag using this method
            if (browser.IsPopup == false)
            {
                //NOTE: I haven't tested with dynamically adding removing regions so this may need some tweaking
                Region draggableRegion = null;

                if (regions.Count > 0)
                {
                    //Take the individual Region and construct a complex Region that represents them all.
                    foreach (var region in regions)
                    {
                        var rect = new Rectangle(region.X, region.Y, region.Width, region.Height);

                        if (draggableRegion == null)
                        {
                            draggableRegion = new Region(rect);
                        }
                        else
                        {
                            if (region.Draggable)
                            {
                                draggableRegion.Union(rect);
                            }
                            else
                            {
                                //In the scenario where we have an outer region, that is draggable and it has
                                // an inner region that's not, we must exclude the non draggable.
                                // Not all scenarios are covered in this example.
                                draggableRegion.Exclude(rect);
                            }
                        }
                    }
                }

                var handler = RegionsChanged;

                if (handler != null)
                {
                    handler(draggableRegion);
                }
            }
        }

        public void Dispose()
        {
            RegionsChanged = null;
        }
    }
}

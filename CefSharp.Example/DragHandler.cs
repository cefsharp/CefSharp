using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Example
{
    public class DragHandler : IDragHandler
    {
        public bool OnDragEnter(IWebBrowser browser, DragData dragData, DragOperationsMask mask)
        {
            //prevents draging object into browser
            return true;
        }
    }
}

namespace CefSharp.Example
{
    public class DragHandler : IDragHandler
    {
        public bool OnDragEnter(IWebBrowser browser, DragData dragData, DragOperationsMask mask)
        {
            return false;
        }
    }
}

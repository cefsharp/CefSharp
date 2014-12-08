namespace CefSharp.Example
{
    public class DragHandler : IDragHandler
    {
        public bool OnDragEnter(IWebBrowser browser, IDragData dragData, DragOperationsMask mask)
        {
            return false;
        }
    }
}

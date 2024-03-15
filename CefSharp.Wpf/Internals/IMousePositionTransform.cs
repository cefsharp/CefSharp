using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// Implement this interface to control transform the mouse position
    /// </summary>
    public interface IMousePositionTransform
    {
        System.Windows.Point UpdatePopupSizeAndPosition(Rect originalRect, Rect viewRect);
        void OnPopupShow(bool isOpen);
        void TransformMousePoint(ref System.Windows.Point point);
    }
}

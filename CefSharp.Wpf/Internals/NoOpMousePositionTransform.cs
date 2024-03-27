using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    public sealed class NoOpMousePositionTransform : IMousePositionTransform
    {
        System.Windows.Point IMousePositionTransform.UpdatePopupSizeAndPosition(Rect originalRect, Rect viewRect)
        {
            return new System.Windows.Point(originalRect.X, originalRect.Y);
        }

        void IMousePositionTransform.OnPopupShow(bool isOpen)
        {
        }

        void IMousePositionTransform.TransformMousePoint(ref System.Windows.Point point)
        {
            
        }
    }
}

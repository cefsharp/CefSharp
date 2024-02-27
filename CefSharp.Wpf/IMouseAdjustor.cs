using System;
using CefSharp.Structs;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Implement this interface to control how the mouse-adjustor is used.
    /// </summary>
    public interface IMouseAdjustor : IDisposable
    {
        System.Windows.Point UpdatePopupSizeAndPosition(Rect originalRect, Rect viewRect);
        void OnPopupShow(bool isOpen);
        Point GetAdjustedMouseCoords(System.Windows.Point point);
    }
}

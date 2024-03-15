using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    public sealed class MousePositionTransform : IMousePositionTransform
    {
        /// <summary>
        /// The x-offset.
        /// </summary>
        private int xOffset;

        /// <summary>
        /// The y-offset.
        /// </summary>
        private int yOffset;

        /// <summary>
        /// The original rect.
        /// </summary>
        private Rect originalRect;

        /// <summary>
        /// The adjusted rect.
        /// </summary>
        private Rect adjustedRect;

        /// <summary>
        /// If the popup is open or not.
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// Updates the size and the position of the popup.
        /// </summary>
        /// <param name="originalRect"></param>
        /// <param name="viewRect"></param>
        /// <returns>The adjusted point.</returns>
        System.Windows.Point IMousePositionTransform.UpdatePopupSizeAndPosition(Rect originalRect, Rect viewRect)
        {
            int x = originalRect.X,
                prevX = originalRect.X,
                y = originalRect.Y,
                prevY = originalRect.Y,
                xOffset = 0,
                yOffset = 0;

            // If popup goes outside the view, try to reposition origin
            if (originalRect.X + originalRect.Width > viewRect.Width)
            {
                x = viewRect.Width - originalRect.Width;
                xOffset = prevX - x;
            }

            if (originalRect.Y + originalRect.Height > viewRect.Height)
            {
                y = y - originalRect.Height - 20;
                yOffset = prevY - y;
            }

            // If x or y became negative, move them to 0 again
            if (x < 0)
            {
                x = 0;
                xOffset = prevX;
            }

            if (y < 0)
            {
                y = 0;
                yOffset = prevY;
            }

            if (x != prevX || y != prevY)
            {
                this.isOpen = true;

                this.xOffset = xOffset;
                this.yOffset = yOffset;

                Rect adjustedRect = new Rect(x, y, x + originalRect.Width, y + originalRect.Height);

                this.originalRect = originalRect;
                this.adjustedRect = adjustedRect;

                if (this.originalRect.Y < this.adjustedRect.Y + this.adjustedRect.Height)
                {
                    var newY = this.adjustedRect.Y + this.adjustedRect.Height;
                    this.originalRect = new Rect(originalRect.X, newY, originalRect.Width, originalRect.Y + originalRect.Height - newY);
                }
            }

            return new System.Windows.Point(x, y);
        }

        /// <summary>
        /// Resets the offsets and original-rect.
        /// <param name="isOpen">If the popup is open or not.</param>
        /// </summary>
        void IMousePositionTransform.OnPopupShow(bool isOpen)
        {
            if (!isOpen)
            {
                this.isOpen = false;

                this.xOffset = 0;
                this.yOffset = 0;

                this.originalRect = new Rect();
                this.originalRect = new Rect();
            }
        }

        /// <summary>
        /// Adjusts the mouse-coordinates when the popup is visible.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <returns>The transformed point if needed, else the original point.</returns>
        void IMousePositionTransform.TransformMousePoint(ref System.Windows.Point point)
        {
            if (!isOpen)
                return;

            if (!IsInsideOriginalRect(point) && IsInsideAdjustedRect(point))
                point = new System.Windows.Point((int)point.X + this.xOffset, (int)point.Y + this.yOffset);
        }

        /// <summary>
        /// Checks if the given point is inside the original-rect.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Returns true if the point is inside the original rect, else return false.</returns>
        private bool IsInsideOriginalRect(System.Windows.Point point)
        {
            return point.X >= this.originalRect.X &&
                   point.X < this.originalRect.X + this.originalRect.Width &&
                   point.Y >= this.originalRect.Y &&
                   point.Y < this.originalRect.Y + this.originalRect.Height;
        }

        /// <summary>
        /// Checks if the given point is inside the adjusted rect.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Returns true if the point is inside the adjusted rect, else return false.</returns>
        private bool IsInsideAdjustedRect(System.Windows.Point point)
        {
            return point.X >= this.adjustedRect.X &&
                   point.X < this.adjustedRect.X + this.adjustedRect.Width &&
                   point.Y >= this.adjustedRect.Y &&
                   point.Y < this.adjustedRect.Y + this.adjustedRect.Height;
        }
    }
}

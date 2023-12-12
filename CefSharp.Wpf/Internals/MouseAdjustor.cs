using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    public class MouseAdjustor
    {
        private int xOffset;
        private int yOffset;
        private Rect originalRect;
        private Rect teleportingRect;

        public System.Windows.Point Update(Rect originalRect, Rect viewRect)
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
                this.xOffset = xOffset;
                this.yOffset = yOffset;

                Rect teleportingRect = new Rect(x, y, x + originalRect.Width, y + originalRect.Height);

                this.originalRect = originalRect;
                this.teleportingRect = teleportingRect;

                if (this.originalRect.Y < this.teleportingRect.Y + this.teleportingRect.Height)
                {
                    var newY = this.teleportingRect.Y + this.teleportingRect.Height;
                    this.originalRect = new Rect(originalRect.X, newY, originalRect.Width, originalRect.Y + originalRect.Height - newY);
                }
            }

            return new System.Windows.Point(x, y);
        }

        public void Reset()
        {
            this.originalRect = new Rect();
            this.originalRect = new Rect();
            this.xOffset = 0;
            this.yOffset = 0;
        }

        public Point GetAdjustedMouseCoords(System.Windows.Point point)
        {
            return !this.IsInsideOriginalRect(point) && IsInsideTeleportingRect(point)
                ? new Point((int)point.X + this.xOffset, (int)point.Y + this.yOffset)
                : new Point((int)point.X, (int)point.Y);
        }

        private bool IsInsideOriginalRect(System.Windows.Point point)
        {
            return point.X >= this.originalRect.X &&
                   point.X < this.originalRect.X + this.originalRect.Width &&
                   point.Y >= this.originalRect.Y &&
                   point.Y < this.originalRect.Y + this.originalRect.Height;
        }

        private bool IsInsideTeleportingRect(System.Windows.Point point)
        {
            return point.X >= this.teleportingRect.X &&
                   point.X < this.teleportingRect.X + this.teleportingRect.Width &&
                   point.Y >= this.teleportingRect.Y &&
                   point.Y < this.teleportingRect.Y + this.teleportingRect.Height;
        }
    }
}

using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    public class MouseTeleport
    {
        public Rect teleportingRect { get; private set; }
        public Rect originalRect { get; private set; }
        public int xOffset { get; private set; }
        public int yOffset { get; private set; }

        public void Update(int xOffset, int yOffset, Rect originalRect, Rect teleportingRect)
        {
            this.originalRect = originalRect;
            this.teleportingRect = teleportingRect;
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            if (this.originalRect.Y < this.teleportingRect.Y + this.teleportingRect.Height)
            {
                var newY = this.teleportingRect.Y + this.teleportingRect.Height;
                this.originalRect = new Rect(originalRect.X, newY, originalRect.Width, originalRect.Y + originalRect.Height - newY);
            }
        }

        public void Reset()
        {
            this.originalRect = new Rect();
            this.originalRect = new Rect();
            this.xOffset = 0;
            this.yOffset = 0;
        }

        public bool IsInsideOriginalRect(int x, int y)
        {
            return x >= this.originalRect.X &&
                   x < this.originalRect.X + this.originalRect.Width &&
                   y >= this.originalRect.Y &&
                   y < this.originalRect.Y + this.originalRect.Height;
        }

        private bool IsInsideTeleportingRect(int x, int y)
        {
            return x >= this.teleportingRect.X &&
                   x < this.teleportingRect.X + this.teleportingRect.Width &&
                   y >= this.teleportingRect.Y &&
                   y < this.teleportingRect.Y + this.teleportingRect.Height;
        }

        public Point GetAdjustedMouseCoords(int x, int y)
        {
            return IsInsideTeleportingRect(x, y) ? new Point(x + this.xOffset, y + this.yOffset) : new Point(x, y);
        }
    }
}

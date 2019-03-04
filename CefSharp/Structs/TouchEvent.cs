namespace CefSharp
{
    public struct TouchEvent
    {
        public int Id { get; set; }
        public int TouchInputType { get; set; }

        /// <summary>
        /// x coordinate - relative to upper-left corner of view
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// y coordinate - relative to upper-left corner of view
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Bit flags describing any pressed modifier keys.
        /// </summary>
        public CefEventFlags Modifiers { get; private set; }

        public TouchEvent(int touchInputType, int id, int x, int y, CefEventFlags modifiers) : this()
        {
            Id = id;
            X = x;
            Y = y;
            TouchInputType = touchInputType;
            Modifiers = modifiers;
        }

    }

    public enum TouchInputType
    {
        Down = 0,
        Move = 1,
        Up = 2
    }
}

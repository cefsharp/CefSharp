namespace CefSharp
{
    /// <summary>
    /// Lists some of the error codes that can be reported by CEF.
    /// </summary>
    public enum CefErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None,

        /// <summary>
        /// A request was aborted, possibly by the user.
        /// </summary>
        Aborted,

        FileNotFound,

        Unknown,
    };
}

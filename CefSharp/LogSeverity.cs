namespace CefSharp
{
    public enum LogSeverity
    {
        /// <summary>
        /// Default logging (currently Info logging)
        /// </summary>
        Default = 0,

        /// <summary>
        /// Verbose logging.
        /// </summary>
        Verbose,

        /// <summary>
        /// Info logging
        /// </summary>
        Info,

        /// <summary>
        /// Warning logging
        /// </summary>
        Warning,

        /// <summary>
        /// Error logging
        /// </summary>
        Error,

        /// <summary>
        /// ErrorReport logging
        /// </summary>
        ErrorReport,

        /// <summary>
        /// Completely disable logging
        /// </summary>
        Disable = 99
    };
}
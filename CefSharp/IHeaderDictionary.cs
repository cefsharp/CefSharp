using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Represents a wrapper for RequestHeaders and ResponseHeaders.
    /// </summary>
    public interface IHeaderDictionary : IDictionary<string, string[]>, IHeaderCollection
    {
        /// <summary>
        /// Get or sets the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a single string or null if the key is not present.</returns>
        new string this[string key] { get; set; }

        /// <summary>
        /// Add a new value. Appends to the header if already present
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        void Append(string key, string value);

        /// <summary>
        /// Add new values. Each item remains a separate array entry.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        void AppendValues(string key, params string[] values);

        /// <summary>
        /// Sets a specific header value.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        void Set(string key, string value);

        /// <summary>
        /// Sets the specified header values without modification.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        void SetValues(string key, params string[] values);
    }
}
namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability to supply a convention to
    /// convert form field names to property names if required.
    /// </summary>
    public interface IFieldNameConverter
    {
        /// <summary>
        /// Converts a field name to a property name
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>Property name</returns>
        string Convert(string fieldName);
    }
}
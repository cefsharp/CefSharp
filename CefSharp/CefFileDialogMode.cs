namespace CefSharp
{
    public enum CefFileDialogMode
    {
        /// <summary>
        /// Requires that the file exists before allowing the user to pick it.
        /// </summary>
        Open = 0,

        /// <summary>
        /// Like Open, but allows picking multiple files to open.
        /// </summary>
        OpenMultiple,

        /// <summary>
        /// Allows picking a nonexistent file, and prompts to overwrite if the file already exists.
        /// </summary>
        Save
    }
}

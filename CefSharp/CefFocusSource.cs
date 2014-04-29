namespace CefSharp
{
	/// <summary>
	/// Focus Source
	/// </summary>
	public enum CefFocusSource
	{
		///
		// The source is explicit navigation via the API (LoadURL(), etc).
		///
		FocusSourceNavigation = 0,
		///
		// The source is a system-generated focus event.
		///
		FocusSourceSystem
	}
}

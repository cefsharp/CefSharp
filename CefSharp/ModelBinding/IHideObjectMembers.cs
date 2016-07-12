namespace CefSharp.ModelBinding
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// Helper interface used to hide the base <see cref="object"/>  members from the fluent API to make it much cleaner
	/// in Visual Studio intellisense.
	/// </summary>
	/// <remarks>Created by Daniel Cazzulino http://www.clariusconsulting.net/blogs/kzu/archive/2008/03/10/58301.aspx</remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IHideObjectMembers
	{
		/// <summary>
		/// Hides the <see cref="Equals"/> method.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object obj);

		/// <summary>
		/// Hides the <see cref="GetHashCode"/> method.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();

		/// <summary>
		/// Hides the <see cref="GetType"/> method.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type GetType();

		/// <summary>
		/// Hides the <see cref="ToString"/> method.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();
	}
}
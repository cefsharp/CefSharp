// Copyright © 2010-2017 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp
{
	public static class AsyncExtensions
	{
		/// <summary>
		/// Deletes all cookies that matches all the provided parameters asynchronously. If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.
		/// </summary>
		/// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
		/// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
		/// <return>A task that represents the delete operation. The value of the TResult parameter contains false if a non-empty invalid URL is specified, or if cookies cannot be accessed; otherwise, true.</return>
		//public static Task<bool> DeleteCookiesAsync(this ICookieManager cookieManager, string url = null, string name = null)
		//{
		//	if(cookieManager == null)
		//	{
		//		throw new NullReferenceException("cookieManager");
		//	}

		//	if(cookieManager.IsDisposed)
		//	{
		//		throw new ObjectDisposedException("cookieManager");
		//	}

		//	auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, name);

		//	if (CefCurrentlyOn(TID_IO))
		//	{
		//		auto source = gcnew TaskCompletionSource<bool>();
		//		CefSharp::Internals::TaskExtensions::TrySetResultAsync<bool>(source, cookieInvoker->DeleteCookies());
		//		return source->Task;
		//	}

		//	return Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::DeleteCookies));
		//}

		//public static Task<bool> SetCookieAsync(this ICookieManager cookieManager, string url, Cookie cookie)
		//{
		//	ThrowIfDisposed();

		//	auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, cookie->Name, cookie->Value, cookie->Domain, cookie->Path, cookie->Secure, cookie->HttpOnly, cookie->Expires.HasValue, cookie->Expires.HasValue ? cookie->Expires.Value : DateTime());

		//	if (CefCurrentlyOn(TID_IO))
		//	{
		//		auto source = gcnew TaskCompletionSource<bool>();
		//		CefSharp::Internals::TaskExtensions::TrySetResultAsync<bool>(source, cookieInvoker->SetCookie());
		//		return source->Task;
		//	}

		//	return Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::SetCookie));
		//}


		/// <summary>
		/// Visits all cookies. The returned cookies are sorted by longest path, then by earliest creation date.
		/// </summary>
		/// <return>A task that represents the VisitAllCookies operation. The value of the TResult parameter contains a List of cookies.</return>
		public static Task<List<Cookie>> VisitAllCookiesAsync(this ICookieManager cookieManager)
		{
			var cookieVisitor = new TaskCookieVisitor();

			if(cookieManager.VisitAllCookies(cookieVisitor))
			{
				return cookieVisitor.Task;
			}

			return null;
		}

		/// <summary>
		/// Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
		/// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
		/// are sorted by longest path, then by earliest creation date.
		/// </summary>
		/// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
		/// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
		/// <return>A task that represents the VisitUrlCookies operation. The value of the TResult parameter contains a List of cookies.</return>
		public static Task<List<Cookie>> VisitUrlCookiesAsync(this ICookieManager cookieManager, string url, bool includeHttpOnly)
		{
			var cookieVisitor = new TaskCookieVisitor();

			if(cookieManager.VisitUrlCookies(url, includeHttpOnly, cookieVisitor))
			{
				return cookieVisitor.Task;
			}

			return null;
		}

		/// <summary>
		/// Flush the backing store (if any) to disk.
		/// </summary>
		/// <param name="cookieManager">cookieManager instance</param>
		/// <returns>A task that represents the FlushStore operation. Result indicates if the flush completed successfully.
		/// Will return null if the cookikes cannot be accessed.</returns>
		public static Task<bool> FlushStoreAsync(this ICookieManager cookieManager)
		{
			var handler = new TaskCompletionCallback();

			if (cookieManager.FlushStore(handler))
			{
				return handler.Task;
			}

			//returns null if cookies cannot be accessed.
			return null;
		}
	}
}

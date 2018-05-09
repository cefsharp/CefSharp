// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Event
{
	/// <summary>
	/// Event arguments for the <see cref="IJavascriptObjectRepository.ObjectsBoundInJavascript"/> event
	/// </summary>
	public class JavascriptBindingMultipleCompleteEventArgs : EventArgs
	{
		/// <summary>
		/// The javascript object repository, used to register objects
		/// </summary>
		public IJavascriptObjectRepository ObjectRepository { get; private set; }

		/// <summary>
		/// Name of the objects bound
		/// </summary>
		public IList<string> ObjectNames { get; private set; }

		public JavascriptBindingMultipleCompleteEventArgs(IJavascriptObjectRepository objectRepository, IList<string> names)
		{
			ObjectRepository = objectRepository;
			ObjectNames = names;
		}
	}
}

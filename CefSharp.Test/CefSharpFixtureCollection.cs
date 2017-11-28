// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;

namespace CefSharp.Test
{
	/// <summary>
	/// All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
	/// </summary>
	[CollectionDefinition(CefSharpFixtureCollection.Key)]
	public class CefSharpFixtureCollection : ICollectionFixture<CefSharpFixture>
	{
		public const string Key = "CefSharp Test Collection";
	}
}

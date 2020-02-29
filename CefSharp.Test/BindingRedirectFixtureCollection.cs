// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;

namespace CefSharp.Test
{
    /// <summary>
    /// All Test classes which require binding redirects defined in app.config must be part of this collection
    /// </summary>
    [CollectionDefinition(BindingRedirectFixtureCollection.Key)]
    public class BindingRedirectFixtureCollection : ICollectionFixture<BindingRedirectFixture>
    {
        public const string Key = "Binding Redirect Collection";
    }
}

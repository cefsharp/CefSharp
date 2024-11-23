// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Enums;
using CefSharp.Example;
using CefSharp.Internals;
using Xunit;

namespace CefSharp.Test.Framework
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class RequestContextTests
    {
        [Fact]
        public void IsSameAs()
        {
            var ctx1 = new RequestContext();
            var ctx2 = ctx1.UnWrap();

            Assert.True(ctx1.IsSame(ctx2));
        }

        [Fact]
        public void IsSharingWith()
        {
            var ctx1 = RequestContext.Configure()
                .WithCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\TempCache1"))
                .Create();
            var ctx2 = new RequestContext(ctx1);

            Assert.True(ctx1.IsSharingWith(ctx2));
        }

        [Fact]
        public void CanGetContentSetting()
        {
            var ctx = RequestContext.Configure()
                .WithCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\TempCache2"))
                .Create();

            var actual = ctx.GetContentSetting(CefExample.DefaultUrl, null, ContentSettingTypes.Autoplay);

            Assert.Equal(ContentSettingValues.Default, actual);
        }

        [Fact]
        public async Task CanSetContentSetting()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            var ctx = RequestContext.Configure()
                .WithCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\TempCache3"))
                .OnInitialize((ctx) =>
                {
                    tcs.SetResult(true);
                })
                .Create();

            await tcs.Task;

            var actual = ContentSettingValues.Default;

            await CefThread.ExecuteOnUiThread(() =>
            {
                ctx.SetContentSetting(CefExample.DefaultUrl, null, ContentSettingTypes.Autoplay, ContentSettingValues.Block);

                actual = ctx.GetContentSetting(CefExample.DefaultUrl, null, ContentSettingTypes.Autoplay);
            });            

            Assert.Equal(ContentSettingValues.Block, actual);
        }

        [Fact]
        public async Task CanSetWebsiteSetting()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            var ctx = RequestContext.Configure()
                .WithCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\TempCache4"))
                .OnInitialize((ctx) =>
                {
                    tcs.SetResult(true);
                })
                .Create();

            await tcs.Task;

            object actual = ContentSettingValues.Default;

            await CefThread.ExecuteOnUiThread(() =>
            {
                ctx.SetWebsiteSetting(CefExample.DefaultUrl, null, ContentSettingTypes.Popups, (int)ContentSettingValues.Allow);

                actual = ctx.GetWebsiteSetting(CefExample.DefaultUrl, null, ContentSettingTypes.Popups);
            });

            Assert.Equal(ContentSettingValues.Allow, (ContentSettingValues)actual);
        }
    }
}

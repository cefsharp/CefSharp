// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Enums;
using CefSharp.Example;
using CefSharp.Wpf;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Wpf
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    [BrowserRefCountDebugging(typeof(ChromiumWebBrowser))]
    public class WpfBrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WpfBrowserTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [WpfFact]
        public async Task ShouldWorkWhenLoadingGoogle()
        {
            using (var browser = new ChromiumWebBrowser(null, "www.google.com", new Size(1024, 786)))
            {
                await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task ShouldWorkWhenLoadUrlAsyncImmediately()
        {
            using (var browser = new ChromiumWebBrowser(null, string.Empty, new Size(1024, 786)))
            {
                var response = await browser.LoadUrlAsync("www.google.com");
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task ShouldWorkWhenLoadUrlImmediately()
        {
            using (var browser = new ChromiumWebBrowser())
            {
                browser.LoadUrl("www.google.com");
                browser.CreateBrowser(null, new Size(1024, 786));

                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task ShouldRespectDisposed()
        {
            ChromiumWebBrowser browser;

            using (browser = new ChromiumWebBrowser(null, CefExample.DefaultUrl, new Size(1024, 786)))
            {
                await browser.WaitForInitialLoadAsync();
            }

            Assert.True(browser.IsDisposed);

            var ex = Assert.Throws<ObjectDisposedException>(() =>
            {
                browser.Copy();
            });
        }

        [WpfFact]
        public void ShouldRaiseCopyingEventAndHonorCancellationWhenStartingDrag()
        {
            using (var browser = new TestChromiumWebBrowser())
            {
                var dragData = new Mock<IDragData>();
                dragData.SetupGet(x => x.FragmentText).Returns("dragged text");
                dragData.SetupGet(x => x.FragmentHtml).Returns("<b>dragged text</b>");

                var eventRaised = false;
                DataObject.AddCopyingHandler(browser, (sender, e) =>
                {
                    eventRaised = true;
                    Assert.Same(browser, sender);
                    Assert.True(e.IsDragDrop);
                    Assert.Equal("dragged text", e.DataObject.GetData(DataFormats.UnicodeText));
                    e.CancelCommand();
                });

                var handled = browser.StartDraggingForTest(dragData.Object);

                Assert.True(eventRaised);
                Assert.False(handled);
                dragData.Verify(x => x.Clone(), Times.Never());
            }
        }

        [WpfFact]
        public void ShouldCloneDragDataWhenStartingDrag()
        {
            using (var browser = new TestChromiumWebBrowser())
            {
                var dragData = new Mock<IDragData>();
                dragData.SetupGet(x => x.FragmentText).Returns("dragged text");
                dragData.SetupGet(x => x.FragmentHtml).Returns("<b>dragged text</b>");
                var clonedDragData = new Mock<IDragData>();
                dragData.Setup(x => x.Clone()).Returns(clonedDragData.Object);

                var handled = browser.StartDraggingForTest(dragData.Object);

                Assert.True(handled);
                dragData.Verify(x => x.Clone(), Times.Once());
            }
        }

        private sealed class TestChromiumWebBrowser : ChromiumWebBrowser
        {
            public bool StartDraggingForTest(IDragData dragData)
            {
                return StartDragging(dragData, DragOperationsMask.Copy, 0, 0);
            }
        }
    }
}

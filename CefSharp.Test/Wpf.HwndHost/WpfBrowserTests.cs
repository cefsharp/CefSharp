// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CefSharp.Example;
using CefSharp.Wpf.HwndHost;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Wpf.HwndHost
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    [BrowserRefCountDebugging(typeof(ChromiumWebBrowser))]
    public class WpfBrowserTests
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);

        private const int HWND_MESSAGE = -3;

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
            var window = CreateAndShowHiddenWindow();

            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                window.Content = browser;

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
            var window = CreateAndShowHiddenWindow();

            using (var browser = new ChromiumWebBrowser(string.Empty))
            {
                window.Content = browser;

                var response = await browser.LoadUrlAsync("www.google.com");
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
            var window = CreateAndShowHiddenWindow();

            ChromiumWebBrowser browser;

            using (browser = new ChromiumWebBrowser(CefExample.DefaultUrl))
            {
                window.Content = browser;

                await browser.WaitForInitialLoadAsync();
            }

            Assert.True(browser.IsDisposed);

            var ex = Assert.Throws<ObjectDisposedException>(() =>
            {
                browser.Copy();
            });
        }

        private static Window CreateAndShowHiddenWindow()
        {
            var window = new Window();
            window.Width = 1024;
            window.Height = 768;

            var helper = new WindowInteropHelper(window);

            helper.EnsureHandle();

            SetParent(helper.Handle, (IntPtr)HWND_MESSAGE);

            window.Show();
            return window;
        }
    }
}

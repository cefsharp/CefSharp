// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;
using CefSharp.Structs;

namespace CefSharp.Example.Handlers
{
    public class AudioHandler : Handler.AudioHandler
    {
        private ChannelLayout channelLayout;
        private int channelCount;
        private int sampleRate;

        protected override bool GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref AudioParameters parameters)
        {
            //Cancel Capture
            return false;
        }
        protected override void OnAudioStreamError(IWebBrowser chromiumWebBrowser, IBrowser browser, string errorMessage)
        {
            base.OnAudioStreamError(chromiumWebBrowser, browser, errorMessage);
        }

        protected override void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, AudioParameters parameters, int channels)
        {
            this.channelLayout = parameters.ChannelLayout;
            this.sampleRate = parameters.SampleRate;
            this.channelCount = channels;
        }

        protected override void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr data, int noOfFrames, long pts)
        {
            //NOTE: data is an array representing the raw PCM data as a floating point type, i.e. 4-byte value(s)
            //Based on and the channelLayout value passed to IAudioHandler.OnAudioStreamStarted
            //you can calculate the size of the data array in bytes.
            //See https://github.com/cefsharp/CefSharp/issues/2806 for discussion on implementing an example.
        }

        protected override void OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            base.OnAudioStreamStopped(chromiumWebBrowser, browser);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

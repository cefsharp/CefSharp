// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using CefSharp.Enums;

namespace CefSharp.Example.Handlers
{
    public class AudioHandler : IAudioHandler
    {
        private ChannelLayout channelLayout;

        void IAudioHandler.OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId, IntPtr data, int noOfFrames, long pts)
        {
            var channelLayoutSize = 2;
            var sizeOfData = noOfFrames * channelLayoutSize * 4;

            float[] managedData = new float[sizeOfData];

            Marshal.Copy(data, managedData, 0, sizeOfData);

        }

        void IAudioHandler.OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId, int channels, ChannelLayout channelLayout, int sampleRate, int framesPerBuffer)
        {
            this.channelLayout = channelLayout;
        }

        void IAudioHandler.OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId)
        {

        }
    }
}

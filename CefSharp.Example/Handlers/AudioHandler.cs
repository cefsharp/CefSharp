// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp.Example.Handlers
{
    public class AudioHandler : IAudioHandler
    {
        private ChannelLayout channelLayout;
        private int channelCount;
        private int sampleRate;

        void IAudioHandler.OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId, IntPtr data, int noOfFrames, long pts)
        {
            //NOTE: data is an array representing the raw PCM data as a floating point type, i.e. 4-byte value(s)
            //Based on and the channelLayout value passed to IAudioHandler.OnAudioStreamStarted
            //you can calculate the size of the data array in bytes.
        }

        void IAudioHandler.OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId, int channels, ChannelLayout channelLayout, int sampleRate, int framesPerBuffer)
        {
            this.channelLayout = channelLayout;
            this.channelCount = channels;
            this.sampleRate = sampleRate;
        }

        void IAudioHandler.OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId)
        {

        }
    }
}

// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle audio events
    /// All methods will be called on the CEF UI thread
    /// </summary>
    public interface IAudioHandler
    {
        /// <summary>
        /// Called when the stream identified by <paramref name="audioStreamId"/> has started.
        /// <see cref="IAudioHandler.OnAudioSteamStopped"/> will always be called after  <see cref="OnAudioStreamStarted"/>;
        /// both methods may be called multiple times for the same stream.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="audioStreamId">will uniquely identify the stream across all future IAudioHandler callbacks</param>
        /// <param name="channels">is the number of channels</param>
        /// <param name="channelLayout">is the layout of the channels</param>
        /// <param name="sampleRate"> is the stream sample rate</param>
        /// <param name="framesPerBuffer">is the maximum number of frames that will occur in the PCM packet passed to OnAudioStreamPacket</param>
        void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser,
            IBrowser browser,
            int audioStreamId,
            int channels,
            ChannelLayout channelLayout,
            int sampleRate,
            int framesPerBuffer);

        /// <summary>
        /// Called when a PCM packet is received for the stream identified by <paramref name="audioStreamId"/>
        /// Based on and the <see cref="ChannelLayout"/> value passed to <see cref="IAudioHandler.OnAudioStreamStarted"/>
        /// you can calculate the size of the <paramref name="data"/> array in bytes.
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="audioStreamId">will uniquely identify the stream across all future IAudioHandler callbacks</param>
        /// <param name="data">is an array representing the raw PCM data as a floating point type, i.e. 4-byte value(s)</param>
        /// <param name="noOfFrames">is the number of frames in the PCM packet</param>
        /// <param name="pts">is the presentation timestamp (in milliseconds since the Unix Epoch)
        /// and represents the time at which the decompressed packet should be presented to the user</param>
        void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId, IntPtr data, int noOfFrames, long pts);

        /// <summary>
        /// Called when the stream identified by <paramref name="audioStreamId"/> has stopped.
        /// <see cref="OnAudioStreamStopped"/> will always be called after <see cref="IAudioHandler.OnAudioStreamStarted"/>;
        /// both methods may be called multiple times for the same stream.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="audioStreamId">will uniquely identify the stream across all future IAudioHandler callbacks</param>
        void OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser, int audioStreamId);
    }
}

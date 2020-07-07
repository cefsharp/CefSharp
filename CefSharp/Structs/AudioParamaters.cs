// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;

namespace CefSharp.Structs
{
    /// <summary>
    /// Structure representing the audio parameters for setting up the audio handler.
    /// </summary>
    public struct AudioParameters
    {
        /// <summary>
        /// Layout of the audio channels
        /// </summary>
        public ChannelLayout ChannelLayout { get; set; }

        /// <summary>
        /// Sample rate
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// Number of frames per buffer
        /// </summary>
        public int FramesPerBuffer { get; set; }

        /// <summary>
        /// Init with default values
        /// </summary>
        /// <param name="channelLayout">channel layout</param>
        /// <param name="sampleRate">sample rate</param>
        /// <param name="framesPerBuffer">frames per buffer</param>
        public AudioParameters(ChannelLayout channelLayout, int sampleRate, int framesPerBuffer)
        {
            ChannelLayout = channelLayout;
            SampleRate = sampleRate;
            FramesPerBuffer = framesPerBuffer;
        }
    }
}

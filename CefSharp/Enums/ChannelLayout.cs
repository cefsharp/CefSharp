// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Enumerates the various representations of the ordering of audio channels.
    /// Logged to UMA, so never reuse a value, always add new/greater ones!
    /// See media\base\channel_layout.h
    /// </summary>
    public enum ChannelLayout
    {
        /// <summary>
        /// None
        /// </summary>
        LayoutNone = 0,

        /// <summary>
        /// Unsupported
        /// </summary>
        LayoutUnsupported = 1,

        /// <summary>
        /// Front C
        /// </summary>
        LayoutMono = 2,

        /// <summary>
        /// Front L, Front R
        /// </summary>
        LayoutStereo = 3,

        /// <summary>
        /// Front L, Front R, Back C
        /// </summary>
        Layout2_1 = 4,

        /// <summary>
        /// Front L, Front R, Front C
        /// </summary>
        LayoutSurround = 5,

        /// <summary>
        /// Front L, Front R, Front C, Back C
        /// </summary>
        Layout4_0 = 6,

        /// <summary>
        /// Front L, Front R, Side L, Side R
        /// </summary>
        Layout2_2 = 7,

        /// <summary>
        /// Front L, Front R, Back L, Back R
        /// </summary>
        LayoutQuad = 8,

        /// <summary>
        /// Front L, Front R, Front C, Side L, Side R
        /// </summary>
        Layout5_0 = 9,

        /// <summary>
        /// Front L, Front R, Front C, LFE, Side L, Side R
        /// </summary>
        Layout5_1 = 10,

        /// <summary>
        /// Front L, Front R, Front C, Back L, Back R
        /// </summary>
        Layout5_0Back = 11,

        /// <summary>
        /// Front L, Front R, Front C, LFE, Back L, Back R
        /// </summary>
        Layout5_1Back = 12,

        /// <summary>
        /// Front L, Front R, Front C, Side L, Side R, Back L, Back R
        /// </summary>
        Layout7_0 = 13,

        /// <summary>
        /// Front L, Front R, Front C, LFE, Side L, Side R, Back L, Back R
        /// </summary>
        Layout7_1 = 14,

        /// <summary>
        /// Front L, Front R, Front C, LFE, Side L, Side R, Front LofC, Front RofC
        /// </summary>
        Layout7_1Wide = 15,

        /// <summary>
        /// Stereo L, Stereo R
        /// </summary>
        LayoutStereoDownMix = 16,

        /// <summary>
        /// Stereo L, Stereo R, LFE
        /// </summary>
        Layout2Point1 = 17,

        /// <summary>
        /// Stereo L, Stereo R, Front C, LFE
        /// </summary>
        Layout3_1 = 18,

        /// <summary>
        /// Stereo L, Stereo R, Front C, Rear C, LFE
        /// </summary>
        Layout4_1 = 19,

        /// <summary>
        /// Stereo L, Stereo R, Front C, Side L, Side R, Back C
        /// </summary>
        Layout6_0 = 20,

        /// <summary>
        /// Stereo L, Stereo R, Side L, Side R, Front LofC, Front RofC
        /// </summary>
        Layout6_0Front = 21,

        /// <summary>
        /// Stereo L, Stereo R, Front C, Rear L, Rear R, Rear C
        /// </summary>
        LayoutHexagonal = 22,

        /// <summary>
        /// Stereo L, Stereo R, Front C, LFE, Side L, Side R, Rear Center
        /// </summary>
        Layout6_1 = 23,

        /// <summary>
        /// Stereo L, Stereo R, Front C, LFE, Back L, Back R, Rear Center
        /// </summary>
        Layout6_1Back = 24,

        /// <summary>
        /// Stereo L, Stereo R, Side L, Side R, Front LofC, Front RofC, LFE
        /// </summary>
        Layout6_1Front = 25,

        /// <summary>
        /// Front L, Front R, Front C, Side L, Side R, Front LofC, Front RofC
        /// </summary>
        Layout7_0Front = 26,

        /// <summary>
        /// Front L, Front R, Front C, LFE, Back L, Back R, Front LofC, Front RofC
        /// </summary>
        Layout7_1WideBack = 27,

        /// <summary>
        /// Front L, Front R, Front C, Side L, Side R, Rear L, Back R, Back C.
        /// </summary>
        LayoutOctagonal = 28,

        /// <summary>
        /// Channels are not explicitly mapped to speakers.
        /// </summary>
        LayoutDiscrete = 29,

        /// <summary>
        /// Front L, Front R, Front C. Front C contains the keyboard mic audio. This
        /// layout is only intended for input for WebRTC. The Front C channel
        /// is stripped away in the WebRTC audio input pipeline and never seen outside
        /// of that.
        /// </summary>
        LayoutStereoKeyboardAndMic = 30,

        /// <summary>
        /// Front L, Front R, Side L, Side R, LFE
        /// </summary>
        Layout4_1QuadSize = 31,

        /// <summary>
        /// Actual channel layout is specified in the bitstream and the actual channel
        /// count is unknown at Chromium media pipeline level (useful for audio
        /// pass-through mode).
        /// </summary>
        LayoutBitstream = 32,

        // Max value, must always equal the largest entry ever logged.
        LayoutMax = LayoutBitstream
    }
}

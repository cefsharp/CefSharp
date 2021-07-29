// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp;
using CefSharp.Enums;
using CefSharp.Handler;
using System;
using System.IO;

namespace CefSharp.Example.Handlers
{
    public class AudioHandlerExample : AudioHandler
    {
        private readonly string pathToSaveAudioData;
        private ChannelLayout channelLayout;
        private int channelCount;
        private int sampleRate;
        private readonly FileStream rawAudioFile;

        public AudioHandlerExample(string path) : base()
        {
            // The output file with raw audio data (PCM, 32-bit, float) will be saved in this path
            this.pathToSaveAudioData = path;
            this.rawAudioFile = new FileStream(this.pathToSaveAudioData, FileMode.Create, FileAccess.Write);
        }

        protected override bool GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref CefSharp.Structs.AudioParameters parameters)
        {
            // return true to activate audio stream capture
            return true;
        }

        protected override void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, CefSharp.Structs.AudioParameters parameters, int channels)
        {
            this.channelLayout = parameters.ChannelLayout;
            this.sampleRate = parameters.SampleRate;
            this.channelCount = channels;
        }

        protected override void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr data, int noOfFrames, long pts)
        {
            /*
             * NOTE: data is an array representing the raw PCM data as a floating point type, i.e. 4-byte value(s)
             * Based on noOfFrames and the channels value passed to IAudioHandler.OnAudioStreamStarted
             * you can calculate the size of the data array in bytes.
             * 
             * Audio data (PCM, 32-bit, float) will be save to rawAudioFile stream.
             */

            unsafe
            {
                float** channelData = (float**)data.ToPointer();
                int size = channelCount * noOfFrames * 4;
                byte[] samples = new byte[size];
                fixed (byte* pDestByte = samples)
                {
                    float* pDest = (float*)pDestByte;

                    for (int i = 0; i < noOfFrames; i++)
                    {
                        for (int c = 0; c < channelCount; c++)
                        {
                            *pDest++ = channelData[c][i];
                        }
                    }

                    var ustream = new UnmanagedMemoryStream(pDestByte, size);
                    ustream.CopyTo(rawAudioFile);
                    ustream.Close();
                }
            }
        }

        protected override void OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            base.OnAudioStreamStopped(chromiumWebBrowser, browser);
        }

        protected override void OnAudioStreamError(IWebBrowser chromiumWebBrowser, IBrowser browser, string errorMessage)
        {
            base.OnAudioStreamError(chromiumWebBrowser, browser, errorMessage);
        }

        protected override void Dispose(bool disposing)
        {
            rawAudioFile.Close();
            base.Dispose(disposing);
        }
    }
}

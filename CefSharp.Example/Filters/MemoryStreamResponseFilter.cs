// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp.Example.Filters
{
	public class MemoryStreamResponseFilter : IResponseFilter
	{
		private MemoryStream memoryStream;

		bool IResponseFilter.InitFilter()
		{
			//NOTE: We could initialize this earlier, just one possible use of InitFilter
			memoryStream = new MemoryStream();
			 
			return true;
		}

		FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
		{
			if (dataIn == null)
			{
				dataInRead = 0;
				dataOutWritten = 0;

				return FilterStatus.Done;
			}

			dataInRead = dataIn.Length;
			dataOutWritten = Math.Min(dataInRead, dataOut.Length);

			//Important we copy dataIn to dataOut
			dataIn.CopyTo(dataOut);

			//Copy data to stream
			dataIn.Position = 0;
			dataIn.CopyTo(memoryStream);

			return FilterStatus.Done;
		}

		void IDisposable.Dispose()
		{
			memoryStream.Dispose();
			memoryStream = null;
		}

		public byte[] Data
		{
			get { return memoryStream.ToArray(); }
		}
	}
}

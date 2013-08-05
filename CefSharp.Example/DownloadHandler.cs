using System;
using System.IO;

namespace CefSharp.Example
{
    internal class DownloadHandler : IDownloadHandler
    {
        public bool OnBeforeDownload(string suggestedName, out string downloadPath, out bool showDialog)
        {
            downloadPath = Path.GetTempPath();
            showDialog = true;

            return true;
        }

        public bool ReceivedData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }
    }
}

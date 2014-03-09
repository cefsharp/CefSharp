using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface IDownloadHandler
    {
        bool OnBeforeDownload(string suggestedName, out string downloadPath, out bool showDialog);
        bool ReceivedData(byte[] data);
        void Complete();
    }
}

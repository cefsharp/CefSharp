using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CefSharp.WinForms.Example
{
    public class SampleDownloadHandler : IDownloadHandler
    {
        private string targetFileName;
        private Stream targetStream;
        #region IDownloadHandler Members

        public void HandleComplete()
        {
            targetStream.Close();
            System.Diagnostics.Process.Start(targetFileName);           
        }

        public bool HandleDownload(IWebBrowser browserControl, string mimeType, string fileName)
        {
            targetFileName = Path.GetTempPath() + Guid.NewGuid().ToString() + "." + Path.GetExtension(fileName);
            targetStream = File.Create(targetFileName);
            return true;
        }

        public bool HandleReceivedData(byte[] data)
        {

            targetStream.Write(data, 0, data.GetLength(0));
            return true;
        }

        #endregion
    }
}

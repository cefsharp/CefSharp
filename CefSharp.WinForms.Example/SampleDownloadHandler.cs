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

            if (MessageBox.Show("Downloaded.  Open?", "Download Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(targetFileName);
            }

            targetStream = null;
            targetFileName = null;
        }

        public bool HandleDownload(IWebBrowser browserControl, string mimeType, long contentLength, string fileName)
        {
            // Bail if ongoing download.
            if (targetStream != null) return false;

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

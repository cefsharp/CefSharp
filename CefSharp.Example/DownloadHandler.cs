namespace CefSharp.Example
{
    internal class DownloadHandler : IDownloadHandler
    {
        bool IDownloadHandler.OnBeforeDownload(string suggestedName, out string downloadPath, out bool showDialog)
        {
            downloadPath = suggestedName;
            showDialog = true;

            return true;
        }

        bool IDownloadHandler.ReceivedData(byte[] data)
        {
            return false;
        }

        void IDownloadHandler.Complete()
        {
            
        }
    }
}

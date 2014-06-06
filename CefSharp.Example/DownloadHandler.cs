namespace CefSharp.Example
{
    internal class DownloadHandler : IDownloadHandler
    {
        public bool OnBeforeDownload(DownloadItem downloadItem, out string downloadPath, out bool showDialog)
        {
            downloadPath = downloadItem.SuggestedFileName;
            showDialog = true;

            return true;
        }

        public bool OnDownloadUpdated(DownloadItem downloadItem)
        {
            return false;
        }
    }
}

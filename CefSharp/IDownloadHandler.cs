namespace CefSharp
{
    public interface IDownloadHandler
    {
        bool OnBeforeDownload(DownloadItem downloadItem, out string downloadPath, out bool showDialog);

        bool OnDownloadUpdated(DownloadItem downloadItem);
    }
}

using System;

namespace CefSharp
{
    public class DownloadItem
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public string ContentDisposition { get; set; }
        public Int64 TotalBytes { get; set; }
        public int PercentComplete { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsComplete { get; set; }
        public bool IsInProgress { get; set; }
        public string SuggestedFileName { get; set; }
        public string FullPath { get; set; }
    }
}

using System;
using System.Drawing;

namespace CefSharp.BrowserSubprocess.Features
{
    [Serializable]
    public class ClipboardEntry
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public byte[] ImageData { get; set; }
        public string[] FilePaths { get; set; }
        public DateTime Timestamp { get; set; }
        public string SourceApp { get; set; }
        public ClipboardEntryType EntryType { get; set; }

        public ClipboardEntry()
        {
            Timestamp = DateTime.Now;
        }

        public string GetPreview(int maxLen = 80)
        {
            if (!string.IsNullOrEmpty(Text))
                return Text.Length > maxLen ? Text.Substring(0, maxLen) + "..." : Text;
            if (ImageData != null && ImageData.Length > 0)
                return $"[Image {ImageData.Length / 1024}KB]";
            if (FilePaths != null && FilePaths.Length > 0)
                return $"[Files: {string.Join(", ", FilePaths)}]";
            if (!string.IsNullOrEmpty(Html))
                return "[HTML Content]";
            return "[Empty]";
        }

        public string GetTypeIcon()
        {
            switch (EntryType)
            {
                case ClipboardEntryType.Text: return "\U0001F4DD";
                case ClipboardEntryType.Html: return "\U0001F310";
                case ClipboardEntryType.Image: return "\U0001F5BC";
                case ClipboardEntryType.Files: return "\U0001F4C1";
                default: return "\u2753";
            }
        }

        public Image GetImage()
        {
            if (ImageData == null || ImageData.Length == 0) return null;
            try
            {
                using (var ms = new System.IO.MemoryStream(ImageData))
                    return Image.FromStream(ms);
            }
            catch { return null; }
        }
    }

    [Serializable]
    public enum ClipboardEntryType
    {
        Text,
        Html,
        Image,
        Files
    }
}

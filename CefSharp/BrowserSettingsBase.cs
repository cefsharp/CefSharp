namespace CefSharp
{
    public abstract class BrowserSettingsBase : ObjectBase
    {
        public virtual string StandardFontFamily { get; set; }

        public virtual string FixedFontFamily { get; set; }

        public virtual string SerifFontFamily { get; set; }

        public virtual string SansSerifFontFamily { get; set; }

        public virtual string CursiveFontFamily { get; set; }

        public virtual string FantasyFontFamily { get; set; }

        public virtual int DefaultFontSize { get; set; }

        public virtual int DefaultFixedFontSize { get; set; }

        public virtual int MinimumFontSize { get; set; }

        public virtual int MinimumLogicalFontSize { get; set; }

        public virtual bool? RemoteFontsDisabled { get; set; }

        public virtual string DefaultEncoding { get; set; }

        public virtual bool? EncodingDetectorEnabled { get; set; }

        public virtual bool? JavaScriptDisabled { get; set; }

        public virtual bool? JavaScriptOpenWindowsDisallowed { get; set; }

        public virtual bool? JavaScriptCloseWindowsDisallowed { get; set; }

        public virtual bool? JavaScriptAccessClipboardDisallowed { get; set; }

        public virtual bool? DomPasteDisabled { get; set; }

        public virtual bool? CaretBrowsingEnabled { get; set; }

        public virtual bool? JavaDisabled { get; set; }

        public virtual bool? PluginsDisabled { get; set; }

        public virtual bool? UniversalAccessFromFileUrlsAllowed { get; set; }

        public virtual bool? FileAccessFromFileUrlsAllowed { get; set; }

        public virtual bool? WebSecurityDisabled { get; set; }

        public virtual bool? XssAuditorEnabled { get; set; }

        public virtual bool? ImageLoadDisabled { get; set; }

        public virtual bool? ShrinkStandaloneImagesToFit { get; set; }

        public virtual bool? SiteSpecificQuirksDisabled { get; set; }

        public virtual bool? TextAreaResizeDisabled { get; set; }

        public virtual bool? PageCacheDisabled { get; set; }

        public virtual bool? TabToLinksDisabled { get; set; }

        public virtual bool? HyperlinkAuditingDisabled { get; set; }

        public virtual bool? UserStyleSheetEnabled { get; set; }

        public virtual string UserStyleSheetLocation { get; set; }

        public virtual bool? AuthorAndUserStylesDisabled { get; set; }

        public virtual bool? LocalStorageDisabled { get; set; }

        public virtual bool? DatabasesDisabled { get; set; }

        public virtual bool? ApplicationCacheDisabled { get; set; }

        public virtual bool? WebGlDisabled { get; set; }

        public virtual bool? AcceleratedCompositingEnabled { get; set; }

        public virtual bool? AcceleratedLayersDisabled { get; set; }

        public virtual bool? Accelerated2dCanvasDisabled { get; set; }

        public virtual bool? AcceleratedPaintingDisabled { get; set; }

        public virtual bool? AcceleratedFiltersDisabled { get; set; }

        public virtual bool? AcceleratedPluginsDisabled { get; set; }

        public virtual bool? DeveloperToolsDisabled { get; set; }

        public virtual bool? FullscreenEnabled { get; set; }
    }
}

// Copyright © 2024 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Supported content setting types. Some types are platform-specific or only
    /// supported with Chrome style. Should be kept in sync with Chromium's
    /// ContentSettingsType type.
    /// </summary>
    public enum ContentSettingTypes
    {
        /// <summary>
        /// This setting governs whether cookies are enabled by the user in the
        /// provided context. However, it may be overridden by other settings. This
        /// enum should NOT be read directly to determine whether cookies are enabled;
        /// the client should instead rely on the CookieSettings API.
        /// </summary>
        Cookies = 0,
        Images,
        JavaScript,

        /// <summary>
        /// This setting governs both popups and unwanted redirects like tab-unders
        /// and framebusting.
        /// </summary>
        Popups,

        Geolocation,
        Notifications,
        AutoSelectCertificate,
        MixedScript,
        MediaStreamMic,
        MediaStreamCamera,
        ProtocolHandlers,
        DeprecatedPpapiBroker,
        AutomaticDownloads,

        /// <summary>
        /// Advanced device-specific functions on MIDI devices. MIDI-SysEx
        /// communications can be used for changing the MIDI device's persistent state
        /// such as firmware.
        /// </summary>
        MidiSysex,

        SslCertDecisions,
        ProtectedMediaIdentifier,
        AppBanner,
        SiteEngagement,
        DurableStorage,
        UsbChooserData,
        BluetoothGuard,
        BackgroundSync,
        Autoplay,
        ImportantSiteInfo,
        PermissionAutoblockerData,
        Ads,

        /// <summary>
        /// Website setting which stores metadata for the subresource filter to aid in
        /// decisions for whether or not to show the UI.
        /// </summary>
        AdsData,

        /// <summary>
        /// MIDI stands for Musical Instrument Digital Interface. It is a standard
        /// that allows electronic musical instruments, computers, and other devices
        /// to communicate with each other.
        /// </summary>
        Midi,

        /// <summary>
        /// This content setting type is for caching password protection service's
        /// verdicts of each origin.
        /// </summary>
        PasswordProtection,

        /// <summary>
        /// Website setting which stores engagement data for media related to a
        /// specific origin.
        /// </summary>
        MediaEngagement,

        /// <summary>
        /// Content setting which stores whether or not the site can play audible
        /// sound. This will not block playback but instead the user will not hear it.
        /// </summary>
        Sound,

        /// <summary>
        /// Website setting which stores the list of client hints that the origin
        /// requested the browser to remember. The browser is expected to send all
        /// client hints in the HTTP request headers for every resource requested
        /// from that origin.
        /// </summary>
        ClientHints,

        /// <summary>
        /// Generic Sensor API covering ambient-light-sensor, accelerometer, gyroscope
        /// and magnetometer are all mapped to a single content_settings_type.
        /// Setting for the Generic Sensor API covering ambient-light-sensor,
        /// accelerometer, gyroscope and magnetometer. These are all mapped to a
        /// single ContentSettingsType.
        /// </summary>
        Sensors,

        /// <summary>
        /// Content setting which stores whether or not the user has granted the site
        /// permission to respond to accessibility events, which can be used to
        /// provide a custom accessibility experience. Requires explicit user consent
        /// because some users may not want sites to know they're using assistive
        /// technology. Deprecated in M131.
        /// </summary>
        DeprecatedAccessibilityEvents,

        /// <summary>
        /// Used to store whether to allow a website to install a payment handler.
        /// </summary>
        PaymentHandler,

        /// <summary>
        /// Content setting which stores whether to allow sites to ask for permission
        /// to access USB devices. If this is allowed specific device permissions are
        /// stored under USB_CHOOSER_DATA.
        /// </summary>
        UsbGuard,

        /// <summary>
        /// Nothing is stored in this setting at present. Please refer to
        /// BackgroundFetchPermissionContext for details on how this permission
        /// is ascertained.
        /// </summary>
        BackgroundFetch,

        /// <summary>
        /// Website setting which stores the amount of times the user has dismissed
        /// intent picker UI without explicitly choosing an option.
        /// </summary>
        IntentPickerDisplay,

        /// <summary>
        /// Used to store whether to allow a website to detect user active/idle state.
        /// </summary>
        IdleDetection,

        /// <summary>
        /// Content settings for access to serial ports. The "guard" content setting
        /// stores whether to allow sites to ask for permission to access a port. The
        /// permissions granted to access particular ports are stored in the "chooser
        /// data" website setting.
        /// </summary>
        SerialGuard,
        SerialChooserData,

        /// <summary>
        /// Nothing is stored in this setting at present. Please refer to
        /// PeriodicBackgroundSyncPermissionContext for details on how this permission
        /// is ascertained.
        /// This content setting is not registered because it does not require access
        /// to any existing providers.
        /// </summary>
        PeriodicBackgroundSync,

        /// <summary>
        /// Content setting which stores whether to allow sites to ask for permission
        /// to do Bluetooth scanning.
        /// </summary>
        BluetoothScanning,

        /// <summary>
        /// Content settings for access to HID devices. The "guard" content setting
        /// stores whether to allow sites to ask for permission to access a device.
        /// The permissions granted to access particular devices are stored in the
        /// "chooser data" website setting.
        /// </summary>
        HidGuard,
        HidChooserData,

        /// <summary>
        /// Wake Lock API, which has two lock types: screen and system locks.
        /// Currently, screen locks do not need any additional permission, and system
        /// locks are always denied while the right UI is worked out.
        /// </summary>
        WakeLockScreen,
        WakeLockSystem,

        /// <summary>
        /// Legacy SameSite cookie behavior. This disables SameSite=Lax-by-default,
        /// SameSite=None requires Secure, and Schemeful Same-Site, forcing the
        /// legacy behavior wherein 1) cookies that don't specify SameSite are treated
        /// as SameSite=None, 2) SameSite=None cookies are not required to be Secure,
        /// and 3) schemeful same-site is not active.
        ///
        /// This will also be used to revert to legacy behavior when future changes
        /// in cookie handling are introduced.
        /// </summary>
        LegacyCookieAccess,

        /// <summary>
        /// Content settings which stores whether to allow sites to ask for permission
        /// to save changes to an original file selected by the user through the
        /// File System Access API.
        /// </summary>
        FileSystemWriteGuard,

        /// <summary>
        /// Used to store whether to allow a website to exchange data with NFC
        /// devices.
        /// </summary>
        Nfc,

        /// <summary>
        /// Website setting to store permissions granted to access particular
        /// Bluetooth devices.
        /// </summary>
        BluetoothChooserData,

        /// <summary>
        /// Full access to the system clipboard (sanitized read without user gesture,
        /// and unsanitized read and write with user gesture).
        /// </summary>
        ClipboardReadWrite,

        /// <summary>
        /// This is special-cased in the permissions layer to always allow, and as
        /// such doesn't have associated prefs data.
        /// </summary>
        ClipboardSanitizedWrite,

        /// <summary>
        /// This content setting type is for caching safe browsing real time url
        /// check's verdicts of each origin.
        /// </summary>
        SafeBrowsingUrlCheckData,

        /// <summary>
        /// Used to store whether a site is allowed to request AR or VR sessions with
        /// the WebXr Device API.
        /// </summary>
        Vr,
        Ar,

        /// <summary>
        /// Content setting which stores whether to allow site to open and read files
        /// and directories selected through the File System Access API.
        /// </summary>
        FileSystemReadGuard,

        /// <summary>
        /// Access to first party storage in a third-party context. Exceptions are
        /// scoped to the combination of requesting/top-level origin, and are managed
        /// through the Storage Access API. For the time being, this content setting
        /// exists in parallel to third-party cookie rules stored in COOKIES.
        /// </summary>
        StorageAccess,

        /// <summary>
        /// Content setting which stores whether to allow a site to control camera
        /// movements. It does not give access to camera.
        /// </summary>
        CameraPanTiltZoom,

        /// <summary>
        /// Content setting for Screen Enumeration and Screen Detail functionality.
        /// Permits access to detailed multi-screen information, like size and
        /// position. Permits placing fullscreen and windowed content on specific
        /// screens. See also: https://w3c.github.io/window-placement
        /// </summary>
        WindowManagement,

        /// <summary>
        /// Stores whether to allow insecure websites to make private network
        /// requests.
        /// See also: https://wicg.github.io/cors-rfc1918
        /// Set through enterprise policies only.
        /// </summary>
        InsecurePrivateNetwork,

        /// <summary>
        /// Content setting which stores whether or not a site can access low-level
        /// locally installed font data using the Local Fonts Access API.
        /// </summary>
        LocalFonts,

        /// <summary>
        /// Stores per-origin state for permission auto-revocation (for all permission
        /// types).
        /// </summary>
        PermissionAutorevocationData,

        /// <summary>
        /// Stores per-origin state of the most recently selected directory for the
        /// use by the File System Access API.
        /// </summary>
        FileSystemLastPickedDirectory,

        /// <summary>
        /// Controls access to the getDisplayMedia API.
        /// </summary>
        DisplayCapture,

        /// <summary>
        /// Website setting to store permissions metadata granted to paths on the
        /// local file system via the File System Access API.
        /// |FILE_SYSTEM_WRITE_GUARD| is the corresponding "guard" setting. The stored
        /// data represents valid permission only if
        /// |FILE_SYSTEM_ACCESS_EXTENDED_PERMISSION| is enabled via user opt-in.
        /// Otherwise, they represent "recently granted but revoked permission", which
        /// are used to restore the permission.
        /// </summary>
        FileSystemAccessChooserData,

        /// <summary>
        /// Stores a grant that allows a relying party to send a request for identity
        /// information to specified identity providers, potentially through any
        /// anti-tracking measures that would otherwise prevent it. This setting is
        /// associated with the relying party's origin.
        /// </summary>
        FederatedIdentitySharing,

        /// <summary>
        /// Whether to use the v8 optimized JIT for running JavaScript on the page.
        /// </summary>
        JavascriptJit,

        /// <summary>
        /// Content setting which stores user decisions to allow loading a site over
        /// HTTP. Entries are added by hostname when a user bypasses the HTTPS-First
        /// Mode interstitial warning when a site does not support HTTPS. Allowed
        /// hosts are exact hostname matches -- subdomains of a host on the allowlist
        /// must be separately allowlisted.
        /// </summary>
        HttpAllowed,

        /// <summary>
        /// Stores metadata related to form fill, such as e.g. whether user data was
        /// autofilled on a specific website.
        /// </summary>
        FormfillMetadata,

        /// <summary>
        /// Setting to indicate that there is an active federated sign-in session
        /// between a specified relying party and a specified identity provider for
        /// a specified account. When this is present it allows access to session
        /// management capabilities between the sites. This setting is associated
        /// with the relying party's origin.
        /// Obsolete on Nov 2023.
        /// </summary>
        DeprecatedFederatedIdentityActiveSession,

        /// <summary>
        /// Setting to indicate whether Chrome should automatically apply darkening to
        /// web content.
        /// </summary>
        AutoDarkWebContent,

        /// <summary>
        /// Setting to indicate whether Chrome should request the desktop view of a
        /// site instead of the mobile one.
        /// </summary>
        RequestDesktopSite,

        /// <summary>
        /// Setting to indicate whether browser should allow signing into a website
        /// via the browser FedCM API.
        /// </summary>
        FederatedIdentityApi,

        /// <summary>
        /// Stores notification interactions per origin for the past 90 days.
        /// Interactions per origin are pre-aggregated over seven-day windows: A
        /// notification interaction or display is assigned to the last Monday
        /// midnight in local time.
        /// </summary>
        NotificationInteractions,

        /// <summary>
        /// Website setting which stores the last reduced accept language negotiated
        /// for a given origin, to be used on future visits to the origin.
        /// </summary>
        ReducedAcceptLanguage,

        /// <summary>
        /// Website setting which is used for NotificationPermissionReviewService to
        /// store origin blocklist from review notification permissions feature.
        /// </summary>
        NotificationPermissionReview,

        /// <summary>
        /// Website setting to store permissions granted to access particular devices
        /// in private network.
        /// </summary>
        PrivateNetworkGuard,
        PrivateNetworkChooserData,

        /// <summary>
        /// Website setting which stores whether the browser has observed the user
        /// signing into an identity-provider based on observing the IdP-SignIn-Status
        /// HTTP header.
        /// </summary>
        FederatedIdentityIdentityProviderSigninStatus,

        /// <summary>
        /// Website setting which is used for UnusedSitePermissionsService to
        /// store revoked permissions of unused sites from unused site permissions
        /// feature.
        /// </summary>
        RevokedUnusedSitePermissions,

        /// <summary>
        /// Similar to STORAGE_ACCESS, but applicable at the page-level rather than
        /// being specific to a frame.
        /// </summary>
        TopLevelStorageAccess,

        /// <summary>
        /// Setting to indicate whether user has opted in to allowing auto re-authn
        /// via the FedCM API.
        /// </summary>
        FederatedIdentityAutoReauthnPermission,

        /// <summary>
        /// Website setting which stores whether the user has explicitly registered
        /// a website as an identity-provider.
        /// </summary>
        FederatedIdentityIdentityProviderRegistration,

        /// <summary>
        /// Content setting which is used to indicate whether anti-abuse functionality
        /// should be enabled.
        /// </summary>
        AntiAbuse,

        /// <summary>
        /// Content setting used to indicate whether third-party storage partitioning
        /// should be enabled.
        /// </summary>
        ThirdPartyStoragePartitioning,

        /// <summary>
        /// Used to indicate whether HTTPS-First Mode is enabled on the hostname.
        /// </summary>
        HttpsEnforced,

        /// <summary>
        /// Setting for enabling the `getAllScreensMedia` API. Spec link:
        /// https://github.com/screen-share/capture-all-screens
        /// </summary>
        AllScreenCapture,

        /// <summary>
        /// Stores per origin metadata for cookie controls.
        /// </summary>
        CookieControlsMetadata,

        /// <summary>
        /// Content Setting for temporary 3PC accesses granted by user behavior
        /// heuristics.
        /// </summary>
        TpcdHeuristicsGrants,

        /// <summary>
        /// Content Setting for 3PC accesses granted by metadata delivered via the
        /// component updater service. This type will only be used when
        /// `net::features::kTpcdMetadataGrants` is enabled.
        /// </summary>
        TpcdMetadataGrants,

        /// <summary>
        /// Content Setting for 3PC accesses granted via 3PC deprecation trial.
        /// </summary>
        TpcdTrial,

        /// <summary>
        /// Content Setting for 3PC accesses granted via top-level 3PC deprecation
        /// trial. Similar to TPCD_TRIAL, but applicable at the page-level for the
        /// lifetime of the page that served the token, rather than being specific to
        /// a requesting-origin/top-level-site combination and persistent.
        /// </summary>
        TopLevelTpcdTrial,

        /// <summary>
        /// Content Setting for a first-party origin trial that allows websites to
        /// enable third-party cookie deprecation.
        /// ALLOW (default): no effect (e.g. third-party cookies allowed, if not
        ///                  blocked otherwise).
        /// BLOCK: third-party cookies blocked, but 3PCD mitigations enabled.
        /// </summary>
        TopLevelTpcdOriginTrial,

        /// <summary>
        /// Content setting used to indicate whether entering picture-in-picture
        /// automatically should be enabled.
        /// </summary>
        AutoPictureInPicture,

        /// <summary>
        /// Whether user has opted into keeping file/directory permissions persistent
        /// between visits for a given origin. When enabled, permission metadata
        /// stored under |FILE_SYSTEM_ACCESS_CHOOSER_DATA| can auto-grant incoming
        /// permission request.
        /// </summary>
        FileSystemAccessExtendedPermission,

        /// <summary>
        /// Whether the FSA Persistent Permissions restore prompt is eligible to be
        /// shown to the user, for a given origin.
        /// </summary>
        FileSystemAccessRestorePermission,

        /// <summary>
        /// Whether an application capturing another tab, may scroll and zoom
        /// the captured tab.
        /// </summary>
        CapturedSurfaceControl,

        /// <summary>
        /// Content setting for access to smart card readers.
        /// The "guard" content setting stores whether to allow sites to access the
        /// Smart Card API.
        /// </summary>
        SmartCardGuard,
        SmartCardData,

        /// <summary>
        /// Content settings for access to printers for the Web Printing API.
        /// </summary>
        WebPrinting,

        /// <summary>
        /// Content setting used to indicate whether entering HTML Fullscreen
        /// automatically (i.e. without transient activation) should be enabled.
        /// </summary>
        AutomaticFullscreen,

        /// <summary>
        /// Content settings used to indicate that a web app is allowed to prompt the
        /// user for the installation of sub apps.
        /// </summary>
        SubAppInstallationPrompts,

        /// <summary>
        /// Whether an application can enumerate audio output device.
        /// </summary>
        SpeakerSelection,

        /// <summary>
        /// Content settings for access to the Direct Sockets API.
        /// </summary>
        DirectSockets,

        /// <summary>
        /// Keyboard Lock API allows a site to capture keyboard inputs that would
        /// otherwise be handled by the OS or the browser.
        /// </summary>
        KeyboardLock,

        /// <summary>
        /// Pointer Lock API allows a site to hide the cursor and have exclusive
        /// access to mouse inputs.
        /// </summary>
        PointerLock,

        /// <summary>
        /// Website setting which is used for UnusedSitePermissionsService to store
        /// auto-revoked notification permissions from abusive sites.
        /// </summary>
        RevokedAbusiveNotificationPermissions,

        /// <summary>
        /// Content setting that controls tracking protection status per site.
        /// BLOCK: Protections enabled. This is the default state.
        /// ALLOW: Protections disabled.
        /// </summary>
        TrackingProtection,

        /// <summary>
        /// With this permission, when the application calls `getDisplayMedia()`, a
        /// system audio track can be returned without showing the display media
        /// selection picker. The application can explicitly specify
        /// `systemAudio: 'exclude'` or `video: true` to still show the display media
        /// selection picker if needed. Please note that the setting only works for
        /// WebUI.
        /// </summary>
        DisplayMediaSystemAudio,

        /// <summary>
        /// Whether to use the higher-tier v8 optimizers for running JavaScript on the
        /// page.
        /// </summary>
        JavascriptOptimizer,

        /// <summary>
        /// Content Setting for the Storage Access Headers persistent origin trial
        /// that allows origins to opt into the storage access header behavior. Should
        /// be scoped to `REQUESTING_ORIGIN_AND_TOP_SCHEMEFUL_SITE_SCOPE` in order to
        /// correspond to the design of persistent origin trials. See also:
        /// https://github.com/cfredric/storage-access-headers
        /// ALLOW: storage access request headers will be attached to cross-site
        ///        requests, and url requests will look for response headers from
        ///        origins to retry a request or load with storage access.
        /// BLOCK (default): no effect.
        /// </summary>
        StorageAccessHeaderOriginTrial,

        /// <summary>
        /// Whether or not sites can request Hand Tracking data within WebXR Sessions.
        /// </summary>
        HandTracking,

        /// <summary>
        /// Website setting to indicate whether user has opted in to allow web apps to
        /// install other web apps.
        /// </summary>
        WebAppInstallation,

        /// <summary>
        /// Content settings for private network access in the context of the
        /// Direct Sockets API.
        /// </summary>
        DirectSocketsPrivateNetworkAccess
    }
}

namespace CefSharp.Enums
{
    /// <summary>
    /// CEF Exit Codes
    /// </summary>
    public enum ResultCode
    {
        // The following values should be kept in sync with Chromium's
        // content::ResultCode type.

        NormalExit,

        /// <summary>
        /// Process was killed by user or system.
        /// </summary>
        Killed,

        /// <summary>
        /// Process hung.
        /// </summary>
        Hung,

        /// <summary>
        /// A bad message caused the process termination.
        /// </summary>
        KilledBadMessage,

        /// <summary>
        /// The GPU process exited because initialization failed.
        /// </summary>
        GpuDeadOnArrival,

        // The following values should be kept in sync with Chromium's
        // chrome::ResultCode type. Unused chrome values are excluded.

        ChromeFirst,

        /// <summary>
        /// A critical chrome file is missing.
        /// </summary>
        MissingData = 7,

        /// <summary>
        /// Command line parameter is not supported.
        /// </summary>
        UnsupportedParam = 13,

        /// <summary>
        /// The profile was in use on another host.
        /// </summary>
        ProfileInUse = 21,

        /// <summary>
        /// Failed to pack an extension via the command line.
        /// </summary>
        PackExtensionError = 22,

        /// <summary>
        /// The browser process exited early by passing the command line to another
        /// running browser.
        /// </summary>
        NormalExitProcessNotified = 24,

        /// <summary>
        /// A browser process was sandboxed. This should never happen.
        /// </summary>
        InvalidSandboxState = 31,

        /// <summary>
        /// Cloud policy enrollment failed or was given up by user.
        /// </summary>
        CloudPolicyEnrollmentFailed = 32,

        /// <summary>
        /// The GPU process was terminated due to context lost.
        /// </summary>
        GpuExitOnContextLost = 34,

        /// <summary>
        /// An early startup command was executed and the browser must exit.
        /// </summary>
        NormalExitPackExtensionSuccess = 36,

        /// <summary>
        /// The browser process exited because system resources are exhausted. The
        /// system state can't be recovered and will be unstable.
        /// </summary>
        SystemResourceExhausted = 37,

        /// <summary>
        /// The browser process exited because it was re-launched without elevation.
        /// </summary>
        /// <remarks>
        /// See https://github.com/chromiumembedded/cef/issues/3960
        /// </remarks>
        NormalExitAutoDeElevated = 38,

        /// <summary>
        /// Upon encountering a commit failure in a process, PartitionAlloc terminated
        /// another process deemed less important.
        /// </summary>
        TerminatedByOtherProcessOnCommitFailure = 39,

        ChromeLast = 40,

        // The following values should be kept in sync with Chromium's
        // sandbox::TerminationCodes type.

        SandboxFatalFirst = 7006,

        /// <summary>
        /// Windows sandbox could not set the integrity level.
        /// </summary>
        SandboxFatalIntegrity = SandboxFatalFirst,

        /// <summary>
        /// Windows sandbox could not lower the token.
        /// </summary>
        SandboxFatalDroptoken,

        /// <summary>
        /// Windows sandbox failed to flush registry handles.
        /// </summary>
        SandboxFatalFlushandles,

        /// <summary>
        /// Windows sandbox failed to forbid HCKU caching.
        /// </summary>
        SandboxFatalCachedisable,

        /// <summary>
        /// Windows sandbox failed to close pending handles.
        /// </summary>
        SandboxFatalClosehandles,

        /// <summary>
        /// Windows sandbox could not set the mitigation policy.
        /// </summary>
        SandboxFatalMitigation,

        /// <summary>
        /// Windows sandbox exceeded the job memory limit.
        /// </summary>
        SandboxFatalMemoryExceeded,

        /// <summary>
        /// Windows sandbox failed to warmup.
        /// </summary>
        SandboxFatalWarmup,

        SandboxFatalLast,
    }
}

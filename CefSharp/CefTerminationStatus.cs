namespace CefSharp
{
    public enum CefTerminationStatus
    {
        ///
        // Non-zero exit status.
        ///
        AbnormalTermination = 0,

        ///
        // SIGKILL or task manager kill.
        ///
        ProcessWasKilled,

        ///
        // Segmentation fault.
        ///
        ProcessCrashed
    }
}

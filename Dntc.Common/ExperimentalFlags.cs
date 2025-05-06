namespace Dntc.Common;

/// <summary>
/// Singleton that allows toggling experimental feature flags
/// </summary>
public static class ExperimentalFlags
{
    /// <summary>
    /// If true, allows reference types to be used. Has to be explicitly enabled due to
    /// reference type support being buggy, causing memory leaks, and not a good user experience yet.
    /// </summary>
    public static bool AllowReferenceTypes { get; set; }
}
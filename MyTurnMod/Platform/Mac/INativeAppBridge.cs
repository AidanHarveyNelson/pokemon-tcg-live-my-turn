namespace MyTurnMod.Platform.Mac;

/// <summary>
/// Abstraction over OS-native calls so <see cref="MacWindowNotifier"/> can be unit-tested
/// without actually invoking the Objective-C runtime.
/// </summary>
public interface INativeAppBridge
{
    /// <summary>
    /// Requests user attention via the dock icon.
    /// </summary>
    /// <param name="critical">
    /// When <c>true</c>, the icon bounces continuously until the user focuses the app (NSCriticalRequest).
    /// When <c>false</c>, the icon bounces once (NSInformationalRequest).
    /// </param>
    void RequestAttention(bool critical);

    /// <summary>Brings the app to the foreground, ignoring other apps' focus.</summary>
    void ActivateIgnoringOtherApps();
}

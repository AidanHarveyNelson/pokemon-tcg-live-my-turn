using MyTurnMod.Platform;

namespace MyTurnMod.Platform.Mac;

public class MacWindowNotifier : IWindowNotifier
{
    private readonly INativeAppBridge _bridge;

    public MacWindowNotifier() : this(new ObjcRuntimeBridge()) { }

    /// <summary>Constructor for testing — inject a mock bridge.</summary>
    public MacWindowNotifier(INativeAppBridge bridge)
    {
        _bridge = bridge;
    }

    /// <summary>
    /// Bounces the dock icon continuously until the user focuses the game window.
    /// Uses NSCriticalRequest so the icon keeps bouncing until the app gains focus.
    /// </summary>
    public void FlashDock() => _bridge.RequestAttention(critical: true);

    public void BringToFront() => _bridge.ActivateIgnoringOtherApps();
}

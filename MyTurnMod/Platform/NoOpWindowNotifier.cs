namespace MyTurnMod.Platform;

/// <summary>
/// Fallback notifier for unsupported platforms. Does nothing.
/// Linux and Windows support can be added later.
/// </summary>
public class NoOpWindowNotifier : IWindowNotifier
{
    public void FlashDock() { }
    public void BringToFront() { }
}

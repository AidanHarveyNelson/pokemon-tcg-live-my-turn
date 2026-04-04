namespace MyTurnMod.Core.Platform;

public interface IWindowNotifier
{
    /// <summary>Bounces the app's dock icon to alert the user without stealing focus.</summary>
    void FlashDock();

    /// <summary>Brings the game window to the foreground immediately.</summary>
    void BringToFront();
}

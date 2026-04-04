namespace MyTurnMod.Core;

public enum NotificationType
{
    /// <summary>Bounces the dock icon until the user focuses the window.</summary>
    FlashDock,

    /// <summary>Immediately brings the game window to the foreground.</summary>
    BringToFront,
}

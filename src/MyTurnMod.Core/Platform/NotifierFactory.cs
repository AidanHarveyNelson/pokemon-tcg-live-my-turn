using System.Runtime.InteropServices;

namespace MyTurnMod.Core.Platform;

public static class NotifierFactory
{
    /// <param name="warn">Optional callback for logging an unsupported-platform warning.</param>
    public static IWindowNotifier Create(Action<string>? warn = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new MacWindowNotifier();

        // TODO: Add Windows support (FlashWindowEx / SetForegroundWindow)
        // TODO: Add Linux support (wmctrl / xdotool)
        warn?.Invoke("[MyTurnMod] Unsupported platform — notifications are disabled.");
        return new NoOpWindowNotifier();
    }
}

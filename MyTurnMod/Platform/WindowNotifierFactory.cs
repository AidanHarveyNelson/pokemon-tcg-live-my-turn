using System.Runtime.InteropServices;
using MyTurnMod.Platform.Linux;
using MyTurnMod.Platform.Mac;
using MyTurnMod.Platform.Windows;

namespace MyTurnMod.Platform;

/// <summary>
/// Detects the current OS at startup and returns the appropriate <see cref="IWindowNotifier"/>
/// implementation. All calling code works against <see cref="IWindowNotifier"/> only and
/// has no knowledge of the underlying platform.
/// </summary>
public static class WindowNotifierFactory
{
    /// <summary>
    /// Creates the correct notifier for the running OS.
    /// </summary>
    /// <param name="warn">Optional callback invoked when the platform has no supported implementation.</param>
    public static IWindowNotifier Create(Action<string>? warn = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new MacWindowNotifier();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new WindowsWindowNotifier();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new LinuxWindowNotifier();

        warn?.Invoke("[MyTurnMod] Unsupported platform — notifications are disabled.");
        return new NoOpWindowNotifier();
    }
}

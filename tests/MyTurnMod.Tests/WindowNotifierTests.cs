using System.Runtime.InteropServices;
using MyTurnMod.Platform;
using MyTurnMod.Platform.Mac;
using MyTurnMod.Platform.Windows;
using MyTurnMod.Platform.Linux;

namespace MyTurnMod.Tests;

public class WindowNotifierTests
{
    [Fact]
    public void Create_ReturnsNonNull()
    {
        var notifier = WindowNotifier.Create();
        Assert.NotNull(notifier);
    }

    [Fact]
    public void Create_OnMac_ReturnsMacWindowNotifier()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        var notifier = WindowNotifier.Create();
        Assert.IsType<MacWindowNotifier>(notifier);
    }

    [Fact]
    public void Create_OnWindows_ReturnsWindowsWindowNotifier()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        var notifier = WindowNotifier.Create();
        Assert.IsType<WindowsWindowNotifier>(notifier);
    }

    [Fact]
    public void Create_OnLinux_ReturnsLinuxWindowNotifier()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        var notifier = WindowNotifier.Create();
        Assert.IsType<LinuxWindowNotifier>(notifier);
    }

    [Fact]
    public void Create_OnSupportedPlatform_DoesNotInvokeWarnCallback()
    {
        bool isSupported = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        if (!isSupported)
            return;

        string? warnMessage = null;
        WindowNotifier.Create(warn => warnMessage = warn);

        Assert.Null(warnMessage);
    }
}

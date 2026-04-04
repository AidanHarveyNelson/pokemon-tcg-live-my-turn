using System.Runtime.InteropServices;
using MyTurnMod.Core.Platform;

namespace MyTurnMod.Tests;

public class NotifierFactoryTests
{
    [Fact]
    public void Create_ReturnsNonNull()
    {
        var notifier = NotifierFactory.Create();
        Assert.NotNull(notifier);
    }

    [Fact]
    public void Create_OnMacOS_ReturnsMacWindowNotifier()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return; // Skip on non-Mac CI runners

        var notifier = NotifierFactory.Create();
        Assert.IsType<MacWindowNotifier>(notifier);
    }

    [Fact]
    public void Create_OnNonMacOS_ReturnsNoOpWindowNotifier()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return; // Skip on Mac

        var notifier = NotifierFactory.Create();
        Assert.IsType<NoOpWindowNotifier>(notifier);
    }

    [Fact]
    public void Create_OnUnsupportedPlatform_InvokesWarnCallback()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return; // No warning on supported platform

        string? warnMessage = null;
        NotifierFactory.Create(warn => warnMessage = warn);

        Assert.NotNull(warnMessage);
        Assert.Contains("MyTurnMod", warnMessage);
    }

    [Fact]
    public void Create_OnMacOS_DoesNotInvokeWarnCallback()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        string? warnMessage = null;
        NotifierFactory.Create(warn => warnMessage = warn);

        Assert.Null(warnMessage);
    }
}

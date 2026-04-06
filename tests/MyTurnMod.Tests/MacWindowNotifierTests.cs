using Moq;
using MyTurnMod.Platform.Mac;

namespace MyTurnMod.Tests;

public class MacWindowNotifierTests
{
    [Fact]
    public void FlashDock_CallsRequestAttention_WithCriticalTrue()
    {
        var bridge = new Mock<INativeAppBridge>();
        var notifier = new MacWindowNotifier(bridge.Object);

        notifier.FlashDock();

        bridge.Verify(b => b.RequestAttention(true), Times.Once);
    }

    [Fact]
    public void BringToFront_CallsActivateIgnoringOtherApps()
    {
        var bridge = new Mock<INativeAppBridge>();
        var notifier = new MacWindowNotifier(bridge.Object);

        notifier.BringToFront();

        bridge.Verify(b => b.ActivateIgnoringOtherApps(), Times.Once);
    }

    [Fact]
    public void FlashDock_DoesNotCallActivateIgnoringOtherApps()
    {
        var bridge = new Mock<INativeAppBridge>();
        var notifier = new MacWindowNotifier(bridge.Object);

        notifier.FlashDock();

        bridge.Verify(b => b.ActivateIgnoringOtherApps(), Times.Never);
    }

    [Fact]
    public void BringToFront_DoesNotCallRequestAttention()
    {
        var bridge = new Mock<INativeAppBridge>();
        var notifier = new MacWindowNotifier(bridge.Object);

        notifier.BringToFront();

        bridge.Verify(b => b.RequestAttention(It.IsAny<bool>()), Times.Never);
    }
}

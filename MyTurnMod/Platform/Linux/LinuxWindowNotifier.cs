using MyTurnMod.Platform;

namespace MyTurnMod.Platform.Linux;

/// <summary>
/// Linux implementation of <see cref="IWindowNotifier"/>.
/// TODO: Implement using wmctrl or xdotool via shell commands.
/// </summary>
public class LinuxWindowNotifier : IWindowNotifier
{
    public void FlashDock()
    {
        // TODO: e.g. run `xdotool getactivewindow windowstate --add DEMANDS_ATTENTION`
    }

    public void BringToFront()
    {
        // TODO: e.g. run `wmctrl -a "Pokemon TCG Live"`
    }
}

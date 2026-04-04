using MelonLoader;
using MyTurnMod.Core;
using MyTurnMod.Core.Config;
using MyTurnMod.Core.Platform;

[assembly: MelonInfo(typeof(MyTurnMod.MyTurnMod), "MyTurnMod", "1.0.0", "YourName")]
[assembly: MelonGame("PTCGL", "Pokemon TCG Live")]

namespace MyTurnMod;

public class MyTurnMod : MelonMod
{
    internal static TurnStateTracker TurnTracker { get; } = new();

    private IModConfig _config = null!;
    private IWindowNotifier _notifier = null!;

    public override void OnInitializeMelon()
    {
        _config = new ModConfig();
        _notifier = NotifierFactory.Create(warn => LoggerInstance.Warning(warn));

        TurnTracker.OnMyTurnStarted += HandleMyTurnStarted;

        TurnHook.Initialize(HarmonyInstance, TurnTracker);

        LoggerInstance.Msg("MyTurnMod loaded. Watching for your turn...");
    }

    private void HandleMyTurnStarted()
    {
        switch (_config.NotificationType)
        {
            case NotificationType.FlashDock:
                _notifier.FlashDock();
                break;
            case NotificationType.BringToFront:
                _notifier.BringToFront();
                break;
        }
    }
}

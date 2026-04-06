using MelonLoader;
using MyTurnMod.Hooks;
using MyTurnMod.Platform;
using MyTurnMod.UI;
using UnityEngine;

namespace MyTurnMod;

public class MyTurnMod : MelonMod
{
    internal static TurnStateTracker TurnTracker { get; } = new();

    private IWindowNotifier _notifier = null!;
    private SettingsOverlay _overlay = null!;

    public override void OnInitializeMelon()
    {
        _overlay = new SettingsOverlay();
        _notifier = WindowNotifierFactory.Create(warn => LoggerInstance.Warning(warn));

        TurnTracker.OnMyTurnStarted += HandleMyTurnStarted;

        TurnHook.Initialize(HarmonyInstance, TurnTracker);

        LoggerInstance.Msg("MyTurnMod loaded. Press F7 to toggle the settings overlay.");
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F1)) _overlay.CycleNotificationType();
        if (Input.GetKeyDown(KeyCode.F7)) _overlay.ToggleVisibility();
    }

    public override void OnGUI() => _overlay.Draw();

    private void HandleMyTurnStarted()
    {
        switch (_overlay.NotificationType)
        {   
            case NotificationType.FlashDock:
                LoggerInstance.Msg("Flashing Dock...");
                _notifier.FlashDock();
                break;
            case NotificationType.BringToFront:
                LoggerInstance.Msg("Bringing window to front...");
                _notifier.BringToFront();
                break;
        }
    }
}

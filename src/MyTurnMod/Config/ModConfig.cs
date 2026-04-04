using MelonLoader;
using MyTurnMod.Core;
using MyTurnMod.Core.Config;

namespace MyTurnMod.Config;

public class ModConfig : IModConfig
{
    private static readonly MelonPreferences_Category Category =
        MelonPreferences.CreateCategory("MyTurnMod", "My Turn Mod");

    private static readonly MelonPreferences_Entry<string> NotificationTypeEntry =
        Category.CreateEntry(
            "NotificationType",
            nameof(NotificationType.FlashDock),
            "Notification type when it is your turn",
            "FlashDock = bounce dock icon  |  BringToFront = focus the window immediately");

    public NotificationType NotificationType =>
        Enum.TryParse<NotificationType>(NotificationTypeEntry.Value, ignoreCase: true, out var parsed)
            ? parsed
            : NotificationType.FlashDock;
}

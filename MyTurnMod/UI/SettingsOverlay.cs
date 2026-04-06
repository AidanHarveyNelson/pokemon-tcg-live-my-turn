using MelonLoader;
using UnityEngine;

namespace MyTurnMod.UI;

internal class SettingsOverlay
{
    private static readonly MelonPreferences_Category _category =
        MelonPreferences.CreateCategory("MyTurnMod", "My Turn Mod");

    private static readonly MelonPreferences_Entry<string> _notificationEntry =
        _category.CreateEntry(
            "NotificationType",
            nameof(NotificationType.FlashDock),
            "Notification type when it is your turn",
            "FlashDock = bounce dock icon  |  BringToFront = focus the window immediately");

    private static readonly NotificationType[] _allTypes =
        (NotificationType[])Enum.GetValues(typeof(NotificationType));

    private bool _isVisible = true;

    public NotificationType NotificationType { get; private set; }

    public SettingsOverlay()
    {
        NotificationType = Enum.TryParse<NotificationType>(
            _notificationEntry.Value, ignoreCase: true, out var parsed)
            ? parsed
            : NotificationType.FlashDock;
    }

    public void CycleNotificationType()
    {
        var idx = Array.IndexOf(_allTypes, NotificationType);
        NotificationType = _allTypes[(idx + 1) % _allTypes.Length];
        _notificationEntry.Value = NotificationType.ToString();
        MelonPreferences.Save();
    }

    public void ToggleVisibility() => _isVisible = !_isVisible;

    public void Draw()
    {
        if (!_isVisible) return;

        var panelRect = new Rect(10, 10, 300, 120);

        var prevColor = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.7f);
        GUI.DrawTexture(panelRect, Texture2D.whiteTexture);
        GUI.color = prevColor;

        GUILayout.BeginArea(new Rect(18, 16, 284, 104));
        GUILayout.Label("MyTurnMod Settings");
        GUILayout.Label($"Notification:  {NotificationType}");
        GUILayout.Label("[F1] cycle notification type");
        GUILayout.Label("[F7] hide overlay");
        GUILayout.EndArea();
    }
}

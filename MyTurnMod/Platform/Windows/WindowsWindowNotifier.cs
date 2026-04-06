using System.Runtime.InteropServices;
using MyTurnMod.Platform;

namespace MyTurnMod.Platform.Windows;

/// <summary>
/// Windows implementation of <see cref="IWindowNotifier"/>.
/// Uses Win32 FlashWindowEx to bounce the taskbar button and SetForegroundWindow to focus.
/// </summary>
public class WindowsWindowNotifier : IWindowNotifier
{
    [StructLayout(LayoutKind.Sequential)]
    private struct FLASHWINFO
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }

    // dwFlags values
    private const uint FLASHW_STOP = 0;
    private const uint FLASHW_CAPTION = 1;
    private const uint FLASHW_TRAY = 2;
    private const uint FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY;
    private const uint FLASHW_TIMER = 4;    // flash continuously until stopped
    private const uint FLASHW_TIMERNOFG = 12; // flash until the window comes to the foreground

    [DllImport("user32.dll")]
    private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    private static IntPtr GetGameWindow()
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        return process.MainWindowHandle;
    }

    /// <summary>Flashes the taskbar button until the user focuses the window.</summary>
    public void FlashDock()
    {
        var hwnd = GetGameWindow();
        if (hwnd == IntPtr.Zero) return;

        var info = new FLASHWINFO
        {
            cbSize = (uint)Marshal.SizeOf<FLASHWINFO>(),
            hwnd = hwnd,
            dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG,
            uCount = uint.MaxValue,
            dwTimeout = 0,
        };
        FlashWindowEx(ref info);
    }

    /// <summary>Brings the game window to the foreground immediately.</summary>
    public void BringToFront()
    {
        var hwnd = GetGameWindow();
        if (hwnd == IntPtr.Zero) return;

        ShowWindow(hwnd, 9 /* SW_RESTORE */);
        SetForegroundWindow(hwnd);
    }
}

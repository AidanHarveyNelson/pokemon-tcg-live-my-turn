# MyTurnMod

A [MelonLoader](https://melonwiki.xyz/) mod for **Pokemon TCG Live** that notifies you the moment it becomes your turn.

> Looking to contribute or build from source? See [DEVELOPER.md](DEVELOPER.md).

## Features

| Option | Behaviour |
|--------|-----------|
| `FlashTaskbar` *(default)* | Flashes the taskbar icon until you focus the game window |
| `BringToFront` | Immediately brings the game window to the foreground |

## Requirements

- Windows 10 or later
- [MelonLoader](https://github.com/LavaGang/MelonLoader) installed in your Pokemon TCG Live game directory

## Installation

1. Download `MyTurnMod.dll` from the [latest release](../../releases/latest).
2. Copy it into `<GameDir>/Mods/`.
3. Launch the game — MelonLoader will load the mod automatically.

Change the value and restart the game to apply.

## Roadmap

- [x] Windows support (`FlashWindowEx` / `SetForegroundWindow`)
- [ ] macOS support (dock bouncing via `NSApplication`)
- [ ] Linux support (`wmctrl` / `xdotool`)
- [ ] Desktop notification as a third option

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

## Developer Notes/FAQ
- Q: My MelonLoader isn't loading your mod correctly, how do I fix this?
  - A: The only valid Melon Loader version for the current PTCGL is v0.5.7
- Q: I'm getting error `Failed to Open Mono Assembly`:

![image](https://github.com/Bratah123/IronTracks/assets/58405975/165a0838-21e5-45f9-b255-588e24b1a493)
  - A: This error is caused by the MelonLoader program being unable to read the `é` in the word "Pokémon". This can be fixed by changing your PTCGL client's folder to something else I.E. `PTCGL`

## Roadmap

- [x] Windows support (`FlashWindowEx` / `SetForegroundWindow`)
- [ ] macOS support (dock bouncing via `NSApplication`)
- [ ] Linux support (`wmctrl` / `xdotool`)
- [ ] Desktop notification as a third option

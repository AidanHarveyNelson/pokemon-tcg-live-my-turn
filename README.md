# MyTurnMod

A [MelonLoader](https://melonwiki.xyz/) mod for **Pokemon TCG Live** that notifies you the moment it becomes your turn.

## Features

| Option | Behaviour |
|--------|-----------|
| `FlashDock` *(default)* | Bounces the dock icon until you focus the game window |
| `BringToFront` | Immediately brings the game window to the foreground |

## Requirements

- macOS (Linux / Windows support can be added later)
- [MelonLoader](https://github.com/LavaGang/MelonLoader) installed in your Pokemon TCG Live game directory

## Installation

1. Download `MyTurnMod.dll` from the [latest release](../../releases/latest).
2. Copy it into `<GameDir>/Mods/`.
3. Launch the game — MelonLoader will load the mod automatically.

## Configuration

After the first launch, a `UserData/MelonPreferences.cfg` entry is created:

```ini
[MyTurnMod]
NotificationType = FlashDock   # or BringToFront
```

Change the value and restart the game to apply.

## Building from source

### Prerequisites

- .NET 9 SDK
- MelonLoader installed in the game directory (to copy DLLs into `libs/`)

### 1. Populate `libs/`

See [`libs/MelonLoader/README.md`](libs/MelonLoader/README.md) and [`libs/Game/README.md`](libs/Game/README.md).

### 2. Build

```bash
dotnet build src/MyTurnMod/MyTurnMod.csproj -c Release -o ./out
```

Copy `./out/MyTurnMod.dll` to your game's `Mods/` folder.

### 3. Run tests

Tests have no dependency on game or MelonLoader DLLs and always work:

```bash
dotnet test tests/MyTurnMod.Tests/MyTurnMod.Tests.csproj
```

## Implementing turn detection

The hook in `src/MyTurnMod/Hooks/TurnHook.cs` uses reflection to find turn-management classes at runtime, so no direct compile-time reference to game types is needed.

You need to identify the correct class and method names by decompiling the game — see [`libs/Game/README.md`](libs/Game/README.md) for instructions.

## CI / CD

| Trigger | Workflow | What it does |
|---------|----------|--------------|
| Pull request → `main` | `pr-tests.yml` | Builds Core lib + runs all tests |
| Push tag `v*.*.*` | `release.yml` | Runs tests, builds the mod, creates a GitHub release with `MyTurnMod.dll` |

## Releasing

```bash
git tag v1.0.0
git push origin v1.0.0
```

GitHub Actions will build and publish the release automatically.

## Roadmap

- [ ] Windows support (`FlashWindowEx` / `SetForegroundWindow`)
- [ ] Linux support (`wmctrl` / `xdotool`)
- [ ] Desktop notification as a third option

# Developer Guide

This document covers everything needed to build, test, and release MyTurnMod.

## Project setup

### Prerequisites

- .NET 9 SDK
- MelonLoader installed in your Pokemon TCG Live game directory (needed to copy DLLs into `libs/`)

### 1. Populate `libs/`

The project depends on game and MelonLoader assemblies that are not checked in.
Follow the instructions in each placeholder README to copy them from your local installation:

- [`libs/MelonLoader/README.md`](libs/MelonLoader/README.md)
- [`libs/Game/README.md`](libs/Game/README.md)

### 2. Build

```bash
dotnet build src/MyTurnMod/MyTurnMod.csproj -c Release -o ./out
```

Copy `./out/MyTurnMod.dll` to your game's `Mods/` folder to test locally.

## Running tests

Tests have no dependency on game or MelonLoader DLLs and always work without a game installation:

```bash
dotnet test tests/MyTurnMod.Tests/MyTurnMod.Tests.csproj
```

All tests follow the pattern of exercising the `Core` library in isolation. If you add a new behaviour, add a corresponding test in `tests/MyTurnMod.Tests/` that verifies it without requiring MelonLoader or game types.

## Implementing turn detection

The hook in `src/MyTurnMod/Hooks/TurnHook.cs` uses reflection to locate turn-management classes at runtime, so there is no direct compile-time reference to game types.

To find the correct class and method names, decompile the game — see [`libs/Game/README.md`](libs/Game/README.md) for step-by-step instructions.

## CI / CD

| Trigger | Workflow | What it does |
|---------|----------|--------------|
| Pull request → `main` | `pr-tests.yml` | Builds Core lib + runs all tests |
| Push tag `v*.*.*` | `release.yml` | Runs tests, builds the mod, creates a GitHub release with `MyTurnMod.dll` |

Both workflows run on GitHub-hosted Windows runners. Neither requires game DLLs because the release build only compiles the mod assembly and the test projects are self-contained.

## Releasing

Tag a commit with a semantic version and push — GitHub Actions does the rest:

```bash
git tag v1.0.0
git push origin v1.0.0
```

The `release.yml` workflow will:

1. Run all tests.
2. Build `MyTurnMod.dll` in Release configuration.
3. Create a GitHub release and attach the DLL as a downloadable asset.

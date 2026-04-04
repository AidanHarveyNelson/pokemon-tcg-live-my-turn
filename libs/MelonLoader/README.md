# libs/MelonLoader

Place the following DLLs here (copy from your game directory after installing MelonLoader):

| File | Source path |
|------|-------------|
| `MelonLoader.dll` | `<GameDir>/MelonLoader/net6/MelonLoader.dll` |
| `0Harmony.dll`    | `<GameDir>/MelonLoader/net6/0Harmony.dll`    |

`<GameDir>` is typically:
- **macOS**: `~/Library/Application Support/com.pokemon.ptcgl/` or wherever the game is installed via the launcher.

Once added, commit both DLLs — they are whitelisted in `.gitignore`.

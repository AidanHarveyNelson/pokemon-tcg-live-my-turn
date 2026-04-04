# libs/Game

Place game assemblies here. After installing MelonLoader and running the game once,
MelonLoader generates managed (Il2CppInterop) assemblies that can be referenced at compile time.

| File | Source path |
|------|-------------|
| `Assembly-CSharp.dll` | `<GameDir>/MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll` |

Add any additional Unity assemblies from the same folder if the compiler reports missing types.

## Finding turn management classes

Once you have `Assembly-CSharp.dll`, open it in **dnSpy** or **ILSpy** and search for:

- `TurnManager`, `GameTurnManager`, `BattleController`, `PhaseManager`
- Methods named `BeginPlayerTurn`, `StartTurn`, `OnTurnStart`, `SetActivePlayer`

Then update the constants at the top of `src/MyTurnMod/Hooks/TurnHook.cs`:

```csharp
private const string TurnManagerTypeName = "YourActualClassName";
private const string BeginTurnMethodName  = "YourActualBeginMethod";
private const string EndTurnMethodName    = "YourActualEndMethod";
```

If the begin-turn method receives a player parameter, check `TurnHook.cs` for the TODO comment
explaining how to filter for the local player only.

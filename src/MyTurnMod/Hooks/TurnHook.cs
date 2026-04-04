using HarmonyLib;
using MelonLoader;
using MyTurnMod.Core;

namespace MyTurnMod;

/// <summary>
/// Harmony patches that detect turn changes in Pokemon TCG Live.
///
/// HOW TO FIND THE RIGHT CLASS AND METHOD NAMES:
///   1. Install dnSpy or ILSpy.
///   2. Open the Il2CppAssemblies version of Assembly-CSharp.dll
///      (found at: &lt;GameDir&gt;/MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll after running the game with MelonLoader once).
///   3. Search for classes related to turns — try "TurnManager", "GameController",
///      "PhaseManager", "BattleController", or similar.
///   4. Look for a method called when the local player's turn starts, e.g.
///      "BeginPlayerTurn", "StartTurn", "OnTurnStart".
///   5. Update <see cref="TurnManagerTypeName"/> and <see cref="BeginTurnMethodName"/> below.
///   6. If the begin-turn method receives a player parameter, add a check to confirm
///      it is the local player before calling SetTurnState(MyTurn).
/// </summary>
internal static class TurnHook
{
    // -------------------------------------------------------------------------
    // TODO: Replace these with the real class/method names from the game.
    // -------------------------------------------------------------------------
    private const string TurnManagerTypeName = "GameTurnManager";
    private const string BeginTurnMethodName = "BeginPlayerTurn";
    private const string EndTurnMethodName   = "EndPlayerTurn";
    // -------------------------------------------------------------------------

    private static TurnStateTracker? _tracker;

    public static void Initialize(HarmonyLib.Harmony harmony, TurnStateTracker tracker)
    {
        _tracker = tracker;

        // Find types by name so we don't need a compile-time reference to Assembly-CSharp.
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return Array.Empty<Type>(); }
            });

        var turnManagerType = allTypes.FirstOrDefault(t => t.Name == TurnManagerTypeName);
        if (turnManagerType is null)
        {
            MelonLogger.Warning(
                $"[MyTurnMod] Could not find type '{TurnManagerTypeName}'. " +
                "Open TurnHook.cs and update TurnManagerTypeName with the correct class name.");
            return;
        }

        PatchMethod(harmony, turnManagerType, BeginTurnMethodName, nameof(Postfix_BeginTurn));
        PatchMethod(harmony, turnManagerType, EndTurnMethodName,   nameof(Postfix_EndTurn));
    }

    private static void PatchMethod(
        HarmonyLib.Harmony harmony, Type targetType, string methodName, string patchMethodName)
    {
        var method = AccessTools.Method(targetType, methodName);
        if (method is null)
        {
            MelonLogger.Warning(
                $"[MyTurnMod] Could not find method '{methodName}' on '{targetType.Name}'. " +
                "Update TurnHook.cs with the correct method name.");
            return;
        }

        harmony.Patch(method, postfix: new HarmonyMethod(typeof(TurnHook), patchMethodName));
        MelonLogger.Msg($"[MyTurnMod] Patched {targetType.Name}.{methodName}");
    }

    // -------------------------------------------------------------------------
    // TODO: If BeginPlayerTurn receives a player parameter, add it here and
    //       check that it is the local player before changing state, e.g.:
    //
    //   private static void Postfix_BeginTurn(PlayerObject player)
    //   {
    //       if (player != LocalPlayerManager.Instance.LocalPlayer) return;
    //       _tracker?.SetTurnState(TurnState.MyTurn);
    //   }
    // -------------------------------------------------------------------------

    private static void Postfix_BeginTurn()
    {
        _tracker?.SetTurnState(TurnState.MyTurn);
    }

    private static void Postfix_EndTurn()
    {
        _tracker?.SetTurnState(TurnState.OpponentTurn);
    }
}

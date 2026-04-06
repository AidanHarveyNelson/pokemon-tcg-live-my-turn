using System.Reflection;
using HarmonyLib;
using MelonLoader;

namespace MyTurnMod.Hooks;

/// <summary>
/// Harmony patch that detects turn changes in Pokemon TCG Live.
///
/// Hooks into <c>MatchManager.SetState(MatchManager.MatchState)</c>, which is the
/// single call-site that transitions the match state for both players.
///
/// <c>MatchManager.MatchState</c> enum (Assembly-CSharp.dll):
///   None        = 0
///   Setup       = 1
///   LocalTurn   = 2  ← local player's turn started
///   OpponentTurn = 3  ← opponent's turn started
/// </summary>
internal static class TurnHook
{
    private const string MatchManagerTypeName = "MatchManager";
    private const string SetStateMethodName   = "SetState";

    // Int values of MatchManager.MatchState (verified via Mono.Cecil inspection).
    private const int StateLocalTurn    = 2;
    private const int StateOpponentTurn = 3;
    private const int StateSetup        = 1;

    private static TurnStateTracker? _tracker;

    public static void Initialize(HarmonyLib.Harmony harmony, TurnStateTracker tracker)
    {
        _tracker = tracker;

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        MelonLogger.Msg($"[MyTurnMod] Scanning {assemblies.Length} loaded assemblies for '{MatchManagerTypeName}'...");

        // Find the type by name so we don't need a compile-time reference to Assembly-CSharp.
        var allTypes = assemblies.SelectMany(a =>
        {
            try { return a.GetTypes(); }
            catch (ReflectionTypeLoadException ex)
            {
                MelonLogger.Warning(
                    $"[MyTurnMod] Could not load all types from '{a.GetName().Name}': " +
                    $"{ex.LoaderExceptions.FirstOrDefault()?.Message}");
                return ex.Types.Where(t => t is not null)!;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[MyTurnMod] Skipped assembly '{a.GetName().Name}': {ex.Message}");
                return Array.Empty<Type>();
            }
        });

        var matchManagerType = allTypes.FirstOrDefault(t => t.Name == MatchManagerTypeName);
        if (matchManagerType is null)
        {
            MelonLogger.Error(
                $"[MyTurnMod] Could not find type '{MatchManagerTypeName}'. " +
                "Turn detection will not work. " +
                "Check that Assembly-CSharp.dll is loaded and the class name is still 'MatchManager'.");
            return;
        }

        MelonLogger.Msg($"[MyTurnMod] Found '{MatchManagerTypeName}' in '{matchManagerType.Assembly.GetName().Name}'.");

        var method = AccessTools.Method(matchManagerType, SetStateMethodName);
        if (method is null)
        {
            // Log all public methods to help diagnose a rename.
            var methodNames = string.Join(", ",
                matchManagerType.GetMethods(
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Static   |
                    System.Reflection.BindingFlags.Public   |
                    System.Reflection.BindingFlags.NonPublic)
                .Select(m => m.Name)
                .Distinct()
                .OrderBy(n => n));
            MelonLogger.Error(
                $"[MyTurnMod] Could not find method '{SetStateMethodName}' on '{MatchManagerTypeName}'. " +
                $"Available methods: {methodNames}");
            return;
        }

        MelonLogger.Msg(
            $"[MyTurnMod] Found '{MatchManagerTypeName}.{SetStateMethodName}' " +
            $"({string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))}).");

        try
        {
            harmony.Patch(method, postfix: new HarmonyMethod(typeof(TurnHook), nameof(Postfix_SetState)));
            MelonLogger.Msg($"[MyTurnMod] Successfully patched {MatchManagerTypeName}.{SetStateMethodName}.");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[MyTurnMod] Failed to apply Harmony patch: {ex}");
        }
    }

    /// <summary>
    /// Called after <c>MatchManager.SetState</c>. <paramref name="__0"/> is the first argument
    /// (the new <c>MatchState</c> value) captured as its underlying <see langword="int"/>.
    /// </summary>
    private static void Postfix_SetState(int __0)
    {
        MelonLogger.Msg($"[MyTurnMod] MatchManager.SetState called with state={__0}.");

        if (__0 == StateLocalTurn)
            _tracker?.SetTurnState(TurnState.MyTurn);
        else if (__0 == StateOpponentTurn)
            _tracker?.SetTurnState(TurnState.OpponentTurn);
        else if (__0 == StateSetup)
            _tracker?.SetTurnState(TurnState.Unknown);
        else
            MelonLogger.Msg($"[MyTurnMod] Unhandled state value {__0} — no turn-state change.");
    }
}

namespace MyTurnMod;

/// <summary>
/// Tracks the current turn state and raises an event when the local player's turn begins.
/// This class has no dependency on MelonLoader or game assemblies and is fully unit-testable.
/// </summary>
public class TurnStateTracker
{
    public TurnState CurrentState { get; private set; } = TurnState.Unknown;

    /// <summary>Fired once each time the state transitions into <see cref="TurnState.MyTurn"/>.</summary>
    public event Action? OnMyTurnStarted;

    public void SetTurnState(TurnState newState)
    {
        var previous = CurrentState;
        CurrentState = newState;

        if (newState == TurnState.MyTurn && previous != TurnState.MyTurn)
            OnMyTurnStarted?.Invoke();
    }
}

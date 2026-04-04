using MyTurnMod.Core;

namespace MyTurnMod.Tests;

public class TurnStateTrackerTests
{
    [Fact]
    public void InitialState_IsUnknown()
    {
        var tracker = new TurnStateTracker();
        Assert.Equal(TurnState.Unknown, tracker.CurrentState);
    }

    [Fact]
    public void SetTurnState_UpdatesCurrentState()
    {
        var tracker = new TurnStateTracker();
        tracker.SetTurnState(TurnState.MyTurn);
        Assert.Equal(TurnState.MyTurn, tracker.CurrentState);
    }

    [Fact]
    public void SetTurnState_ToMyTurn_FiresOnMyTurnStarted()
    {
        var tracker = new TurnStateTracker();
        var fired = false;
        tracker.OnMyTurnStarted += () => fired = true;

        tracker.SetTurnState(TurnState.MyTurn);

        Assert.True(fired);
    }

    [Fact]
    public void SetTurnState_ToMyTurn_WhenAlreadyMyTurn_DoesNotFireAgain()
    {
        var tracker = new TurnStateTracker();
        var count = 0;
        tracker.OnMyTurnStarted += () => count++;

        tracker.SetTurnState(TurnState.MyTurn);
        tracker.SetTurnState(TurnState.MyTurn);

        Assert.Equal(1, count);
    }

    [Fact]
    public void SetTurnState_ToOpponentTurn_DoesNotFireEvent()
    {
        var tracker = new TurnStateTracker();
        var fired = false;
        tracker.OnMyTurnStarted += () => fired = true;

        tracker.SetTurnState(TurnState.OpponentTurn);

        Assert.False(fired);
    }

    [Fact]
    public void SetTurnState_OpponentToMyTurn_FiresEvent()
    {
        var tracker = new TurnStateTracker();
        var count = 0;
        tracker.OnMyTurnStarted += () => count++;

        tracker.SetTurnState(TurnState.OpponentTurn);
        tracker.SetTurnState(TurnState.MyTurn);
        tracker.SetTurnState(TurnState.OpponentTurn);
        tracker.SetTurnState(TurnState.MyTurn);

        Assert.Equal(2, count);
    }
}

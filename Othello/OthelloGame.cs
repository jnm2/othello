namespace Othello;

public sealed class OthelloGame
{
    private int stateIndex;
    private readonly List<OthelloState> history = new() { OthelloState.NewGame() };

    public OthelloState State => history[stateIndex];

    public Player CurrentPlayer => State.CurrentPlayer;

    public bool Move(int x, int y)
    {
        var newState = State.Move(x, y, State.CurrentPlayer);
        if (newState == null) return false;

        if (stateIndex < history.Count - 1) history.RemoveRange(stateIndex + 1, history.Count - 1 - stateIndex);
        history.Add(newState);
        stateIndex++;

        return true;
    }

    public bool Undo()
    {
        if (stateIndex == 0) return false;
        stateIndex--;
        return true;
    }

    public bool Redo()
    {
        if (stateIndex == history.Count - 1) return false;
        stateIndex++;
        return true;
    }
}

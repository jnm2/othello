namespace Othello;

public sealed class OthelloGame
{
    private int stateIndex = 0;
    private readonly List<OthelloState> history = new List<OthelloState>();

    public OthelloGame()
    {
        history.Add(OthelloState.NewGame());
    }

    public OthelloGame(OthelloState state)
    {
        history.Add(state);
    }

    public OthelloState State => history[stateIndex];

    public Player CurrentPlayer => State.CurrentPlayer;

    public bool Move(int x, int y)
    {
        var newState = this.State.Move(x, y, State.CurrentPlayer);
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

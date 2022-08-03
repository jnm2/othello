namespace Othello;

public sealed class OthelloState
{
    public Player?[,] Board { get; } = new Player?[8, 8];
    public Player CurrentPlayer { get; private set; }

    public static OthelloState NewGame()
    {
        var state = new OthelloState();
        state.Board[3, 3] = Player.Black;
        state.Board[3, 4] = Player.White;
        state.Board[4, 3] = Player.White;
        state.Board[4, 4] = Player.Black;
        state.CurrentPlayer = Player.Black;
        return state;
    }

    public bool IsLegalMove(int x, int y, Player player)
    {
        if (Board[x, y] is not null) return false;

        if (y >= 2 && Board[x, y - 1] is not null && Board[x, y - 1] != player)
            for (var i = 2; y - i >= 0; i++)
            {
                if (Board[x, y - i] is null) break;
                if (Board[x, y - i] == player) return true;
            }

        if (y < Board.GetLength(1) - 2 && Board[x, y + 1] is not null && Board[x, y + 1] != player)
            for (var i = 2; y + i < Board.GetLength(1); i++)
            {
                if (Board[x, y + i] is null) break;
                if (Board[x, y + i] == player) return true;
            }

        if (x >= 2 && Board[x - 1, y] is not null && Board[x - 1, y] != player)
            for (var i = 2; x - i >= 0; i++)
            {
                if (Board[x - i, y] is null) break;
                if (Board[x - i, y] == player) return true;
            }

        if (x < Board.GetLength(0) - 2 && Board[x + 1, y] is not null && Board[x + 1, y] != player)
            for (var i = 2; x + i < Board.GetLength(0); i++)
            {
                if (Board[x + i, y] is null) break;
                if (Board[x + i, y] == player) return true;
            }

        if (x >= 2 && y >= 2 && Board[x - 1, y - 1] is not null && Board[x - 1, y - 1] != player)
            for (var i = 2; x - i >= 0 && y - i >= 0; i++)
            {
                if (Board[x - i, y - i] is null) break;
                if (Board[x - i, y - i] == player) return true;
            }

        if (x < Board.GetLength(0) - 2 && y >= 2 && Board[x + 1, y - 1] is not null && Board[x + 1, y - 1] != player)
            for (var i = 2; x + i < Board.GetLength(0) && y - i >= 0; i++)
            {
                if (Board[x + i, y - i] is null) break;
                if (Board[x + i, y - i] == player) return true;
            }

        if (x >= 2 && y < Board.GetLength(0) - 2 && Board[x - 1, y + 1] is not null && Board[x - 1, y + 1] != player)
            for (var i = 2; x - i >= 0 && y + i < Board.GetLength(1); i++)
            {
                if (Board[x - i, y + i] is null) break;
                if (Board[x - i, y + i] == player) return true;
            }

        if (x < Board.GetLength(0) - 2 && y < Board.GetLength(0) - 2 && Board[x + 1, y + 1] is not null && Board[x + 1, y + 1] != player)
            for (var i = 2; x + i < Board.GetLength(0) && y + i < Board.GetLength(1); i++)
            {
                if (Board[x + i, y + i] is null) break;
                if (Board[x + i, y + i] == player) return true;
            }

        return false;
    }

    public bool CanMove(Player player)
    {
        for (var x = 0; x < Board.GetLength(0); x++)
            for (var y = 0; y < Board.GetLength(1); y++)
                if (IsLegalMove(x, y, player)) return true;

        return false;
    }

    public bool IsGameOver()
    {
        return !(CanMove(Player.Black) || CanMove(Player.White));
    }

    public OthelloState Move(int x, int y, Player player)
    {
        if (Board[x, y] is not null) return null;

        var up = (int?)null;
        var down = (int?)null;
        var left = (int?)null;
        var right = (int?)null;
        var upleft = (int?)null;
        var upright = (int?)null;
        var downleft = (int?)null;
        var downright = (int?)null;

        if (y >= 2 && Board[x, y - 1] is not null && Board[x, y - 1] != player)
            for (var i = 2; y - i >= 0; i++)
            {
                if (Board[x, y - i] is null) break;
                if (Board[x, y - i] == player) { up = i; break; }
            }

        if (y < Board.GetLength(1) - 2 && Board[x, y + 1] is not null && Board[x, y + 1] != player)
            for (var i = 2; y + i < Board.GetLength(1); i++)
            {
                if (Board[x, y + i] is null) break;
                if (Board[x, y + i] == player) { down = i; break; }
            }

        if (x >= 2 && Board[x - 1, y] is not null && Board[x - 1, y] != player)
            for (var i = 2; x - i >= 0; i++)
            {
                if (Board[x - i, y] is null) break;
                if (Board[x - i, y] == player) { left = i; break; }
            }

        if (x < Board.GetLength(0) - 2 && Board[x + 1, y] is not null && Board[x + 1, y] != player)
            for (var i = 2; x + i < Board.GetLength(0); i++)
            {
                if (Board[x + i, y] is null) break;
                if (Board[x + i, y] == player) { right = i; break; }
            }

        if (x >= 2 && y >= 2 && Board[x - 1, y - 1] is not null && Board[x - 1, y - 1] != player)
            for (var i = 2; x - i >= 0 && y - i >= 0; i++)
            {
                if (Board[x - i, y - i] is null) break;
                if (Board[x - i, y - i] == player) { upleft = i; break; }
            }

        if (x < Board.GetLength(0) - 2 && y >= 2 && Board[x + 1, y - 1] is not null && Board[x + 1, y - 1] != player)
            for (var i = 2; x + i < Board.GetLength(0) && y - i >= 0; i++)
            {
                if (Board[x + i, y - i] is null) break;
                if (Board[x + i, y - i] == player) { upright = i; break; }
            }

        if (x >= 2 && y < Board.GetLength(0) - 2 && Board[x - 1, y + 1] is not null && Board[x - 1, y + 1] != player)
            for (var i = 2; x - i >= 0 && y + i < Board.GetLength(1); i++)
            {
                if (Board[x - i, y + i] is null) break;
                if (Board[x - i, y + i] == player) { downleft = i; break; }
            }

        if (x < Board.GetLength(0) - 2 && y < Board.GetLength(0) - 2 && Board[x + 1, y + 1] is not null && Board[x + 1, y + 1] != player)
            for (var i = 2; x + i < Board.GetLength(0) && y + i < Board.GetLength(1); i++)
            {
                if (Board[x + i, y + i] is null) break;
                if (Board[x + i, y + i] == player) { downright = i; break; }
            }

        if (up is null && down is null && left is null && right is null && upleft is null && upright is null && downleft is null && downright is null) return null;

        var newState = new OthelloState();
        Array.Copy(Board, newState.Board, Board.Length);

        if (up is not null)
            for (var i = 1; i < up; i++)
                newState.Board[x, y - i] = player;

        if (down is not null)
            for (var i = 1; i < down; i++)
                newState.Board[x, y + i] = player;

        if (left is not null)
            for (var i = 1; i < left; i++)
                newState.Board[x - i, y] = player;

        if (right is not null)
            for (var i = 1; i < right; i++)
                newState.Board[x + i, y] = player;

        if (upleft is not null)
            for (var i = 1; i < upleft; i++)
                newState.Board[x - i, y - i] = player;

        if (upright is not null)
            for (var i = 1; i < upright; i++)
                newState.Board[x + i, y - i] = player;

        if (downleft is not null)
            for (var i = 1; i < downleft; i++)
                newState.Board[x - i, y + i] = player;

        if (downright is not null)
            for (var i = 1; i < downright; i++)
                newState.Board[x + i, y + i] = player;

        newState.Board[x, y] = player;

        newState.CurrentPlayer = CurrentPlayer == Player.Black ? Player.White : Player.Black;
        if (!newState.CanMove(newState.CurrentPlayer)) newState.CurrentPlayer = CurrentPlayer;

        return newState;
    }
}

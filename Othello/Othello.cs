namespace Othello
{
    class OthelloGame
    {
        public OthelloGame()
        {
            history.Add(OthelloState.NewGame());
        }
        public OthelloGame(OthelloState state)
        {
            history.Add(state);
        }

        public OthelloState State { get { return history[stateIndex]; } }

        public Player CurrentPlayer { get { return State.CurrentPlayer; } }

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

        int stateIndex = 0;
        public List<OthelloState> history = new List<OthelloState>();

    }

    public class OthelloState
    {
        private readonly Player?[,] board = new Player?[8, 8];
        public Player?[,] Board { get { return board; } }
        public Player CurrentPlayer { get; set; }

        public static OthelloState NewGame()
        {
            var state = new OthelloState();
            state.Board[3, 3] = Player.Black;
            state.board[3, 4] = Player.White;
            state.Board[4, 3] = Player.White;
            state.board[4, 4] = Player.Black;
            state.CurrentPlayer = Player.Black;
            return state;
        }


        public bool IsLegalMove(int x, int y, Player player)
        {
            if (Board[x, y].HasValue) return false;

            if (y >= 2 && board[x, y - 1].HasValue && board[x, y - 1] != player)
                for (var i = 2; y - i >= 0; i++)
                {
                    if (!board[x, y - i].HasValue) break;
                    if (board[x, y - i] == player) return true;
                }

            if (y < board.GetLength(1) - 2 && board[x, y + 1].HasValue && board[x, y + 1] != player)
                for (var i = 2; y + i < board.GetLength(1); i++)
                {
                    if (!board[x, y + i].HasValue) break;
                    if (board[x, y + i] == player) return true;
                }

            if (x >= 2 && board[x - 1, y].HasValue && board[x - 1, y] != player)
                for (var i = 2; x - i >= 0; i++)
                {
                    if (!board[x - i, y].HasValue) break;
                    if (board[x - i, y] == player) return true;
                }

            if (x < board.GetLength(0) - 2 && board[x + 1, y].HasValue && board[x + 1, y] != player)
                for (var i = 2; x + i < board.GetLength(0); i++)
                {
                    if (!board[x + i, y].HasValue) break;
                    if (board[x + i, y] == player) return true;
                }

            if (x >= 2 && y >= 2 && board[x - 1, y - 1].HasValue && board[x - 1, y - 1] != player)
                for (var i = 2; x - i >= 0 && y - i >= 0; i++)
                {
                    if (!board[x - i, y - i].HasValue) break;
                    if (board[x - i, y - i] == player) return true;
                }

            if (x < board.GetLength(0) - 2 && y >= 2 && board[x + 1, y - 1].HasValue && board[x + 1, y - 1] != player)
                for (var i = 2; x + i < board.GetLength(0) && y - i >= 0; i++)
                {
                    if (!board[x + i, y - i].HasValue) break;
                    if (board[x + i, y - i] == player) return true;
                }

            if (x >= 2 && y < board.GetLength(0) - 2 && board[x - 1, y + 1].HasValue && board[x - 1, y + 1] != player)
                for (var i = 2; x - i >= 0 && y + i < board.GetLength(1); i++)
                {
                    if (!board[x - i, y + i].HasValue) break;
                    if (board[x - i, y + i] == player) return true;
                }

            if (x < board.GetLength(0) - 2 && y < board.GetLength(0) - 2 && board[x + 1, y + 1].HasValue && board[x + 1, y + 1] != player)
                for (var i = 2; x + i < board.GetLength(0) && y + i < board.GetLength(1); i++)
                {
                    if (!board[x + i, y + i].HasValue) break;
                    if (board[x + i, y + i] == player) return true;
                }

            return false;
        }

        public bool CanMove(Player player)
        {
            for (var x = 0; x < board.GetLength(0); x++)
                for (var y = 0; y < board.GetLength(1); y++)
                    if (IsLegalMove(x, y, player)) return true;

            return false;
        }

        public bool IsGameOver()
        {
            return !(CanMove(Player.Black) || CanMove(Player.White));
        }

        public OthelloState Move(int x, int y, Player player)
        {
            if (board[x, y].HasValue) return null;

            var up = (int?)null;
            var down = (int?)null;
            var left = (int?)null;
            var right = (int?)null;
            var upleft = (int?)null;
            var upright = (int?)null;
            var downleft = (int?)null;
            var downright = (int?)null;

            if (y >= 2 && board[x, y - 1].HasValue && board[x, y - 1] != player)
                for (var i = 2; y - i >= 0; i++)
                {
                    if (!board[x, y - i].HasValue) break;
                    if (board[x, y - i] == player) { up = i; break; }
                }

            if (y < board.GetLength(1) - 2 && board[x, y + 1].HasValue && board[x, y + 1] != player)
                for (var i = 2; y + i < board.GetLength(1); i++)
                {
                    if (!board[x, y + i].HasValue) break;
                    if (board[x, y + i] == player) { down = i; break; }
                }

            if (x >= 2 && board[x - 1, y].HasValue && board[x - 1, y] != player)
                for (var i = 2; x - i >= 0; i++)
                {
                    if (!board[x - i, y].HasValue) break;
                    if (board[x - i, y] == player) { left = i; break; }
                }

            if (x < board.GetLength(0) - 2 && board[x + 1, y].HasValue && board[x + 1, y] != player)
                for (var i = 2; x + i < board.GetLength(0); i++)
                {
                    if (!board[x + i, y].HasValue) break;
                    if (board[x + i, y] == player) { right = i; break; }
                }

            if (x >= 2 && y >= 2 && board[x - 1, y - 1].HasValue && board[x - 1, y - 1] != player)
                for (var i = 2; x - i >= 0 && y - i >= 0; i++)
                {
                    if (!board[x - i, y - i].HasValue) break;
                    if (board[x - i, y - i] == player) { upleft = i; break; }
                }

            if (x < board.GetLength(0) - 2 && y >= 2 && board[x + 1, y - 1].HasValue && board[x + 1, y - 1] != player)
                for (var i = 2; x + i < board.GetLength(0) && y - i >= 0; i++)
                {
                    if (!board[x + i, y - i].HasValue) break;
                    if (board[x + i, y - i] == player) { upright = i; break; }
                }

            if (x >= 2 && y < board.GetLength(0) - 2 && board[x - 1, y + 1].HasValue && board[x - 1, y + 1] != player)
                for (var i = 2; x - i >= 0 && y + i < board.GetLength(1); i++)
                {
                    if (!board[x - i, y + i].HasValue) break;
                    if (board[x - i, y + i] == player) { downleft = i; break; }
                }

            if (x < board.GetLength(0) - 2 && y < board.GetLength(0) - 2 && board[x + 1, y + 1].HasValue && board[x + 1, y + 1] != player)
                for (var i = 2; x + i < board.GetLength(0) && y + i < board.GetLength(1); i++)
                {
                    if (!board[x + i, y + i].HasValue) break;
                    if (board[x + i, y + i] == player) { downright = i; break; }
                }

            if (!(up.HasValue || down.HasValue || left.HasValue || right.HasValue || upleft.HasValue || upright.HasValue || downleft.HasValue || downright.HasValue)) return null;

            var newState = new OthelloState();
            Array.Copy(board, newState.board, board.Length);

            if (up.HasValue)
                for (var i = 1; i < up; i++)
                    newState.board[x, y - i] = player;

            if (down.HasValue)
                for (var i = 1; i < down; i++)
                    newState.board[x, y + i] = player;

            if (left.HasValue)
                for (var i = 1; i < left; i++)
                    newState.board[x - i, y] = player;

            if (right.HasValue)
                for (var i = 1; i < right; i++)
                    newState.board[x + i, y] = player;

            if (upleft.HasValue)
                for (var i = 1; i < upleft; i++)
                    newState.board[x - i, y - i] = player;

            if (upright.HasValue)
                for (var i = 1; i < upright; i++)
                    newState.board[x + i, y - i] = player;

            if (downleft.HasValue)
                for (var i = 1; i < downleft; i++)
                    newState.board[x - i, y + i] = player;

            if (downright.HasValue)
                for (var i = 1; i < downright; i++)
                    newState.board[x + i, y + i] = player;

            newState.board[x, y] = player;

            newState.CurrentPlayer = this.CurrentPlayer == Player.Black ? Player.White : Player.Black;
            if (!newState.CanMove(newState.CurrentPlayer)) newState.CurrentPlayer = this.CurrentPlayer;

            return newState;
        }
    }

    public enum Player { Black, White }
}
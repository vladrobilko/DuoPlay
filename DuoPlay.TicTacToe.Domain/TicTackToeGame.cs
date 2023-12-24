namespace DuoPlay.TicTacToe.Domain
{
    public class TicTackToeGame
    {
        public TicTacToeResult DetermineGameResult(char[] board)
        {
            if (CheckHorizontalWin(board) || CheckVerticalWin(board) || CheckDiagonalWin(board))
                return TicTacToeResult.Win;
            else if (CheckDraw(board))
                return TicTacToeResult.Draw;
            else
                return TicTacToeResult.GameContinue;
        }

        private bool CheckHorizontalWin(char[] board)
        {
            return (board[0] == board[1] && board[1] == board[2]) ||
                   (board[3] == board[4] && board[4] == board[5]) ||
                   (board[6] == board[7] && board[7] == board[8]);
        }

        private bool CheckVerticalWin(char[] board)
        {
            return (board[0] == board[3] && board[3] == board[6]) ||
                   (board[1] == board[4] && board[4] == board[7]) ||
                   (board[2] == board[5] && board[5] == board[8]);
        }

        private bool CheckDiagonalWin(char[] board)
        {
            return (board[0] == board[4] && board[4] == board[8]) ||
                   (board[2] == board[4] && board[4] == board[6]);
        }

        private bool CheckDraw(char[] board)
        {
            return board.All(cell => cell != '1' && cell != '2' && cell != '3' &&
                                    cell != '4' && cell != '5' && cell != '6' &&
                                    cell != '7' && cell != '8' && cell != '9');
        }
    }
}

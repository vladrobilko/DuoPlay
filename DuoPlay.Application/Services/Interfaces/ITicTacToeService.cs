using DuoPlay.TicTacToe.Domain;

namespace DuoPlay.Application.Services.Interfaces
{
    public interface ITicTacToeService
    {
        Task ReadyToGame(string playerName, string sessionName);

        Task<bool> IsGameStarted(string sessionName);

        Task<bool> IsPlayerTurn(string playerName, string sessionName);

        Task<TicTacToeResult> MakeMove(string playerName, string sessionName, int move);

        Task<char[]> GetBoard(string sessionName);

        Task<string> GetGameMessage(string sessionName);
    }
}

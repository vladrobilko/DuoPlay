using DuoPlay.Application.Helpers;

namespace DuoPlay.Application.Services.Interfaces
{
    public interface ISessionService
    {
        Task<string> CreateSession(string playerName, GameType gameType);

        Task<List<string>> GetNotStartedSessions(GameType gameType);

        Task<bool> JoinSession(string playerJoinName, string sessionName);
    }
}

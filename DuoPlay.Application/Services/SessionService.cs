using DuoPlay.Application.Helpers;
using DuoPlay.Application.Services.Interfaces;
using DuoPlay.DataManagement;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DuoPlay.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context) => _context = context;

        public async Task<string> CreateSession(string playerName, GameType gameType)
        {
            var nameSession = gameType + " " + playerName;
            using var transaction = _context.Database.BeginTransaction();
            var player = await ReadPlayerByName(playerName);
            _context.Sessions.Add(CreateSessionInstance(player.Id, nameSession));
            await _context.SaveChangesAsync();
            transaction.Commit();
            return nameSession;
        }

        public async Task<List<string>> GetNotStartedSessions(GameType gameType)
        {
            return await _context.Sessions
                .Where(s => s.StartSession == null && s.EndSession == null && s.IdPlayerJoin == null && s.Name.Contains(gameType.ToString()))
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<bool> JoinSession(string playerJoinName, string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            if (session.IdPlayerJoin != null || session.StartSession != null) return false;
            await UpdateSession(session, playerJoinName);
            return true;
        }

        private async Task UpdateSession(SessionDto session, string playerJoinName)
        {
            using var transaction = _context.Database.BeginTransaction();
            session.StartSession = DateTime.UtcNow;
            var player = await ReadPlayerByName(playerJoinName);
            session.IdPlayerJoin = player.Id;
            _context.Sessions.Update(session);
            _context.SaveChanges();
            transaction.Commit();
        }

        private SessionDto CreateSessionInstance(long idPlayer, string nameSession)
        {
            return new SessionDto()
            {
                IdPlayerHost = idPlayer,
                Name = nameSession
            };
        }

        private async Task<PlayerDto> ReadPlayerByName(string namePlayer) =>
            await _context.Players.SingleAsync(p => p.Name == namePlayer);

        private async Task<SessionDto> ReadSessionByName(string nameSession) =>
            await _context.Sessions.SingleOrDefaultAsync(p => p.Name == nameSession);
    }
}
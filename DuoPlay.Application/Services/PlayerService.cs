using DuoPlay.Application.Services.Interfaces;
using DuoPlay.DataManagement;
using Microsoft.EntityFrameworkCore;

namespace DuoPlay.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context) => _context = context;

        public async Task CreatePlayer(string name)
        {
            using var transaction = _context.Database.BeginTransaction();
            _context.Players.Add(new PlayerDto { Name = name });
            await _context.SaveChangesAsync();
            transaction.Commit();
        }

        public async Task<bool> IsPlayerExist(string name)
        {
            var player = await _context.Players.FirstOrDefaultAsync(x => x.Name == name);
            return player != null;
        }
    }
}

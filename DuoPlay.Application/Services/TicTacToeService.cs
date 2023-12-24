using DuoPlay.Application.Helpers;
using DuoPlay.Application.Services.Interfaces;
using DuoPlay.DataManagement;
using DuoPlay.TicTacToe.Domain;
using Microsoft.EntityFrameworkCore;

namespace DuoPlay.Application.Services
{
    public class TicTacToeService : ITicTacToeService
    {
        private readonly ApplicationDbContext _context;

        private readonly TicTackToeGame _game;

        public TicTacToeService(ApplicationDbContext context)
        {
            _context = context;
            _game = new TicTackToeGame();
        }

        public async Task ReadyToGame(string playerName, string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var player = await ReadPlayerByName(playerName);
            if (session.IdPlayerJoin == null) await CreateGame(player.Id, session.Id);
            else await UpdateGame(session.Id);
        }

        public async Task<bool> IsGameStarted(string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameBySessionId(session.Id);
            await _context.Entry(game).ReloadAsync();
            if (game == null || game.StartGame == null) return false;
            return true;
        }

        public async Task<bool> IsPlayerTurn(string playerName, string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameBySessionId(session.Id);
            await _context.Entry(game).ReloadAsync();
            var playerTurn = await _context.Players.FirstAsync(x => x.Id == game.IdPlayerTurn);
            return playerTurn.Name == playerName;
        }

        public async Task<TicTacToeResult> MakeMove(string playerName, string sessionName, int move)
        {
            var session = await ReadSessionByName(sessionName);
            await _context.Entry(session).ReloadAsync();
            var player = await ReadPlayerByName(playerName);
            var game = await ReadGameBySessionId(session.Id);
            var board = game.BoardState.ConvertToCharArray();
            if (board[move] == 'X' || board[move] == 'O') throw new NotImplementedException();
            ChooseMoveValue(board, move, session, player);
            ChoosePlayerMove(game, session);
            var moveResult = _game.DetermineGameResult(board);
            await UpdateGame(game, player, moveResult, board, playerName);
            return moveResult;
        }

        public async Task<char[]> GetBoard(string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameBySessionId(session.Id);
            return game.BoardState.ConvertToCharArray();
        }

        public async Task<string> GetGameMessage(string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameBySessionId(session.Id);
            return game.GameMessage;
        }

        private void ChooseMoveValue(char[] board, int move, SessionDto session, PlayerDto player)
        {
            if (session.IdPlayerHost == player.Id)
                board[move] = 'X';
            else
                board[move] = 'O';
        }

        private void ChoosePlayerMove(TicTakToeGameDto game, SessionDto session)
        {
            if (game.IdPlayerTurn == session.IdPlayerHost)
                game.IdPlayerTurn = session.IdPlayerJoin;
            else
                game.IdPlayerTurn = session.IdPlayerHost;
        }

        private async Task CreateGame(long playerId, long sessionId)
        {
            char[] initialBoard = TicTakToeBoardHelper.GetInitialBoard();
            var game = new TicTakToeGameDto()
            {
                IdPlayerTurn = playerId,
                IdSession = sessionId,
                BoardState = new string(initialBoard),
                GameMessage = string.Empty,
            };
            _context.TictaktoeGames.Add(game);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateGame(TicTakToeGameDto game, PlayerDto player, TicTacToeResult moveResult, char[] board, string playerName)
        {
            game.BoardState = board.ConvertToString();
            if (moveResult == TicTacToeResult.Win)
            {
                await UpdateSessionForEnd(game);
                game.IdPlayerWin = player.Id;
                game.EndGame = DateTime.UtcNow;
                game.GameMessage = $"{playerName} WIN!";
            }
            if (moveResult == TicTacToeResult.Draw)
            {
                await UpdateSessionForEnd(game);
                game.EndGame = DateTime.UtcNow;
                game.GameMessage = $"Draw!";
            }
            await _context.SaveChangesAsync();
        }

        private async Task UpdateSessionForEnd(TicTakToeGameDto game)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == game.IdSession);
            session.EndSession = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private async Task UpdateGame(long sessionId)
        {
            var game = await ReadGameBySessionId(sessionId);
            game.StartGame = DateTime.UtcNow;
            _context.TictaktoeGames.Attach(game);
            _context.Entry(game).Property(r => r.StartGame).IsModified = true;
            await _context.SaveChangesAsync();
        }

        private async Task<PlayerDto> ReadPlayerByName(string playerName)
        {
            return await _context.Players.FirstAsync(x => x.Name == playerName);
        }

        private async Task<TicTakToeGameDto> ReadGameBySessionId(long sessionId)
        {
            return await _context.TictaktoeGames.FirstAsync(x => x.IdSession == sessionId);
        }

        private async Task<SessionDto> ReadSessionByName(string sessionName)
        {
            return await _context.Sessions.FirstAsync(x => x.Name == sessionName);
        }
    }
}
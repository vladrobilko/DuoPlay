using DuoPlay.Application.Helpers;
using DuoPlay.Application.Services.Interfaces;
using DuoPlay.BattleShip.Domain;
using DuoPlay.DataManagement;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DuoPlay.Application.Services
{
    public class BattleShipService : IBattleShipService
    {
        private readonly ApplicationDbContext _context;

        private readonly BattleShipStateChanger _seaBattleGameChanger;

        public BattleShipService(ApplicationDbContext context)
        {
            _context = context;
            _seaBattleGameChanger = new BattleShipStateChanger();
        }

        public PlayerStateModel GetPlayerStateModelToChoosePlayArea(string playerName)
        {
            var stateModel = new PlayerStateModel(playerName);
            stateModel.FillShips();
            return stateModel;
        }

        public async Task<bool> IsPlayerTurn(string playerName, string sessionName)
        {
            using var transaction = _context.Database.BeginTransaction();
            var player = await ReadPlayerByName(playerName);
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameByIdSession(session.Id);
            await _context.Entry(game).ReloadAsync();
            transaction.Commit();
            return game.IdPlayerTurn == player.Id;
        }

        public async Task<PlayerStateModel> GetPlayerStateModel(string playerName, string sessionName)
        {
            var stateModel = new PlayerStateModel(playerName);
            var session = await ReadSessionByName(sessionName);
            await _context.Entry(session).ReloadAsync();
            var player = await ReadPlayerByName(playerName);
            var playAreaDb = await ReadPlayAreaByPlayerId(player.Id);
            await _context.Entry(playAreaDb).ReloadAsync();
            var enemyPlayArea = await ReadEnemyPlayarea(session, player.Id);
            if (enemyPlayArea == null || enemyPlayArea.ConfirmedPlayarea == null) return null;
            await _context.Entry(enemyPlayArea).ReloadAsync();
            AssignPlayAreasAndShipsToPlayerStateModel(stateModel, playAreaDb, enemyPlayArea);
            await AssignShootModel(stateModel, session);
            return stateModel;
        }

        public async Task ReadyToGame(PlayerStateModel battleShipStateModel, string playerName, string sessionName)
        {
            var player = await ReadPlayerByName(playerName);
            var playAreaId = await UpdatePlayAreaDbInstance(battleShipStateModel, player);
            await UpdateShips(battleShipStateModel, playAreaId);
            var session = await ReadSessionByName(sessionName);
            if (session.StartSession != null) await StartGame(session);
        }

        public async Task<GameState> GetGameStateModel(string nameSession)
        {
            using var transaction = _context.Database.BeginTransaction();
            var session = await ReadSessionByName(nameSession);
            var gameStateDto = await ReadGameByIdSession(session.Id);
            var playerHost = await ReadPlayerById(session.IdPlayerHost);
            var playerJoin = await ReadPlayerById(session.IdPlayerJoin);
            var playerTurn = await ReadPlayerById(gameStateDto.IdPlayerTurn);
            var playerHostModel = await GetPlayerStateModel(playerHost.Name, session.Name);
            var playerJoinModel = await GetPlayerStateModel(playerJoin.Name, session.Name);
            transaction.Commit();
            return new GameState(playerHostModel, playerJoinModel, playerTurn.Name, gameStateDto.EndGame == null, gameStateDto.GameMessage);
        }

        private void CreateShoot(PlayerDto player, SeabattleGameDto game, int y, int x)
        {
            var shootIntoDto = new ShootDto();
            shootIntoDto.IdPlayerShoot = player.Id;
            shootIntoDto.IdSeabattleGame = game.Id;
            shootIntoDto.ShootCoordinateY = y;
            shootIntoDto.ShootCoordinateX = x;
            shootIntoDto.TimeShoot = DateTime.UtcNow;
            _context.Shoots.Add(shootIntoDto);
            _context.SaveChanges();
        }

        private void UpdateShoot(ShootDto shootInDb, PlayerDto player, int y, int x)
        {
            shootInDb.IdPlayerShoot = player.Id;
            shootInDb.ShootCoordinateY = y;
            shootInDb.ShootCoordinateX = x;
            shootInDb.TimeShoot = DateTime.UtcNow;
            _context.Shoots.Attach(shootInDb);
            _context.Entry(shootInDb).Property(r => r.IdPlayerShoot).IsModified = true;
            _context.Entry(shootInDb).Property(r => r.ShootCoordinateY).IsModified = true;
            _context.Entry(shootInDb).Property(r => r.ShootCoordinateX).IsModified = true;
            _context.Entry(shootInDb).Property(r => r.TimeShoot).IsModified = true;
            _context.SaveChanges();
        }

        public async Task Shoot(int x, int y, string playerName, string sessionName)
        {
            var session = await ReadSessionByName(sessionName);
            var game = await ReadGameByIdSession(session.Id);
            var player = await ReadPlayerByName(playerName);
            var shootInDb = await ReadShootByGameId(game.Id);
            if (shootInDb == null && game.IdPlayerTurn == player.Id) CreateShoot(player, game, y, x);
            else if (shootInDb != null && game.IdPlayerTurn == player.Id) UpdateShoot(shootInDb, player, y, x);
            var lastGameState = await GetGameStateModel(sessionName);
            var changeGameModel = _seaBattleGameChanger.ChangeGameState(lastGameState);
            await UpdateGameStateModel(changeGameModel, session);
        }

        private async Task UpdatePlayer(IPlayer player, PlayerDto playerDb, string textModel)
        {
            var playAreaInDb = await ReadPlayAreaById(playerDb.Id);
            playAreaInDb.Playarea1 = textModel;
            _context.Playareas.Attach(playAreaInDb);
            _context.Entry(playAreaInDb).Property(r => r.Playarea1).IsModified = true;
            _context.SaveChanges();
            ReCreateShips(player, playAreaInDb);
        }

        private void ReCreateShips(IPlayer player, PlayareaDto playAreaInDb)
        {
            var ships = player.Ships;
            var listShipsInDto = _context.Ships.Where(p => p.IdPlayarea == playAreaInDb.Id);
            foreach (var ship in listShipsInDto)
            {
                _context.Ships.Remove(ship);
            }
            _context.SaveChanges();
            foreach (var ship in ships)
            {
                var shipDto = new ShipDto
                {
                    IdPlayarea = playAreaInDb.Id,
                    Length = ship.Length,
                    DecksJson = ship.Decks.ToJson()
                };
                _context.Ships.Add(shipDto);
                _context.SaveChanges();
            }
        }

        private void UpdateGame(SeabattleGameDto lastGameStateFromDb, PlayerDto playerTurn, GameState state)
        {
            lastGameStateFromDb.IdPlayerTurn = playerTurn.Id;
            lastGameStateFromDb.GameMessage = state.GameMessage;
            _context.SeabattleGames.Attach(lastGameStateFromDb);
            _context.Entry(lastGameStateFromDb).Property(r => r.IdPlayerTurn).IsModified = true;
            _context.Entry(lastGameStateFromDb).Property(r => r.GameMessage).IsModified = true;
            _context.SaveChanges();
        }

        private void UpdateGameForEnd(SeabattleGameDto lastGameStateFromDb, GameState state)
        {
            UpdateSessionForEndGame(lastGameStateFromDb);
            lastGameStateFromDb.IdPlayerWin = lastGameStateFromDb.IdPlayerTurn;
            lastGameStateFromDb.GameMessage = state.GameMessage;
            lastGameStateFromDb.EndGame = DateTime.UtcNow;
            _context.SeabattleGames.Attach(lastGameStateFromDb);
            _context.Entry(lastGameStateFromDb).Property(r => r.IdPlayerTurn).IsModified = true;
            _context.Entry(lastGameStateFromDb).Property(r => r.GameMessage).IsModified = true;
            _context.Entry(lastGameStateFromDb).Property(r => r.EndGame).IsModified = true;
            _context.SaveChanges();
        }

        private void UpdateSessionForEndGame(SeabattleGameDto game)
        {
            var session = _context.Sessions.First(x => x.Id == game.IdSession);
            session.EndSession = DateTime.UtcNow;
            _context.SaveChanges();
        }

        private async Task UpdateGameStateModel(GameState state, SessionDto session)
        {
            using var transaction = _context.Database.BeginTransaction();
            var playerTurn = await ReadPlayerByName(state.NamePlayerTurn);
            var lastGameStateFromDb = await ReadGameByIdSession(session.Id);
            if (lastGameStateFromDb == null) CreateGame(playerTurn, session, state.GameMessage);
            else if (lastGameStateFromDb != null && state.IsGameOn)
            {
                var player1 = await ReadPlayerByName(state.Player1.NamePlayer);
                var textModel1 = PlayAreaConverter.ToStringForDb(state.Player1.GetPlayArea());
                await UpdatePlayer(state.Player1, player1, textModel1);

                var player2 = await ReadPlayerByName(state.Player2.NamePlayer);
                var textModel2 = PlayAreaConverter.ToStringForDb(state.Player2.GetPlayArea());
                await UpdatePlayer(state.Player2, player2, textModel2);
                UpdateGame(lastGameStateFromDb, playerTurn, state);
            }
            else if (lastGameStateFromDb != null && !state.IsGameOn)
            {
                var player1 = await ReadPlayerByName(state.Player1.NamePlayer);
                var textModel1 = PlayAreaConverter.ToStringForDb(state.Player1.GetPlayArea());
                await UpdatePlayer(state.Player1, player1, textModel1);
                var player2 = await ReadPlayerByName(state.Player2.NamePlayer);
                var textModel2 = PlayAreaConverter.ToStringForDb(state.Player2.GetPlayArea());
                await UpdatePlayer(state.Player2, player2, textModel2);
                UpdateGameForEnd(lastGameStateFromDb, state);
            }
            transaction.Commit();
        }

        public async Task<string> GetGameMessage(string nameSession)
        {
            var session = await ReadSessionByName(nameSession);
            var game = await ReadGameByIdSession(session.Id);
            await _context.Entry(game).ReloadAsync();
            return game.GameMessage;
        }

        private async Task StartGame(SessionDto session)
        {
            var player1 = await ReadPlayerById(session.IdPlayerHost);
            var player1StateModel = await GetPlayerStateModel(player1.Name, session.Name) ?? throw new NotImplementedException();
            var player2 = await ReadPlayerById(session.IdPlayerJoin);
            var player2StateModel = await GetPlayerStateModel(player2.Name, session.Name) ?? throw new NotImplementedException();
            var gameState = new GameState(
                player1StateModel,
                player2StateModel,
                player2StateModel.NamePlayer,
                true,
                GameStateMessage.WhoShoot(player2StateModel.NamePlayer));
            await UpdateGameStateModel(gameState, session);
        }

        private async Task<SeabattleGameDto?> ReadGameByIdSession(long id)
        {
            return await _context.SeabattleGames.SingleOrDefaultAsync(p => p.IdSession == id);
        }

        private async Task<PlayareaDto> ReadEnemyPlayarea(SessionDto session, long idPlayer)
        {
            PlayareaDto enemyPlayArea;
            if (session.IdPlayerHost == idPlayer)
                enemyPlayArea = await ReadPlayAreaById(session.IdPlayerJoin);
            else
                enemyPlayArea = await ReadPlayAreaById(session.IdPlayerHost);
            return enemyPlayArea;
        }

        private async Task<PlayareaDto> ReadPlayAreaById(long? id)
        {
            return await _context.Playareas.FirstOrDefaultAsync(x => x.IdPlayer == id);
        }

        private async Task<PlayerDto> ReadPlayerByName(string name)
        {
            return await _context.Players.SingleAsync(p => p.Name == name);
        }

        private async Task<PlayerDto> ReadPlayerById(long? id)
        {
            return await _context.Players.SingleAsync(p => p.Id == id);
        }

        private async Task<SessionDto> ReadSessionByName(string name)
        {
            return await _context.Sessions.FirstAsync(p => p.Name == name);
        }

        private async Task<ShootDto> ReadShootByGameId(long id)
        {
            return await _context.Shoots.FirstOrDefaultAsync(p => p.IdSeabattleGame == id);
        }

        private async Task AssignShootModel(PlayerStateModel stateModel, SessionDto session)
        {
            var game = await ReadGameByIdSession(session.Id);
            if (game != null)
            {
                var shootDb = await ReadShootByGameId(game.Id);
                if (shootDb != null)
                {
                    var playerShoot = await ReadPlayerById(shootDb.IdPlayerShoot);
                    stateModel.LastShoot = new ShootModel(Convert.ToInt32(shootDb.ShootCoordinateY), Convert.ToInt32(shootDb.ShootCoordinateX), playerShoot.Name, session.Name);
                }
            }
        }

        private List<Ship> ReadShipsByPlayerAreaId(long id)
        {
            return _context.Ships.Where(p => p.IdPlayarea == id).ToList().ToShips();
        }

        private void AssignPlayAreasAndShipsToPlayerStateModel(PlayerStateModel stateModel, PlayareaDto playAreaDb, PlayareaDto enemyPlayArea)
        {
            stateModel.PlayArea = playAreaDb.Playarea1.ToPlayArea();
            stateModel.EnemyPlayArea = enemyPlayArea.Playarea1.ToPlayArea();
            stateModel.Ships = ReadShipsByPlayerAreaId(playAreaDb.Id);
        }

        private async Task<PlayareaDto> ReadPlayAreaByPlayerId(long id)
        {
            return await _context.Playareas.FirstAsync(x => x.IdPlayer == id);
        }

        private async Task<long> UpdatePlayAreaDbInstance(PlayerStateModel battleShipStateModel, PlayerDto player)
        {
            var playArea = new PlayareaDto()
            {
                IdPlayer = player.Id,
                Playarea1 = battleShipStateModel.PlayArea.ToStringForDb(),
                ConfirmedPlayarea = DateTime.UtcNow
            };
            _context.Playareas.Add(playArea);
            await _context.SaveChangesAsync();
            return playArea.Id;
        }

        private async Task UpdateShips(PlayerStateModel battleShipStateModel, long playAreaId)
        {
            foreach (var ship in battleShipStateModel.Ships)
            {
                var shipDto = new ShipDto
                {
                    IdPlayarea = playAreaId,
                    Length = ship.Length,
                    DecksJson = ship.Decks.ToJson()
                };
                _context.Ships.Add(shipDto);
                await _context.SaveChangesAsync();
            }
        }

        private void CreateGame(PlayerDto playerTurn, SessionDto session, string gameMessage)
        {
            var newGameStateIntoDto = new SeabattleGameDto();
            newGameStateIntoDto.IdPlayerTurn = playerTurn.Id;
            newGameStateIntoDto.IdSession = session.Id;
            newGameStateIntoDto.GameMessage = gameMessage;
            newGameStateIntoDto.StartGame = DateTime.UtcNow;
            _context.SeabattleGames.Add(newGameStateIntoDto);
            _context.SaveChanges();
        }
    }
}
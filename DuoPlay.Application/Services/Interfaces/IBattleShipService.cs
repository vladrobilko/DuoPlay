using DuoPlay.BattleShip.Domain;

namespace DuoPlay.Application.Services.Interfaces
{
    public interface IBattleShipService
    {
        PlayerStateModel GetPlayerStateModelToChoosePlayArea(string playerName);

        Task ReadyToGame(PlayerStateModel battleShipStateModel, string playerName, string sessionName);

        Task<PlayerStateModel> GetPlayerStateModel(string playerName, string sessionName);

        Task<GameState> GetGameStateModel(string nameSession);

        Task<string> GetGameMessage(string nameSession);

        Task<bool> IsPlayerTurn(string playerName, string sessionName);

        Task Shoot(int x, int y, string playerName, string sessionName);
    }
}

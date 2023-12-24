using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.BattleShip.Domain
{
    public class BattleShipStateChanger
    {
        public GameState ChangeGameState(GameState gameState)
        {
            if (gameState.IsGameOn)
            {
                if (gameState.NamePlayerTurn == gameState.Player1.NamePlayer)
                    ProcessPlayerTurn(gameState.Player1, gameState.Player2, gameState.Player1.NamePlayer, gameState);
                else if (gameState.NamePlayerTurn == gameState.Player2.NamePlayer)
                    ProcessPlayerTurn(gameState.Player2, gameState.Player1, gameState.Player2.NamePlayer, gameState);
            }
            return gameState;
        }

        private void ProcessPlayerTurn(IPlayer shooter, IPlayer targetPlayer, string shooterName, GameState gameState)
        {
            var target = shooter.GetNextValidShootTarget();
            var result = targetPlayer.OnShoot(target);
            gameState.IsGameOn = (result != ShootResultType.GameOver);
            AssignGameMessage(gameState, result, shooterName);
            if (result == ShootResultType.Miss) gameState.NamePlayerTurn = targetPlayer.NamePlayer;
        }

        private void AssignGameMessage(GameState gameState, ShootResultType shootResultType, string namePlayer1)
        {
            if (shootResultType == ShootResultType.Miss)
            {
                gameState.GameMessage = GameStateMessage.WhoMissAndShoot(namePlayer1);
            }
            else if (shootResultType == ShootResultType.Hit)
            {
                gameState.GameMessage = GameStateMessage.WhoHitAndShoot(namePlayer1);
            }
            else if (shootResultType == ShootResultType.Kill)
            {
                gameState.GameMessage = GameStateMessage.WhoKillAndShoot(namePlayer1);
            }
            else if (shootResultType == ShootResultType.GameOver)
            {
                gameState.GameMessage = GameStateMessage.WhoWinGame(namePlayer1);
            }
        }
    }
}

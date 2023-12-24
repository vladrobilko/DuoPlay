namespace DuoPlay.BattleShip.Domain
{
    public class GameState
    {
        public IPlayer Player1 { get; }

        public IPlayer Player2 { get; }

        public string NamePlayerTurn { get; set; }

        public bool IsGameOn { get; set; }

        public string GameMessage { get; set; }

        public GameState(IPlayer player1, IPlayer player2, string namePlayerTurn, bool isGameOn, string gameMessage)
        {
            Player1 = player1;
            Player2 = player2;
            NamePlayerTurn = namePlayerTurn;
            IsGameOn = isGameOn;
            GameMessage = gameMessage;
        }
    }
}

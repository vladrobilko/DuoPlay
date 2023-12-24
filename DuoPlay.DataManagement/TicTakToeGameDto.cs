namespace DuoPlay.DataManagement
{
    public partial class TicTakToeGameDto
    {
        public long Id { get; set; }

        public long? IdPlayerTurn { get; set; }

        public long? IdPlayerWin { get; set; }

        public long IdSession { get; set; }

        public string GameMessage { get; set; } = null!;

        public DateTime? StartGame { get; set; }

        public DateTime? EndGame { get; set; }

        public string BoardState { get; set; }

        public virtual PlayerDto? IdPlayerTurnNavigation { get; set; }

        public virtual PlayerDto? IdPlayerWinNavigation { get; set; }

        public virtual SessionDto IdSessionNavigation { get; set; } = null!;

    }
}

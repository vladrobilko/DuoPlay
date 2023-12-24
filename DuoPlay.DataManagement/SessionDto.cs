namespace DuoPlay.DataManagement
{
    public partial class SessionDto
    {
        public long Id { get; set; }

        public long IdPlayerHost { get; set; }

        public long? IdPlayerJoin { get; set; }

        public string Name { get; set; } = null!;

        public DateTime? StartSession { get; set; }

        public DateTime? EndSession { get; set; }

        public virtual PlayerDto IdPlayerHostNavigation { get; set; } = null!;

        public virtual PlayerDto? IdPlayerJoinNavigation { get; set; }

        public virtual TicTakToeGameDto? TicTakToeGame { get; set; }

        public virtual SeabattleGameDto? SeabattleGame { get; set; }
    }
}

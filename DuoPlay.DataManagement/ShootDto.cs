namespace DuoPlay.DataManagement
{
    public partial class ShootDto
    {
        public long Id { get; set; }

        public long IdSeabattleGame { get; set; }

        public long IdPlayerShoot { get; set; }

        public long ShootCoordinateY { get; set; }

        public long ShootCoordinateX { get; set; }

        public DateTime TimeShoot { get; set; }

        public virtual PlayerDto IdPlayerShootNavigation { get; set; } = null!;

        public virtual SeabattleGameDto IdSeabattleGameNavigation { get; set; } = null!;
    }
}

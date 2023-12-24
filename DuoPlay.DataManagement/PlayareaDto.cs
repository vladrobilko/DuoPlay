namespace DuoPlay.DataManagement
{
    public partial class PlayareaDto
    {
        public long Id { get; set; }

        public long IdPlayer { get; set; }

        public string? Playarea1 { get; set; }

        public DateTime? ConfirmedPlayarea { get; set; }

        public virtual ICollection<ShipDto> Ships { get; } = new List<ShipDto>();
    }
}

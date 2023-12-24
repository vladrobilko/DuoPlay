namespace DuoPlay.DataManagement
{
    public partial class ShipDto
    {
        public long Id { get; set; }

        public long IdPlayarea { get; set; }

        public long Length { get; set; }

        public string DecksJson { get; set; }

        public virtual PlayareaDto IdPlayareaNavigation { get; set; }
    }
}

namespace SutraPlus_DAL.Models
{
    public partial class EmailConfig
    {
        public int Id { get; set; }
        public string FromEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string EmailServerHost { get; set; } = null!;
        public string EmailServerPort { get; set; } = null!;
        public Boolean IsActive { get; set; }
        public string PreparedBy { get; set; } = null!;
        public DateTime PreparedDate { get; set; }
    }
}

namespace DemoWebAPI.Models
{
    public class AuthMaster
    {
        public int AuthMasterId { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}

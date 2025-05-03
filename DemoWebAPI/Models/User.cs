using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoWebAPI.Models
{
    public class User
    {
        public int? UserMasterId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? Gender { get; set; }

        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;

    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoWebAPI.Dtos
{
    public class UserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

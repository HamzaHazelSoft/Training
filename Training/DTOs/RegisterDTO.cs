using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserManagementSystem.Helper;

namespace UserManagementSystem.DTOs
{
    public class RegisterDTO
    {

        public string? Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly? DOB { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [ValidatePhoneNumber]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonConverter(typeof(RolesConverter))]
        public List<string> Roles { get; set; }
    }
}

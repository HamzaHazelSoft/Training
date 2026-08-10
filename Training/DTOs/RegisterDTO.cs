using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.DTOs
{
    public class RegisterDTO
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly? DOB { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}

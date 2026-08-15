using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserManagementSystem.Helper;

namespace UserManagementSystem.DTOs
{
    public class UserDTO
    {

       public string? Id { get; set; }

       [Required]
       public string UserName { get; set; }
       [Required]
       public string FirstName { get; set; }
       [Required]
       public string LastName { get; set; }
       [Required]
       public DateOnly? DOB { get; set; }
       
       [EmailAddress]
       [Required]
       public string Email { get; set; }

       [Required]
       public string PhoneNumber { get; set; }

    }
}

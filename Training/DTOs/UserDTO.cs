using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.DTOs
{
    public class UserDTO
    {
       public string? Id { get; set; }
       public string UserName { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public DateOnly? DOB { get; set; }
       public string Email { get; set; }
       public string PhoneNumber { get; set; }

    }
}

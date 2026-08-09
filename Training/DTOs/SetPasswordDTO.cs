namespace UserManagementSystem.DTOs
{
    public class SetPasswordDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string UserId { get; set; }
    }
}

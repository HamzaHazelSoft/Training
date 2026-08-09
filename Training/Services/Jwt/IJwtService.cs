using UserManagementSystem.Models;

namespace UserManagementSystem.Services.Jwt
{
    public interface IJwtService
    {
        public string CreateToken(User user,IList<string> roles);
    }
}

using Training.Models;

namespace Training.Services.Jwt
{
    public interface ITokenService
    {
        string CreateToken(User user,IList<string> roles);
    }
}

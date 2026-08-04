using Training.Models;

namespace Training.Services.Jwt
{
    public interface ITokenService
    {
        public string CreateToken(User user,IList<string> roles);
    }
}

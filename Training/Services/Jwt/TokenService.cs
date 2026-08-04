using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using Training.Models;

namespace Training.Services.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration; //I use here as i want to retreive some key from appsettings.json file

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Generate Token here
        public string CreateToken(User user, IList<string> roles)
        {
            /*
            Claims are used to store user info in the token . Why not anonymous object ? Because claims
            are standard way of Asp.net core and it can easily identify by the framework . Like framework can do User.Identity.Name to
            get the name of the user from the token
            */ 
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //Here we are adding the roles to the claims . Using Addrange
                                                                                     //because we are adding multiple roles to the claims
            //Secret key for signing the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); //Here we are getting the key from the appsettings.json file.SymmetricSecurityKey means
                                                                                                   //same key used for siginging and verifying the token
            //Header ke liye signing algorithm aur key provide karti hai
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // JWT ko is secret key se aur HMAC-SHA256 algorithm use karke digitally sign karo
            

            //Payload and collect headers Information
            var token = new JwtSecurityToken
            (
                signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), //Token will expire in 1 hour
                issuer : _configuration["Jwt:Issuer"], //Issuer is the entity that issues the token. Here we are getting it from the appsettings.json file
                audience : _configuration["Jwt:Audience"] //Audience is the entity that consumes the token. Here we are getting it from the appsettings.json file
            );


            //Header + Payload ko encode karke Signature generate karta hai aur final Header.Payload.Signature string banata hai.
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}

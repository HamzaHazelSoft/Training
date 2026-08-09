using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.Jwt.Implementation
{
    /// <summary>
    /// Provides functionality for creating JWT access tokens for authenticated users.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _jwtConfiguration; 

        public JwtService(IConfiguration configuration)
        {
            _jwtConfiguration = configuration.GetSection("Jwt");
        }

        /// <summary>
        /// Creates a signed JWT access token containing the user's identity and roles.
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string CreateToken(User user, IList<string> roles)
        {
            // Add standard claims instead of anonymous object that identify by the framework the authenticated user.
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            // Add each user role as a role claim.
            // ASP.NET Core uses role claims for role-based authorization.
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            // Retrieve the JWT signing key from application configuration.
            // The same symmetric key is used by the application to sign and validate the token.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration["Key"]));


            // These credentials are used to digitally sign the JWT.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT with the configured claims, issuer, audience,
            // signing credentials, and expiration time.
            var token = new JwtSecurityToken
            (
                signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtConfiguration.GetValue<int>("ExpiryHours")), 
                issuer : _jwtConfiguration["Issuer"], 
                audience : _jwtConfiguration["Audience"] 
            );

            // Serialize the JWT into the standard Header.Payload.Signature format.
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}

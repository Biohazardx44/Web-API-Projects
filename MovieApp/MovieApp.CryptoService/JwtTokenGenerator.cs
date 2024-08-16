using Microsoft.IdentityModel.Tokens;
using MovieApp.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieApp.CryptoService
{
    public static class JwtTokenGenerator
    {
        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user with extended validity.
        /// </summary>
        /// <param name="user">The user for whom the JWT is generated.</param>
        /// <returns>A JWT string with an extended validity period.</returns>
        public static string GenerateJwtToken(this User user)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our very hidden secret secret key");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("userFullName", $"{user.FirstName} {user.LastName}"),
                        new Claim("userId", $"{user.Id}"),
                        new Claim(ClaimTypes.Name, user.Username)
                    }
                )
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
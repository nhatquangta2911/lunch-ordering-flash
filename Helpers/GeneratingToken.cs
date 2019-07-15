using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CourseApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CourseApi.Helpers
{
    public class GeneratingToken
    {
        private const int EXPIRE_DATE = 3;
        public static string GenerateToken(string secret, User user) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(EXPIRE_DATE),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
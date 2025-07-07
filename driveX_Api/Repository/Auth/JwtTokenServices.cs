using driveX_Api.DTOs.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace driveX_Api.Repository.Auth
{
    public class JwtTokenServices : IJwtToken
    {
        private readonly JwtToken jwtTokenDto;

        public JwtTokenServices(IOptions<JwtToken> jwtTokenOptn)
        {
            jwtTokenDto = jwtTokenOptn.Value;
        }

        public string GenerateToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenDto.Key));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtTokenDto.Expiration),
                SigningCredentials = credential,
                Issuer = jwtTokenDto.Issuer,
                Audience = jwtTokenDto.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}


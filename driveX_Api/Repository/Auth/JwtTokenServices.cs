using driveX_Api.CommonClasses;
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
        private readonly JwtToken _accessToken;
        private readonly JwtToken _sessionToken;

        public JwtTokenServices(IOptionsMonitor<JwtToken> jwtTokenOptn)
        {
            _accessToken = jwtTokenOptn.Get("AccessToken");
            _sessionToken = jwtTokenOptn.Get("SessionToken");
        }

        public string GenerateAccessToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessToken.Key));
            var credential = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credential,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _accessToken.Issuer,
                Audience = _accessToken.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public string GenerateSessionToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sessionToken.Key));
            var credential = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credential,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _sessionToken.Issuer,
                Audience = _sessionToken.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securitToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securitToken);
        }

        public ApiResponse<object> GetNewToken(string userId)
        {
            ApiResponse<object> apiResponse = new();

            var accessToken = GenerateAccessToken(userId);
            var sessionToken = GenerateSessionToken(userId);

            object tokens = new
            {
                AccessToken = accessToken,
                SessionToken = sessionToken
            };

            apiResponse.SetSuccess(tokens, "Generated new tokens");
            apiResponse.SetToken(tokens);

            return apiResponse;
        }
    }
}


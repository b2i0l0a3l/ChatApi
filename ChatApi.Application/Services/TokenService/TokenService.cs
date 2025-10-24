using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApi.Application.Interfaces;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.presistence;
using Microsoft.IdentityModel.Tokens;

namespace ChatApi.Application.Services.TokenService
{
    public class TokenService : IToken
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public TokenService(Microsoft.Extensions.Configuration.IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;

        }

        public async Task<(bool,string)> AddTokenToDB(ApplicationUser user, string refreshToken)
        {
            try
            {
                var tokenInfo = _context.Tokens.
                FirstOrDefault(a => a.Username == user.Email);
                    
                    if (tokenInfo == null)
                    {
                        TokenInfo tokenInfo1 = new()
                        {
                            Username = user.Email!,
                            RefreshToken = refreshToken,
                            Expiration = DateTime.Now.AddDays(7)
                        };
                        _context.Tokens.Add(tokenInfo1);
                    }
                    else
                    {
                        tokenInfo.RefreshToken = refreshToken;
                        tokenInfo.Expiration = DateTime.Now.AddDays(7);
                    }
                await _context.SaveChangesAsync();
                return (true, "Token Add To Db Successfully");
            }catch(Exception ex)
            {
                return (false,ex.Message);
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey
                       (Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);

        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
                ValidateLifetime = false
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
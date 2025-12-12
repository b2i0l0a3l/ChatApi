using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Infrastructure.Identity;

namespace ChatApi.Infrastructure.JWT
{
    public class UserClaims
    {
        public static List<Claim> GenerateDefaultClaims(ApplicationUser user)
        {
              List<Claim> claims = new()
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()!),
                    new Claim(ClaimTypes.Name,(user.FirstName + " " + user.LastName)!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            return claims;
        }
    }
}
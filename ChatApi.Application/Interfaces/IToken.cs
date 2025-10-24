using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApi.Application.Interfaces
{
    public interface IToken
    {
        Task<(bool,string)> AddTokenToDB(Infrastructure.Identity.ApplicationUser user, string refreshToken);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

    }
}
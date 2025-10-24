using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Token.req;
using ChatApi.Application.Interfaces;
using ChatApi.Application.Interfaces.Auth;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services.AuthService.Refresh
{
    public class RefreshService : IRefresh
    {
        private readonly IToken _tokenService;
        private readonly AppDbContext _context;
        public RefreshService(IToken token, AppDbContext context)
        {
            _tokenService = token;
            _context = context;
        }
        public async Task<GeneralResponse<TokenReq>> Refresh(TokenReq model)
        {
            if(model == null)
                return GeneralResponse<TokenReq>.Failure("Invalid token data", StatusCodes.Status400BadRequest);
             try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
                var username = principal.Identity?.Name;

                var tokenInfo = _context.Tokens.SingleOrDefault(u => u.Username == username);

                if (tokenInfo == null
                    || tokenInfo.RefreshToken != model.RefreshToken 
                    || tokenInfo.Expiration <= DateTime.UtcNow)
                {
                    return GeneralResponse<TokenReq>.Failure("Invalid refresh token. Please login again.", StatusCodes.Status400BadRequest);    
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                tokenInfo.RefreshToken = newRefreshToken; 

                await _context.SaveChangesAsync();

                return GeneralResponse<TokenReq>.Success(new TokenReq
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }, "Token refreshed successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return GeneralResponse<TokenReq>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
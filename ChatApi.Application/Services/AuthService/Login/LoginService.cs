using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Res;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Interfaces;
using ChatApi.Application.Interfaces.Auth;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApi.Application.Services.AuthService.Login
{
    public class LoginService : ILogin
    {
        private readonly UserManager<Infrastructure.Identity.ApplicationUser> _userManager;
        private readonly IToken _tokenService; 

        public LoginService(UserManager<Infrastructure.Identity.ApplicationUser> userManager, IToken tokenService,
        AppDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        private async Task<bool> CheckEmailAndPassword(ApplicationUser? user, string Password)
        {
            if (user == null)
                return false;

            if (!await _userManager.CheckPasswordAsync(user, Password))
                return false;

            return true;
        }
        private async Task<(bool,string)> AddTokenToDB(Infrastructure.Identity.ApplicationUser user, string refreshToken)
        {
            return await _tokenService.AddTokenToDB(user, refreshToken);
        }
        private List<Claim> generateClaims(ApplicationUser user)
        {
            List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name,(user.FirstName + " " + user.LastName)!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };
            return claims;
        }
        private async Task<(string, string)> GenerateJwtToken(Infrastructure.Identity.ApplicationUser user)
        {

            var token = _tokenService.GenerateAccessToken(generateClaims(user));

            string refreshToken = _tokenService.GenerateRefreshToken();

            (bool , string) r = await AddTokenToDB(user, refreshToken);
            return (token, refreshToken);
        }

        public async Task<GeneralResponse<AuthRes>> Login(LoginModel model)
        {
             if (model == null)
                return GeneralResponse<AuthRes>.Failure("Invalid login data", StatusCodes.Status400BadRequest);
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (!await CheckEmailAndPassword(user, model.Password))
                {
                    return GeneralResponse<AuthRes>.Failure("Invalid email or password", StatusCodes.Status401Unauthorized);
                }

                var (token, refreshToken) = await GenerateJwtToken(user!);
                AuthRes authRes = new()
                {
                    Token = token,
                    RefreshToken = refreshToken
                };
                return GeneralResponse<AuthRes>.Success(authRes, "Login successful", StatusCodes.Status200OK);

            }
            catch (System.Exception ex)
            {

                return GeneralResponse<AuthRes>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
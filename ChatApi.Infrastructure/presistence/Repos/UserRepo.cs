using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class UserRepo : Repository<ApplicationUser,string> , IUserRepo
    {
        private ILogger<UserRepo> _logger;
        public UserRepo(AppDbContext context, ILogger<UserRepo> logger) : base(context) { _logger = logger; }


        public async Task<UserDto?> GetUser(string UserID)
        {
            
            if (string.IsNullOrEmpty(UserID))
            {
                _logger.LogError("User Id Invalid!");
                return null;
            }
            try
            {

                var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserID);
                if (User == null)
                {
                    _logger.LogWarning("User not Found");
                    return null;
                }
                return new UserDto{Id = User.Id, AvatarPath = User.ProfileImage,Email = User.Email, FullName = User.FullName};
            }catch (System.Exception ex)
            {
                _logger.LogError("Error Happend! {ex.Message}",ex.Message);

                return null;
            }
        }
    }
}
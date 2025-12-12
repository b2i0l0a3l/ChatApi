using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.User.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.presistence.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ChatApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo user;
        private readonly ICUrrentUser _CurrentUser;
        private readonly ILogger<UserService> _Logger;
        public UserService(IUserRepo user, ICUrrentUser CurrentUser,ILogger<UserService> Logger)
        {
            this.user = user;
            _CurrentUser = CurrentUser;
            _Logger = Logger;
        }
        public async Task<GeneralResponse<UserRes>> GetProfileInfo()
        {
            if (string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<UserRes>.Failure("UnAuthorized", 401);

            var r = await user.GetUser(_CurrentUser.UserId);
            if (r == null)
                return GeneralResponse<UserRes>.Failure("User Not Found", 404);

            return GeneralResponse<UserRes>.Success(new UserRes { Id = r.Id, FullName = r.FullName, AvatarPath = r.AvatarPath, Email = r.Email }, "Success", 200);
        }
        
          public async Task<GeneralResponse<UserRes>> GetProfileInfoById(string UserID)
        {
            
            if (string.IsNullOrEmpty(UserID))
                return GeneralResponse<UserRes>.Failure("UnAuthorized", 401);

            var r = await user.GetUser(UserID);
            if (r == null)
                return GeneralResponse<UserRes>.Failure("User Not Found", StatusCodes.Status404NotFound);

            return GeneralResponse<UserRes>.Success(new UserRes { Id = r.Id, FullName = r.FullName, AvatarPath = r.AvatarPath, Email = r.Email }, "Success", 200);
        }
    }
}
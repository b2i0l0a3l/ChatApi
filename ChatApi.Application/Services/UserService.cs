using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.User.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.presistence.Repos;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services
{
    public class UserService(IUserRepo user) : IUserService
    {

        public async Task<GeneralResponse<UserRes>> GetProfileInfo(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
                return GeneralResponse<UserRes>.Failure("Invalid Data", 400);

            var r = await user.GetUser(UserID);
            if (r == null)
                return GeneralResponse<UserRes>.Failure("User Not Found", StatusCodes.Status404NotFound);

            return GeneralResponse<UserRes>.Success(new UserRes { Id = r.Id, FullName = r.FullName, AvatarPath = r.AvatarPath, Email = r.Email }, "Success", 200);
        }
        
    }
}
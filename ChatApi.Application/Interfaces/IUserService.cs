using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.User.res;

namespace ChatApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<GeneralResponse<UserRes>> GetProfileInfo(string UserID); 
    }
}
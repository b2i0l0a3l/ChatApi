using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;

namespace ChatApi.Core.Interfaces
{
    public interface IUserRepo
    {
        Task<UserDto?> GetUser(string UserID);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Application.Interfaces.Auth
{
    public interface IAuth : ILogin , IRegister , IRefresh
    {
        
    }
}
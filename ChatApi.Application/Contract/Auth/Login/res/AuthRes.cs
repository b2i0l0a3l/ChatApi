using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Application.Contract.Auth.Res
{
    public class AuthRes
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
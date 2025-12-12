using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Infrastructure.JWT
{
    public class CurrentUser : ICUrrentUser
    {
        private IHttpContextAccessor _Http;
        public CurrentUser(IHttpContextAccessor Http) => _Http = Http;

        public string? UserId => _Http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
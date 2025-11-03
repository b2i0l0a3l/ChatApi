using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Profile")]
    [Authorize]
    public class ProfileController(IUserService user) : ControllerBase
    {
        [HttpGet("ProfileInfo")]
        public async Task<IActionResult> ProfileInfo()
        => Ok(await user.GetProfileInfo(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
    }
}
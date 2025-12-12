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
    [Route("api/v1/Profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _user;

        public ProfileController(IUserService user)
        => _user = user;

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ProfileInfo()
        => Ok(await _user.GetProfileInfo());


        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserProfile(string UserId)
        => Ok(await _user.GetProfileInfoById(UserId));
    }
}
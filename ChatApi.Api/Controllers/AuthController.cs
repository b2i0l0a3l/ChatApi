using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Res;
using ChatApi.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{

    [ApiController]
    [Route("api/v1/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromForm] Application.Contract.Auth.Req.SingUp model)
        => Ok(await _authService.Register(model));

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        => Ok(await _authService.Login(model));

        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromBody] Application.Contract.Token.req.TokenReq model)
        => Ok(await _authService.Refresh(model));
        

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Conversation")]
    public class ConversationController(IUniteOfWork _UOW) : ControllerBase
    {
        [HttpGet("user/{userId}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserConversations(string userId)
          =>   Ok(await _UOW.Coversation.GetUserConversations(userId));
    }
}
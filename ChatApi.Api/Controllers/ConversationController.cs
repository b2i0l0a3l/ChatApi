using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Conversation")]
    [Authorize]
    public class ConversationController(IUniteOfWork _UOW) : ControllerBase
    {
//         [HttpGet("me")]
//          [ProducesResponseType(StatusCodes.Status200OK)]
//          [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<IActionResult> GetMyConversations()
//     {
//         var userId = User.FindFirst("sub")?.Value;
//         if (string.IsNullOrEmpty(userId)) return Unauthorized();
//         var conversations = await _UOW.Coversation.GetUserConversations(userId);
//         if()
//         return Ok(conversations);
// }

    }
}
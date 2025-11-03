using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Conversation")]
    [Authorize]
    public class ConversationController(IConversationService conversation) : ControllerBase
    {
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyConversations()
        => Ok(await conversation.GetAllConversations(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

        [HttpPut("ChangeConversationTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeConversationTitle(ConversationReq conversationReq)
        => Ok(await conversation.ChangeConversationName(conversationReq));

    }
}
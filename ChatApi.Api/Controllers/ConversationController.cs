using System;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Conversation")]
    [Authorize] 
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversation;

        public ConversationController(IConversationService conversation)
        {
            _conversation = conversation;
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyConversations()
        => Ok(await _conversation.GetAllConversations());

        [HttpPut("title")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public async Task<IActionResult> ChangeConversationTitle([FromBody] ConversationReq conversationReq)
        => Ok(await _conversation.ChangeConversationName(conversationReq));
             
    }
}
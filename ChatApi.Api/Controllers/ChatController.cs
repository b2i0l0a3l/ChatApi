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
    [Route("api/Chat")]
    [Authorize]
        public class ChatController(IUniteOfWork _UOW) : ControllerBase
        {

            [HttpGet("Conversations/{conversationId:guid}/messages")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> GetMessages([FromBody] Guid conversationId,int Page , int Size)
            =>
                Ok(
                    await _UOW.Message.GetMessagesForConversation( conversationId , Page , Size)
                );

            [HttpGet()]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> UnreadMessages(Guid conversationId, string UserId)
            => Ok(await _UOW.Message.GetUnreadCount(conversationId, UserId));
        }

}
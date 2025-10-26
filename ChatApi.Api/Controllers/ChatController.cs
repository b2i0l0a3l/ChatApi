using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Chat")]
    public class ChatController(IUniteOfWork _UOW) : ControllerBase
    {

        [HttpGet("Conversations/{conversationId:guid}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessages(Guid conversationId)
          =>
            Ok(
                await _UOW.Message.FindAsync(m => m.ConversationId == conversationId)
            );
    }

}
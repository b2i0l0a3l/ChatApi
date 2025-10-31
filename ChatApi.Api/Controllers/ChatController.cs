using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.MessageContract.req;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Chat")]
    [Authorize]
        public class ChatController(IMessageService _service) : ControllerBase
        {

            [HttpGet("GetMessages")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> GetMessages([FromQuery] MessageReq model)
            =>
                Ok(
                    await _service.GetAllMessages(model)
                );

            // [HttpGet()]
            // [ProducesResponseType(StatusCodes.Status200OK)]
            // public async Task<IActionResult> UnreadMessages(Guid conversationId, string UserId)
            // => Ok(await _UOW.Message.GetUnreadCount(conversationId, UserId));
        }

}
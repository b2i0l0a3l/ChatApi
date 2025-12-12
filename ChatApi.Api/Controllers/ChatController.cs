using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.MessageContract.req;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Message")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _service;

        public ChatController(IMessageService service)
        {
            _service = service;
        }

        [HttpGet("messages")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessages([FromQuery] MessageReq model)
       => Ok(await _service.GetAllMessages(model));


        [HttpGet("unread-count")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadCount()
      => Ok(await _service.GetUnreadCount());

        [HttpGet("search")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchMessages(
            [FromQuery] string query,
            [FromQuery] Guid? conversationId = null)
        => Ok(await _service.SearchMessages(query, conversationId));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{
    [ApiController]
    [Route("api/Participant")]
    public class ParticipantController(IParticipantService participant) : ControllerBase
    {
        [HttpGet("GetParticipantInConversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetParticipantInConversation(string conversationId)
        => Ok(await participant.GetParticipantByConversationID(conversationId));
    }
}
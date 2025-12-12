
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Api.Controllers
{

    [ApiController]
    [Route("api/v1/Participant")]
    [Authorize]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participant;

        public ParticipantController(IParticipantService participant)
        {
            _participant = participant;
        }


        [HttpGet("conversation/{conversationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParticipantInConversation(string conversationId)
        => Ok(await _participant.GetParticipantByConversationID(conversationId));
          
        
    }
}
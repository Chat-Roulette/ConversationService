using ConversationService.Dtos;
using ConversationService.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ConversationService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost]
        public async Task<ConversationDto> CreateConversation(CreateConversationDto createConversationDto)
        {
            var result = await _conversationService.CreateConversationAsync(createConversationDto.Participants);

            return new ConversationDto
            {
                ConversationId = result.Id,
                Participants = result.Participants,
            };
        }

        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteConversation(Guid conversationId)
        {
            var result = await _conversationService.DeleteConversationAsync(conversationId);

            if (!result)
            {
                return BadRequest("Conversation doesn't exist");
            }

            return Ok();
        }
    }
}

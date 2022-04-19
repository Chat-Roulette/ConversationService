using ConversationService.Models;

namespace ConversationService.Services.Abstractions
{
    public interface IConversationService
    {
        Task<ConversationModel> CreateConversationAsync(IEnumerable<Guid> participants);
        Task<bool> DeleteConversationAsync(Guid conversationId);
    }
}

using ConversationService.Models;

namespace ConversationService.Services.Abstractions
{
    public interface IConversationService
    {
        Task<ConversationModel> GetConversationAsync(Guid conversationId);
        Task<ConversationModel> CreateConversationAsync(IEnumerable<Guid> participants);
        Task<bool> DeleteConversationAsync(Guid conversationId);
    }
}

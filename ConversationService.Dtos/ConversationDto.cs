namespace ConversationService.Dtos
{
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public IEnumerable<Guid> Participants { get; set; }
    }
}

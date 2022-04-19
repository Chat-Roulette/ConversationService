namespace ConversationService.Models
{
    public class ConversationModel
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Participants { get; set; }
    }
}

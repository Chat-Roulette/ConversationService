using System.ComponentModel.DataAnnotations;

namespace ConversationService.Dtos
{
    public class CreateConversationDto
    {
        [Required]
        [MaxLength(2)]
        [MinLength(2)]
        public IEnumerable<Guid> Participants { get; set; }
    }
}

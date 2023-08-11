using EduProject.Models.Common;

namespace EduProject.Models
{
    public class EventSpeaker : BaseEntity
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set;}
    }
}

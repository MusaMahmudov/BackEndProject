using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Speaker : BaseSectionEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Duty { get; set; }
        public ICollection<EventSpeaker> eventSpeakers { get; set; }

    }
}

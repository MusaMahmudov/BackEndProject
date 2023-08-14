using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Event : BaseSectionEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public ICollection<EventSpeaker> eventSpeakers { get; set; }



    }
}

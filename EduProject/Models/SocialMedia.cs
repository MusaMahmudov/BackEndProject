using EduProject.Models.Common;

namespace EduProject.Models
{
    public class SocialMedia : BaseSectionEntity
    {
        public string value { get; set; }
        public string Icon { get; set; } 
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}

using EduProject.Models.Common;

namespace EduProject.ViewModels.SpeakerViewModels
{
    public class SpeakerViewModel : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        public string Duty { get; set; }
        public string Name { get; set; }
    }
}

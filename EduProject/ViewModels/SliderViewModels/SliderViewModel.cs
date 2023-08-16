using EduProject.Models.Common;

namespace EduProject.ViewModels.SliderViewModels
{
    public class SliderViewModel : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

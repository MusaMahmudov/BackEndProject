using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.AdminCourseViewModels
{
    public class CourseViewModel : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}

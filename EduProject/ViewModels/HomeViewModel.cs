using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Models;
using EduProject.ViewModels.BlogViewModels;
using EduProject.ViewModels.CourseViewModels;
using EduProject.ViewModels.EventViewModels;

namespace EduProject.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<EventViewModel> Events { get; set; }
        public List<HomeCourseViewModel> Courses { get; set; }
        public List<HomeBlogViewModel> Blogs { get; set; }
    }
}

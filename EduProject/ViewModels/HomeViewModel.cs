using EduProject.Models;
using EduProject.ViewModels.EventViewModels;

namespace EduProject.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<EventViewModel> Events { get; set; }
    }
}

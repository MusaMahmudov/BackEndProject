using EduProject.Models;
using EduProject.Models.Common;

namespace EduProject.ViewModels.EventViewModels
{
    public class EventViewModel : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Date {  get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        


    }
}

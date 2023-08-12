using EduProject.Models;

namespace EduProject.ViewModels.EventViewModels
{
    public class DetailEventViewModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Image {  get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public ICollection<Speaker> Speakers { get; set; }


    }
}

using EduProject.Models;

namespace EduProject.Areas.Admin.ViewModels.AdminEventViewModels
{
    public class AdminDetailEventViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<string> Speakers { get; set; }
        
    }
}

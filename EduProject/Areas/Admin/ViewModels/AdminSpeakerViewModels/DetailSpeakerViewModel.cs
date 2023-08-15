namespace EduProject.Areas.Admin.ViewModels.AdminSpeakerViewModels
{
    public class DetailSpeakerViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Duty { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get;set; }
        public DateTime UpdatedDate { get;set;}
        public List<string> EventName { get; set; }
    }
}

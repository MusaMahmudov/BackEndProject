namespace EduProject.Areas.Admin.ViewModels.AdminSpeakerViewModels
{
    public class UpdateSpeakerViewModel
    {
        public IFormFile? Image { get; set; }
        public string Duty { get; set; }
        public string Name { get; set; }
        public List<int>? eventId { get; set; }
    }
}

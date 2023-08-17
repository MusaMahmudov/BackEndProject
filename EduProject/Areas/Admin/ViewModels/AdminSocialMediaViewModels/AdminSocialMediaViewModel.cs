using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.AdminSocialMediaViewModels
{
    public class AdminSocialMediaViewModel : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }

    }
}

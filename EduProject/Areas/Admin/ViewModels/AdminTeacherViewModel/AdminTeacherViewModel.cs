using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel
{
    public class AdminTeacherViewModel : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Rank { get; set; }
        
    }
}

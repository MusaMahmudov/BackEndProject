using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.UserViewModels
{
    public class UserViewModel 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
    }
}

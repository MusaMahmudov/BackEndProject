using EduProject.Utils.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduProject.Areas.Admin.ViewModels.UserViewModels
{
    public class DetailUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public List<IdentityRole> Role { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set;}
        
    }
}

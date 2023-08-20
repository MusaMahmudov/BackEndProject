using EduProject.Utils.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduProject.Areas.Admin.ViewModels.UserViewModels
{
    public class ChangeUserViewModel
    {
        public string Id { get; set; }
        public List<string>? RoleId{ get; set; }
        //public bool IsActive { get; set; }

    }
}

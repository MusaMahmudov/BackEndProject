using EduProject.Models;
using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel
{
    public class AdminDetailTeacherViewModel 
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<byte> Percent { get; set; }
        public List<string>? SkillName { get; set; }
        public List<SocialMedia>? socialMedia { get; set; }
    }
}

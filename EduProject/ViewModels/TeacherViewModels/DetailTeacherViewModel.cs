using EduProject.Models;
using EduProject.Models.Common;

namespace EduProject.ViewModels.TeacherViewModels
{
    public class DetailTeacherViewModel : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experience {  get; set; }
        public string Hobbies { get; set; }

        public string Faculty { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public List<SocialMedia> socialMedia { get; set; }
        public List<string> SkillNames { get; set; }
        public List<byte> Percent { get; set; }


    }
}

using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Skill : BaseEntity
    {
        public string Name {  get; set; }
        public ICollection<TeacherSkill> TeacherSkill { get; set; }
    }
}

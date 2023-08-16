using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Skill : BaseSectionEntity
    {
        public string Name {  get; set; }
        public ICollection<TeacherSkill> TeacherSkill { get; set; }
    }
}

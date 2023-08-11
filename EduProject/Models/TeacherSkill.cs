using EduProject.Models.Common;

namespace EduProject.Models
{
    public class TeacherSkill : BaseEntity
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }
        public byte Percent { get; set; }

    }
}

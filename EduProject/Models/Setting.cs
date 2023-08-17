using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

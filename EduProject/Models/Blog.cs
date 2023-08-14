using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Blog : BaseSectionEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
       
        public string Description { get; set; }

    }
}

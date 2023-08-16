using EduProject.Models.Common;
using System.Security.Cryptography.X509Certificates;

namespace EduProject.Models
{
    public class Category : BaseSectionEntity
    {
        public string Name { get; set; }
        public ICollection<CourseCategory> courseCategories { get; set; }

    }
}
